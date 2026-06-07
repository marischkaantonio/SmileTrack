using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SmileTrack
{
    public partial class Patient_Info_Appoinment : Form
    {
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False";

        public Patient_Info_Appoinment()
        {
            InitializeComponent();

            // initialize runtime UI state once
            rbWalkin.Checked = false;
            rbAppointment.Checked = false;
            rbMale.Checked = false;
            rbFemale.Checked = false;

            // Make PatientID read-only so receptionist selects existing records via Search
            // (prevents accidental insertion vs update confusion).
            txtPatientID.ReadOnly = true;
            txtPatientID.TabStop = false;

            // Wire additional handlers that designer does not wire
            txtPatientID.KeyPress -= txtPatientID_KeyPress;
            txtPatientID.KeyPress += txtPatientID_KeyPress;

            txtPatientID.KeyDown -= txtPatientID_KeyDown;
            txtPatientID.KeyDown += txtPatientID_KeyDown;
        }

         

        private void Patient_Info_Appoinment_Load(object sender, EventArgs e)
        {
            // No automatic DB lookup on load
        }

        // Keep TextChanged lightweight: only clear fields when PatientID cleared.
        // Designer wires this event, keep the handler name.
        private void txtPatientID_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPatientID.Text))
            {
                ClearPatientFieldsOnly();
            }
        }

        // Allow only numbers in PatientID box
        private void txtPatientID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // Enter triggers the same behavior as Search button
        private void txtPatientID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSearchPatient.PerformClick();
            }
        }

        // Open patient records dialog; user picks a patient which will populate fields
        private void btnSearchPatient_Click(object sender, EventArgs e)
        {
            // If receptionist entered a PatientID, try to populate directly.
            var text = txtPatientID.Text?.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                if (int.TryParse(text, out int pid) && pid > 0)
                {
                    PopulatePatientById(pid);
                    return;
                }
                else
                {
                    MessageBox.Show("Enter a valid numeric Patient ID or use the patient picker.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // No ID entered — open patient picker (existing behavior)
            using (var records = new frmPatientRecords())
            {
                // subscribe to patient selection event
                records.PatientSelected -= Records_PatientSelected;
                records.PatientSelected += Records_PatientSelected;

                // Pre-load list for better UX
                try { records.LoadPatients(); } catch { /* ignore load errors */ }

                // Show modal so selection returns before continuing
                records.ShowDialog(this);
            }
        }

        // Called when frmPatientRecords raises PatientSelected
        private void Records_PatientSelected(int patientId)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(Records_PatientSelected), patientId);
                return;
            }

            PopulatePatientById(patientId);
        }

        // Populate patient fields and (optionally) load last appointment summary
        private void PopulatePatientById(int patientId)
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    "SELECT * FROM Patients WHERE PatientID = @id",
                    new SqlParameter("@id", patientId));

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No patient found with that ID.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearPatientFieldsOnly();
                    return;
                }

                var row = dt.Rows[0];
                txtPatientID.Text = patientId.ToString();
                txtFname.Text = row["FirstName"]?.ToString() ?? string.Empty;
                txtLname.Text = row["LastName"]?.ToString() ?? string.Empty;

                DateTime bdate;
                if (DateTime.TryParse(row["BirthDate"]?.ToString(), out bdate))
                    dtpBirthdate.Value = bdate;
                else
                    dtpBirthdate.Value = DateTime.Today;

                int age;
                if (int.TryParse(row["Age"]?.ToString(), out age))
                    nudAge.Value = Math.Max(nudAge.Minimum, Math.Min(nudAge.Maximum, age));
                else
                    nudAge.Value = 0;

                var gender = (row["Gender"] ?? string.Empty).ToString();
                rbMale.Checked = gender.Equals("Male", StringComparison.OrdinalIgnoreCase);
                rbFemale.Checked = gender.Equals("Female", StringComparison.OrdinalIgnoreCase);

                txtContact.Text = row["ContactNo"]?.ToString() ?? string.Empty;
                txtEmail.Text = row["Email"]?.ToString() ?? string.Empty;
                txtAddress.Text = row["Address"]?.ToString() ?? string.Empty;

                // Optionally load last appointment for display in appointment fields
                var apptDt = DatabaseHelper.ExecuteQuery(
                    @"SELECT TOP(1) * FROM Appointments WHERE PatientID = @id ORDER BY AppointmentDateTime DESC",
                    new SqlParameter("@id", patientId));
                if (apptDt.Rows.Count > 0)
                {
                    var appt = apptDt.Rows[0];
                    txtAppointmentID.Text = appt["AppointmentID"]?.ToString() ?? string.Empty;
                    cmbDentist.Text = appt["Dentist"]?.ToString() ?? string.Empty;
                    cmbTreatmentType.Text = appt["Treatment"]?.ToString() ?? string.Empty;
                    cmbStatus.Text = appt["Status"]?.ToString() ?? string.Empty;

                    DateTime apptDate;
                    if (DateTime.TryParse(appt["AppointmentDateTime"]?.ToString(), out apptDate))
                        dtAppoinment.Value = apptDate;
                    else
                        dtAppoinment.Value = DateTime.Now;

                    richtxtNotes.Text = appt["Notes"]?.ToString() ?? string.Empty;

                    var visitType = (appt["VisitType"] ?? string.Empty).ToString();
                    rbWalkin.Checked = visitType.Equals("Walk-in", StringComparison.OrdinalIgnoreCase);
                    rbAppointment.Checked = visitType.Equals("Appointment", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    // clear appointment fields when none found
                    txtAppointmentID.Clear();
                    cmbDentist.SelectedIndex = -1;
                    cmbTreatmentType.SelectedIndex = -1;
                    cmbStatus.SelectedIndex = -1;
                    dtAppoinment.Value = DateTime.Now;
                    richtxtNotes.Clear();
                    rbWalkin.Checked = false;
                    rbAppointment.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading patient: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (birthdate.Date > today.AddYears(-age)) age--;

            lblAge.Text = "Age: " + age.ToString();
            nudAge.Value = age;
        }

        private void nudAge_ValueChanged(object sender, EventArgs e)
        {
            lblAge.Text = "Age: " + nudAge.Value.ToString();
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked) lblGender.Text = "Gender: Male";
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked) lblGender.Text = "Gender: Female";
        }

        private void txtContact_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtContact.Text, @"^\d*$"))
                errorProvider1.SetError(txtContact, "Numbers only");
            else
                errorProvider1.Clear();
        }

        private void cmbDentist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDentist.SelectedItem == null) return;
            // Do not overwrite combobox text - keep selection only.
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Hide();
            var receptionistForm = new frmReceptionistDashboard();
            receptionistForm.Show();
        }

        private void grpGender_Enter(object sender, EventArgs e)
        {
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }

        // Clears only patient fields (keeps appointment controls untouched)
        private void ClearPatientFieldsOnly()
        {
            txtFname.Clear();
            txtLname.Clear();
            dtpBirthdate.Value = DateTime.Today;
            nudAge.Value = 0;
            rbMale.Checked = false;
            rbFemale.Checked = false;
            txtContact.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
        }

        // Clears everything
        private void ClearFields()
        {
            txtPatientID.Clear();
            ClearPatientFieldsOnly();

            cmbDentist.SelectedIndex = -1;
            cmbTreatmentType.SelectedIndex = -1;
            rbWalkin.Checked = false;
            rbAppointment.Checked = false;
            cmbStatus.SelectedIndex = -1;
            dtAppoinment.Value = DateTime.Now;
            richtxtNotes.Clear();
            txtAppointmentID.Clear();
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                // If PatientID contains an existing numeric id → update.
                var idText = txtPatientID.Text?.Trim();
                if (!string.IsNullOrEmpty(idText) && int.TryParse(idText, out int existingId) && existingId > 0)
                {
                    // verify existence before attempting update
                    var existsDt = DatabaseHelper.ExecuteQuery(
                        "SELECT COUNT(1) AS C FROM Patients WHERE PatientID = @id",
                        new SqlParameter("@id", existingId));

                    var exists = existsDt.Rows.Count > 0 && Convert.ToInt32(existsDt.Rows[0]["C"]) > 0;
                    if (exists)
                    {
                        bool updated = DatabaseHelper.UpdatePatient(
                            existingId,
                            txtFname.Text.Trim(),
                            txtLname.Text.Trim(),
                            dtpBirthdate.Value,
                            (int)nudAge.Value,
                            rbMale.Checked ? "Male" : rbFemale.Checked ? "Female" : string.Empty,
                            txtContact.Text.Trim(),
                            txtEmail.Text.Trim(),
                            txtAddress.Text.Trim());

                        if (updated)
                            MessageBox.Show($"Patient updated successfully! Patient ID: {existingId}", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("No patient was updated. The ID may not exist.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        // refresh any open records window
                        foreach (Form f in Application.OpenForms)
                            if (f is frmPatientRecords fr) try { fr.LoadPatients(); } catch { }

                        return;
                    }

                    // ID present but not found in DB — offer to create new instead
                    if (MessageBox.Show($"Patient ID {existingId} was not found. Create a new patient instead?", "Not found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    // fall through to create new
                }

                // Create new patient
                int newId = DatabaseHelper.AddPatient(
                    txtFname.Text.Trim(),
                    txtLname.Text.Trim(),
                    dtpBirthdate.Value,
                    (int)nudAge.Value,
                    rbMale.Checked ? "Male" : rbFemale.Checked ? "Female" : string.Empty,
                    txtContact.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtAddress.Text.Trim());

                MessageBox.Show($"Patient saved successfully! Patient ID: {newId}", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // set the generated id so user sees the saved record
                txtPatientID.Text = newId.ToString();

                // Refresh or open patient records window
                frmPatientRecords existing = null;
                foreach (Form f in Application.OpenForms)
                {
                    if (f is frmPatientRecords fr) { existing = fr; break; }
                }

                if (existing != null) { try { existing.LoadPatients(); existing.BringToFront(); existing.Focus(); } catch { } }
                else
                {
                    try { var recordForm = new frmPatientRecords(); recordForm.LoadPatients(); recordForm.StartPosition = FormStartPosition.CenterParent; recordForm.Show(this); }
                    catch { /* ignore UI refresh errors */ }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving patient: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new SqlConnection(connectionString))
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = @"
                        UPDATE Appointments 
                        SET Dentist=@dentist, Treatment=@treatment, Status=@status, 
                            AppointmentDateTime=@datetime, Notes=@notes, VisitType=@visittype
                        WHERE AppointmentID=@apptid";

                    cmd.Parameters.AddWithValue("@dentist", cmbDentist.SelectedItem?.ToString() ?? cmbDentist.Text ?? string.Empty);
                    cmd.Parameters.AddWithValue("@treatment", cmbTreatmentType.SelectedItem?.ToString() ?? cmbTreatmentType.Text ?? string.Empty);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem?.ToString() ?? cmbStatus.Text ?? string.Empty);
                    cmd.Parameters.AddWithValue("@datetime", dtAppoinment.Value);
                    cmd.Parameters.AddWithValue("@notes", richtxtNotes.Text);
                    cmd.Parameters.AddWithValue("@visittype", rbWalkin.Checked ? "Walk-in" : rbAppointment.Checked ? "Appointment" : string.Empty);
                    cmd.Parameters.AddWithValue("@apptid", txtAppointmentID.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Appointment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var recordsForm = new frmPatientRecords();
                recordsForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating appointment: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Use Search button fallback: if patient id filled, populate directly; otherwise open records picker
        private void btnSearchPatient_Fallback_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtPatientID.Text.Trim(), out int pid) && pid > 0)
            {
                PopulatePatientById(pid);
                return;
            }

            // otherwise open picker
            btnSearchPatient_Click(sender, e);
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            ClearFields();
        }

        // Add public notifier so other forms can call into this form when a patient was deleted
        public void NotifyPatientDeleted(int patientId)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action<int>(NotifyPatientDeleted), patientId);
                    return;
                }

                if (int.TryParse(txtPatientID.Text.Trim(), out int shownId) && shownId == patientId)
                {
                    ClearFields();
                    MessageBox.Show("The patient you were viewing was deleted.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                // ignore notification errors
            }
        }
    }
}

        // Delete a patient and related records safely