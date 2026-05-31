using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SmileTrack
{
    public partial class Patient_Info_Appoinment : Form
    {
        SqlConnection con = new SqlConnection(
    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False");

        public Patient_Info_Appoinment()
        {
            InitializeComponent();

            // Gender Group
            GroupBox grpGender = new GroupBox();
            grpGender.Text = "Gender";
            grpGender.Location = new Point(20, 20);
            grpGender.Size = new Size(200, 60);

            RadioButton rbMale = new RadioButton();
            rbMale.Text = "Male";
            rbMale.Location = new Point(10, 20);

            RadioButton rbFemale = new RadioButton();
            rbFemale.Text = "Female";
            rbFemale.Location = new Point(100, 20);

            grpGender.Controls.Add(rbMale);
            grpGender.Controls.Add(rbFemale);
            this.Controls.Add(grpGender);

            // Visit Type Group
            GroupBox grpVisitType = new GroupBox();
            grpVisitType.Text = "Visit Type";
            grpVisitType.Location = new Point(20, 100);
            grpVisitType.Size = new Size(200, 60);

            RadioButton rbWalkIn = new RadioButton();
            rbWalkIn.Text = "Walk-In";
            rbWalkIn.Location = new Point(10, 20);

            RadioButton rbAppointment = new RadioButton();
            rbAppointment.Text = "Appointment";
            rbAppointment.Location = new Point(100, 20);

            grpVisitType.Controls.Add(rbWalkIn);
            grpVisitType.Controls.Add(rbAppointment);
            this.Controls.Add(grpVisitType);
        }

        

        private void Patient_Info_Appoinment_Load(object sender, EventArgs e)
        {

        }



        private void btnSave_Click(object sender, EventArgs e)
        {

            using (SqlConnection con = new SqlConnection(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"))
            {
                con.Open();

                // Insert Patient
                SqlCommand cmdPatient = new SqlCommand(
                    @"INSERT INTO Patients (FirstName, LastName, BirthDate, Age, Gender, ContactNo, Email) 
              VALUES (@fname, @lname, @bdate, @age, @gender, @contact, @email);
              SELECT SCOPE_IDENTITY();", con);

                cmdPatient.Parameters.AddWithValue("@fname", txtFname.Text);
                cmdPatient.Parameters.AddWithValue("@lname", txtLname.Text);
                cmdPatient.Parameters.AddWithValue("@bdate", dtpBirthdate.Value.Date);
                cmdPatient.Parameters.AddWithValue("@age", nudAge.Value);
                cmdPatient.Parameters.AddWithValue("@gender", rbMale.Checked ? "Male" : "Female");
                cmdPatient.Parameters.AddWithValue("@contact", txtContact.Text);
                cmdPatient.Parameters.AddWithValue("@email", txtEmail.Text);

                int patientID = Convert.ToInt32(cmdPatient.ExecuteScalar());

                // Insert Appointment
                SqlCommand cmdAppt = new SqlCommand(
                    @"INSERT INTO Appointments (PatientID, DentistName, Treatment, Status, AppointmentDateTime, Notes) 
              VALUES (@pid, @dentist, @treatment, @status, @datetime, @notes)", con);

                cmdAppt.Parameters.AddWithValue("@pid", patientID);
                cmdAppt.Parameters.AddWithValue("@dentist", cmbDentist.SelectedItem?.ToString() ?? "");
                cmdAppt.Parameters.AddWithValue("@treatment", cmbTreatmentType.SelectedItem?.ToString() ?? "");
                cmdAppt.Parameters.AddWithValue("@status", cmbStatus.SelectedItem?.ToString() ?? "");
                cmdAppt.Parameters.AddWithValue("@datetime", dtAppoinment.Value);
                cmdAppt.Parameters.AddWithValue("@notes", richtxtNotes.Text);

                cmdAppt.ExecuteNonQuery();

                MessageBox.Show("Patient and appointment saved successfully!", "Success");
            }


            frmPatientRecords recordsForm = new frmPatientRecords();
            recordsForm.Show();
        }


        private void txtPatientID_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Patients WHERE PatientID=@id", con);
            cmd.Parameters.AddWithValue("@id", txtPatientID.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                txtFname.Text = dt.Rows[0]["Firstname"].ToString();
                txtLname.Text = dt.Rows[0]["Lastname"].ToString();
                txtContact.Text = dt.Rows[0]["ContactNo"].ToString();
                txtEmail.Text = dt.Rows[0]["Email"].ToString();
            }
        }



        private void txtFname_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFname.Text))
                errorProvider1.SetError(txtFname, "First name required");
            else
                errorProvider1.Clear();
        }

        private void txtLname_TextChanged(object sender, EventArgs e)
        {

        }
        private void dtpBirthdate_ValueChanged(object sender, EventArgs e)
        {
            DateTime birthdate = dtpBirthdate.Value;
            DateTime today = DateTime.Today;

            int age = today.Year - birthdate.Year;

            // Adjust if birthday hasn’t occurred yet this year
            if (birthdate.Date > today.AddYears(-age))
            {
                age--;
            }

            lblAge.Text = "Age: " + age.ToString();
            nudAge.Value = age; // optional: sync with NumericUpDown
        }
        private void nudAge_ValueChanged(object sender, EventArgs e)
        {
            nudAge.Text = "Age: " + nudAge.Value.ToString();
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked)
                lblGender.Text = "Gender: Male";
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked)
                lblGender.Text = "Gender: Female";
        }



        private void txtContact_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtContact.Text, @"^\d*$"))
            {
                errorProvider1.SetError(txtContact, "Numbers only");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void cmbDentist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDentist = cmbDentist.SelectedItem.ToString();
            cmbDentist.Text = "Selected Dentist: " + selectedDentist;

            // Example: load dentist schedule
            SqlCommand cmd = new SqlCommand("SELECT * FROM Appointments WHERE Dentist=@dentist", con);
            cmd.Parameters.AddWithValue("@dentist", selectedDentist);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

        }

        private void cmbTreatmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTreatmentType.SelectedItem != null)
            {
                string treatment = cmbTreatmentType.SelectedItem.ToString();

                using (SqlConnection con = new SqlConnection(
                    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Cost, Description FROM Treatments WHERE TreatmentName=@treatment", con);
                    cmd.Parameters.AddWithValue("@treatment", treatment);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader.Read())
                        {
                            cmbTreatmentType.Text = treatment;

                        }
                    }
                }
            }
        }




        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFname.Clear();
            txtLname.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtAddress.Clear();


            // NumericUpDown
            nudAge.Value = 0;

            // DateTimePickers
            dtpBirthdate.Value = DateTime.Today;


            // RadioButtons
            rbMale.Checked = false;
            rbFemale.Checked = false;

            // ComboBoxes
            cmbTreatmentType.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            cmbDentist.SelectedIndex = -1;

            // Notes TextBox (if you have one)
            richtxtNotes.Clear();


        }

        private void btnSearchPatient_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(
     @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT TOP 1 * FROM Patients WHERE PatientID = @pid", con);
                cmd.Parameters.AddWithValue("@pid", txtPatientID.Text); // <-- use PatientID textbox

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Auto-fill patient info into form fields
                    txtFname.Text = reader["FirstName"].ToString();
                    txtLname.Text = reader["LastName"].ToString();
                    txtContact.Text = reader["Contact"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtAddress.Text = reader["Address"].ToString();

                    // Birthdate + auto-calculate age
                    DateTime birthdate = Convert.ToDateTime(reader["Birthdate"]);
                    dtpBirthdate.Value = birthdate;
                    int age = DateTime.Today.Year - birthdate.Year;
                    if (birthdate.Date > DateTime.Today.AddYears(-age)) age--;
                    nudAge.Value = age;
                }
                else
                {
                    MessageBox.Show("No patient found with that PatientID.", "Search",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
             using (SqlConnection con = new SqlConnection(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Appointments 
              SET DentistName=@dentist, Treatment=@treatment, Status=@status, 
                  AppointmentDateTime=@datetime, Notes=@notes 
              WHERE AppointmentID=@apptid", con);

                cmd.Parameters.AddWithValue("@dentist", cmbDentist.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@treatment", cmbTreatmentType.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@datetime", dtAppoinment.Value);
                cmd.Parameters.AddWithValue("@notes", richtxtNotes.Text);
                cmd.Parameters.AddWithValue("@apptid", txtAppointmentID.Text); // hidden field or selected row

                cmd.ExecuteNonQuery();
                MessageBox.Show("Appointment updated successfully!", "Success");
            }

            frmPatientRecords recordsForm = new frmPatientRecords();
            recordsForm.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();


            frmReceptionistDashboard receptionistForm = new frmReceptionistDashboard();
            receptionistForm.Show();
        }

        private void grpGender_Enter(object sender, EventArgs e)
        {
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }
    }
}




