using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static SmileTrack.ReceptionistDashboard;

namespace SmileTrack
{
    public partial class PatientForm : Form
    {
        List<Patient> allPatients = new List<Patient>();
        List<Patient> filteredPatients = new List<Patient>();
        public PatientForm()
        {
            InitializeComponent();
        }

        private void PatientForm_Load(object sender, EventArgs e)
        {
            // Setup DataGridView columns if not already in Designer
            dgv.Columns.Add("ID", "ID");
            dgv.Columns.Add("Name", "Name");
            dgv.Columns.Add("Gender", "Gender");
            dgv.Columns.Add("Contact", "Contact");
            dgv.Columns.Add("LastVisit", "Last Visit");
            dgv.Columns.Add("Treatment", "Treatment");

            // Sample data
            dgv.Rows.Add("1", "Juan Dela Cruz", "Male", "09123456789", "2026-05-13", "Cleaning");
            dgv.Rows.Add("2", "Maria Santos", "Female", "09987654321", "2026-05-14", "Extraction");

            // Load treatments into ComboBox
            cmbTreatmentType.DataSource = Class1.GetTreatmentTypes();

            // Optional: prevent editing
            cmbTreatmentType.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        

        // ADD PATIENT
        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            string gender = rbMale.Checked ? "Male" : rbFemale.Checked ? "Female" : "";

            if (string.IsNullOrWhiteSpace(txtFname.Text) ||
                string.IsNullOrWhiteSpace(txtLname.Text) ||
                string.IsNullOrWhiteSpace(txtContact.Text) ||
                cmbTreatmentType.SelectedItem == null ||
                string.IsNullOrWhiteSpace(gender))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation");
                return;
            }

            string id = (dgv.Rows.Count + 1).ToString();
            string fullName = $"{txtFname.Text} {txtLname.Text}";

            dgv.Rows.Add(
                id,
                fullName,
                gender,
                txtContact.Text,
                dtpRegistrationDate.Value.ToString("yyyy-MM-dd"),
                cmbTreatmentType.SelectedItem.ToString()
            );

            MessageBox.Show("Patient added successfully!", "Success");
            ClearFields();
        }

