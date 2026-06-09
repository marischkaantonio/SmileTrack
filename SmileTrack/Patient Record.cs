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

            // Ensure keyboard / row navigation updates the detail panel as well
            dgvPatientRecord.SelectionChanged -= dgvPatientRecord_SelectionChanged;
            dgvPatientRecord.SelectionChanged += dgvPatientRecord_SelectionChanged;
        }

        // Public so other forms can call after saving a patient
        public void LoadPatients()
        {
            try
            {
                // Use LoadPatientRecords to include last appointment/treatment information
                LoadPatientRecords();
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
    la.AppointmentDateTime AS [Last Appointment],
    la.Treatment AS [Treatment],
    la.Dentist AS [Dentist],
    la.Status AS [Status]
FROM Patients p
OUTER APPLY (
    SELECT TOP(1) a.AppointmentDateTime, a.Treatment, a.Dentist, a.Status
    FROM Appointments a
    WHERE a.PatientID = p.PatientID
    ORDER BY a.AppointmentDateTime DESC
 ) la
ORDER BY p.PatientID DESC
";

            try
            {
                if (string.IsNullOrWhiteSpace(searchQuery))
                {
                    var dt = DatabaseHelper.ExecuteQuery(baseSql);
                    dgvPatientRecord.AutoGenerateColumns = true;
                    dgvPatientRecord.DataSource = dt;

                    if (dgvPatientRecord.Rows.Count > 0)
                        ShowRowSummary(dgvPatientRecord.Rows[0]);
                    else
                        ClearDetails();

                    return;
                }

                var sql = baseSql + " WHERE p.FirstName LIKE @search OR p.LastName LIKE @search OR CAST(p.PatientID AS VARCHAR) LIKE @search";
                var dt2 = DatabaseHelper.ExecuteQuery(sql, new SqlParameter("@search", "%" + searchQuery + "%"));
                dgvPatientRecord.AutoGenerateColumns = true;
                dgvPatientRecord.DataSource = dt2;

                if (dgvPatientRecord.Rows.Count > 0)
                    ShowRowSummary(dgvPatientRecord.Rows[0]);
                else
                    ClearDetails();
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

                if (dgvPatientRecord.Rows.Count > 0)
                    ShowRowSummary(dgvPatientRecord.Rows[0]);
                else
                    ClearDetails();
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

                if (dgvPatientRecord.Rows.Count > 0)
                    ShowRowSummary(dgvPatientRecord.Rows[0]);
                else
                    ClearDetails();
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

                if (dgvPatientRecord.Rows.Count > 0)
                    ShowRowSummary(dgvPatientRecord.Rows[0]);
                else
                    ClearDetails();
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

        // Show selected row summary on right panel (wired to CellClick)
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

        // Also update details when selection changes (keyboard navigation, programmatic selection)
        private void dgvPatientRecord_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPatientRecord.CurrentRow != null && dgvPatientRecord.CurrentRow.Index >= 0)
                ShowRowSummary(dgvPatientRecord.CurrentRow);
            else
                ClearDetails();
        }

        private void ShowRowSummary(DataGridViewRow row)
        {
            if (row == null)
            {
                ClearDetails();
                return;
            }

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
                        try
                        {
                            if (appt["AppointmentDateTime"] != DBNull.Value && DateTime.TryParse(Convert.ToString(appt["AppointmentDateTime"]), out DateTime ladate))
                                lblLastAppointment.Text = ladate.ToString("yyyy-MM-dd");
                            else
                                lblLastAppointment.Text = "No Appointment";
                        }
                        catch { lblLastAppointment.Text = "No Appointment"; }

                        lblTreatment.Text = appt.Table.Columns.Contains("Treatment") && appt["Treatment"] != DBNull.Value ? appt["Treatment"].ToString() : string.Empty;
                        lblDentist.Text = appt.Table.Columns.Contains("Dentist") && appt["Dentist"] != DBNull.Value ? appt["Dentist"].ToString() : string.Empty;
                        lblStatus.Text = appt.Table.Columns.Contains("Status") && appt["Status"] != DBNull.Value ? appt["Status"].ToString() : string.Empty;
                    }
                    else
                    {
                        // No appointment rows found; fallback to reading grid columns if present
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
                else
                {
                    // No patient id in row — try to read appointment columns if present in grid
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

        // Helper to read selected patient id from current row
        private int? GetSelectedPatientId()
        {
            var row = dgvPatientRecord.CurrentRow;
            if (row == null) return null;

            var val = GetCellValueIfColumnExists(row, "PatientID", "Patient ID", "PatientId");
            if (val == null) return null;

            if (int.TryParse(val.ToString(), out int id)) return id;
            return null;
        }

        // Clears the details panel labels
        private void ClearDetails()
        {
            lblPatientID.Text = string.Empty;
            lblFname.Text = string.Empty;
            lblLName.Text = string.Empty;
            lblContact.Text = string.Empty;
            lblEmail.Text = string.Empty;
            lblBdate.Text = string.Empty;
            lblGender.Text = string.Empty;
            lblLastAppointment.Text = string.Empty;
            lblTreatment.Text = string.Empty;
            lblDentist.Text = string.Empty;
            lblStatus.Text = string.Empty;
        }

        // Public helper so other forms can ask this view to refresh and select a patient
        public void RefreshAndSelectPatient(int patientId)
        {
            try
            {
                // Prefer using LoadPatientRecords so appointment info is present
                try { LoadPatientRecords(); }
                catch { LoadPatients(); }

                // Find row that contains the patientId
                for (int i = 0; i < dgvPatientRecord.Rows.Count; i++)
                {
                    var row = dgvPatientRecord.Rows[i];
                    var val = GetCellValueIfColumnExists(row, "PatientID", "Patient ID", "PatientId");
                    if (val != null && int.TryParse(val.ToString(), out int id) && id == patientId)
                    {
                        // Select row and ensure current cell is valid so SelectionChanged fires
                        dgvPatientRecord.ClearSelection();
                        if (row.Cells.Count > 0)
                        {
                            dgvPatientRecord.CurrentCell = row.Cells[0];
                            row.Selected = true;
                        }
                        ShowRowSummary(row);
                        return;
                    }
                }
            }
            catch
            {
                // ignore refresh errors
            }
        }

        
        
        // Quote helper - if missing elsewhere in project, replace with simple CSV escaping here.
        private static string Quote(string s)
        {
            if (s == null) return "\"\"";
            var escaped = s.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {
                DataGridView patientGrid = this.dgvPatientRecord;

                if (patientGrid == null || patientGrid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row from the table first.", "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedRow = patientGrid.SelectedRows[0];
                string idColumnName = "";


                if (patientGrid.Columns.Contains("AppointmentID")) idColumnName = "AppointmentID";
                else if (patientGrid.Columns.Contains("Appointment ID")) idColumnName = "Appointment ID";
                else if (patientGrid.Columns.Contains("PatientID")) idColumnName = "PatientID";
                else if (patientGrid.Columns.Contains("Patient ID")) idColumnName = "Patient ID";

                if (string.IsNullOrEmpty(idColumnName) || selectedRow.Cells[idColumnName].Value == null)
                {
                    MessageBox.Show("System Error: Could not find a valid ID column (AppointmentID or PatientID) in your table design.", "Missing Column Identifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int targetId = Convert.ToInt32(selectedRow.Cells[idColumnName].Value);

                // Safe name retrieval fix
                string firstName = patientGrid.Columns.Contains("FirstName") && selectedRow.Cells["FirstName"].Value != null ? selectedRow.Cells["FirstName"].Value.ToString() : "";
                string lastName = patientGrid.Columns.Contains("LastName") && selectedRow.Cells["LastName"].Value != null ? selectedRow.Cells["LastName"].Value.ToString() : "";
                string patientName = (firstName + " " + lastName).Trim();
                if (string.IsNullOrEmpty(patientName)) patientName = "this selected row";

                // 2. Ask user for confirmation
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the record for {patientName}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // If both AppointmentID and PatientID exist, ask what to delete
                    bool hasAppt = patientGrid.Columns.Contains("AppointmentID") && selectedRow.Cells["AppointmentID"].Value != null;
                    bool hasPatient = (patientGrid.Columns.Contains("PatientID") || patientGrid.Columns.Contains("Patient ID")) && (selectedRow.Cells[ "PatientID" ].Value != null || (patientGrid.Columns.Contains("Patient ID") && selectedRow.Cells["Patient ID"].Value != null));

                    if (hasAppt && hasPatient)
                    {
                        var choice = MessageBox.Show("This row contains both an appointment and a patient.\nYes = delete PATIENT and all related records.\nNo = delete only the appointment.\nCancel = do nothing.", "Delete Choice", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (choice == DialogResult.Cancel) return;
                        if (choice == DialogResult.No)
                        {
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM Appointments WHERE AppointmentID = @ID", conn))
                            {
                                cmd.Parameters.AddWithValue("@ID", targetId);
                                conn.Open(); cmd.ExecuteNonQuery();
                            }
                            MessageBox.Show("Appointment deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try { DatabaseHelper.RaiseAppointmentsChanged(); } catch { }
                        }
                        else
                        {
                            // delete patient
                            bool deleted = DatabaseHelper.DeletePatient(targetId);
                            if (deleted) { MessageBox.Show("Patient deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); try { DatabaseHelper.RaiseAppointmentsChanged(); } catch { } }
                            else MessageBox.Show("Patient not deleted.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else if (idColumnName.Contains("Appointment"))
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM Appointments WHERE AppointmentID = @ID", conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", targetId);
                            conn.Open(); cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Appointment deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        try { DatabaseHelper.RaiseAppointmentsChanged(); } catch { }
                    }
                    else
                    {
                        bool deleted = DatabaseHelper.DeletePatient(targetId);
                        if (deleted)
                        {
                            MessageBox.Show("Patient deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try { DatabaseHelper.RaiseAppointmentsChanged(); } catch { }
                        }
                        else
                        {
                            MessageBox.Show("Patient not deleted.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    // Refresh UI
                    try { LoadPatientRecords(); } catch { LoadPatients(); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the row:\n\n" + ex.Message, "SQL/Runtime Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {

        }
    }
}









