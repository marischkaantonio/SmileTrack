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
        private readonly string connectionString = string.IsNullOrWhiteSpace(DatabaseHelper.ConnectionString)
            ? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False"
            : DatabaseHelper.ConnectionString;

        // Other forms can set this to receive the selected patient id
        public Action<int> PatientSelected { get; internal set; }

        public frmPatientRecords()
        {
            InitializeComponent();
            this.Load += frmPatientRecords_Load;

            // Wire events (designer may already wire these; we detach then attach to be safe)
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

            // btnDelete may or may not exist in the designer — attach if present
            try
            {
                btnDelete.Click -= btnDelete_Click;
                btnDelete.Click += btnDelete_Click;
            }
            catch { /* ignore */ }

            dgvPatientRecord.CellClick -= dgvPatientRecord_CellContentClick;
            dgvPatientRecord.CellClick += dgvPatientRecord_CellContentClick;

            dgvPatientRecord.CellDoubleClick -= dgvPatientRecord_CellDoubleClick;
            dgvPatientRecord.CellDoubleClick += dgvPatientRecord_CellDoubleClick;
        }

        // Public so other forms can call after saving a patient
        public void LoadPatients()
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT PatientID, FirstName, LastName, BirthDate, Age, Gender, ContactNo, Email, Address FROM Patients ORDER BY PatientID DESC");
                dgvPatientRecord.AutoGenerateColumns = true;
                dgvPatientRecord.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading patients: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPatientRecords(string searchQuery = "")
        {
            const string baseSql = @"
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
    a.AppointmentDateTime AS [Last Appointment],
    a.Treatment AS [Treatment],
    a.Dentist AS [Dentist],
    a.Status AS [Status]
FROM Patients p
LEFT JOIN (
    SELECT PatientID, MAX(AppointmentDateTime) AS AppointmentDateTime
    FROM Appointments
    GROUP BY PatientID
) la ON p.PatientID = la.PatientID
LEFT JOIN Appointments a ON p.PatientID = a.PatientID AND a.AppointmentDateTime = la.AppointmentDateTime
";

            try
            {
                if (string.IsNullOrWhiteSpace(searchQuery))
                {
                    var dt = DatabaseHelper.ExecuteQuery(baseSql);
                    dgvPatientRecord.AutoGenerateColumns = true;
                    dgvPatientRecord.DataSource = dt;
                    return;
                }

                var sql = baseSql + " WHERE p.FirstName LIKE @search OR p.LastName LIKE @search OR CAST(p.PatientID AS VARCHAR) LIKE @search";
                var dt2 = DatabaseHelper.ExecuteQuery(sql, new SqlParameter("@search", "%" + searchQuery + "%"));
                dgvPatientRecord.AutoGenerateColumns = true;
                dgvPatientRecord.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading records: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var keyword = txtSearch.Text?.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadPatients();
                return;
            }

            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    "SELECT * FROM Patients WHERE FirstName LIKE @keyword OR LastName LIKE @keyword OR ContactNo LIKE @keyword OR Email LIKE @keyword",
                    new SqlParameter("@keyword", "%" + keyword + "%"));
                dgvPatientRecord.AutoGenerateColumns = true;
                dgvPatientRecord.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => btnSearch.PerformClick();

        private void cmbFilterbyDentist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterbyDentist.SelectedItem == null) return;
            var dentist = cmbFilterbyDentist.SelectedItem.ToString();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT a.AppointmentID, a.AppointmentDateTime, ISNULL(p.FirstName,'')+' '+ISNULL(p.LastName,'') AS PatientName, a.Treatment, a.Status, a.Dentist FROM Appointments a INNER JOIN Patients p ON a.PatientID=p.PatientID WHERE a.Dentist=@dentist ORDER BY a.AppointmentDateTime", new SqlParameter("@dentist", dentist));
                dgvPatientRecord.AutoGenerateColumns = true;
                dgvPatientRecord.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Filter error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbFilterbyStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterbyStatus.SelectedItem == null) return;
            var status = cmbFilterbyStatus.SelectedItem.ToString();
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT a.AppointmentID, a.AppointmentDateTime, ISNULL(p.FirstName,'')+' '+ISNULL(p.LastName,'') AS PatientName, a.Treatment, a.Status, a.Dentist FROM Appointments a INNER JOIN Patients p ON a.PatientID=p.PatientID WHERE a.Status=@status ORDER BY a.AppointmentDateTime", new SqlParameter("@status", status));
                dgvPatientRecord.AutoGenerateColumns = true;
                dgvPatientRecord.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Filter error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var dt = dgvPatientRecord.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (var sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                {
                    sw.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => Quote(c.ColumnName))));
                    foreach (DataRow row in dt.Rows)
                    {
                        var values = dt.Columns.Cast<DataColumn>().Select(c => Quote(Convert.ToString(row[c]) ?? string.Empty));
                        sw.WriteLine(string.Join(",", values));
                    }
                }
            }

            MessageBox.Show("Export completed.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbFilterbyDentist.SelectedIndex = -1;
            cmbFilterbyStatus.SelectedIndex = -1;
            LoadPatients();
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

        // Show selected row summary on right panel
        private void dgvPatientRecord_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            ShowRowSummary(dgvPatientRecord.Rows[e.RowIndex]);
        }

        private void dgvPatientRecord_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int? id = GetSelectedPatientId();
            if (id.HasValue)
            {
                try { PatientSelected?.Invoke(id.Value); } catch { /* ignore */ }
                // If caller expects modal selection, close this form to return control
                if (this.Modal) this.DialogResult = DialogResult.OK;
            }
        }

        private void ShowRowSummary(DataGridViewRow row)
        {
            try
            {
                // Determine patient id from the row (do not fall back to first cell for other fields)
                object pidObj = GetCellValueIfColumnExists(row, "PatientID") ??
                                GetCellValueIfColumnExists(row, "Patient ID") ??
                                GetCellValueIfColumnExists(row, "PatientId");
                int? patientId = null;
                if (pidObj != null && int.TryParse(pidObj.ToString(), out int pid)) patientId = pid;

                // Basic patient fields (only if those columns actually exist in the grid)
                lblPatientID.Text = patientId?.ToString() ?? string.Empty;
                lblFname.Text = Convert.ToString(GetCellValueIfColumnExists(row, "FirstName") ?? GetCellValueIfColumnExists(row, "First Name") ?? string.Empty);
                lblLName.Text = Convert.ToString(GetCellValueIfColumnExists(row, "LastName") ?? GetCellValueIfColumnExists(row, "Last Name") ?? string.Empty);
                lblContact.Text = Convert.ToString(GetCellValueIfColumnExists(row, "ContactNo") ?? GetCellValueIfColumnExists(row, "Contact No") ?? GetCellValueIfColumnExists(row, "Contact") ?? string.Empty);
                lblEmail.Text = Convert.ToString(GetCellValueIfColumnExists(row, "Email") ?? string.Empty);

                // BirthDate: format if present
                var bdateObj = GetCellValueIfColumnExists(row, "BirthDate", "Birth Date", "BDate");
                if (bdateObj != null && DateTime.TryParse(bdateObj.ToString(), out DateTime bdt))
                    lblBdate.Text = bdt.ToString("yyyy-MM-dd");
                else
                    lblBdate.Text = bdateObj?.ToString() ?? string.Empty;

                lblGender.Text = Convert.ToString(GetCellValueIfColumnExists(row, "Gender") ?? string.Empty);

                // Appointment details: prefer querying DB for latest appointment if we have patientId
                if (patientId.HasValue)
                {
                    var apptDt = DatabaseHelper.ExecuteQuery(
                        "SELECT TOP(1) AppointmentDateTime, Treatment, Dentist, Status FROM Appointments WHERE PatientID = @id ORDER BY AppointmentDateTime DESC",
                        new SqlParameter("@id", patientId.Value));

                    if (apptDt.Rows.Count > 0)
                    {
                        var appt = apptDt.Rows[0];
                        // Last appointment (date)
                        if (DateTime.TryParse(Convert.ToString(appt["AppointmentDateTime"]), out DateTime ladate))
                            lblLastAppointment.Text = ladate.ToString("yyyy-MM-dd");
                        else
                            lblLastAppointment.Text = Convert.ToString(appt["AppointmentDateTime"]) ?? "No Appointment";

                        lblTreatment.Text = appt["Treatment"]?.ToString() ?? string.Empty;
                        lblDentist.Text = appt["Dentist"]?.ToString() ?? string.Empty;
                        lblStatus.Text = appt["Status"]?.ToString() ?? string.Empty;
                    }
                    else
                    {
                        lblLastAppointment.Text = "No Appointment";
                        lblTreatment.Text = string.Empty;
                        lblDentist.Text = string.Empty;
                        lblStatus.Text = string.Empty;
                    }
                }
                else
                {
                    // No patient id in row — try to read appointment columns if present in grid (but do not fallback)
                    var last = GetCellValueIfColumnExists(row, "Last Appointment", "AppointmentDateTime", "LastAppointment");
                    if (last != null && DateTime.TryParse(last.ToString(), out DateTime dt))
                        lblLastAppointment.Text = dt.ToString("yyyy-MM-dd");
                    else
                        lblLastAppointment.Text = last?.ToString() ?? "No Appointment";

                    lblTreatment.Text = Convert.ToString(GetCellValueIfColumnExists(row, "Treatment") ?? string.Empty);
                    lblDentist.Text = Convert.ToString(GetCellValueIfColumnExists(row, "Dentist") ?? string.Empty);
                    lblStatus.Text = Convert.ToString(GetCellValueIfColumnExists(row, "Status") ?? string.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error showing details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Return value only if the grid actually contains a matching column or header.
        // Does NOT fall back to arbitrary cells.
        private object GetCellValueIfColumnExists(DataGridViewRow row, params string[] names)
        {
            if (row == null || row.DataGridView == null) return null;

            foreach (var name in names)
            {
                // direct column name
                if (row.DataGridView.Columns.Contains(name))
                {
                    var val = row.Cells[name].Value;
                    if (val != null && val != DBNull.Value) return val;
                }

                // match header text or column name (case-insensitive)
                for (int i = 0; i < row.DataGridView.Columns.Count; i++)
                {
                    var col = row.DataGridView.Columns[i];
                    if (string.Equals(col.HeaderText, name, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(col.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        var val = row.Cells[i].Value;
                        if (val != null && val != DBNull.Value) return val;
                    }
                }
            }

            return null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var id = GetSelectedPatientId();
            if (!id.HasValue)
            {
                MessageBox.Show("Please select a patient to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                "This will permanently delete the patient and related records (appointments, invoices). Proceed?",
                "Confirm delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                bool deleted = DatabaseHelper.DeletePatient(id.Value);
                if (deleted)
                {
                    MessageBox.Show("Patient deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPatients();

                    // notify any open Patient Info form to clear the deleted patient if it is showing
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f is Patient_Info_Appoinment info)
                        {
                            try { info.NotifyPatientDeleted(id.Value); } catch { /* ignore if not available */ }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No patient found with that ID.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting patient: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int? GetSelectedPatientId()
        {
            if (dgvPatientRecord.CurrentRow == null) return null;

            var candidates = new[] { "PatientID", "Patient ID", "PatientId", "Patient" };
            foreach (var name in candidates)
            {
                if (dgvPatientRecord.Columns.Contains(name))
                {
                    var v = dgvPatientRecord.CurrentRow.Cells[name].Value;
                    if (v != null && v != DBNull.Value && int.TryParse(v.ToString(), out int id))
                        return id;
                }
            }

            var first = dgvPatientRecord.CurrentRow.Cells[0].Value;
            if (first != null && first != DBNull.Value && int.TryParse(first.ToString(), out int fid))
                return fid;

            return null;
        }

        private static string Quote(string s)
        {
            if (s == null) return "\"\"";
            var escaped = s.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        private void cmbFilterbyDentist_SelectedIndexChanged_1(object sender, EventArgs e)
        {
                    
        }
    }
}









