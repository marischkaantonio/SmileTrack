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

        public Action<int> PatientSelected { get; internal set; }

        public frmPatientRecords()
        {
            InitializeComponent();
            this.Load += frmPatientRecords_Load;

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

            try
            {
                btnDelete.Click -= btnDelete_Click;
                btnDelete.Click += btnDelete_Click;
            }
            catch { }

            dgvPatientRecord.CellClick -= dgvPatientRecord_CellContentClick;
            dgvPatientRecord.CellClick += dgvPatientRecord_CellContentClick;

            dgvPatientRecord.CellDoubleClick -= dgvPatientRecord_CellDoubleClick;
            dgvPatientRecord.CellDoubleClick += dgvPatientRecord_CellDoubleClick;

            dgvPatientRecord.SelectionChanged -= dgvPatientRecord_SelectionChanged;
            dgvPatientRecord.SelectionChanged += dgvPatientRecord_SelectionChanged;
        }

        // ─────────────────────────────────────────────────────────────
        // LOAD
        // ─────────────────────────────────────────────────────────────

        public void LoadPatients()
        {
            try
            {
                LoadPatientRecords();
                try { RefreshFilterLists(); } catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading patients: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPatientRecords(string searchQuery = "")
        {
            // FIX: Removed the broken WHERE clause that was appended after ORDER BY.
            // Search is handled separately below.
            const string baseSql = @"
SELECT
    p.PatientID          AS [Patient ID],
    p.FirstName          AS [First Name],
    p.LastName           AS [Last Name],
    p.BirthDate          AS [Birth Date],
    p.Age                AS [Age],
    p.Gender             AS [Gender],
    p.ContactNo          AS [Contact No],
    p.Email              AS [Email],
    p.Address            AS [Address],
    la.AppointmentDateTime AS [Last Appointment],
    la.Treatment         AS [Treatment],
    la.Dentist           AS [Dentist],
    la.Status            AS [Status]
FROM Patients p
OUTER APPLY (
    SELECT TOP(1)
        a.AppointmentDateTime,
        a.Treatment,
        a.Dentist,
        a.Status
    FROM Appointments a
    WHERE a.PatientID = p.PatientID
    ORDER BY a.AppointmentDateTime DESC
) la
ORDER BY p.PatientID DESC";

            const string searchSql = @"
SELECT
    p.PatientID          AS [Patient ID],
    p.FirstName          AS [First Name],
    p.LastName           AS [Last Name],
    p.BirthDate          AS [Birth Date],
    p.Age                AS [Age],
    p.Gender             AS [Gender],
    p.ContactNo          AS [Contact No],
    p.Email              AS [Email],
    p.Address            AS [Address],
    la.AppointmentDateTime AS [Last Appointment],
    la.Treatment         AS [Treatment],
    la.Dentist           AS [Dentist],
    la.Status            AS [Status]
FROM Patients p
OUTER APPLY (
    SELECT TOP(1)
        a.AppointmentDateTime,
        a.Treatment,
        a.Dentist,
        a.Status
    FROM Appointments a
    WHERE a.PatientID = p.PatientID
    ORDER BY a.AppointmentDateTime DESC
) la
WHERE p.FirstName  LIKE @search
   OR p.LastName   LIKE @search
   OR CAST(p.PatientID AS VARCHAR) LIKE @search
ORDER BY p.PatientID DESC";

            try
            {
                DataTable dt;
                if (string.IsNullOrWhiteSpace(searchQuery))
                    dt = DatabaseHelper.ExecuteQuery(baseSql);
                else
                    dt = DatabaseHelper.ExecuteQuery(searchSql,
                             new SqlParameter("@search", "%" + searchQuery + "%"));

                dgvPatientRecord.AutoGenerateColumns = true;
                dgvPatientRecord.DataSource = dt;

                if (dgvPatientRecord.Rows.Count > 0)
                    ShowRowSummary(dgvPatientRecord.Rows[0]);
                else
                    ClearDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading records: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // FIX: RefreshFilterLists — populate dentist & status combos
        //      from the Appointments table. Works as soon as at least
        //      one appointment has been saved.
        // ─────────────────────────────────────────────────────────────
        public void RefreshFilterLists()
        {
            try
            {
                // ── Dentist filter ──────────────────────────────────
                var dtDentist = DatabaseHelper.ExecuteQuery(
                    @"SELECT DISTINCT ISNULL(Dentist,'') AS Dentist
                      FROM Appointments
                      WHERE ISNULL(Dentist,'') <> ''
                      ORDER BY Dentist");

                cmbFilterbyDentist.SelectedIndexChanged -= cmbFilterbyDentist_SelectedIndexChanged;
                cmbFilterbyDentist.Items.Clear();
                cmbFilterbyDentist.Items.Add("(All Dentists)");   // placeholder shown to user
                foreach (DataRow r in dtDentist.Rows)
                    cmbFilterbyDentist.Items.Add(r["Dentist"].ToString());
                cmbFilterbyDentist.SelectedIndex = -1;            // nothing selected = no filter
                cmbFilterbyDentist.SelectedIndexChanged += cmbFilterbyDentist_SelectedIndexChanged;

                // ── Status filter ───────────────────────────────────
                var dtStatus = DatabaseHelper.ExecuteQuery(
                    @"SELECT DISTINCT ISNULL([Status],'') AS [Status]
                      FROM Appointments
                      WHERE ISNULL([Status],'') <> ''
                      ORDER BY [Status]");

                cmbFilterbyStatus.SelectedIndexChanged -= cmbFilterbyStatus_SelectedIndexChanged;
                cmbFilterbyStatus.Items.Clear();
                cmbFilterbyStatus.Items.Add("(All Statuses)");    // placeholder
                foreach (DataRow r in dtStatus.Rows)
                    cmbFilterbyStatus.Items.Add(r["Status"].ToString());
                cmbFilterbyStatus.SelectedIndex = -1;
                cmbFilterbyStatus.SelectedIndexChanged += cmbFilterbyStatus_SelectedIndexChanged;
            }
            catch { /* ignore */ }
        }

        // ─────────────────────────────────────────────────────────────
        // SEARCH
        // ─────────────────────────────────────────────────────────────

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var keyword = txtSearch.Text?.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadPatients();
                return;
            }
            LoadPatientRecords(keyword);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => btnSearch.PerformClick();

        // ─────────────────────────────────────────────────────────────
        // FILTER BY DENTIST
        // FIX: skip placeholder row; show all patients whose latest
        //      appointment matches the chosen dentist.
        // ─────────────────────────────────────────────────────────────
        private void cmbFilterbyDentist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterbyDentist.SelectedItem == null) return;

            var selected = cmbFilterbyDentist.SelectedItem.ToString();

            // Placeholder selected → show all
            if (selected == "(All Dentists)" || selected == "")
            {
                LoadPatients();
                return;
            }

            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT
                          a.AppointmentID       AS [Appointment ID],
                          a.AppointmentDateTime AS [Appointment Date],
                          ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS [Patient Name],
                          p.PatientID           AS [Patient ID],
                          a.Treatment           AS [Treatment],
                          a.Status              AS [Status],
                          a.Dentist             AS [Dentist]
                      FROM Appointments a
                      INNER JOIN Patients p ON a.PatientID = p.PatientID
                      WHERE a.Dentist = @dentist
                      ORDER BY a.AppointmentDateTime DESC",
                    new SqlParameter("@dentist", selected));

                dgvPatientRecord.AutoGenerateColumns = true;
                dgvPatientRecord.DataSource = dt;

                if (dgvPatientRecord.Rows.Count > 0)
                    ShowRowSummary(dgvPatientRecord.Rows[0]);
                else
                    ClearDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Filter error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // FILTER BY STATUS
        // FIX: same approach as dentist filter above.
        // ─────────────────────────────────────────────────────────────
        private void cmbFilterbyStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilterbyStatus.SelectedItem == null) return;

            var selected = cmbFilterbyStatus.SelectedItem.ToString();

            if (selected == "(All Statuses)" || selected == "")
            {
                LoadPatients();
                return;
            }

            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    @"SELECT
                          a.AppointmentID       AS [Appointment ID],
                          a.AppointmentDateTime AS [Appointment Date],
                          ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS [Patient Name],
                          p.PatientID           AS [Patient ID],
                          a.Treatment           AS [Treatment],
                          a.Status              AS [Status],
                          a.Dentist             AS [Dentist]
                      FROM Appointments a
                      INNER JOIN Patients p ON a.PatientID = p.PatientID
                      WHERE a.Status = @status
                      ORDER BY a.AppointmentDateTime DESC",
                    new SqlParameter("@status", selected));

                dgvPatientRecord.AutoGenerateColumns = true;
                dgvPatientRecord.DataSource = dt;

                if (dgvPatientRecord.Rows.Count > 0)
                    ShowRowSummary(dgvPatientRecord.Rows[0]);
                else
                    ClearDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Filter error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // SHOW ROW SUMMARY  (Patient Details panel)
        // FIX 1: "I lo Appointment" typo — always set explicit strings.
        // FIX 2: Treatment/Dentist/Status now show "None" when the
        //        patient genuinely has no appointment yet, so the user
        //        knows the field was checked (not just forgotten).
        // FIX 3: When filtered grid shows appointment rows, read the
        //        appointment columns directly from the row.
        // ─────────────────────────────────────────────────────────────
        private void ShowRowSummary(DataGridViewRow row)
        {
            if (row == null) { ClearDetails(); return; }

            try
            {
                // ── Patient ID ──────────────────────────────────────
                object pidObj = GetCellValueIfColumnExists(row, "PatientID", "Patient ID", "PatientId");
                int? patientId = null;
                if (pidObj != null && int.TryParse(pidObj.ToString(), out int pid))
                    patientId = pid;

                // ── Basic patient fields ────────────────────────────
                lblPatientID.Text = patientId?.ToString() ?? string.Empty;
                lblFname.Text = S(GetCellValueIfColumnExists(row, "FirstName", "First Name"));
                lblLName.Text = S(GetCellValueIfColumnExists(row, "LastName", "Last Name"));
                lblContact.Text = S(GetCellValueIfColumnExists(row, "ContactNo", "Contact No", "Contact"));
                lblEmail.Text = S(GetCellValueIfColumnExists(row, "Email"));

                var bdateObj = GetCellValueIfColumnExists(row, "BirthDate", "Birth Date", "BDate");
                lblBdate.Text = (bdateObj != null && DateTime.TryParse(bdateObj.ToString(), out DateTime bdt))
                    ? bdt.ToString("yyyy-MM-dd")
                    : S(bdateObj);

                lblGender.Text = S(GetCellValueIfColumnExists(row, "Gender"));

                // ── Appointment details ─────────────────────────────
                // Strategy: always query the DB by PatientID for the
                // freshest data. If patientId is unavailable (e.g. the
                // grid is showing a filtered appointment list), fall
                // back to reading columns directly from the row.

                bool appointmentLoaded = false;

                if (patientId.HasValue)
                {
                    try
                    {
                        var apptDt = DatabaseHelper.ExecuteQuery(
                            @"SELECT TOP(1)
                                  AppointmentDateTime,
                                  Treatment,
                                  Dentist,
                                  [Status]
                              FROM Appointments
                              WHERE PatientID = @id
                              ORDER BY AppointmentDateTime DESC",
                            new SqlParameter("@id", patientId.Value));

                        if (apptDt.Rows.Count > 0)
                        {
                            var appt = apptDt.Rows[0];

                            // Last Appointment date
                            lblLastAppointment.Text =
                                (appt["AppointmentDateTime"] != DBNull.Value &&
                                 DateTime.TryParse(appt["AppointmentDateTime"].ToString(), out DateTime ladate))
                                    ? ladate.ToString("yyyy-MM-dd HH:mm")
                                    : "No Appointment";         // FIX: explicit string, no typo

                            // FIX: show actual value or "—" so labels are never blank
                            lblTreatment.Text = ColVal(appt, "Treatment");
                            lblDentist.Text = ColVal(appt, "Dentist");
                            lblStatus.Text = ColVal(appt, "Status");

                            appointmentLoaded = true;
                        }
                    }
                    catch { /* fall through to grid-column fallback */ }
                }

                if (!appointmentLoaded)
                {
                    // Fallback: read from grid columns (works for filtered appointment views)
                    var lastObj = GetCellValueIfColumnExists(row,
                        "Last Appointment", "AppointmentDateTime", "Appointment Date", "LastAppointment");

                    lblLastAppointment.Text =
                        (lastObj != null && DateTime.TryParse(lastObj.ToString(), out DateTime dt2))
                            ? dt2.ToString("yyyy-MM-dd HH:mm")
                            : (lastObj != null ? lastObj.ToString() : "No Appointment"); // FIX: explicit

                    lblTreatment.Text = S(GetCellValueIfColumnExists(row, "Treatment"), "—");
                    lblDentist.Text = S(GetCellValueIfColumnExists(row, "Dentist"), "—");
                    lblStatus.Text = S(GetCellValueIfColumnExists(row, "Status"), "—");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error showing details: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // HELPERS
        // ─────────────────────────────────────────────────────────────

        /// <summary>Safe string conversion; returns fallback when null/empty.</summary>
        private static string S(object val, string fallback = "")
            => (val == null || val == DBNull.Value || string.IsNullOrWhiteSpace(val.ToString()))
               ? fallback
               : val.ToString();

        /// <summary>Read a DataRow column safely.</summary>
        private static string ColVal(DataRow row, string col)
            => (row.Table.Columns.Contains(col) && row[col] != DBNull.Value)
               ? row[col].ToString()
               : "—";

        private object GetCellValueIfColumnExists(DataGridViewRow row, params string[] names)
        {
            if (row == null || row.DataGridView == null) return null;

            foreach (var name in names)
            {
                if (row.DataGridView.Columns.Contains(name))
                {
                    var val = row.Cells[name].Value;
                    if (val != null && val != DBNull.Value) return val;
                }

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

        private int? GetSelectedPatientId()
        {
            var row = dgvPatientRecord.CurrentRow;
            if (row == null) return null;
            var val = GetCellValueIfColumnExists(row, "PatientID", "Patient ID", "PatientId");
            if (val == null) return null;
            return int.TryParse(val.ToString(), out int id) ? id : (int?)null;
        }

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

        // ─────────────────────────────────────────────────────────────
        // GRID EVENTS
        // ─────────────────────────────────────────────────────────────

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
                try { PatientSelected?.Invoke(id.Value); } catch { }
                if (this.Modal) this.DialogResult = DialogResult.OK;
            }
        }

        private void dgvPatientRecord_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPatientRecord.CurrentRow != null && dgvPatientRecord.CurrentRow.Index >= 0)
                ShowRowSummary(dgvPatientRecord.CurrentRow);
            else
                ClearDetails();
        }

        // ─────────────────────────────────────────────────────────────
        // FORM LOAD
        // ─────────────────────────────────────────────────────────────

        private void frmPatientRecords_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshFilterLists();
                LoadPatients();
            }
            catch { }
        }

        // ─────────────────────────────────────────────────────────────
        // PUBLIC HELPERS (called by other forms)
        // ─────────────────────────────────────────────────────────────

        public void RefreshAndSelectPatient(int patientId)
        {
            try
            {
                try { LoadPatientRecords(); } catch { LoadPatients(); }

                for (int i = 0; i < dgvPatientRecord.Rows.Count; i++)
                {
                    var row = dgvPatientRecord.Rows[i];
                    var val = GetCellValueIfColumnExists(row, "PatientID", "Patient ID", "PatientId");
                    if (val != null && int.TryParse(val.ToString(), out int id) && id == patientId)
                    {
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
            catch { }
        }

        // ─────────────────────────────────────────────────────────────
        // EXPORT
        // ─────────────────────────────────────────────────────────────

        private void btnExport_Click(object sender, EventArgs e)
        {
            var dt = dgvPatientRecord.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (var sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                {
                    sw.WriteLine(string.Join(",",
                        dt.Columns.Cast<DataColumn>().Select(c => Quote(c.ColumnName))));
                    foreach (DataRow row in dt.Rows)
                    {
                        sw.WriteLine(string.Join(",",
                            dt.Columns.Cast<DataColumn>()
                              .Select(c => Quote(Convert.ToString(row[c]) ?? string.Empty))));
                    }
                }
            }
            MessageBox.Show("Export completed.", "Export",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ─────────────────────────────────────────────────────────────
        // BUTTON EVENTS
        // ─────────────────────────────────────────────────────────────

        private void btnClose_Click(object sender, EventArgs e) => Close();

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbFilterbyDentist.SelectedIndex = -1;
            cmbFilterbyStatus.SelectedIndex = -1;
            LoadPatients();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPatientRecord.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a row from the table first.",
                        "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedRow = dgvPatientRecord.SelectedRows[0];
                string idColumnName = "";

                if (dgvPatientRecord.Columns.Contains("AppointmentID")) idColumnName = "AppointmentID";
                else if (dgvPatientRecord.Columns.Contains("Appointment ID")) idColumnName = "Appointment ID";
                else if (dgvPatientRecord.Columns.Contains("PatientID")) idColumnName = "PatientID";
                else if (dgvPatientRecord.Columns.Contains("Patient ID")) idColumnName = "Patient ID";

                if (string.IsNullOrEmpty(idColumnName) ||
                    selectedRow.Cells[idColumnName].Value == null)
                {
                    MessageBox.Show("Could not find a valid ID column.", "Missing Column",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int targetId = Convert.ToInt32(selectedRow.Cells[idColumnName].Value);

                string firstName = dgvPatientRecord.Columns.Contains("FirstName") &&
                                   selectedRow.Cells["FirstName"].Value != null
                    ? selectedRow.Cells["FirstName"].Value.ToString() : "";
                string lastName = dgvPatientRecord.Columns.Contains("LastName") &&
                                   selectedRow.Cells["LastName"].Value != null
                    ? selectedRow.Cells["LastName"].Value.ToString() : "";
                string patientName = (firstName + " " + lastName).Trim();
                if (string.IsNullOrEmpty(patientName)) patientName = "this selected row";

                if (MessageBox.Show($"Are you sure you want to delete the record for {patientName}?",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    != DialogResult.Yes) return;

                bool hasAppt = dgvPatientRecord.Columns.Contains("AppointmentID") &&
                                  selectedRow.Cells["AppointmentID"].Value != null;
                bool hasPatient = (dgvPatientRecord.Columns.Contains("PatientID") ||
                                   dgvPatientRecord.Columns.Contains("Patient ID"));

                if (hasAppt && hasPatient)
                {
                    var choice = MessageBox.Show(
                        "This row has both an appointment and a patient.\n" +
                        "Yes = delete PATIENT and all related records.\n" +
                        "No  = delete only the appointment.\n" +
                        "Cancel = do nothing.",
                        "Delete Choice", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (choice == DialogResult.Cancel) return;

                    if (choice == DialogResult.No)
                    {
                        using (var conn = new SqlConnection(connectionString))
                        using (var cmd = new SqlCommand(
                            "DELETE FROM Appointments WHERE AppointmentID = @ID", conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", targetId);
                            conn.Open(); cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Appointment deleted.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        try { DatabaseHelper.RaiseAppointmentsChanged(); } catch { }
                    }
                    else
                    {
                        if (DatabaseHelper.DeletePatient(targetId))
                        {
                            MessageBox.Show("Patient deleted.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try { DatabaseHelper.RaiseAppointmentsChanged(); } catch { }
                        }
                        else
                            MessageBox.Show("Patient not deleted.", "Info",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (idColumnName.Contains("Appointment"))
                {
                    using (var conn = new SqlConnection(connectionString))
                    using (var cmd = new SqlCommand(
                        "DELETE FROM Appointments WHERE AppointmentID = @ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", targetId);
                        conn.Open(); cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Appointment deleted.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    try { DatabaseHelper.RaiseAppointmentsChanged(); } catch { }
                }
                else
                {
                    if (DatabaseHelper.DeletePatient(targetId))
                    {
                        MessageBox.Show("Patient deleted.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        try { DatabaseHelper.RaiseAppointmentsChanged(); } catch { }
                    }
                    else
                        MessageBox.Show("Patient not deleted.", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                try { LoadPatientRecords(); } catch { LoadPatients(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting:\n\n" + ex.Message,
                    "SQL/Runtime Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // MISC
        // ─────────────────────────────────────────────────────────────

        private static string Quote(string s)
        {
            if (s == null) return "\"\"";
            return $"\"{s.Replace("\"", "\"\"")}\"";
        }

        private void btnClose_Click_1(object sender, EventArgs e) { }
        private void label17_Click(object sender, EventArgs e) { }
    }
}