using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SmileTrack
{
    public partial class DentistDashboard : Form
    {
        private string currentDoctor;
        private readonly List<string> TreatmentTypes = new List<string>();
        private Timer refreshTimer;

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
            // Ensure UI basics
            txtTreatmentDone.ReadOnly = true;
            txtTreatmentDone.BorderStyle = BorderStyle.None;
            txtTreatmentDone.Font = new Font("Segoe UI", 18, FontStyle.Bold);

            // lbbell exists in designer; wire its click handler
            if (lbbell != null)
            {
                lbbell.Cursor = Cursors.Hand;
                lbbell.Click -= Lbbell_Click;
                lbbell.Click += Lbbell_Click;
            }

            // determine current doctor if not set
            if (string.IsNullOrWhiteSpace(currentDoctor))
            {
                var identityName = System.Threading.Thread.CurrentPrincipal?.Identity?.Name;
                if (!string.IsNullOrWhiteSpace(identityName))
                    currentDoctor = identityName;
                else
                    currentDoctor = Environment.UserName ?? "Unknown Dentist";
            }

            // Load data
            LoadAppointmentsFromDb();
            LoadChartData();
            PopulatePatientOverview();

            // optional periodic refresh for bell/appointments
            refreshTimer = new Timer { Interval = 60_000 }; // 1 minute
            refreshTimer.Tick += (s, ev) =>
            {
                LoadAppointmentsFromDb(); // safe light-weight reload
            };
            refreshTimer.Start();
        }

        #region Appointment model + manager

        public class Appointment
        {
            public DateTime Date { get; set; }
            public string Patient { get; set; }
            public string Treatment { get; set; }
            public string Status { get; set; }
            public string Doctor { get; set; }
        }

        public static class AppointmentManager
        {
            public static List<Appointment> Appointments = new List<Appointment>();
        }

        #endregion

        #region Database operations / loading

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
WHERE ISNULL(a.Dentist, '') = @doctor
ORDER BY a.AppointmentDateTime;";

                var param = new System.Data.SqlClient.SqlParameter("@doctor", currentDoctor);

                var dt = DatabaseHelper.ExecuteQuery(sql, param);

                var list = new List<Appointment>();
                foreach (System.Data.DataRow r in dt.Rows)
                {
                    DateTime date;
                    DateTime.TryParse(Convert.ToString(r["AppointmentDateTime"]), out date);

                    var appt = new Appointment
                    {
                        Date = date,
                        Patient = r["PatientName"]?.ToString() ?? string.Empty,
                        Treatment = r["Treatment"]?.ToString() ?? string.Empty,
                        Status = r["Status"]?.ToString() ?? string.Empty,
                        Doctor = r["Dentist"]?.ToString() ?? string.Empty
                    };
                    list.Add(appt);
                }

                AppointmentManager.Appointments = list;

                // Update UI
                LoadMySchedule();
                UpdateBellNotification();
                UpdateSummaryBoxes();
                PopulatePatientOverview();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region UI updates

        private void UpdateBellNotification()
        {
            var upcoming = AppointmentManager.Appointments
                .Where(a => a.Date >= DateTime.Now && string.Equals(a.Doctor, currentDoctor, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.Date)
                .FirstOrDefault();

            if (lbbell == null)
                return;

            if (upcoming != null)
            {
                lbbell.Text = $"🔔 Next: {upcoming.Date:hh:mm tt} - {upcoming.Patient}";
                lbbell.ForeColor = Color.DarkGoldenrod;
            }
            else
            {
                lbbell.Text = "🔔 No upcoming appointments";
                lbbell.ForeColor = Color.Gray;
            }
        }

        private void Lbbell_Click(object sender, EventArgs e)
        {
            var todays = AppointmentManager.Appointments
                .Where(a => a.Date.Date == DateTime.Today && string.Equals(a.Doctor, currentDoctor, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.Date)
                .Select(a => $"{a.Date:hh:mm tt} - {a.Patient} ({a.Treatment})")
                .ToList();

            string message = todays.Any()
                ? string.Join(Environment.NewLine, todays)
                : "No appointments today.";

            MessageBox.Show(message, $"Today's Appointments — {currentDoctor}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadMySchedule()
        {
            if (dgvSched == null)
                return;

            dgvSched.Rows.Clear();

            var myAppointments = AppointmentManager.Appointments
                .Where(a => a.Date.Date == DateTime.Today && string.Equals(a.Doctor, currentDoctor, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.Date)
                .ToList();

            foreach (var appt in myAppointments)
            {
                dgvSched.Rows.Add(appt.Date.ToString("hh:mm tt"), appt.Patient, appt.Treatment, appt.Status);
            }

            txtTodaysAppoinment.Text = myAppointments.Count.ToString();
        }

        private void UpdateSummaryBoxes()
        {
            // Today's appointments count already set in LoadMySchedule
            // Patients seen count: number of appointments today with Status = "Completed" (best effort)
            var seenCount = AppointmentManager.Appointments
                .Count(a => a.Date.Date == DateTime.Today && string.Equals(a.Doctor, currentDoctor, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(a.Status, "Completed", StringComparison.OrdinalIgnoreCase));
            txtPatientSeen.Text = seenCount.ToString();

            // Upcoming appointments (future scheduled)
            var upcomingCount = AppointmentManager.Appointments
                .Count(a => a.Date > DateTime.Now && string.Equals(a.Doctor, currentDoctor, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(a.Status, "Scheduled", StringComparison.OrdinalIgnoreCase));
            txtUpApp.Text = upcomingCount.ToString();

            // Treatment done count (from in-memory treatment types)
            txtTreatmentDone.Text = TreatmentTypes.Count.ToString();
        }

        private void PopulatePatientOverview()
        {
            // Simple population using last visits from Appointments grouped by patient
            if (dataGridView1 == null)
                return;

            dataGridView1.Rows.Clear();

            var grouped = AppointmentManager.Appointments
                .GroupBy(a => a.Patient)
                .Select(g => new
                {
                    Patient = g.Key,
                    LastVisit = g.Max(x => x.Date)
                })
                .OrderByDescending(x => x.LastVisit)
                .ToList();

            foreach (var item in grouped)
            {
                dataGridView1.Rows.Add(item.Patient, item.LastVisit.ToString("yyyy-MM-dd hh:mm tt"));
            }
        }

        private void LoadChartData()
        {
            if (chart1 == null)
                return;

            chart1.Series.Clear();
            var series = new Series("Treatments") { ChartType = SeriesChartType.Column, IsValueShownAsLabel = true };

            var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var type in TreatmentTypes)
            {
                if (string.IsNullOrWhiteSpace(type))
                    continue;
                if (counts.ContainsKey(type))
                    counts[type]++;
                else
                    counts[type] = 1;
            }

            foreach (var kvp in counts)
            {
                series.Points.AddXY(kvp.Key, kvp.Value);
            }

            // if no data, add a placeholder
            if (series.Points.Count == 0)
            {
                series.Points.AddXY("No Data", 0);
            }

            chart1.Series.Add(series);

            // ensure treatment count display
            txtTreatmentDone.Text = TreatmentTypes.Count.ToString();
        }

        #endregion

        #region Event handlers

        private void btnSave_Click(object sender, EventArgs e)
        {
            var treatmentType = cmbTreatmentType.SelectedItem?.ToString() ?? "Unknown";
            var notes = txtNotes?.Text ?? string.Empty;

            TreatmentTypes.Add(treatmentType);

            var record = $"{DateTime.Now:yyyy-MM-dd HH:mm} - {treatmentType} - {notes}";
            lbTreatment.Items.Add(record);

            LoadChartData();
            UpdateSummaryBoxes();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?",
                                 "Log‑out",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                refreshTimer?.Stop();
                this.Hide();
                var login = new LoginForm();
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

        private void btnTreatments_Click(object sender, EventArgs e)
        {
            pnlTreatmet.Visible = !pnlTreatmet.Visible;
        }

        private void cmbTreatmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

                private void btnMySched_Click(object sender, EventArgs e)
                {
                    try
                    {
                        var myScheduleForm = new My_Schedule();
                        // Show non-modal and set this form as owner so it centers relative to the dashboard
                        myScheduleForm.StartPosition = FormStartPosition.CenterParent;
                        myScheduleForm.Show(this);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error opening schedule: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            base.OnFormClosing(e);
        }

        // Public method so other forms can request a reload
        public void RefreshAppointments()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshAppointments));
                return;
            }

            LoadAppointmentsFromDb();
        }

        private void panelDentistDashboard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlTreatmet_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void TreatmentDone_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbTreatment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtNotes_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbbell_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dgvSched_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void txtTreatmentDone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPatientSeen_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUpApp_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
