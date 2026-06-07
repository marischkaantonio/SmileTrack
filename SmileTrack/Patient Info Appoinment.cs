using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SmileTrack
{
    public partial class Patient_Info_Appoinment : Form

    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False";
        private SqlConnection con;

        public Patient_Info_Appoinment()
        {
            InitializeComponent();
            // Optionally initialize any runtime-only UI state
            rbWalkin.Checked = false;
            rbAppointment.Checked = false;
            rbMale.Checked = false;
            rbFemale.Checked = false;            // Optionally initialize any runtime-only UI state
            rbWalkin.Checked = false;
            rbAppointment.Checked = false;
            rbMale.Checked = false;
            rbFemale.Checked = false;           // Optionally initialize any runtime-only UI state
            rbWalkin.Checked = false;
            rbAppointment.Checked = false;
            rbMale.Checked = false;
            rbFemale.Checked = false;            // Optionally initialize any runtime-only UI state
            rbWalkin.Checked = false;
            rbAppointment.Checked = false;
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }



        private void Patient_Info_Appoinment_Load(object sender, EventArgs e)
        {
               
        }





        private void txtPatientID_TextChanged(object sender, EventArgs e)
        {

            if (int.TryParse(txtPatientID.Text, out int patientID))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM Patients WHERE PatientID = @id", con))
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = patientID;

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                txtFname.Text = dt.Rows[0]["FirstName"].ToString();
                                txtLname.Text = dt.Rows[0]["LastName"].ToString();
                            }
                            else
                            {
                                txtFname.Clear();
                                txtLname.Clear();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading patient: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void txtPatientID_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT * FROM Appointments WHERE Dentist=@dentist";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@dentist", selectedDentist);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dentist schedule: " + ex.Message,
                                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

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
        private void ClearFields()
        {

            txtPatientID.Clear();
            txtFname.Clear();
            txtLname.Clear();
            dtpBirthdate.Value = DateTime.Now;
            nudAge.Value = 0;
            rbMale.Checked = false;
            rbFemale.Checked = false;
            txtContact.Clear();
            txtEmail.Clear();
            txtAddress.Clear();


            cmbDentist.SelectedIndex = -1;
            cmbTreatmentType.SelectedIndex = -1;
            rbWalkin.Checked = false;
            rbAppointment.Checked = false;
            cmbStatus.SelectedIndex = -1;
            dtAppoinment.Value = DateTime.Now;
            richtxtNotes.Clear();
        }
       
        private void btnSave_Click_1(object sender, EventArgs e)

        {
           
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"INSERT INTO Patients 
                             (FirstName, LastName, BirthDate, Age, Gender, ContactNo, Email, Address) 
                             VALUES (@fname, @lname, @bdate, @age, @gender, @contact, @email, @address)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@fname", txtFname.Text.Trim());
                        cmd.Parameters.AddWithValue("@lname", txtLname.Text.Trim());
                        cmd.Parameters.AddWithValue("@bdate", dtpBirthdate.Value);
                        cmd.Parameters.AddWithValue("@age", nudAge.Value);
                        cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Male" : "Female");
                        cmd.Parameters.AddWithValue("@contact", txtContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Patient saved successfully!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);


                frmPatientRecords recordForm = new frmPatientRecords();
                recordForm.Show();
                NewMethod(recordForm);

                frmReceptionistDashboard dash = new frmReceptionistDashboard();
                dash.Show();
                dash.LoadDashboard();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving patient: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void NewMethod(frmPatientRecords recordForm)
        {
            recordForm.LoadPatients();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(
           @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Appointments 
              SET Dentist=@dentist, Treatment=@treatment, Status=@status, 
                  AppointmentDateTime=@datetime, Notes=@notes 
              WHERE AppointmentID=@apptid", con);

                cmd.Parameters.AddWithValue("@dentist", cmbDentist.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@treatment", cmbTreatmentType.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@datetime", dtAppoinment.Value);
                cmd.Parameters.AddWithValue("@notes", richtxtNotes.Text);
                cmd.Parameters.AddWithValue("@apptid", txtAppointmentID.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Appointment updated successfully!", "Success");
            }

            frmPatientRecords recordsForm = new frmPatientRecords();
            recordsForm.Show();
        }

        private void btnSearchPatient_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = "SELECT * FROM Patients WHERE PatientID = @pid";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@pid", txtPatientID.Text.Trim());

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            txtFname.Text = row["FirstName"].ToString();
                            txtLname.Text = row["LastName"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No patient found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching patient: " + ex.Message);
            }
        }



        private void btnClear_Click_1(object sender, EventArgs e)
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

    }
}

    