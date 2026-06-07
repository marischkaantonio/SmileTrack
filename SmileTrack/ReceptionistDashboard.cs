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
    public partial class frmReceptionistDashboard : Form
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False";

        public frmReceptionistDashboard()
        {
            InitializeComponent();

        }
            public void LoadDashboard()
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // Appointments today
                    SqlDataAdapter daAppointments = new SqlDataAdapter(
                        "SELECT AppointmentDateTime, FirstName+' '+LastName AS PatientName, Dentist, Treatment " +
                        "FROM Appointments INNER JOIN Patients ON Appointments.PatientID=Patients.PatientID " +
                        "WHERE Status='Scheduled' AND CAST(AppointmentDateTime AS DATE)=CAST(GETDATE() AS DATE)", con);
                    DataTable dtAppointments = new DataTable();
                    daAppointments.Fill(dtAppointments);
                    dgvAppointments.DataSource = dtAppointments;

                    // Walk-ins today
                    SqlDataAdapter daWalkins = new SqlDataAdapter(
                        "SELECT FirstName+' '+LastName AS PatientName, AppointmentDateTime " +
                        "FROM Appointments INNER JOIN Patients ON Appointments.PatientID=Patients.PatientID " +
                        "WHERE VisitType='Walk-in' AND CAST(AppointmentDateTime AS DATE)=CAST(GETDATE() AS DATE)", con);
                    DataTable dtWalkins = new DataTable();
                    daWalkins.Fill(dtWalkins);
                    dgvWalkIn.DataSource = dtWalkins;

                    // Reminders (future appointments)
                    SqlDataAdapter daReminders = new SqlDataAdapter(
                        "SELECT AppointmentDateTime, FirstName+' '+LastName AS PatientName " +
                        "FROM Appointments INNER JOIN Patients ON Appointments.PatientID=Patients.PatientID " +
                        "WHERE Status='Scheduled' AND AppointmentDateTime > GETDATE()", con);
                    DataTable dtReminders = new DataTable();
                    daReminders.Fill(dtReminders);
                    dgvReminders.DataSource = dtReminders;
                }
            }

         
            private void frmReceptionistDashboard_Load(object sender, EventArgs e)
            {
                LoadDashboard();
               
            }

        private void btnPatients_Click(object sender, EventArgs e)
        {

            Patient_Info_Appoinment patientForm = new Patient_Info_Appoinment();
            patientForm.Show();
        }

        private void btnBillings_Click(object sender, EventArgs e)
        {

        }
    }
}













