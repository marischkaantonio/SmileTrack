using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SmileTrack
{
    public partial class DentistDashboard : Form
    {
        private string currentDoctor = "Dr. Margie";


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

        public static class AppointmentManager
        {
            public static List<Appointment> Appointments = new List<Appointment>();
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
            foreach (var item in counts)
            {


                txtTreatmentDone.Text = TreatmentTypes.Count.ToString();
                txtTreatmentDone.SelectAll();
                txtTreatmentDone.SelectionAlignment = HorizontalAlignment.Center;
                txtTreatmentDone.DeselectAll();
            }



        }

        private void btnMySched_Click(object sender, EventArgs e)
        {
            LoadMySchedule();
        }
        private void LoadMySchedule()
        {
            var myAppoinments = AppointmentManager.Appointments
                .Where(a => a.Date.Date == DateTime.Today && a.Doctor == currentDoctor)
                .ToList();

            dgvSched.DataSource = null;
            dgvSched.DataSource = myAppoinments;


        }

        private void label1_Click(object sender, EventArgs e)
        {

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
            // Automatically reload today's appointments for this dentist
            LoadMySchedule();

            // Optional: visually highlight if there are appointments today
            if (int.TryParse(txtTodaysAppoinment.Text, out int count))
            {
                txtTodaysAppoinment.ForeColor = count > 0 ? Color.Green : Color.Red;
                txtTodaysAppoinment.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            }
        }
    }
}
    
    


