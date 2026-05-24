using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Text.Json;


namespace SmileTrack
{
    public partial class FormAdminDashboard : Form
    {
        public FormAdminDashboard()
        {

            InitializeComponent();
        }
        private void SaveUsersToFile()
        {
            var users = new List<User>();

            foreach (DataGridViewRow row in dgvUserMngt.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["UserName"].Value != null && row.Cells["Password"].Value != null)
                {
                    users.Add(new User
                    {
                        UserName = row.Cells["UserName"].Value?.ToString(),
                        Password = row.Cells["Password"].Value?.ToString(),
                        Role = row.Cells["Role"].Value?.ToString(),
                        Status = row.Cells["Status"].Value?.ToString()
                    });
                }
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
            panelUserManagement.Visible = false;
            panelReports.Visible = false;
            panelAuditLogs.Visible = false;
        }

        private void btnDashboard_Click_1(object sender, EventArgs e)
        {
            panelDashboard.Visible = true;
            panelUserManagement.Visible = false;
            panelReports.Visible = false;
            panelAuditLogs.Visible = false;
        }


        private void btnUserMngt_Click(object sender, EventArgs e)
        {
            panelDashboard.Visible = false;
            panelUserManagement.Visible = true;
            panelReports.Visible = false;
            panelAuditLogs.Visible = false;
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            panelDashboard.Visible = false;
            panelUserManagement.Visible = false;
            panelReports.Visible = true;
            panelAuditLogs.Visible = false;
        }

        private void btnAuditLogs_Click(object sender, EventArgs e)
        {
            panelDashboard.Visible = false;
            panelUserManagement.Visible = false;
            panelReports.Visible = false;
            panelAuditLogs.Visible = true;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?",
                                 "Log-out",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                this.Hide();


                LoginForm login = new LoginForm();
                login.Show();


            }
        }

        private void LoadUserData()
        {


            dgvUserMngt.Columns.Clear();


            dgvUserMngt.Columns.Add("Name", "Name");
            dgvUserMngt.Columns.Add("Role", "Role");
            dgvUserMngt.Columns.Add("Status", "Status");


            dgvUserMngt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
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

        private void btnEdit_Click(object sender, EventArgs e)
        {


            if (dgvAuditLogs.CurrentRow != null)
            {

                dgvAuditLogs.BeginEdit(true);
            }
            else
            {
                MessageBox.Show("Please select a row to edit.",
                                "Edit User",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }

        private void FormAdminDashboard_Load(object sender, EventArgs e)
        {
            LoadUsersFromFile();

            LoadAuditLogs();

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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAuditLogs();
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

    }

}




  










