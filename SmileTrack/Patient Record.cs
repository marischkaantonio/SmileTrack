using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmileTrack
{
    public partial class frmPatientRecords : Form
    {
        private string connectionString;

        public frmPatientRecords()
        {
            InitializeComponent();
            this.Load += frmPatientRecords_Load;

            // Wire events
            btnSearch.Click -= btnSearch_Click;
            btnSearch.Click += btnSearch_Click;

            txtSearch.TextChanged -= txtSearch_TextChanged;
            txtSearch.TextChanged += txtSearch_TextChanged;

            cmbFilterbyDentist.SelectedIndexChanged -= cmbFilterbyDentist_SelectedIndexChanged;
            cmbFilterbyDentist.SelectedIndexChanged += cmbFilterbyDentist_SelectedIndexChanged;

            cmbFilterbyStatus.SelectedIndexChanged -= cmbFilterbyStatus_SelectedIndexChanged;
            cmbFilterbyStatus.SelectedIndexChanged += cmbFilterbyStatus_SelectedIndexChanged;

            btnExport.Click -= btnExport_Click;
            btnExport.Click += btnExport_Click;

            btnClose.Click -= btnClose_Click;
            btnClose.Click += btnClose_Click;

            btnClear.Click -= btnClear_Click;
            btnClear.Click += btnClear_Click;

            dgvPatientRecord.CellClick -= dgvPatientRecord_CellContentClick;
            dgvPatientRecord.CellClick += dgvPatientRecord_CellContentClick;
        }

        // Public so other forms can call after saving a patient
        public void LoadPatients()
        {
            var connStr = string.IsNullOrWhiteSpace(connectionString)
                ? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"
                : connectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string query = "SELECT PatientID, FirstName, LastName, BirthDate, Age, Gender, ContactNo, Email FROM Patients";
                using (var da = new SqlDataAdapter(query, con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgvPatientRecord.DataSource = dt;
                }
            }
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
                query += " WHERE p.FirstName LIKE @search OR p.LastName LIKE @search OR CAST(p.PatientID AS VARCHAR) LIKE @search";

            try
            {
                var connStr = string.IsNullOrWhiteSpace(connectionString)
                    ? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"
                    : connectionString;

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (!string.IsNullOrWhiteSpace(searchQuery))
                        cmd.Parameters.AddWithValue("@search", "%" + searchQuery + "%");

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);

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
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Patients WHERE FirstName LIKE @keyword OR LastName LIKE @keyword OR ContactNo LIKE @keyword OR Email LIKE @keyword", con))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        dgvPatientRecord.DataSource = dt;
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            btnSearch.PerformClick();
        }

        private void cmbFilterbyDentist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterbyDentist.SelectedItem == null) return;
            string dentist = cmbFilterbyDentist.SelectedItem.ToString();

            using (SqlConnection con = new SqlConnection(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Appointments WHERE Dentist = @dentist", con))
                {
                    cmd.Parameters.AddWithValue("@dentist", dentist);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        dgvPatientRecord.DataSource = dt;
                    }
                }
            }
        }

        private void cmbFilterbyStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterbyStatus.SelectedItem == null) return;

            string status = cmbFilterbyStatus.SelectedItem.ToString();

            using (SqlConnection con = new SqlConnection(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"))
            {
                con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Appointments WHERE Status = @status", con))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        dgvPatientRecord.DataSource = dt;
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv" };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            var dt = dgvPatientRecord.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
            {
                sw.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => Quote(c.ColumnName))));
                foreach (DataRow row in dt.Rows)
                {
                    var values = dt.Columns.Cast<DataColumn>().Select(c => Quote(Convert.ToString(row[c]) ?? string.Empty));
                    sw.WriteLine(string.Join(",", values));
                }
            }

            MessageBox.Show("Export completed.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbFilterbyDentist.SelectedIndex = -1;
            cmbFilterbyStatus.SelectedIndex = -1;
            dgvPatientRecord.DataSource = null;
        }

        private void frmPatientRecords_Load(object sender, EventArgs e)
        {
            try
            {
                var dtDentist = DatabaseHelper.ExecuteQuery("SELECT DISTINCT ISNULL(Dentist,'') AS Dentist FROM Appointments WHERE ISNULL(Dentist,'') <> '' ORDER BY Dentist");
                cmbFilterbyDentist.Items.Clear();
                cmbFilterbyDentist.Items.Add("");
                foreach (DataRow r in dtDentist.Rows) cmbFilterbyDentist.Items.Add(r["Dentist"].ToString());

                var dtStatus = DatabaseHelper.ExecuteQuery("SELECT DISTINCT ISNULL([Status],'') AS [Status] FROM Appointments WHERE ISNULL([Status],'') <> '' ORDER BY [Status]");
                cmbFilterbyStatus.Items.Clear();
                cmbFilterbyStatus.Items.Add("");
                foreach (DataRow r in dtStatus.Rows) cmbFilterbyStatus.Items.Add(r["Status"].ToString());

                LoadPatients();
            }
            catch
            {
                // ignore non-fatal load errors
            }
        }

        private void dgvPatientRecord_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow row = dgvPatientRecord.Rows[e.RowIndex];

                lblPatientID.Text = row.Cells["Patient ID"].Value?.ToString();
                lblFname.Text = row.Cells["First Name"].Value?.ToString();
                lblLName.Text = row.Cells["Last Name"].Value?.ToString();
                lblContact.Text = row.Cells["Contact No"].Value?.ToString();
                lblEmail.Text = row.Cells["Email"].Value?.ToString();
                lblBdate.Text = row.Cells["Birth Date"].Value?.ToString();
                lblGender.Text = row.Cells["Gender"].Value?.ToString();
                lblStatus.Text = row.Cells["Status"] != null ? row.Cells["Status"].Value?.ToString() : string.Empty;
                lblTreatment.Text = row.Cells["Treatment"] != null ? row.Cells["Treatment"].Value?.ToString() : string.Empty;
                lblDentist.Text = row.Cells["Dentist"] != null ? row.Cells["Dentist"].Value?.ToString() : string.Empty;

                // Use DataGridView.Columns.Contains instead of row.Cells.Table which doesn't exist
                if (dgvPatientRecord.Columns.Contains("Last Appointment") && row.Cells["Last Appointment"].Value != DBNull.Value)
                    lblLastAppointment.Text = Convert.ToDateTime(row.Cells["Last Appointment"].Value).ToShortDateString();
                else
                    lblLastAppointment.Text = "No Appointment";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error showing details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Quote(string s)
        {
            if (s == null) return "\"\"";
            var escaped = s.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
    }
}





