using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SmileTrack
{
    public partial class DentistDashboard : Form
    {
        private string currentDoctor = "Dr. Margie";
        private Label lblBell;   // 🔔 Notification bell

        List<string> TreatmentTypes = new List<string>();

        public DentistDashboard()
        {
            InitializeComponent();
        }

        private void DentistDashboard_Load(object sender, EventArgs e)
        {
            txtTreatmentDone.ReadOnly = true;
            txtTreatmentDone.BorderStyle = BorderStyle.None;
            txtTreatmentDone.Font = new Font("Segoe UI", 18, FontStyle.Bold);

            InitializeBell();
            UpdateBellNotification();   // show initial upcoming appointment

            LoadChartData();
        }

        // 🟦 Appointment model
        public class Appointment
        {
            public DateTime Date { get; set; }
            public string Patient { get; set; }
            public string Treatment { get; set; }
            public string Status { get; set; }
            public string Doctor { get; set; }
        }

        // 🟩 Appointment manager (shared list)
        public static class AppointmentManager
        {
            public static List<Appointment> Appointments = new List<Appointment>();
        }

        // 🟦 Initialize the bell label
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

        // 🟩 Update bell with next appointment
        private void UpdateBellNotification()
        {
            var upcoming = AppointmentManager.Appointments
                .Where(a => a.Date >= DateTime.Now && a.Doctor == currentDoctor)
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

        // 🟦 Bell click handler
        private void LblBell_Click(object sender, EventArgs e)
        {
            var todays = AppointmentManager.Appointments
                .Where(a => a.Date.Date == DateTime.Today && a.Doctor == currentDoctor)
                .Select(a => $"{a.Date:hh:mm tt} - {a.Patient}")
                .ToList();

            string message = todays.Any()
                ? string.Join(Environment.NewLine, todays)
                : "No appointments today.";

            MessageBox.Show(message, "Today's Appointments");
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

        private void btnMySched_Click(object sender, EventArgs e)
        {
            AppointmentUpdater.RefreshTodaysAppointments(dgvSched);
        }

        private void LoadMySchedule()
        {
            var myAppointments = AppointmentManager.Appointments
                .Where(a => a.Date.Date == DateTime.Today && a.Doctor == currentDoctor)
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
