using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SmileTrack
{
    public partial class DentistDashboard : Form
    {
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False";
        private Timer refreshTimer;

        public event EventHandler AppointmentChanged;

        protected virtual void OnAppointmentChanged()
        {
            AppointmentChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateAppointments()
        {
            OnAppointmentChanged();
        }

        public DentistDashboard()
        {
            InitializeComponent();
            try { DatabaseHelper.AppointmentsChanged -= DatabaseHelper_AppointmentsChanged; } catch { }
            DatabaseHelper.AppointmentsChanged += DatabaseHelper_AppointmentsChanged;

            // Wire dynamic action click handler for clinical treatments
            this.dgvSched.CellContentClick += dgvSched_CellContentClick;
        }

        private void DentistDashboard_Load(object sender, EventArgs e)
        {
            RefreshDashboard();

            refreshTimer = new Timer();
            refreshTimer.Interval = 60000; // Nagre-refresh kada 1 minuto
            refreshTimer.Tick += (s, ev) => RefreshDashboard();
            refreshTimer.Start();
        }

        private void RefreshDashboard()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshDashboard));
                return;
            }

            LoadAppointmentsFromDb();
         
            LoadDashboardSummary();
           
        }

        // -------------------------------------------------------------------------
        // FIXED: Nagpapakita na ng Checked-In at mga Upcoming Appointments
        // -------------------------------------------------------------------------
        private void LoadAppointmentsFromDb()
        {
            try
            {
                // Kinuha ang mga appointment ngayon hanggang sa mga susunod na araw (>= GETDATE)
                // Inalis ang mga Cancelled at No-Show para malinis ang listahan
                string sql = @"SELECT a.AppointmentID, a.PatientID,
                                      FORMAT(a.AppointmentDateTime, 'yyyy-MM-dd hh:mm tt') AS [Date & Time],
                                      ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS [Patient],
                                      ISNULL(a.Dentist,'') AS Dentist, 
                                      ISNULL(a.Treatment,'') AS Treatment, 
                                      ISNULL(a.Status,'') AS [Status]
                               FROM Appointments a
                               LEFT JOIN Patients p ON a.PatientID = p.PatientID
                               WHERE CAST(a.AppointmentDateTime AS DATE) >= CAST(GETDATE() AS DATE)
                                 AND ISNULL(a.Status,'') NOT IN ('Cancelled', 'No-Show')
                               ORDER BY 
                                  -- Priority 1: Unahin ang mga nandyan na sa clinic (Checked-In)
                                  CASE WHEN a.Status = 'Checked-In' THEN 1 
                                  -- Priority 2: Kasunod ang mga darating pa lang o nakaiskedyul
                                       WHEN a.Status IN ('Pending', 'Confirmed', 'Scheduled') THEN 2 
                                  -- Priority 3: Pinakahuli ang mga tapos na ngayon
                                       ELSE 3 END, 
                                  a.AppointmentDateTime ASC";

                var dt = DatabaseHelper.ExecuteQuery(sql);

                dgvSched.SuspendLayout();
                try
                {
                    dgvSched.DataSource = null;
                    dgvSched.Columns.Clear();
                    dgvSched.AutoGenerateColumns = true;
                    dgvSched.DataSource = dt;
                    dgvSched.ReadOnly = true;
                    dgvSched.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dgvSched.AllowUserToAddRows = false;
                    dgvSched.AllowUserToDeleteRows = false;
                    dgvSched.RowHeadersVisible = false;
                    dgvSched.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Action Column allowing Dentist to document and bill straight from grid
                    if (!dgvSched.Columns.Contains("CompleteAction"))
                    {
                        var compBtn = new DataGridViewButtonColumn
                        {
                            Name = "CompleteAction",
                            HeaderText = "Action",
                            Text = "Treat & Charge",
                            UseColumnTextForButtonValue = true,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                        };
                        dgvSched.Columns.Add(compBtn);
                    }
                }
                finally { dgvSched.ResumeLayout(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading schedule queue: " + ex.Message);
            }
        }

        // -------------------------------------------------------------------------
        // Writes: Clinical Notes & Treatment Charges
        // -------------------------------------------------------------------------
        private void dgvSched_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvSched.Columns[e.ColumnIndex].Name == "CompleteAction")
            {
                var row = dgvSched.Rows[e.RowIndex];
                string currentStatus = row.Cells["Status"].Value.ToString();

                if (currentStatus == "Completed")
                {
                    MessageBox.Show("This patient session is already completed and filed to billing.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int apptId = Convert.ToInt32(row.Cells["AppointmentID"].Value);
                int patientId = Convert.ToInt32(row.Cells["PatientID"].Value);
                string currentPatient = row.Cells["Patient"].Value.ToString();

                // 1. Inputs Clinical Notes
                string treatment = Prompt("Clinical Records", $"Enter treatment details provided to {currentPatient}:");
                if (string.IsNullOrEmpty(treatment)) return;

                // 2. Inputs Treatment Charge
                string feeInput = Prompt("Billing Setup", "Enter Treatment Charge Amount (₱):");
                if (!decimal.TryParse(feeInput, out decimal totalCharge) || totalCharge < 0)
                {
                    MessageBox.Show("Invalid amount entered. Treatment completion cancelled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    // Update Appointment status to Completed
                    DatabaseHelper.ExecuteNonQuery(
                        @"UPDATE Appointments 
                          SET Treatment = @Treatment, Status = 'Completed' 
                          WHERE AppointmentID = @AppointmentID",
                        new SqlParameter("@Treatment", treatment),
                        new SqlParameter("@AppointmentID", apptId));

                    // Write dynamic Invoice record for Receptionist Dashboard to read
                    DatabaseHelper.ExecuteNonQuery(
                        @"INSERT INTO Invoices (PatientID, AppointmentID, InvoiceDate, TotalAmount, PaidAmount, BalanceAmount, Status)
                          VALUES (@PatientID, @AppointmentID, GETDATE(), @Total, 0, @Total, 'Unpaid')",
                        new SqlParameter("@PatientID", patientId),
                        new SqlParameter("@AppointmentID", apptId),
                        new SqlParameter("@Total", totalCharge));

                    MessageBox.Show("Clinical documentation and system charges updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DatabaseHelper.RaiseAppointmentsChanged();
                    RefreshDashboard();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("System failed to commit clinical entries: " + ex.Message);
                }
            }
        }

        private string Prompt(string title, string promptText)
        {
            using (var prompt = new Form())
            {
                prompt.Width = 400; prompt.Height = 150; prompt.Text = title;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.StartPosition = FormStartPosition.CenterParent;
                var textLabel = new Label() { Left = 20, Top = 20, Width = 340, Text = promptText };
                var textBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
                var confirmation = new Button() { Text = "OK", Left = 260, Width = 100, Top = 80, DialogResult = DialogResult.OK };
                confirmation.Click += (s, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox); prompt.Controls.Add(textLabel); prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;
                return prompt.ShowDialog(this) == DialogResult.OK ? textBox.Text.Trim() : string.Empty;
            }
        }

       
      

        private void LoadDashboardSummary()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmdToday = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE) AND ISNULL(Status,'') NOT IN ('Cancelled')", conn);
                    txtTodaysAppoinment.Text = cmdToday.ExecuteScalar()?.ToString() ?? "0";

                    SqlCommand cmdSeen = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE Status='Completed' AND CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)", conn);
                    txtPatientSeen.Text = cmdSeen.ExecuteScalar()?.ToString() ?? "0";

                    SqlCommand cmdTreat = new SqlCommand(@"SELECT ISNULL(Treatment,'<unspecified>') AS Treatment, COUNT(*) AS C
                                                           FROM Appointments
                                                           WHERE Status='Completed' AND CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)
                                                           GROUP BY ISNULL(Treatment,'<unspecified>')", conn);
                    var sb = new StringBuilder();
                    using (var rdr = cmdTreat.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            sb.AppendLine($"{rdr["Treatment"]}: {rdr["C"]}");
                        }
                    }
                    txtTreatmentDone.Text = sb.Length > 0 ? sb.ToString().TrimEnd() : "0";

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Summary Error: " + ex.Message);
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Hide();
                new LoginForm().Show();
            }
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            RefreshDashboard();
        }

        private void DgvSched_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                var row = dgvSched.Rows[e.RowIndex];
                int apptId = -1;
                int patientId = -1;

                if (dgvSched.Columns.Contains("AppointmentID") && row.Cells["AppointmentID"].Value != null)
                    int.TryParse(row.Cells["AppointmentID"].Value.ToString(), out apptId);

                if (dgvSched.Columns.Contains("PatientID") && row.Cells["PatientID"].Value != null)
                    int.TryParse(row.Cells["PatientID"].Value.ToString(), out patientId);

                var pi = new Patient_Info_Appoinment();
                if (patientId > 0 && apptId > 0)
                {
                    pi.LoadPatientAndAppointment(patientId, apptId);
                }
                else if (patientId > 0)
                {
                    pi.PopulatePatientById(patientId);
                }

                pi.StartPosition = FormStartPosition.CenterParent;
                pi.ShowDialog(this);

                RefreshDashboard();
            }
            catch { }
        }

        private void btnMySched_Click_1(object sender, EventArgs e)
        {
            new My_Schedule().Show(this);
        }

        public void RefreshAppointments()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(RefreshAppointments));
                    return;
                }

                LoadAppointmentsFromDb();
                LoadDashboardSummary();
               
            }
            catch { }
        }

        private void DatabaseHelper_AppointmentsChanged()
        {
            try
            {
                if (this.InvokeRequired) { this.Invoke(new Action(RefreshAppointments)); }
                else RefreshAppointments();
            }
            catch { }
        }

        private void DentistDashboard_Load_1(object sender, EventArgs e)
        {
            // Pwedeng iwanang bakante kung may load handler ka na sa taas
        }
    }
}