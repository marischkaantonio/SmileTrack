using System;
using System.Data;
using System.Data.SqlClient;
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
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT a.AppointmentDateTime AS [Time], 
                                     p.FirstName + ' ' + p.LastName AS [Patient], 
                                     a.Treatment, a.Status 
                                     FROM Appointments a 
                                     INNER JOIN Patients p ON a.PatientID = p.PatientID";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // I-update ang GridView
                    dgvSched.DataSource = dt;
                }
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

                    // Today's Appointments
                    SqlCommand cmdToday = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE CAST(AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)", conn);
                    txtTodaysAppoinment.Text = cmdToday.ExecuteScalar().ToString();

                    // Patients Seen (Completed)
                    SqlCommand cmdSeen = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE Status='Completed'", conn);
                    txtPatientSeen.Text = cmdSeen.ExecuteScalar().ToString();

                    // Treatment Done
                    txtTreatmentDone.Text = cmdSeen.ExecuteScalar().ToString();

                    // Upcoming
                    SqlCommand cmdUpcoming = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE AppointmentDateTime > GETDATE()", conn);
                    txtUpApp.Text = cmdUpcoming.ExecuteScalar().ToString();
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

        private void btnMySched_Click(object sender, EventArgs e)
        {
            new My_Schedule().Show(this);
        }
    }
}