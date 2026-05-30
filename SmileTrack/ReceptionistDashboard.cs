using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SmileTrack.DentistDashboard;

namespace SmileTrack
{
    public partial class ReceptionistDashboard : Form
    {


        public ReceptionistDashboard()
        {
            InitializeComponent();
        }
        public static class AppointmentManager
        {
            public static List<Appointment> Appointments = new List<Appointment>();
        }

        public class Appointment
        {
            public DateTime Date { get; set; }
            public string Status { get; set; }

            public string PatientName { get; set; }
            public string Dentist { get; set; }
            public string Treatment { get; set; }

        }

        public static class PatientManager
        {
            public static List<Patient> Patients = new List<Patient>();
        }

        public class Patient
        {
            public DateTime RegistrationDate { get; set; }
        }

        public static class ReminderManager
        {
            public static List<Reminder> Reminders = new List<Reminder>();
        }

        public class Reminder
        {
            public DateTime Date { get; set; }
        }
        public static class WalkInQeueManager
        {
            public static List<WalkInPatient> Reminders { get; set; } = new List<WalkInPatient>();
        }
        public class WalkInPatient
        {
            public DateTime Date { get; set; }
            public string Status { get; set; }
        }

        private void LoadTodaysAppointments()
        {
            // Clear existing rows
            dgvTA.Rows.Clear();

            // Ensure columns exist
            if (dgvTA.Columns.Count == 0)
            {
                dgvTA.Columns.Add("Time", "Time");
                dgvTA.Columns.Add("PatientName", "Patient Name");
                dgvTA.Columns.Add("Dentist", "Dentist");
                dgvTA.Columns.Add("Treatment", "Treatment");
                dgvTA.Columns.Add("Status", "Status");
            }

            // Filter appointments for today
            var todaysAppointments = AppointmentManager.Appointments
                .Where(a => a.Date.Date == DateTime.Today)
                .ToList();

            // Populate DataGridView
            foreach (var appt in todaysAppointments)
            {
                dgvTA.Rows.Add(
                    appt.Date.ToString("hh:mm tt"),
                    appt.PatientName,      // Add these properties to Appointment class if not yet present
                    appt.Dentist,
                    appt.Treatment,
                    appt.Status
                );
            }

            // Optional: show message if none found
            if (todaysAppointments.Count == 0)
            {
                MessageBox.Show("No appointments scheduled for today.", "Information");
            }
        }


        public string LoggedInUser { get; set; }
        private void ReceptionistDashboard_Load(object sender, EventArgs e)
        {
            LoadSummaryCards();

            lblWelcome.Text = $"Welcome, {LoggedInUser}!";
            LoadTodaysAppointments();

        }


        private void LoadSummaryCards()
        {
            int todaysCount = AppointmentManager.Appointments
                .Count(a => a.Date.Date == DateTime.Today);

            lblTodaysAppoinment.Text = todaysCount.ToString();

            // Update DentistDashboard textbox if open
            foreach (Form form in Application.OpenForms)
            {
                if (form is DentistDashboard dentistDash)
                {
                    dentistDash.txtTodaysAppoinment.Text = todaysCount.ToString();
                }
            }

            lblNewPatient.Text = PatientManager.Patients
                .Count(p => p.RegistrationDate.Date == DateTime.Today).ToString();

            lblPendingConfirmation.Text = AppointmentManager.Appointments
                .Count(a => a.Status == "Pending").ToString();

            lblReminders.Text = ReminderManager.Reminders
                .Count(r => r.Date.Date >= DateTime.Today).ToString();

            lblWalkin.Text = WalkInQeueManager.Reminders
                .Count(r => r.Date.Date >= DateTime.Today).ToString();
        }

        private void LoadReminders()
        {
            if (!dataGridView2.Columns.Contains("Time")) return;

            var todayReminders = dataGridView2.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.Cells["Time"].Value != null &&
                            DateTime.Parse(r.Cells["Time"].Value.ToString()).Date == DateTime.Today)
                .Select(r => new
                {
                    Date = DateTime.Parse(r.Cells["Time"].Value.ToString()).ToShortDateString(),
                    PatientName = r.Cells["PatientName"].Value?.ToString(),
                    Time = DateTime.Parse(r.Cells["Time"].Value.ToString()).ToShortTimeString()
                })
                .ToList();

            dgvReminders.DataSource = null;
            dgvReminders.DataSource = todayReminders;
        }




        private void btnPatients_Click(object sender, EventArgs e)
        {
            PatientForm patientForm = new PatientForm();
            patientForm.Show();
        }

