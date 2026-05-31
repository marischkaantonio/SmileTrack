using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=SmileTrackDB;Integrated Security=True");


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
            dgvAppointments.Rows.Clear();

            // Ensure columns exist
            if (dgvAppointments.Columns.Count == 0)
            {
                dgvAppointments.Columns.Add("Time", "Time");
                dgvAppointments.Columns.Add("PatientName", "Patient Name");
                dgvAppointments.Columns.Add("Dentist", "Dentist");
                dgvAppointments.Columns.Add("Treatment", "Treatment");
                dgvAppointments.Columns.Add("Status", "Status");
            }

            // Filter appointments for today
            var todaysAppointments = AppointmentManager.Appointments
                .Where(a => a.Date.Date == DateTime.Today)
                .ToList();

            // Populate DataGridView
            foreach (var appt in todaysAppointments)
            {
                dgvAppointments.Rows.Add(
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
           

        }
        private void LoadDashboard()
        {
            // Appointments today
            SqlDataAdapter daAppointments = new SqlDataAdapter(
                "SELECT DateTime, Firstname+' '+Lastname AS PatientName, Dentist, TreatmentType " +
                "FROM Appointments INNER JOIN Patients ON Appointments.PatientID=Patients.PatientID " +
                "WHERE Status='Scheduled' AND CAST(DateTime AS DATE)=CAST(GETDATE() AS DATE)", con);
            DataTable dtAppointments = new DataTable();
            daAppointments.Fill(dtAppointments);
            dgvAppointments.DataSource = dtAppointments;

            // Walk-ins today
            SqlDataAdapter daWalkins = new SqlDataAdapter(
                "SELECT Firstname+' '+Lastname AS PatientName, DateTime " +
                "FROM Appointments INNER JOIN Patients ON Appointments.PatientID=Patients.PatientID " +
                "WHERE VisitType='Walk-in' AND CAST(DateTime AS DATE)=CAST(GETDATE() AS DATE)", con);
            DataTable dtWalkins = new DataTable();
            daWalkins.Fill(dtWalkins);
            dgvWalkIn.DataSource = dtWalkins;

            // Reminders
            SqlDataAdapter daReminders = new SqlDataAdapter(
                "SELECT DateTime, Firstname+' '+Lastname AS PatientName " +
                "FROM Appointments INNER JOIN Patients ON Appointments.PatientID=Patients.PatientID " +
                "WHERE Status='Scheduled' AND DateTime>GETDATE()", con);
            DataTable dtReminders = new DataTable();
            daReminders.Fill(dtReminders);
            dgvReminders.DataSource = dtReminders;
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
        
            SqlDataAdapter daReminders = new SqlDataAdapter(
                "SELECT DateTime, Firstname+' '+Lastname AS PatientName " +
                "FROM Appointments INNER JOIN Patients ON Appointments.PatientID=Patients.PatientID " +
                "WHERE Status='Scheduled' AND DateTime>GETDATE()", con);

            DataTable dtReminders = new DataTable();
            daReminders.Fill(dtReminders);
            dgvReminders.DataSource = dtReminders;
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
                DataGridViewRow row = dgvAppointments.Rows[e.RowIndex];
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

      
       

        private void btnAddAppointment_Click_2(object sender, EventArgs e)
        {
            int rowNumber = dgvAppointments.Rows.Count + 1;

            // Generate random patient and dentist
            string randomPatient = AppointmentGenerator.GetRandomPatientName();
            string randomDentist = AppointmentGenerator.GetRandomDentist();

            // Add appointment to today's list
            dgvAppointments.Rows.Add(
                DateTime.Now.ToString("hh:mm tt"),  // Time
                randomPatient,                      // Random Patient Name
                randomDentist,                      // Random Dentist
                AppointmentGenerator.GetRandomTreatment(),  // Treatment
                "Scheduled"                         // Status
            );

            // Also add to Walk-in queue if needed
            int walkInRow = dgvWalkIn.Rows.Count + 1;
            dgvWalkIn.Rows.Add(
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

      

        private void btnPatients_Click(object sender, EventArgs e)
        {
            Patient_Info_Appoinment patientForm = new Patient_Info_Appoinment();
            patientForm.Show();
        }
    }
}




      






