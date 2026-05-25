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

        private void btnAddWalkIn_Click(object sender, EventArgs e)
        {
            int rowNumber = dataGridView2.Rows.Count + 1;

            dataGridView2.Rows.Add(
                rowNumber,
                "Walk-in Patient " + rowNumber,   // placeholder name
                DateTime.Now.ToString("hh:mm tt"),
                "Waiting"                         // default status
            );
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
            if (dgvTA.CurrentRow != null)
            {
                string patientName = dgvTA.CurrentRow.Cells["PatientName"].Value?.ToString();

                // Example: mark appointment as Completed
                AppointmentUpdater.UpdateAppointmentStatus(patientName, "Completed");
            }

            // Refresh grid
            AppointmentUpdater.RefreshTodaysAppointments(dgvTA);

            // Refresh summary cards
            LoadSummaryCards();

            MessageBox.Show("Today's appointments updated successfully.", "Update");

            foreach (Form form in Application.OpenForms)
            {
                if (form is ReceptionistDashboard receptionistDash)
                {
                    receptionistDash.LoadTodaysAppointments();
                    receptionistDash.LoadSummaryCards();
                }
            }
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


    }
}




      






