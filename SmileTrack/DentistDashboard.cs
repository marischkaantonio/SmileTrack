using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SmileTrack
{
    public partial class DentistDashboard : Form
    {
       
        private string currentDoctor;
        private Label lblBell;  

        List<string> TreatmentTypes = new List<string>();

        public DentistDashboard()
        {
            InitializeComponent();
        }

        public DentistDashboard(string doctorName) : this()
        {
            SetCurrentDoctor(doctorName);
        }

       
        public void SetCurrentDoctor(string doctorName)
        {
            currentDoctor = (doctorName ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(currentDoctor))
            {
              
                currentDoctor = Environment.UserName ?? string.Empty;
            }

            this.Text = $"Dentist Dashboard - {currentDoctor}";
        }

        private void DentistDashboard_Load(object sender, EventArgs e)
        {
            txtTreatmentDone.ReadOnly = true;
            txtTreatmentDone.BorderStyle = BorderStyle.None;
            txtTreatmentDone.Font = new Font("Segoe UI", 18, FontStyle.Bold);

            InitializeBell();

            if (string.IsNullOrWhiteSpace(currentDoctor))
            {
            
                var identityName = System.Threading.Thread.CurrentPrincipal?.Identity?.Name;
                if (!string.IsNullOrWhiteSpace(identityName))
                    currentDoctor = identityName;
                else
                    currentDoctor = Environment.UserName ?? "Unknown Dentist";
            }

           
            LoadAppointmentsFromDb();

            UpdateBellNotification();  

            LoadChartData();
        }

        public class Appointment
        {
            public DateTime Date { get; set; }
            public string Patient { get; set; }
            public string Treatment { get; set; }
            public string Status { get; set; }
            public string Doctor { get; set; }
        }

        // Appointment manager (shared list)
        public static class AppointmentManager
        {
            public static List<Appointment> Appointments = new List<Appointment>();
        }

        // Initialize the bell label
        private void InitializeBell()
        {
            lblBell = new Label();
            lblBell.Text = "🔔 No upcoming appointments";
            lblBell.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblBell.Cursor = Cursors.Hand;
            lblBell.Location = new Point(10, 10);
            lblBell.AutoSize = true;

            lblBell.Click += LblBell_Click;
            this.Controls.Add(lblBell);
        }

        // Load appointments from the database for the current doctor
        private void LoadAppointmentsFromDb()
        {
            try
            {
                DatabaseHelper.EnsureDatabaseAndTables();

                const string sql = @"
SELECT a.AppointmentDateTime AS AppointmentDateTime,
       ISNULL(p.FirstName, '') + ' ' + ISNULL(p.LastName, '') AS PatientName,
       a.Treatment,
       a.Status,
       a.Dentist
FROM Appointments a
LEFT JOIN Patients p ON a.PatientID = p.PatientID
WHERE a.Dentist = @doctor
ORDER BY a.AppointmentDateTime;";


                var param = new SqlParameter("@doctor", currentDoctor);

                DataTable dt = DatabaseHelper.ExecuteQuery(sql, param);

                var list = new List<Appointment>();
                foreach (DataRow r in dt.Rows)
                {
                    var appt = new Appointment
                    {
                        Date = Convert.ToDateTime(r["AppointmentDateTime"]),
                        Patient = r["PatientName"]?.ToString() ?? string.Empty,
                        Treatment = r["Treatment"]?.ToString() ?? string.Empty,
                        Status = r["Status"]?.ToString() ?? string.Empty,
                        Doctor = r["Dentist"]?.ToString() ?? string.Empty
                    };
                    list.Add(appt);
                }

                // Replace in-memory list and refresh UI
                AppointmentManager.Appointments = list;
                LoadMySchedule();
                UpdateBellNotification();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Update bell with next appointment
        private void UpdateBellNotification()
        {
            var upcoming = AppointmentManager.Appointments
                .Where(a => a.Date >= DateTime.Now && string.Equals(a.Doctor, currentDoctor, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.Date)
                .FirstOrDefault();

            if (upcoming != null)
            {
                lblBell.Text = $"🔔 Next: {upcoming.Date:hh:mm tt} - {upcoming.Patient}";
            }
            else
            {
                lblBell.Text = "🔔 No upcoming appointments";
            }
        }

        // Bell click handler shows today's appointments
        private void LblBell_Click(object sender, EventArgs e)
        {
            var todays = AppointmentManager.Appointments
                .Where(a => a.Date.Date == DateTime.Today && string.Equals(a.Doctor, currentDoctor, StringComparison.OrdinalIgnoreCase))
                .Select(a => $"{a.Date:hh:mm tt} - {a.Patient}")
                .ToList();

            string message = todays.Any()
                ? string.Join(Environment.NewLine, todays)
                : "No appointments today.";

            MessageBox.Show(message, $"Today's Appointments — {currentDoctor}");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string treatmentType = cmbTreatmentType.SelectedItem?.ToString() ?? "Unknown";
            string notes = txtNotes.Text ?? "";

            TreatmentTypes.Add(treatmentType);

            string record = $"{DateTime.Now}: {treatmentType} - {notes}";
            lbTreatment.Items.Add(record);

            chart1.Invalidate();
            chart1.Update();
            chart1.Refresh();

            LoadChartData();

            txtTreatmentDone.Invalidate();
            txtTreatmentDone.Update();
            txtTreatmentDone.Refresh();
        }

        private void LoadChartData()
        {
            chart1.Series.Clear();
            var series = chart1.Series.Add("Treatments");
            series.ChartType = SeriesChartType.Pie;
            series.IsValueShownAsLabel = true;

            Dictionary<string, int> counts = new Dictionary<string, int>();
            foreach (string type in TreatmentTypes)
            {
                if (counts.ContainsKey(type))
                    counts[type]++;
                else
                    counts[type] = 1;
            }

            foreach (var item in counts)
            {
                series.Points.AddXY(item.Key, item.Value);
            }

            txtTreatmentDone.Text = TreatmentTypes.Count.ToString();
            txtTreatmentDone.SelectAll();
            txtTreatmentDone.SelectionAlignment = HorizontalAlignment.Center;
            txtTreatmentDone.DeselectAll();
        }

        private void LoadMySchedule()
        {
            var myAppointments = AppointmentManager.Appointments
                .Where(a => a.Date.Date == DateTime.Today && string.Equals(a.Doctor, currentDoctor, StringComparison.OrdinalIgnoreCase))
                .ToList();

            dgvSched.DataSource = null;
            dgvSched.DataSource = myAppointments;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?",
                                 "Log‑out",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm login = new LoginForm();
                login.Show();
            }
        }

        private void txtTodaysAppoinment_TextChanged(object sender, EventArgs e)
        {
            LoadMySchedule();

            if (int.TryParse(txtTodaysAppoinment.Text, out int count))
            {
                txtTodaysAppoinment.ForeColor = count > 0 ? Color.Green : Color.Red;
                txtTodaysAppoinment.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            }
        }

        private void btnTreatments_Click(object sender, EventArgs e) { }
        private void cmbTreatmentType_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