        private void btnBillings_Click(object sender, EventArgs e)
        {
            this.Hide();

            BillingForm billing = new BillingForm();
            billing.ShowDialog();

            this.Show();
        }

        private void btnAppoinment_Click(object sender, EventArgs e)
        {

        }

        


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.Columns.Add("No", "No.");
            dataGridView2.Columns.Add("PatientName", "Patient Name");
            dataGridView2.Columns.Add("TimeIn", "Time-in");
            dataGridView2.Columns.Add("Status", "Status");
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?",
                                 "Log-out",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                this.Hide();


                LoginForm login = new LoginForm();
                login.Show();


            }
        }

        private void btnReminders_Click(object sender, EventArgs e)
        {
            Reminders Reminders = new Reminders();
            Reminders.Show();
        }

        private void dgvTA_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTA.Rows[e.RowIndex];
                string patientName = row.Cells["PatientName"].Value?.ToString();
                string dentist = row.Cells["Dentist"].Value?.ToString();
                string treatment = row.Cells["Treatment"].Value?.ToString();
                string status = row.Cells["Status"].Value?.ToString();

                MessageBox.Show(
                    $"Patient: {patientName}\nDentist: {dentist}\nTreatment: {treatment}\nStatus: {status}",
                    "Appointment Details"
                );
            }
        }


        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // C#
            var row = dgvTA.CurrentRow;
            if (row == null)
            {
                MessageBox.Show("No appointment selected.", "Update");
                return;
            }

            string patientName = null;

            // 1) Preferred: check DataBoundItem
            if (row.DataBoundItem is System.Data.DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("PatientName"))
                    patientName = drv["PatientName"]?.ToString();
            }
            else if (row.DataBoundItem != null)
            {
                var prop = row.DataBoundItem.GetType().GetProperty("PatientName");
                if (prop != null)
                    patientName = prop.GetValue(row.DataBoundItem)?.ToString();
            }

            // 2) Fallback: find a column by Name/DataPropertyName/HeaderText
            if (string.IsNullOrEmpty(patientName))
            {
                var col = dgvTA.Columns
                    .Cast<DataGridViewColumn>()
                    .FirstOrDefault(c =>
                        string.Equals(c.Name, "PatientName", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(c.DataPropertyName, "PatientName", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(c.HeaderText, "PatientName", StringComparison.OrdinalIgnoreCase));

                if (col != null)
                    patientName = row.Cells[col.Index].Value?.ToString();
            }

            if (string.IsNullOrEmpty(patientName))
            {
                MessageBox.Show("Could not determine patient name. Check grid columns.", "Update");
                return;
            }

            // proceed with update...
            AppointmentUpdater.UpdateAppointmentStatus(patientName, "Completed");

            // Perform update once
            AppointmentUpdater.UpdateAppointmentStatus(patientName, "Completed");

            // Refresh UI
            AppointmentUpdater.RefreshTodaysAppointments(dgvTA);
            LoadSummaryCards();
            MessageBox.Show("Today's appointment updated successfully.", "Update");
        }
        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            AppointmentUpdater.AddAppointment(
                DateTime.Now,
                "Juan Dela Cruz",
                "Dr. Margie",
                "Cleaning",
                "Pending"
            );

            AppointmentUpdater.RefreshTodaysAppointments(dgvTA);
            LoadSummaryCards();
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void btnWalkIn_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void btnAddAppointment_Click_1(object sender, EventArgs e)
        {

        }

        private void btnAddAppointment_Click_2(object sender, EventArgs e)
        {
            int rowNumber = dgvTA.Rows.Count + 1;

            // Generate random patient and dentist
            string randomPatient = AppointmentGenerator.GetRandomPatientName();
            string randomDentist = AppointmentGenerator.GetRandomDentist();

            // Add appointment to today's list
            dgvTA.Rows.Add(
                DateTime.Now.ToString("hh:mm tt"),  // Time
                randomPatient,                      // Random Patient Name
                randomDentist,                      // Random Dentist
                AppointmentGenerator.GetRandomTreatment(),  // Treatment
                "Scheduled"                         // Status
            );

            // Also add to Walk-in queue if needed
            int walkInRow = dataGridView2.Rows.Count + 1;
            dataGridView2.Rows.Add(
                walkInRow,
                randomPatient,
                DateTime.Now.ToString("hh:mm tt"),
                "Waiting"
            );

            // Refresh reminders if grid exists
            if (dgvReminders != null)
            {
                LoadReminders();
            }
        }


        private void dgvReminders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

    }
}




      






