using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SmileTrack
{
    public partial class My_Schedule : Form
    {
        private readonly string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False";

        public My_Schedule()
        {
            InitializeComponent();

            this.Load -= My_Schedule_Load;
            this.Load += My_Schedule_Load;

            // Wire UI events (designer may already wire some)
            dtpDate.ValueChanged -= DtpDate_ValueChanged;
            dtpDate.ValueChanged += DtpDate_ValueChanged;

            cmbView.SelectedIndexChanged -= CmbView_SelectedIndexChanged;
            cmbView.SelectedIndexChanged += CmbView_SelectedIndexChanged;

            txtSearch.TextChanged -= TxtSearch_TextChanged;
            txtSearch.TextChanged += TxtSearch_TextChanged;

            btnDone.Click -= BtnDone_Click;
            btnDone.Click += BtnDone_Click;

            btnResched.Click -= BtnReschedule_Click;
            btnResched.Click += BtnReschedule_Click;

            btnCancel.Click -= BtnCancel_Click;
            btnCancel.Click += BtnCancel_Click;

            dgvSched.CellDoubleClick -= DgvSched_CellDoubleClick;
            dgvSched.CellDoubleClick += DgvSched_CellDoubleClick;
        }

        private void My_Schedule_Load(object sender, EventArgs e)
        {
            // Populate view options
            cmbView.Items.Clear();
            cmbView.Items.AddRange(new object[] { "All", "Today", "Upcoming", "Completed" });
            cmbView.SelectedIndex = 0;

            // Set date to today
            dtpDate.Value = DateTime.Today;

            // Setup grid columns if not provided by designer
            if (dgvSched.Columns.Count == 0)
            {
                dgvSched.Columns.Add("AppointmentID", "AppointmentID");
                dgvSched.Columns["AppointmentID"].Visible = false;

                dgvSched.Columns.Add("Time", "Time");
                dgvSched.Columns.Add("PatientName", "Patient Name");
                dgvSched.Columns.Add("Treatment", "Treatment");
                dgvSched.Columns.Add("Status", "Status");
                dgvSched.Columns.Add("Note", "Note");
                dgvSched.Columns.Add("Dentist", "Dentist");
            }

            LoadSchedule();
        }

        private void DtpDate_ValueChanged(object sender, EventArgs e) => LoadSchedule();

        private void CmbView_SelectedIndexChanged(object sender, EventArgs e) => LoadSchedule();

        private void TxtSearch_TextChanged(object sender, EventArgs e) => LoadSchedule();

        // Load appointments into grid with simple filters
        private void LoadSchedule()
        {
            try
            {
                string view = (cmbView.SelectedItem?.ToString() ?? "All");
                DateTime selectedDate = dtpDate.Value.Date;
                string search = txtSearch.Text?.Trim();

                string sql = @"
SELECT 
    a.AppointmentID,
    a.AppointmentDateTime,
    ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS PatientName,
    a.Treatment,
    a.Status,
    a.Notes,
    a.Dentist
FROM Appointments a
LEFT JOIN Patients p ON a.PatientID = p.PatientID
WHERE 1=1
";

                // Apply view filters
                if (view == "Today")
                {
                    sql += " AND CAST(a.AppointmentDateTime AS DATE) = @date";
                }
                else if (view == "Upcoming")
                {
                    sql += " AND a.AppointmentDateTime >= @now";
                }
                else if (view == "Completed")
                {
                    sql += " AND a.Status = 'Completed'";
                }
                else
                {
                    // All - still allow date filter if user explicitly changed date to limit results
                    // We'll show appointments for that date by default
                    sql += " AND CAST(a.AppointmentDateTime AS DATE) = @date";
                }

                if (!string.IsNullOrWhiteSpace(search))
                    sql += " AND (p.FirstName LIKE @search OR p.LastName LIKE @search OR a.Notes LIKE @search)";

                sql += " ORDER BY a.AppointmentDateTime";

                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, con))
                {
                    if (sql.Contains("@date"))
                        cmd.Parameters.AddWithValue("@date", selectedDate);

                    if (sql.Contains("@now"))
                        cmd.Parameters.AddWithValue("@now", DateTime.Now);

                    if (sql.Contains("@search"))
                        cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    var dt = new DataTable();
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    dgvSched.Rows.Clear();
                    foreach (DataRow r in dt.Rows)
                    {
                        var apptDT = Convert.ToDateTime(r["AppointmentDateTime"]);
                        int idx = dgvSched.Rows.Add(
                            r["AppointmentID"],
                            apptDT.ToString("hh:mm tt"),
                            r["PatientName"],
                            r["Treatment"],
                            r["Status"],
                            r["Notes"],
                            r["Dentist"]
                        );

                        // Optional: color rows by status
                        var row = dgvSched.Rows[idx];
                        var status = (r["Status"] ?? string.Empty).ToString();
                        if (string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase))
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                        else if (string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase))
                            row.DefaultCellStyle.BackColor = Color.LightCoral;
                        else if (apptDT < DateTime.Now && !string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase))
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading schedule: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper: get selected appointment id from grid
        private int? GetSelectedAppointmentId()
        {
            if (dgvSched.CurrentRow == null) return null;
            var val = dgvSched.CurrentRow.Cells["AppointmentID"].Value;
            if (val == null || val == DBNull.Value) return null;
            if (int.TryParse(val.ToString(), out int id)) return id;
            return null;
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            var id = GetSelectedAppointmentId();
            if (!id.HasValue)
            {
                MessageBox.Show("Please select an appointment.", "Select", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Mark selected appointment as Completed?", "Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("UPDATE Appointments SET Status = 'Completed' WHERE AppointmentID = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id.Value);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating appointment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            var id = GetSelectedAppointmentId();
            if (!id.HasValue)
            {
                MessageBox.Show("Please select an appointment.", "Select", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Cancel selected appointment? This cannot be undone.", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("UPDATE Appointments SET Status = 'Cancelled' WHERE AppointmentID = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id.Value);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cancelling appointment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReschedule_Click(object sender, EventArgs e)
        {
            var id = GetSelectedAppointmentId();
            if (!id.HasValue)
            {
                MessageBox.Show("Please select an appointment.", "Select", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Simple reschedule dialog (modal) with DateTimePicker
            using (var f = new Form { Text = "Reschedule Appointment", StartPosition = FormStartPosition.CenterParent, Size = new Size(320, 140), FormBorderStyle = FormBorderStyle.FixedDialog, MinimizeBox = false, MaximizeBox = false })
            {
                var dtp = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd hh:mm tt", ShowUpDown = true, Width = 260, Location = new Point(20, 10) };
                var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(50, 60), Size = new Size(80, 25) };
                var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(160, 60), Size = new Size(80, 25) };
                f.Controls.Add(dtp);
                f.Controls.Add(ok);
                f.Controls.Add(cancel);
                f.AcceptButton = ok;
                f.CancelButton = cancel;

                // pre-fill with existing appointment datetime
                DateTime existing = DateTime.Now;
                try
                {
                    using (var con = new SqlConnection(connectionString))
                    using (var cmd = new SqlCommand("SELECT AppointmentDateTime FROM Appointments WHERE AppointmentID = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", id.Value);
                        con.Open();
                        var obj = cmd.ExecuteScalar();
                        if (obj != null && obj != DBNull.Value) existing = Convert.ToDateTime(obj);
                    }
                }
                catch { /* ignore */ }

                dtp.Value = existing;

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        using (var con = new SqlConnection(connectionString))
                        using (var cmd = new SqlCommand("UPDATE Appointments SET AppointmentDateTime = @dt WHERE AppointmentID = @id", con))
                        {
                            cmd.Parameters.AddWithValue("@dt", dtp.Value);
                            cmd.Parameters.AddWithValue("@id", id.Value);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }

                        LoadSchedule();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error rescheduling: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DgvSched_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Double-click => reschedule
            if (e.RowIndex < 0) return;
            dgvSched.CurrentCell = dgvSched.Rows[e.RowIndex].Cells[0];
            BtnReschedule_Click(this, EventArgs.Empty);
        }
    }
}
