using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SmileTrack
{
    public partial class My_Schedule : Form
    {
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False";

        public My_Schedule()
        {
            InitializeComponent();

            // Wire events
            this.Load += My_Schedule_Load;
            dtpDate.ValueChanged += DtpDate_ValueChanged;
            cmbView.SelectedIndexChanged += CmbView_SelectedIndexChanged;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            btnDone.Click += btnDone_Click_1; // Binago para ituro sa updated button click event mo
            btnResched.Click += BtnReschedule_Click;
            btnCancel.Click += BtnCancel_Click;
            dgvSched.CellDoubleClick += dgvSched_CellDoubleClick;
        }

        private void My_Schedule_Load(object sender, EventArgs e)
        {
            cmbView.Items.Clear();
            cmbView.Items.AddRange(new object[] { "All", "Today", "Upcoming", "Completed" });
            cmbView.SelectedIndex = 0;
            dtpDate.Value = DateTime.Today;

            if (dgvSched.Columns.Count == 0)
            {
                // Siguraduhing naka-set nang maayos ang Name ng mga Columns
                int idIdx = dgvSched.Columns.Add("AppointmentID", "AppointmentID");
                dgvSched.Columns[idIdx].Name = "AppointmentID";
                dgvSched.Columns[idIdx].Visible = false;

                int timeIdx = dgvSched.Columns.Add("Time", "Time");
                dgvSched.Columns[timeIdx].Name = "Time";

                int nameIdx = dgvSched.Columns.Add("PatientName", "Patient Name");
                dgvSched.Columns[nameIdx].Name = "PatientName";

                int treatIdx = dgvSched.Columns.Add("Treatment", "Treatment");
                dgvSched.Columns[treatIdx].Name = "Treatment";

                int statIdx = dgvSched.Columns.Add("Status", "Status");
                dgvSched.Columns[statIdx].Name = "Status";

                int noteIdx = dgvSched.Columns.Add("Note", "Note");
                dgvSched.Columns[noteIdx].Name = "Note";

                int dentIdx = dgvSched.Columns.Add("Dentist", "Dentist");
                dgvSched.Columns[dentIdx].Name = "Dentist";
            }
            LoadSchedule();
        }

        private void DtpDate_ValueChanged(object sender, EventArgs e) => LoadSchedule();
        private void CmbView_SelectedIndexChanged(object sender, EventArgs e) => LoadSchedule();
        private void TxtSearch_TextChanged(object sender, EventArgs e) => LoadSchedule();

        private void LoadSchedule()
        {
            try
            {
                string view = (cmbView.SelectedItem?.ToString() ?? "All");
                DateTime selectedDate = dtpDate.Value.Date;
                string search = txtSearch.Text?.Trim();

                string sql = @"SELECT a.AppointmentID, a.AppointmentDateTime, 
                               ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS PatientName, 
                               a.Treatment, a.Status, a.Notes, a.Dentist 
                               FROM Appointments a 
                               LEFT JOIN Patients p ON a.PatientID = p.PatientID 
                               WHERE 1=1 ";

                if (view == "Today") sql += " AND CAST(a.AppointmentDateTime AS DATE) = @date";
                else if (view == "Upcoming") sql += " AND a.AppointmentDateTime >= @now";
                else if (view == "Completed") sql += " AND a.Status = 'Completed'";

                if (!string.IsNullOrWhiteSpace(search))
                    sql += " AND (p.FirstName LIKE @search OR p.LastName LIKE @search OR a.Notes LIKE @search)";

                sql += " ORDER BY a.AppointmentDateTime";

                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@date", selectedDate);
                    cmd.Parameters.AddWithValue("@now", DateTime.Now);
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    var dt = new DataTable();
                    using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);

                    dgvSched.Rows.Clear();
                    foreach (DataRow r in dt.Rows)
                    {
                        var apptDT = Convert.ToDateTime(r["AppointmentDateTime"]);
                        int idx = dgvSched.Rows.Add(r["AppointmentID"], apptDT.ToString("hh:mm tt"), r["PatientName"], r["Treatment"], r["Status"], r["Notes"], r["Dentist"]);

                        var row = dgvSched.Rows[idx];
                        string status = r["Status"].ToString();
                        if (status == "Completed") row.DefaultCellStyle.BackColor = Color.LightGreen;
                        else if (status == "Cancelled") row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading: " + ex.Message); }
        }

        private int? GetSelectedAppointmentId()
        {
            if (dgvSched.CurrentRow == null) return null;
            var val = dgvSched.CurrentRow.Cells[0].Value;
            if (val != null && int.TryParse(val.ToString(), out int id)) return id;
            return null;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            var id = GetSelectedAppointmentId();
            if (id == null) { MessageBox.Show("Select an appointment!"); return; }
            if (MessageBox.Show("Cancel this appointment?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ExecuteUpdate("UPDATE Appointments SET Status = 'Cancelled' WHERE AppointmentID = @id", id.Value);
            }
        }

        private void BtnReschedule_Click(object sender, EventArgs e)
        {
            var id = GetSelectedAppointmentId();
            if (id == null) { MessageBox.Show("Select an appointment!"); return; }

            MessageBox.Show("Reschedule logic triggered for ID: " + id);
        }

        private void ExecuteUpdate(string query, int id)
        {
            try
            {
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open(); cmd.ExecuteNonQuery();
                }
                LoadSchedule();
                try { DatabaseHelper.RaiseAppointmentsChanged(); } catch { }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void dgvSched_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) BtnReschedule_Click(this, EventArgs.Empty);
        }

        
        private void btnDone_Click_1(object sender, EventArgs e)
        {
            if (dgvSched.CurrentRow == null)
            {
                MessageBox.Show("Mangyaring pumili muna ng appointment mula sa listahan.");
                return;
            }

           
            var appointmentId = GetSelectedAppointmentId();
            if (appointmentId == null)
            {
                MessageBox.Show("Hindi mahanap ang valid na Appointment ID.");
                return;
            }

            string patientName = dgvSched.CurrentRow.Cells[2].Value?.ToString() ?? "Patient";

            if (MessageBox.Show($"Mark appointment for {patientName} as Completed?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string query = "UPDATE Appointments SET Status = 'Completed' WHERE AppointmentID = @AppID";

                   
                    using (var con = new SqlConnection(connectionString))
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@AppID", appointmentId.Value);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    // I-refresh ang data sa DataGridView pagkatapos mag-update
                    LoadSchedule();

                    // Mga real-time notification helpers mo
                    try
                    {
                        DatabaseHelper.NotifyAppointmentsChanged();
                        DatabaseHelper.RaiseAppointmentsChanged();
                    }
                    catch { }

                    DatabaseHelper.TriggerNotification($"Patient {patientName} record has been updated to COMPLETED.");

                    MessageBox.Show("Appointment marked as done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}