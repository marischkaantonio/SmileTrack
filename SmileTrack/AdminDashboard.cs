using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace SmileTrack
{
    public partial class FormAdminDashboard : Form
    {
        private readonly string UsersFilePath = Path.Combine(Application.StartupPath, "users.json");
        private readonly string AuditFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "auditlog.json");

        public FormAdminDashboard()
        {
            InitializeComponent();

            // Ensure nav visible at startup
            Panel1.Visible = true;

            // Ensure click handlers are attached (idempotent)
            btnUserMngt.Click -= btnUserMngt_Click;
            btnUserMngt.Click += btnUserMngt_Click;

            btnDashboard.Click -= btnDashboard_Click_1;
            btnDashboard.Click += btnDashboard_Click_1;

            btnAuditLogs.Click -= btnAuditLogs_Click;
            btnAuditLogs.Click += btnAuditLogs_Click;
            }

        private void FormAdminDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                DatabaseHelper.EnsureDatabaseAndTables();

                panelDashboard.Visible = true;
                Panel1.Visible = false;


                LoadUsersFromDb();
                LoadAuditLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize Admin Dashboard: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveUsersToFile()
        {
            var users = new List<User>();

            foreach (DataGridViewRow row in dgvUserMngt.Rows)
            {
                if (row.IsNewRow) continue;

                string userName = row.Cells["UserName"].Value?.ToString();
                string password = row.Cells["Password"].Value?.ToString();
                string role = row.Cells["Role"].Value?.ToString();
                string status = row.Cells["Status"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                {

                    continue;
                }

                users.Add(new User
                {
                    UserName = userName,
                    Password = password,
                    Role = role,
                    Status = status
                });
            }


            string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("users.json", json);

            MessageBox.Show("Users saved successfully to users.json",
                            "Save Complete",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            panelDashboard.Visible = true;

            Panel1.Visible = false;

        }

        private void ShowPanel(Panel panelToShow, string headerText)
        {
            // Keep the left navigation visible
            Panel1.Visible = true;

            // If the panel to show is a child of the main dashboard container, display it inside the dashboard
            if (panelToShow != null && panelToShow.Parent == panelDashboard)
            {
                panelDashboard.Visible = true;

                // Show only the requested child of panelDashboard
                foreach (Control c in panelDashboard.Controls)
                {
                    c.Visible = (c == panelToShow);
                }

                panelToShow.Dock = DockStyle.Fill;
                panelToShow.BringToFront();
            }
            else
            {
                // Otherwise, hide the dashboard container and show the requested top-level panel
                panelDashboard.Visible = false;

                if (panelToShow != null)
                {
                    panelToShow.Visible = true;
                    panelToShow.Dock = DockStyle.Fill;
                    panelToShow.BringToFront();
                }
            }

            lblViewTitle.Text = headerText ?? string.Empty;
        }


        private void btnDashboard_Click_1(object sender, EventArgs e)
        {
            ShowPanel(panelDashboard, "Dashboard View");
        }

        private void btnUserMngt_Click(object sender, EventArgs e)
        {
            ShowPanel(panelUManagement, "User Management");
        }

        private void btnAuditLogs_Click(object sender, EventArgs e)
        {
            ShowPanel(Panel1, "Audit Logs");
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Log-out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                this.Hide();
                var login = new LoginForm();
                login.Show();


            }
        }

        private void LoadUsersFromDb()
        {
            try
            {
                dgvUserMngt.Rows.Clear();

                const string sql = @"
SELECT UserId, UserName, Password, Role, Status
FROM dbo.UsersSimple
ORDER BY UserName;";


                DataTable dt = DatabaseHelper.ExecuteQuery(sql);

                foreach (DataRow r in dt.Rows)
                {
                    int userId = Convert.ToInt32(r["UserId"]);
                    string userName = r["UserName"]?.ToString();
                    string password = r["Password"]?.ToString();
                    string role = r["Role"]?.ToString();
                    string status = r["Status"]?.ToString();

                    int idx = dgvUserMngt.Rows.Add(userName, role, password, status);
                    dgvUserMngt.Rows[idx].Tag = userId;
                }


                dgvUserMngt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load users from database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadUsersFromFile()
        {
            string filePath = Path.Combine(Application.StartupPath, "users.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var users = JsonConvert.DeserializeObject<List<User>>(json);

                dgvUserMngt.Rows.Clear();
                foreach (var u in users)
                {
                    dgvUserMngt.Rows.Add(u.UserName, u.Password, u.Role, u.Status);
                }
            }
            LoadAuditLogs();
        }

        private void LoadAuditLogs()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "auditlog.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var logs = JsonConvert.DeserializeObject<List<AuditLog>>(json);

                dgvAuditLogs.Rows.Clear();
                foreach (var log in logs)
                {
                    dgvAuditLogs.Rows.Add(
                        log.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                        log.User,
                        log.Action,
                        log.Details
                    );
                }
            }
        }


        private void btnHome_Click(object sender, EventArgs e)
        {
            FormAdminDashboard dashboard = new FormAdminDashboard();
            dashboard.Show();
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (dgvUserMngt.CurrentRow != null)
            {

                dgvUserMngt.BeginEdit(true);
            }
            else
            {
                MessageBox.Show("Please select a row to edit.",
                                "Edit User",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            if (dgvUserMngt.CurrentRow != null)
            {
                dgvUserMngt.Rows.Remove(dgvUserMngt.CurrentRow);
                MessageBox.Show("User successfully removed.");
            }
            else
            {
                MessageBox.Show("Please select a row to remove.");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (dgvUserMngt.CurrentRow != null)
            {
                DataGridViewRow row = dgvUserMngt.CurrentRow;

                string name = row.Cells["UserName"].Value?.ToString();
                string role = row.Cells["Role"].Value?.ToString();
                string password = row.Cells["Password"].Value?.ToString();
                string status = row.Cells["Status"].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    SaveUsersToFile();

                    MessageBox.Show("User successfully added.");
                }
                else
                {
                    MessageBox.Show("Please enter a valid name before adding.");
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to clear all audit logs?",
                               "Clear Logs",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                AuditLogger.ClearAuditLogs();
                dgvAuditLogs.Rows.Clear();
                MessageBox.Show("Audit logs cleared successfully.");
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            LoadAuditLogs();
        }



        private void FormAdminDashboard_Load_1(object sender, EventArgs e)
        {

            LoadUsersFromFile();

            LoadAuditLogs();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            try
            {
                var reportsForm = new Transaction_Billing_Reports();
                // show modal so user returns to dashboard after closing the report
                reportsForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Reports form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FormAdminDashboard dashboard = new FormAdminDashboard();
            dashboard.Show();
            this.Close();
        }
    }
}


      
    

    









































































































































































