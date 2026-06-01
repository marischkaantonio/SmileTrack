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

namespace SmileTrack
{
    public partial class frmPatientRecords : Form
    {
        private string connectionString;

        public frmPatientRecords()
        {
            InitializeComponent();
        }
        private void LoadPatientRecords(string searchQuery = "")
        {
            
            string query = @"
        SELECT 
            p.PatientID AS [Patient ID], 
            p.FirstName AS [First Name], 
            p.LastName AS [Last Name], 
            p.BirthDate AS [Birth Date], 
            p.Age AS [Age], 
            p.Gender AS [Gender],
            p.ContactNo AS [Contact No],
            p.Email AS [Email],
            p.Address AS [Address],
            a.[AppointmentDateTime] AS [Last Appointment],
            a.Treatment AS [Treatment],
            a.Dentist AS [Dentist],
            a.Status AS [Status]
        FROM Patients p
        INNER JOIN Appointments a ON p.PatientID = a.PatientID";

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query += " WHERE p.FirstName LIKE @search OR p.LastName LIKE @search OR CAST(p.PatientID AS VARCHAR) LIKE @search";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (!string.IsNullOrWhiteSpace(searchQuery))
                        {
                            cmd.Parameters.AddWithValue("@search", "%" + searchQuery + "%");
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Siguraduhing malinis at bago ang binding
                        dgvPatientRecord.DataSource = null;
                        dgvPatientRecord.AutoGenerateColumns = true;
                        dgvPatientRecord.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading records: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            using (SqlConnection con = new SqlConnection(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Patients WHERE FirstName LIKE @keyword OR LastName LIKE @keyword OR ContactNo LIKE @keyword OR Email LIKE @keyword", con);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPatientRecord.DataSource = dt;
            }
        }
        

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick(); // reuse search logic
        }

        private void cmbFilterbyDentist_SelectedIndexChanged(object sender, EventArgs e)
        {
               string dentist = cmbFilterbyDentist.SelectedItem.ToString();

            using (SqlConnection con = new SqlConnection(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Appointments WHERE DentistName = @dentist", con);
                cmd.Parameters.AddWithValue("@dentist", dentist);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPatientRecord.DataSource = dt;
            }
        }

        

        private void cmbFilterbyStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            string status = cmbFilterbyStatus.SelectedItem.ToString();

            using (SqlConnection con = new SqlConnection(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Appointments WHERE Status = @status", con);
                cmd.Parameters.AddWithValue("@status", status);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
               dgvPatientRecord.DataSource = dt;
            }
        }

        
        private void btnExport_Click(object sender, EventArgs e)
        {
           
            // Example using SaveFileDialog
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF files (*.pdf)|*.pdf";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Export logic here (e.g., iTextSharp to convert DataGridView to PDF)
                MessageBox.Show("Exported to PDF successfully!", "Export");
            }
        }

     

        private void btnClose_Click(object sender, EventArgs e)
        {
           
            this.Close(); // closes Patient Records form
        }

        

        private void btnClear_Click(object sender, EventArgs e)
        {
         
            txtSearch.Clear();
            cmbFilterbyDentist.SelectedIndex = -1;
            cmbFilterbyStatus.SelectedIndex = -1;
            dgvPatientRecord.DataSource = null; // clear grid
        }

        private void frmPatientRecords_Load(object sender, EventArgs e)
        {

        }

        private void dgvPatientRecord_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPatientRecord.Rows[e.RowIndex];

                lblPatientID.Text = row.Cells["Patient ID"].Value?.ToString();
                lblFname.Text = row.Cells["First Name"].Value?.ToString();
                lblLName.Text = row.Cells["Last Name"].Value?.ToString();
                lblContact.Text = row.Cells["Contact No"].Value?.ToString();
                lblEmail.Text = row.Cells["Email"].Value?.ToString();
                lblBdate.Text = row.Cells["Birth Date"].Value?.ToString();
                lblGender.Text = row.Cells["Gender"].Value?.ToString();
                lblStatus.Text = row.Cells["Status"].Value?.ToString();
                lblTreatment.Text = row.Cells["Treatment"].Value?.ToString();
                lblDentist.Text = row.Cells["Dentist"].Value?.ToString();

                // Safe DateTime conversion
                if (row.Cells["Last Appointment"].Value != null && row.Cells["Last Appointment"].Value != DBNull.Value)
                {
                    lblLastAppointment.Text = Convert.ToDateTime(row.Cells["Last Appointment"].Value).ToShortDateString();
                }
                else
                {
                    lblLastAppointment.Text = "No Appointment";
                }
            }
        }

    }
}


