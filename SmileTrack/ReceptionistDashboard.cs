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

        public string LoggedInUser { get; set; }
        private void ReceptionistDashboard_Load(object sender, EventArgs e)
        {
            LoadSummaryCards();

            lblWelcome.Text = $"Welcome, {LoggedInUser}!";
        }


        private void LoadSummaryCards()
        {
            lblTodaysAppoinment.Text = AppointmentManager.Appointments
                 .Count(a => a.Date.Date == DateTime.Today).ToString();

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
    }

        }





