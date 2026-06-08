using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace SmileTrack
{
    public partial class DentistDashboard : Form
    {
        // Palitan ang connection string kung iba ang settings ng SQL mo
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False";
        private Timer refreshTimer;

        public DentistDashboard()
        {
            InitializeComponent();
        }

        private void DentistDashboard_Load(object sender, EventArgs e)
        {
            // Tawagin lahat ng loaders
            RefreshDashboard();

            // Setup timer para auto-refresh kada 1 minuto
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
        }

        private void LoadAppointmentsFromDb()
        {
            try
            {
                // Only show today's appointments (exclude cancelled)
                string sql = @"SELECT FORMAT(a.AppointmentDateTime,'hh:mm tt') AS [Time], 
                                      ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS [Patient], 
                                      a.Treatment, ISNULL(a.Status,'') AS [Status]
                               FROM Appointments a
                               LEFT JOIN Patients p ON a.PatientID = p.PatientID
                               WHERE CAST(a.AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)
                                 AND ISNULL(a.Status,'') NOT IN ('Cancelled')
                               ORDER BY a.AppointmentDateTime";

                var dt = DatabaseHelper.ExecuteQuery(sql);

                // Bind safely to avoid duplicate columns defined in designer
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
                                     MAX(a.AppointmentDateTime) AS LastVisit 
                                     FROM Patients p 
                                     LEFT JOIN Appointments a ON p.PatientID = a.PatientID 
                                     GROUP BY p.FirstName, p.LastName";

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

        private void LoadDashboardSummary()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Today's Appointments (excluding cancelled)
                    SqlCommand cmdToday = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE) AND ISNULL(Status,'') NOT IN ('Cancelled')", conn);
                    txtTodaysAppoinment.Text = cmdToday.ExecuteScalar()?.ToString() ?? "0";

                    // Patients Seen (Completed today)
                    SqlCommand cmdSeen = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE Status='Completed' AND CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)", conn);
                    txtPatientSeen.Text = cmdSeen.ExecuteScalar()?.ToString() ?? "0";

                    // Treatment Done breakdown (completed today grouped by treatment)
                    SqlCommand cmdTreat = new SqlCommand(@"SELECT ISNULL(Treatment,'<unspecified>') AS Treatment, COUNT(*) AS C
                                                          FROM Appointments
                                                          WHERE Status='Completed' AND CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)
                                                          GROUP BY ISNULL(Treatment,'<unspecified>')", conn);
                    var sb = new StringBuilder();
                    using (var rdr = cmdTreat.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            sb.AppendLine($"{rdr["Treatment"].ToString()}: {rdr["C"].ToString()}");
                        }
                    }
                    txtTreatmentDone.Text = sb.Length > 0 ? sb.ToString().TrimEnd() : "0";

                    // Upcoming (future scheduled, not cancelled)
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

       

        private void btnMySched_Click_1(object sender, EventArgs e)
        {
            new My_Schedule().Show(this);
        }

        // Public method so other forms can request the dentist dashboard to refresh appointments
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
            catch
            {
                // swallow exceptions to avoid disrupting caller
            }
        }
    }
}