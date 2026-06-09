using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Text.Json;


namespace SmileTrack
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private List<User> LoadUsersFromFile()
        {

            if (File.Exists("users.json"))
            {
                string json = File.ReadAllText("users.json");
                var users = JsonConvert.DeserializeObject<List<User>>(json);
                return users ?? new List<User>();
            }
            else
            {
                return new List<User>();
            }
        }
        //yes

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUname.Text;
            string password = txtPsswrd.Text;

            bool loggedIn = false;

           
            if (username == "admin" && password == "admin123")
            {
                new FormAdminDashboard().Show();
                this.Hide();
                loggedIn = true;
            }
            else if ((username == "dr.Margie" && password == "Marg123") ||
                     (username == "dr.Dapeg" && password == "Dapeg"))
            {
                new DentistDashboard().Show();
                this.Hide();
                loggedIn = true;
            }
            else if (username == "recept1" && password == "recept")
            {
                new frmReceptionistDashboard().Show();
                this.Hide();
                loggedIn = true;
            }

            
            if (!loggedIn)
            {
                var users = LoadUsersFromFile();
                var match = users.FirstOrDefault(u =>
                    u.UserName == username && u.Password == password && u.Status == "Active");

                if (match != null)
                {

                    MessageBox.Show($"Welcome {match.UserName}! Role: {match.Role}\nLogin Success",
                                    "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    switch (match.Role.ToLower())
                    {
                        case "admin":
                            new FormAdminDashboard().Show();
                            break;
                        case "doctor":
                        case "dentist":
                            new DentistDashboard().Show();
                            break;
                        case "receptionist":
                            new frmReceptionistDashboard().Show();
                            break;
                        default:
                            MessageBox.Show("Unknown role. Please check user setup.");
                            break;
                    }

                    this.Hide();
                    loggedIn = true;
                }
            }

            
            if (!loggedIn)
            {
                MessageBox.Show("Invalid username or password.",
                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            AuditLogger.SaveAuditLog(username, "Login", "User logged in successfully");
           
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            
            txtUname.Clear();
            txtPsswrd.Clear();

            
            txtUname.Focus();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        
        }
    }
    }
