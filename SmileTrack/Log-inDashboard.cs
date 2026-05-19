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

namespace SmileTrack
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUname.Text;
            string password = txtPsswrd.Text;

            if (username == "admin" && password == "admin123")
            {
                AdminDashboard admin = new AdminDashboard();
                admin.Show();
                this.Hide();
            }
            else if ((username == "dr.Margie" && password == "Marg123") ||
                (username == "dr.Dapeg" && password == "Dapeg"))
            {
                DentistDashboard dentist = new DentistDashboard();
                dentist.Show();
                this.Hide();
            }
            else if (username == "recept1" && password == "recept") 
            {
                ReceptionistDashboard receptionist = new ReceptionistDashboard();
                receptionist.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            ReceptionistDashboard receptionistDashboard = new ReceptionistDashboard();
            receptionistDashboard.LoggedInUser = txtUname.Text;
            receptionistDashboard.Show();
        }
    }
}