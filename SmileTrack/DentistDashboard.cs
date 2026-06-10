using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Tiyaking kasama ito para sa Chart

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
            // Ligtas na pag-subscribe sa static event ng DatabaseHelper
            try { DatabaseHelper.AppointmentsChanged -= DatabaseHelper_AppointmentsChanged; } catch { }
            DatabaseHelper.AppointmentsChanged += DatabaseHelper_AppointmentsChanged;
        }

        private void DentistDashboard_Load(object sender, EventArgs e)
        {
            // Unang load ng data
            RefreshDashboard();

            // Setup ng auto-refresh bawat 60 segundo
            refreshTimer = new Timer();
            refreshTimer.Interval = 60000;
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
            LoadPatientOverview();
            LoadDashboardSummary();
            LoadTreatmentSummaryChart();
        }

        private void LoadAppointmentsFromDb()
        {
            try
            {
                string sql = @"SELECT a.AppointmentID, a.PatientID,
                                      FORMAT(a.AppointmentDateTime,'hh:mm tt') AS [Time],
                                      ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS [Patient],
                                      ISNULL(a.Dentist,'') AS Dentist, ISNULL(a.Treatment,'') AS Treatment, ISNULL(a.Status,'') AS [Status]
                               FROM Appointments a
                               LEFT JOIN Patients p ON a.PatientID = p.PatientID
                               WHERE CAST(a.AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)
                                 AND ISNULL(a.Status,'') NOT IN ('Cancelled')
                               ORDER BY a.AppointmentDateTime";

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
                }
                finally { dgvSched.ResumeLayout(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sa pag-load ng Schedule: " + ex.Message);
            }
        }

        private void LoadPatientOverview()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT p.FirstName + ' ' + p.LastName AS Patient,
                                     la.AppointmentDateTime AS LastVisit,
                                     la.Treatment AS Treatment,
                                     la.Dentist AS Dentist,
                                     la.Status AS [Status]
                                     FROM Patients p
                                     OUTER APPLY (
                                         SELECT TOP(1) a.AppointmentDateTime, a.Treatment, a.Dentist, a.Status
                                         FROM Appointments a
                                         WHERE a.PatientID = p.PatientID
                                         ORDER BY a.AppointmentDateTime DESC
                                     ) la";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvOverview.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sa Patient Overview: " + ex.Message);
            }
        }

        private void LoadTreatmentSummaryChart()
        {
            try
            {
                // Kukunin ang mga 'Completed' treatments ng kasalukuyang dentist
                string sql = @"SELECT ISNULL(Treatment,'<unspecified>') AS Treatment, COUNT(*) AS C
                               FROM Appointments
                               WHERE ISNULL(Status,'') = 'Completed'
                               GROUP BY ISNULL(Treatment,'<unspecified>')";

                var dt = DatabaseHelper.ExecuteQuery(sql);

                chart1.Series.Clear();
                chart1.Legends.Clear();
                chart1.ChartAreas[0].AxisX.Interval = 1; // Para hindi mag-skip ang mga labels sa X axis

                var s = chart1.Series.Add("TreatmentSummary");

                // Binago sa 'Column' (Bar) para magtugma sa hitsura ng iyong UI Design
                s.ChartType = SeriesChartType.Column;
                s.IsValueShownAsLabel = true;

                foreach (DataRow r in dt.Rows)
                {
                    string t = r["Treatment"]?.ToString() ?? "<unspecified>";
                    int c = Convert.ToInt32(r["C"]);
                    s.Points.AddXY(t, c);
                }

                chart1.Update();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chart Error: " + ex.Message);
            }
        }

        private void LoadDashboardSummary()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Today's Appointments (Excluding Cancelled)
                    SqlCommand cmdToday = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE) AND ISNULL(Status,'') NOT IN ('Cancelled')", conn);
                    txtTodaysAppoinment.Text = cmdToday.ExecuteScalar()?.ToString() ?? "0";

                    // 2. Patients Seen (Completed Today)
                    SqlCommand cmdSeen = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE Status='Completed' AND CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)", conn);
                    txtPatientSeen.Text = cmdSeen.ExecuteScalar()?.ToString() ?? "0";

                    // 3. Treatment Done (Listahan ng mga nagawa ngayong araw)
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

                    // 4. Upcoming Appointments (Lahat ng darating na schedule sa hinaharap)
                    SqlCommand cmdUpcoming = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE AppointmentDateTime > GETDATE() AND ISNULL(Status,'') NOT IN ('Cancelled')", conn);
                    txtUpApp.Text = cmdUpcoming.ExecuteScalar()?.ToString() ?? "0";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Summary Error: " + ex.Message);
            }
        }

        // --- BUTTON HANDLERS ---
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
            catch { /* ignore */ }
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
                LoadTreatmentSummaryChart();
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
            // Iwanang blanko o tanggalin kung hindi ginagamit sa events
        }

        private void btnMySched_Click(object sender, EventArgs e)
        {
            new My_Schedule().Show(this);

            string activeDentist = "Dr. Margie";

            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtPatientSeen_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDashboard_Click_1(object sender, EventArgs e)
        {

        }

        private void txtTodaysAppoinment_TextChanged(object sender, EventArgs e)
        {

        }
    }
}