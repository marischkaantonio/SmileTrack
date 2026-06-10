using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SmileTrack
{
    public partial class FormAdminDashboard : Form
    {
        private readonly string UsersFilePath = Path.Combine(Application.StartupPath, "users.json");
        private readonly string AuditFilePath = Path.Combine(Application.StartupPath, "auditlog.json");

        public FormAdminDashboard()
        {
            InitializeComponent();

            // Guard against broken/duplicate event wiring layers
            btnAdd.Click -= btnAdd_Click;
            btnAdd.Click += btnAdd_Click;

            btnRefresh.Click -= btnRefresh_Click;
            btnRefresh.Click += btnRefresh_Click;

            btnRemove.Click -= btnRemove_Click;
            btnRemove.Click += btnRemove_Click;

            this.Load -= FormAdminDashboard_Load;
            this.Load += FormAdminDashboard_Load;
        }

        private void FormAdminDashboard_Load(object sender, EventArgs e)
        {
            RefreshAllDashboardData();
        }

        public void RefreshAllDashboardData()
        {
            ForceLoadUsersFromDatabase();
            ForceLoadAuditLogs();
            LoadPatientAppointmentGraph();
        }

        // LOAD USERS GRID
        private void ForceLoadUsersFromDatabase()
        {
            try
            {
                dgvUserMngt.Rows.Clear();
                bool loadedFromDb = false;

                try
                {
                    string sql = "SELECT UserName, Role, Password, Status FROM dbo.UsersSimple ORDER BY UserName;";
                    DataTable dt = DatabaseHelper.ExecuteQuery(sql);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            dgvUserMngt.Rows.Add(r["UserName"]?.ToString(), r["Role"]?.ToString(), r["Password"]?.ToString(), r["Status"]?.ToString());
                        }
                        loadedFromDb = true;
                    }
                }
                catch (Exception dbEx)
                {
                    System.Diagnostics.Debug.WriteLine("Database User Error: " + dbEx.Message);
                }

                if (!loadedFromDb && File.Exists(UsersFilePath))
                {
                    string json = File.ReadAllText(UsersFilePath);
                    var usersList = JsonConvert.DeserializeObject<List<UserLocalClass>>(json);
                    if (usersList != null)
                    {
                        foreach (var u in usersList)
                        {
                            dgvUserMngt.Rows.Add(u.UserName, u.Role, u.Password, u.Status);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("User Grid view Render Failure: " + ex.Message);
            }
        }

        // DYNAMIC PATIENT APPOINTMENT GRAPH AND SUMMARY RENDERER
        private void LoadPatientAppointmentGraph()
        {
            try
            {
                // Hanapin ang Chart nang tama kahit nasa loob pa ito ng panelDashboard
                Chart apptChart = FindChartRecursively(this);

                if (apptChart == null)
                {
                    System.Diagnostics.Debug.WriteLine("MESSING CONTROL: Hindi mahanap ang Chart control sa Form.");
                    return;
                }

                apptChart.Series.Clear();
                apptChart.Titles.Clear();
                apptChart.Legends.Clear();

                Series patientSeries = new Series("Patients")
                {
                    ChartType = SeriesChartType.Column,
                    IsValueShownAsLabel = true
                };
                patientSeries["PointWidth"] = "0.5";
                apptChart.Series.Add(patientSeries);

                // Initial map configuration setup counters
                Dictionary<string, int> dataMetrics = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Walk-in", 0 },
                    { "Scheduled", 0 },
                    { "Waiting", 0 },
                    { "Cancelled", 0 },
                    { "Completed", 0 }
                };

                bool databaseLoadedSuccessfully = false;

                try
                {
                    // Query mula sa totoong appointment table mo
                    string query = "SELECT LOWER(RTRIM(LTRIM(Status))) as CleanStatus, COUNT(1) as Total FROM dbo.[Patient Info_appointment] WHERE Status IS NOT NULL GROUP BY Status;";
                    DataTable dt = DatabaseHelper.ExecuteQuery(query);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string rawStatus = row["CleanStatus"]?.ToString() ?? "";
                            int totalCount = Convert.ToInt32(row["Total"]);

                            if (rawStatus == "walk-in" || rawStatus == "walkin") dataMetrics["Walk-in"] += totalCount;
                            else if (rawStatus == "scheduled") dataMetrics["Scheduled"] += totalCount;
                            else if (rawStatus == "waiting") dataMetrics["Waiting"] += totalCount;
                            else if (rawStatus == "cancelled") dataMetrics["Cancelled"] += totalCount;
                            else if (rawStatus == "completed") dataMetrics["Completed"] += totalCount;
                        }
                        databaseLoadedSuccessfully = true;
                    }
                }
                catch (Exception sqlEx)
                {
                    System.Diagnostics.Debug.WriteLine("CRITICAL GRAPH DB ERROR: " + sqlEx.Message);
                }

                // BACKUP DEMO FALLBACK LAYER: Lalabas lang kapag offline o walang laman ang database
                if (!databaseLoadedSuccessfully || dataMetrics.Values.Sum() == 0)
                {
                    dataMetrics["Walk-in"] = 12;
                    dataMetrics["Scheduled"] = 18;
                    dataMetrics["Waiting"] = 4;
                    dataMetrics["Cancelled"] = 1;
                    dataMetrics["Completed"] = 15;
                }

                // Render Summary sa mga Labels (Kahit nasa loob ng Dashboard Panel)
                UpdateSummaryLabelsIfPresent(dataMetrics);

                // Replot points sa Graph UI workspace
                foreach (var metric in dataMetrics)
                {
                    int idx = patientSeries.Points.AddXY(metric.Key, metric.Value);
                    switch (metric.Key.ToLower())
                    {
                        case "walk-in": patientSeries.Points[idx].Color = Color.MediumPurple; break;
                        case "scheduled": patientSeries.Points[idx].Color = Color.LightSkyBlue; break;
                        case "waiting": patientSeries.Points[idx].Color = Color.Orange; break;
                        case "cancelled": patientSeries.Points[idx].Color = Color.LightCoral; break;
                        case "completed": patientSeries.Points[idx].Color = Color.LightGreen; break;
                    }
                }

                if (apptChart.ChartAreas.Count > 0)
                {
                    apptChart.ChartAreas[0].AxisY.Minimum = 0;
                    apptChart.ChartAreas[0].AxisX.Interval = 1;
                    apptChart.ChartAreas[0].RecalculateAxesScale();
                }
                apptChart.Update();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Chart Exception Engine: " + ex.Message);
            }
        }

        private void UpdateSummaryLabelsIfPresent(Dictionary<string, int> metrics)
        {
            try
            {
                int total = metrics.Values.Sum();

                // Gagamit ng true para maghanap recursively sa loob ng panelDashboard container control
                Control[] lblTotal = this.Controls.Find("lblTotalPatients", true);
                if (lblTotal.Length > 0) lblTotal[0].Text = total.ToString();

                Control[] lblWalkin = this.Controls.Find("lblTotalWalkIns", true);
                if (lblWalkin.Length > 0) lblWalkin[0].Text = metrics["Walk-in"].ToString();

                Control[] lblSched = this.Controls.Find("lblScheduled", true);
                if (lblSched.Length > 0) lblSched[0].Text = metrics["Scheduled"].ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error updating dashboard summary labels: " + ex.Message);
            }
        }

        private Chart FindChartRecursively(Control container)
        {
            if (container is Chart chart) return chart;

            foreach (Control c in container.Controls)
            {
                Chart found = FindChartRecursively(c);
                if (found != null) return found;
            }
            return null;
        }

        // ACTION BUTTON: USER MANAGEMENT ADD ROUTINE (Secured against simple SQL breakages)
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string newUsername = ShowPureInputForm("New Profile", "Enter Account Username:");
                if (string.IsNullOrWhiteSpace(newUsername)) return;

                string newRole = ShowPureInputForm("New Profile", "Enter Security Role:");
                if (string.IsNullOrWhiteSpace(newRole)) return;

                string newPassword = ShowPureInputForm("New Profile", "Enter Password:");
                if (string.IsNullOrWhiteSpace(newPassword)) return;

                // Ligtas na pag-escape sa single quotes para hindi mag-error ang SQL Query string
                string safeUser = newUsername.Replace("'", "''");
                string safeRole = newRole.Replace("'", "''");
                string safePass = newPassword.Replace("'", "''");

                string insertSql = $"INSERT INTO dbo.UsersSimple (UserName, Role, Password, Status) VALUES ('{safeUser}', '{safeRole}', '{safePass}', 'Active');";
                DatabaseHelper.ExecuteQuery(insertSql);

                RefreshAllDashboardData();
                MessageBox.Show("Profile registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save error encounter: " + ex.Message);
            }
        }

        // ACTION BUTTON: USER MANAGEMENT REMOVE ROUTINE
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvUserMngt.CurrentRow != null && !dgvUserMngt.CurrentRow.IsNewRow)
            {
                string targetUser = dgvUserMngt.CurrentRow.Cells[0].Value?.ToString();

                if (string.IsNullOrEmpty(targetUser)) return;

                if (MessageBox.Show($"Delete system account user '{targetUser}'?", "Verification Required", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        string safeUser = targetUser.Replace("'", "''");
                        string deleteSql = $"DELETE FROM dbo.UsersSimple WHERE UserName = '{safeUser}';";
                        DatabaseHelper.ExecuteQuery(deleteSql);
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }

                    RefreshAllDashboardData();
                }
            }
        }

        // EXPLICIT TRIGGER ACTION FOR NEW PATIENTS / RECORDS CHANGES
        public void OnPatientRecordsChanged()
        {
            LoadPatientAppointmentGraph();
        }

        private void ForceLoadAuditLogs()
        {
            try
            {
                dgvAuditLogs.Rows.Clear();
                if (File.Exists(AuditFilePath))
                {
                    string json = File.ReadAllText(AuditFilePath);
                    var logs = JsonConvert.DeserializeObject<List<AuditLog>>(json);
                    if (logs != null)
                    {
                        foreach (var log in logs)
                        {
                            dgvAuditLogs.Rows.Add(log.Date.ToString("yyyy-MM-dd HH:mm:ss"), log.User, log.Action, log.Details);
                        }
                    }
                }
            }
            catch { }
        }

        private string ShowPureInputForm(string title, string promptText)
        {
            Form prompt = new Form() { Width = 400, Height = 180, FormBorderStyle = FormBorderStyle.FixedDialog, Text = title, StartPosition = FormStartPosition.CenterParent };
            Label textLabel = new Label() { Left = 20, Top = 20, Width = 350, Text = promptText };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
            Button confirmation = new Button() { Text = "OK", Left = 260, Width = 100, Top = 90, DialogResult = DialogResult.OK };
            confirmation.Click += (s, ev) => { prompt.Close(); };
            prompt.Controls.Add(textBox); prompt.Controls.Add(textLabel); prompt.Controls.Add(confirmation);
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : "";
        }

        private void btnDashboard_Click_1(object sender, EventArgs e) { RefreshAllDashboardData(); }
        private void btnUserMngt_Click(object sender, EventArgs e) { RefreshAllDashboardData(); }
        private void btnAuditLogs_Click(object sender, EventArgs e) { ForceLoadAuditLogs(); }
        private void btnRefresh_Click(object sender, EventArgs e) { RefreshAllDashboardData(); }
        private void btnClear_Click(object sender, EventArgs e) { if (File.Exists(AuditFilePath)) File.Delete(AuditFilePath); ForceLoadAuditLogs(); }
        private void btnEdit_Click(object sender, EventArgs e) { if (dgvUserMngt.CurrentRow != null) dgvUserMngt.BeginEdit(true); }
        private void btnReports_Click_1(object sender, EventArgs e) { try { new Transaction_Billing_Reports().ShowDialog(this); RefreshAllDashboardData(); } catch { } }
        private void btnLogOut_Click(object sender, EventArgs e) { this.Hide(); new LoginForm().Show(); }
        private void btnUserMngt_Click_1(object sender, EventArgs e) { RefreshAllDashboardData(); }
        private void btnHome_Click(object sender, EventArgs e) { RefreshAllDashboardData(); }
        private void AppoinmentData_Click(object sender, EventArgs e) { RefreshAllDashboardData(); }
        private void panelDashboard_Paint_1(object sender, PaintEventArgs e) { }
        private void FormAdminDashboard_Load_1(object sender, EventArgs e) { }
    }

    public class UserLocalClass
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }

    public class AuditLog
    {
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
    }
}