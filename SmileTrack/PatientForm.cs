using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SmileTrack.ReceptionistDashboard;

namespace SmileTrack
{
    public partial class PatientForm : Form
    {
        private int currentPage;
        private int totalPages;
        public PatientForm()
        {
            InitializeComponent();

            currentPage = 1;
            totalPages = 1;
        }

        private void PatientForm_Load(object sender, EventArgs e)
        {

            btnPrev.Enabled = currentPage > 1;
            btnFirst.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
            btnLast.Enabled = currentPage < totalPages;
        }
        public static class PatientManager
        {
            public static List<Patient> Patients = new List<Patient>();
        }
        public class Patient
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime Birthdate { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
            public string Contact { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string TreatmentType { get; set; }
            public DateTime RegistrationDate { get; set; }
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            Patient newPatient = new Patient
            {
                FirstName = txtFname.Text,
                LastName = txtLname.Text,
                Birthdate = DateTime.Now,
                Age = (int)numAge.Value,
                Gender = rbMale.Checked ? "Male" : "Female",
                Contact = txtContact.Text,
                Email = txtEmail.Text,
                Address = txtAdd.Text,
                TreatmentType = cmbTreatmentType.SelectedItem?.ToString() ?? string.Empty,
                RegistrationDate = DateTime.Today
            };
            MessageBox.Show("Patient added successfully!");
        }
    }
}




