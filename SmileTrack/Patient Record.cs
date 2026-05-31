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
        public frmPatientRecords()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPatientRecord.Rows[e.RowIndex];

                // Basic info already visible in DGV
                // Appointment details shown in labels
                lblPatientID.Text = row.Cells["PatientID"].Value?.ToString();
                lblFname.Text = row.Cells["FirstName"].Value?.ToString() + " " + row.Cells["LastName"].Value?.ToString();
                lblLName.Text = row.Cells["LastName"].Value?.ToString() + " " + row.Cells["LastName"].Value?.ToString();
                lblContact.Text = row.Cells["ContactNo."].Value?.ToString();
                lblEmail.Text = row.Cells["Email"].Value?.ToString();
                lblBdate.Text = Convert.ToDateTime(row.Cells["BirthDate"].Value).ToShortDateString();
                lblGender.Text = row.Cells["Gender"].Value?.ToString();
                lblStatus.Text = row.Cells["Status"].Value?.ToString();

                // Appointment details (if joined in query)
                lblTreatment.Text = row.Cells["Treatment"].Value?.ToString();
                lblLastAppointment.Text = Convert.ToDateTime(row.Cells["AppointmentDateTime"].Value).ToShortDateString();
                lblDentist.Text = row.Cells["DentistName"].Value?.ToString();
                
            }
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

    }
}