        // UPDATE PATIENT
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null)
            {
                string gender = rbMale.Checked ? "Male" : rbFemale.Checked ? "Female" : "";
                dgv.CurrentRow.Cells["Name"].Value = $"{txtFname.Text} {txtLname.Text}";
                dgv.CurrentRow.Cells["Gender"].Value = gender;
                dgv.CurrentRow.Cells["Contact"].Value = txtContact.Text;
                dgv.CurrentRow.Cells["LastVisit"].Value = dtpRegistrationDate.Value.ToString("yyyy-MM-dd");
                dgv.CurrentRow.Cells["Treatment"].Value = cmbTreatmentType.SelectedItem?.ToString();

                MessageBox.Show("Patient record updated!", "Update");
            }
        }

        // DELETE PATIENT
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow != null)
            {
                dgv.Rows.Remove(dgv.CurrentRow);
                MessageBox.Show("Patient record deleted.", "Delete");
            }
        }

        // CLEAR FIELDS
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtFname.Clear();
            txtLname.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtAdd.Clear();
            numAge.Value = 0;
            rbMale.Checked = false;
            rbFemale.Checked = false;
            cmbTreatmentType.SelectedIndex = -1;
            dtpBdate.Value = DateTime.Now;
            dtpRegistrationDate.Value = DateTime.Now;
        }




        // PAGINATION (simple demo)
        private int currentPage = 1;
        private int rowsPerPage = 5;

        private void ShowPage(int page)
        {
            int start = (page - 1) * rowsPerPage;
            int end = start + rowsPerPage;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Visible = (i >= start && i < end);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ShowPage(currentPage);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int maxPage = (int)Math.Ceiling((double)dgv.Rows.Count / rowsPerPage);
            if (currentPage < maxPage)
            {
                currentPage++;
                ShowPage(currentPage);
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            ShowPage(currentPage);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            currentPage = (int)Math.Ceiling((double)dgv.Rows.Count / rowsPerPage);
            ShowPage(currentPage);
        }


        // Firstname textbox → auto-capitalize first letter
        private void txtFname_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFname.Text))
            {
                txtFname.Text = char.ToUpper(txtFname.Text[0]) + txtFname.Text.Substring(1);
                txtFname.SelectionStart = txtFname.Text.Length; // keep cursor at end
            }
        }

        // Lastname textbox → auto-capitalize first letter
        private void txtLname_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtLname.Text))
            {
                txtLname.Text = char.ToUpper(txtLname.Text[0]) + txtLname.Text.Substring(1);
                txtLname.SelectionStart = txtLname.Text.Length;
            }
        }

        // Birthdate → auto-calculate Age
        private void dtpBdate_ValueChanged(object sender, EventArgs e)
        {
            DateTime birthDate = dtpBdate.Value;
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
                age--;

            numAge.Value = age < 0 ? 0 : age;
        }

        // Age numeric → validation (prevent unrealistic ages)
        private void numAge_ValueChanged(object sender, EventArgs e)
        {
            if (numAge.Value < 0 || numAge.Value > 120)
            {
                MessageBox.Show("Please enter a valid age (0–120).", "Validation");
                numAge.Value = 0;
            }
        }

        // Gender radio buttons → show selection in a message (optional)
        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked)
                Console.WriteLine("Gender selected: Male");
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked)
                Console.WriteLine("Gender selected: Female");
        }

        // Contact → allow only digits
        private void txtContact_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtContact.Text, @"^\d*$"))
            {
                MessageBox.Show("Contact number must contain digits only.", "Validation");
                txtContact.Text = new string(txtContact.Text.Where(char.IsDigit).ToArray());
                txtContact.SelectionStart = txtContact.Text.Length;
            }
        }

        // Email → basic validation
        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) &&
                !txtEmail.Text.Contains("@"))
            {
                lblEmailError.Text = "Invalid email format"; // assume you have a label
                lblEmailError.ForeColor = Color.Red;
            }
            else
            {
                lblEmailError.Text = "";
            }
        }

        // Address → auto-trim spaces
        private void txtAdd_TextChanged(object sender, EventArgs e)
        {
            txtAdd.Text = txtAdd.Text.TrimStart();
            txtAdd.SelectionStart = txtAdd.Text.Length;
        }

        // Treatment Type → auto-assign dentist
        private void cmbTreatmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTreatmentType.SelectedItem != null)
            {
                string treatment = cmbTreatmentType.SelectedItem.ToString();
                switch (treatment)
                {
                    case "Root Canal Treatment":
                        txtDentist.Text = "Dr. Smith (Endodontist)";
                        break;
                    case "Orthodontic Treatment":
                    case "Clear Aligners":
                    case "Complete Ortho Package":
                        txtDentist.Text = "Dr. Adams (Orthodontist)";
                        break;
                    case "Implant":
                    case "Bone Grafting":
                    case "Sinus Lifting":
                        txtDentist.Text = "Dr. Cruz (Oral Surgeon)";
                        break;
                    default:
                        txtDentist.Text = "";
                        break;
                }
            }
        }

        // Registration Date → log selection
        private void dtpRegistrationDate_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"Registration Date set to: {dtpRegistrationDate.Value:yyyy-MM-dd}");
        }

        // Live search as you type
        // Live search as you type
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter(txtSearch.Text);
            SelectFirstVisibleRow();
        }

        // Search button click
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            {
                ApplySearchFilter(txtSearch.Text);
                SelectFirstVisibleRow();
            }
        }

        // Shared search method
        private void ApplySearchFilter(string keyword)
        {
            keyword = keyword.ToLower();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                bool match = false;

                foreach (string colName in new[] { "ID", "Name", "Contact", "Treatment" })
                {
                    var cellValue = row.Cells[colName].Value;
                    if (cellValue != null && cellValue.ToString().ToLower().Contains(keyword))
                    {
                        match = true;
                        break;
                    }
                }

                row.Visible = match;
            }
        }

        // Helper: select first visible row and trigger CellContentClick
        private void SelectFirstVisibleRow()
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Visible && !row.IsNewRow)
                {
                    row.Selected = true;
                    // Safely trigger the same logic as clicking
                    dataGridView1_CellContentClick(dgv,
                        new DataGridViewCellEventArgs(0, row.Index));
                    break;
                }
            }
        }

        // Cell click → load details into form
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgv.Rows[e.RowIndex].Cells["Name"].Value != null)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                // Split Name into Firstname and Lastname
                string[] names = row.Cells["Name"].Value.ToString().Split(' ');
                txtFname.Text = names.Length > 0 ? names[0] : "";
                txtLname.Text = names.Length > 1 ? names[1] : "";

                // Gender
                string gender = row.Cells["Gender"].Value?.ToString();
                rbMale.Checked = gender == "Male";
                rbFemale.Checked = gender == "Female";

                // Contact
                txtContact.Text = row.Cells["Contact"].Value?.ToString();

                // Last Visit → Registration Date
                if (DateTime.TryParse(row.Cells["LastVisit"].Value?.ToString(), out DateTime lastVisit))
                {
                    dtpRegistrationDate.Value = lastVisit;
                }

                // Treatment
                cmbTreatmentType.SelectedItem = row.Cells["Treatment"].Value?.ToString();
            }
        }

     
    }
}

