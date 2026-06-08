using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SmileTrack
{
    public partial class frmReceptionistDashboard : Form
    {
        // Connection string sa iyong local database
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False";

        public frmReceptionistDashboard()
        {
            InitializeComponent();
            WireGridEvents();
            try { DatabaseHelper.AppointmentsChanged -= DatabaseHelper_AppointmentsChanged; } catch { }
            DatabaseHelper.AppointmentsChanged += DatabaseHelper_AppointmentsChanged;
            try { btnAddWalkIn.Click -= btnAddWalkIn_Click; } catch { }
            btnAddWalkIn.Click += btnAddWalkIn_Click;
        }

        // Nangyayari ito kapag bumukas na ang form sa screen
        private void frmReceptionistDashboard_Load(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void frmReceptionistDashboard_Load_1(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        public void RefreshDashboard()
        {
            LoadDashboard();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try { AuditLogger.SaveAuditLog(Environment.UserName, "Logout", "User logged out from receptionist dashboard"); } catch { }
                this.Hide();
                new LoginForm().Show();
            }
        }

        private void DatabaseHelper_AppointmentsChanged()
        {
            try
            {
                if (this.InvokeRequired) { this.Invoke(new Action(LoadDashboard)); }
                else LoadDashboard();
            }
            catch { }
        }

        public void LoadDashboard()
        {
            try
            {
                // Siguraduhing may database connection at tables
                DatabaseHelper.EnsureDatabaseAndTables();

                // -------------------------------------------------------------------------
                // 1. APPOINTMENTS SUMMARY (Top-Left Grid: Regular Appointments TODAY)
                // -------------------------------------------------------------------------

                // Include Completed in today's list; only exclude Cancelled
                var dtAppointments = DatabaseHelper.ExecuteQuery(
                    @"SELECT a.AppointmentID,
                             a.PatientID,
                             FORMAT(a.AppointmentDateTime,'hh:mm tt') AS Time,
                             ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS [Patient Name],
                             a.Dentist,
                             a.Treatment,
                             ISNULL(a.Status,'') AS Status
                      FROM Appointments a
                      LEFT JOIN Patients p ON a.PatientID = p.PatientID
                      WHERE CAST(a.AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)
                        AND ISNULL(a.Status,'') NOT IN ('Cancelled')
                        AND ISNULL(a.VisitType,'') <> 'Walk-in' 
                      ORDER BY a.AppointmentDateTime");

                if (this.dgvAppointments != null)
                {
                    BindGridClean(this.dgvAppointments, dtAppointments);
                    HideIdColumns(this.dgvAppointments);
                    AddViewColumn(this.dgvAppointments);
                    ApplyStatusRowStyles(this.dgvAppointments);

                    // Ensure column order and format: Time, Patient Name, Dentist, Treatment, Status, View
                    try
                    {
                        if (dgvAppointments.Columns.Contains("Time")) dgvAppointments.Columns["Time"].DisplayIndex = 0;
                        if (dgvAppointments.Columns.Contains("Patient Name")) dgvAppointments.Columns["Patient Name"].DisplayIndex = 1;
                        if (dgvAppointments.Columns.Contains("Dentist")) dgvAppointments.Columns["Dentist"].DisplayIndex = 2;
                        if (dgvAppointments.Columns.Contains("Treatment")) dgvAppointments.Columns["Treatment"].DisplayIndex = 3;
                        if (dgvAppointments.Columns.Contains("Status")) dgvAppointments.Columns["Status"].DisplayIndex = 4;
                        if (dgvAppointments.Columns.Contains("View")) dgvAppointments.Columns["View"].DisplayIndex = 5;
                    }
                    catch { }
                }

                // -------------------------------------------------------------------------
                // 2. WALK-IN SUMMARY (Bottom-Left Grid: Walk-ins TODAY)
                // -------------------------------------------------------------------------
                var dtWalkins = DatabaseHelper.ExecuteQuery(
                    @"SELECT a.AppointmentID,
                             a.PatientID,
                             ROW_NUMBER() OVER (ORDER BY a.AppointmentDateTime) AS [No.],
                             ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS [Patient Name],
                             FORMAT(a.AppointmentDateTime,'hh:mm tt') AS [Time-in],
                             ISNULL(a.Status,'') AS Status
                      FROM Appointments a
                      LEFT JOIN Patients p ON a.PatientID = p.PatientID
                      WHERE CAST(a.AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)
                        AND ISNULL(a.Status,'') NOT IN ('Cancelled','Completed')
                        AND ISNULL(a.VisitType,'') = 'Walk-in'
                      ORDER BY a.AppointmentDateTime");

                if (this.dgvWalkIn != null)
                {
                    BindGridClean(this.dgvWalkIn, dtWalkins);
                    HideIdColumns(this.dgvWalkIn);
                    AddViewColumn(this.dgvWalkIn);
                    ApplyStatusRowStyles(this.dgvWalkIn);
                    try
                    {
                        if (dgvWalkIn.Columns.Contains("No.")) dgvWalkIn.Columns["No."].DisplayIndex = 0;
                        if (dgvWalkIn.Columns.Contains("Patient Name")) dgvWalkIn.Columns["Patient Name"].DisplayIndex = 1;
                        if (dgvWalkIn.Columns.Contains("Time-in")) dgvWalkIn.Columns["Time-in"].DisplayIndex = 2;
                        if (dgvWalkIn.Columns.Contains("Status")) dgvWalkIn.Columns["Status"].DisplayIndex = 3;
                        if (dgvWalkIn.Columns.Contains("View")) dgvWalkIn.Columns["View"].DisplayIndex = 4;
                    }
                    catch { }
                }

                // -------------------------------------------------------------------------
                // 3. REMINDERS SUMMARY (Top-Right Grid: BOTH Walk-in & Appointments)
                // -------------------------------------------------------------------------
                LoadUpcomingReminders(hoursAhead: 72);

                // 4. BILLING SUMMARY (Bottom-Right Panel)
                LoadBillingSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUpcomingReminders(int hoursAhead = 72)
        {
            try
            {
                // Kinukuha ang parehong Appointment at Walk-in para sa Reminders grid
                var dtReminders = DatabaseHelper.ExecuteQuery(
                    @"SELECT a.AppointmentID,
                             a.PatientID,
                             CAST(a.AppointmentDateTime AS DATE) AS [Date],
                             ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS [Patient Name],
                             FORMAT(a.AppointmentDateTime,'hh:mm tt') AS Time,
                             ISNULL(a.VisitType,'') AS [Type],
                             ISNULL(a.Status,'') AS Status
                      FROM Appointments a
                      LEFT JOIN Patients p ON a.PatientID = p.PatientID
                      WHERE a.AppointmentDateTime >= GETDATE() 
                        AND a.AppointmentDateTime <= DATEADD(HOUR, @hoursAhead, GETDATE()) 
                        AND ISNULL(a.Status,'') NOT IN ('Cancelled','Completed')
                        AND (ISNULL(a.VisitType,'') = 'Appointment' OR ISNULL(a.VisitType,'') = 'Walk-in' OR ISNULL(a.VisitType,'') = '')
                      ORDER BY a.AppointmentDateTime",
                    new SqlParameter("@hoursAhead", hoursAhead));

                if (this.dgvReminders != null)
                {
                    BindGridClean(this.dgvReminders, dtReminders);
                    HideIdColumns(this.dgvReminders);
                    AddViewColumn(this.dgvReminders);
                    ApplyStatusRowStyles(this.dgvReminders);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading reminders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBillingSummary()
        {
            try
            {
                var dtPaid = DatabaseHelper.ExecuteQuery(@"SELECT COUNT(1) AS PaidCount, ISNULL(SUM(PaidAmount),0) AS PaidSum FROM Invoices WHERE CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE) AND ISNULL(Status,'') = 'Paid'");
                var dtUnpaid = DatabaseHelper.ExecuteQuery(@"SELECT COUNT(1) AS UnpaidCount, ISNULL(SUM(BalanceAmount),0) AS UnpaidSum FROM Invoices WHERE CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE) AND ISNULL(Status,'') <> 'Paid'");

                lblPaid.Text = dtPaid.Rows.Count > 0 ? dtPaid.Rows[0]["PaidCount"].ToString() : "0";
                lblUnpaid.Text = dtUnpaid.Rows.Count > 0 ? dtUnpaid.Rows[0]["UnpaidCount"].ToString() : "0";

                decimal revenue = dtPaid.Rows.Count > 0 ? Convert.ToDecimal(dtPaid.Rows[0]["PaidSum"]) : 0;
                lblTotalRevenue.Text = "₱" + revenue.ToString("N2");
            }
            catch
            {
                lblPaid.Text = "0";
                lblUnpaid.Text = "0";
                lblTotalRevenue.Text = "₱0.00";
            }
        }

        // Helper para sa malinis na pag-bind ng Data nang hindi nasisira ang control UI
        private void BindGridClean(DataGridView grid, DataTable dt)
        {
            if (grid == null) return;
            grid.SuspendLayout();
            try
            {
                grid.DataSource = null;
                grid.Columns.Clear();
                grid.AutoGenerateColumns = true;
                grid.DataSource = dt ?? new DataTable();

                // UI Settings base sa wireframe mo
                grid.ReadOnly = true;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
                grid.RowHeadersVisible = false;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            finally
            {
                grid.ResumeLayout();
            }
        }

        private void HideIdColumns(DataGridView grid)
        {
            if (grid == null) return;
            if (grid.Columns.Contains("AppointmentID")) grid.Columns["AppointmentID"].Visible = false;
            if (grid.Columns.Contains("PatientID")) grid.Columns["PatientID"].Visible = false;
        }

        private void AddViewColumn(DataGridView grid)
        {
            if (grid == null || grid.Columns.Contains("View")) return;
            var viewButton = new DataGridViewButtonColumn
            {
                Name = "View",
                HeaderText = "Action",
                Text = "View",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };
            grid.Columns.Add(viewButton);
        }

        private void ApplyStatusRowStyles(DataGridView grid)
        {
            if (grid == null) return;
            int statusIdx = -1;
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (string.Equals(grid.Columns[i].Name, "Status", StringComparison.OrdinalIgnoreCase))
                { statusIdx = i; break; }
            }
            if (statusIdx < 0) return;

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                string st = Convert.ToString(row.Cells[statusIdx].Value ?? string.Empty).Trim();

                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.ForeColor = Color.Black;

                if (string.Equals(st, "Scheduled", StringComparison.OrdinalIgnoreCase))
                    row.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                else if (string.Equals(st, "Waiting", StringComparison.OrdinalIgnoreCase))
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
                else if (string.Equals(st, "Cancelled", StringComparison.OrdinalIgnoreCase))
                { row.DefaultCellStyle.BackColor = Color.LightCoral; row.DefaultCellStyle.ForeColor = Color.White; }
                else if (string.Equals(st, "Completed", StringComparison.OrdinalIgnoreCase))
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
            }
        }

        // Pag-wire sa mga Click Events ng tatlong Tables mo
        private void WireGridEvents()
        {
            if (this.dgvAppointments != null) this.dgvAppointments.CellContentClick += Grid_CellContentClick;
            if (this.dgvWalkIn != null) this.dgvWalkIn.CellContentClick += Grid_CellContentClick;
            if (this.dgvReminders != null) this.dgvReminders.CellContentClick += Grid_CellContentClick;
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var grid = sender as DataGridView;
            if (grid == null || grid.Columns.Count <= e.ColumnIndex || grid.Columns[e.ColumnIndex].Name != "View") return;

            int? patientId = null;
            int? appointmentId = null;

            if (grid.Columns.Contains("PatientID") && grid.Rows[e.RowIndex].Cells["PatientID"].Value != null)
                if (int.TryParse(grid.Rows[e.RowIndex].Cells["PatientID"].Value.ToString(), out int pid)) patientId = pid;

            if (grid.Columns.Contains("AppointmentID") && grid.Rows[e.RowIndex].Cells["AppointmentID"].Value != null)
                if (int.TryParse(grid.Rows[e.RowIndex].Cells["AppointmentID"].Value.ToString(), out int aid)) appointmentId = aid;

            if (patientId.HasValue)
            {
                var pi = new Patient_Info_Appoinment();
                if (appointmentId.HasValue) pi.LoadPatientAndAppointment(patientId.Value, appointmentId.Value);
                else pi.PopulatePatientById(patientId.Value);

                pi.StartPosition = FormStartPosition.CenterParent;
                pi.ShowDialog(this);

               
                LoadDashboard();
            }
        }



        private void btnPatients_Click(object sender, EventArgs e)
        {
            var addForm = new Patient_Info_Appoinment();
            addForm.ShowDialog();
            LoadDashboard();
        }

        private void btnAddWalkIn_Click(object sender, EventArgs e)
        {
            try
            {
                // Ask for patient id
                string input = Prompt("Add Walk-in", "Enter Patient ID for Walk-in:");
                if (!int.TryParse(input, out int patientId) || patientId <= 0)
                {
                    MessageBox.Show("Invalid Patient ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create a walk-in appointment for now with default dentist empty
                // signature: AddAppointment(int patientId, DateTime appointmentDateTime, string dentist, string treatment, string status, string visitType, string notes)
                int apptId = DatabaseHelper.AddAppointment(patientId, DateTime.Now, string.Empty, string.Empty, "Waiting", "Walk-in", "Walk-in created from receptionist dashboard");
                MessageBox.Show($"Walk-in created (Appointment ID: {apptId}).", "Walk-in", MessageBoxButtons.OK, MessageBoxIcon.Information);
                try { DatabaseHelper.RaiseAppointmentsChanged(); } catch { }
                LoadDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating walk-in: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string Prompt(string title, string promptText)
        {
            using (var prompt = new Form())
            {
                prompt.Width = 400;
                prompt.Height = 150;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = title;
                prompt.StartPosition = FormStartPosition.CenterParent;

                var textLabel = new Label() { Left = 20, Top = 20, Width = 340, Text = promptText };
                var textBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
                var confirmation = new Button() { Text = "OK", Left = 260, Width = 100, Top = 80, DialogResult = DialogResult.OK };
                confirmation.Click += (s, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;
                return prompt.ShowDialog(this) == DialogResult.OK ? textBox.Text.Trim() : string.Empty;
            }
        }

        private void btnBillings_Click_1(object sender, EventArgs e)
        {
        
            new BillingForm().Show();
        }
    }
    }
    