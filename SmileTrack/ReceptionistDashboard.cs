using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static SmileTrack.DentistDashboard;

namespace SmileTrack
{
    public partial class frmReceptionistDashboard : Form
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmileTrackDB;Integrated Security=True;Encrypt=False";

        public frmReceptionistDashboard()
        {
            InitializeComponent();
        }

        public void LoadDashboard()
        {
            try
            {
                // Appointments today (use DatabaseHelper for consistency)
                var dtAppointments = DatabaseHelper.ExecuteQuery(
                    @"SELECT FORMAT(a.AppointmentDateTime,'hh:mm tt') AS Time,
                             ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS PatientName,
                             a.Dentist, a.Treatment, a.Status
                      FROM Appointments a
                      LEFT JOIN Patients p ON a.PatientID = p.PatientID
                      WHERE ISNULL(a.Status,'') = 'Scheduled' 
                        AND CAST(a.AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)
                      ORDER BY a.AppointmentDateTime");
                dgvAppointments.AutoGenerateColumns = true;
                dgvAppointments.DataSource = dtAppointments;

                // Walk-ins today
                var dtWalkins = DatabaseHelper.ExecuteQuery(
                    @"SELECT ROW_NUMBER() OVER (ORDER BY a.AppointmentDateTime) AS [No],
                             ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS PatientName,
                             FORMAT(a.AppointmentDateTime,'hh:mm tt') AS TimeIn,
                             a.Status
                      FROM Appointments a
                      LEFT JOIN Patients p ON a.PatientID = p.PatientID
                      WHERE ISNULL(a.VisitType,'') = 'Walk-in'
                        AND CAST(a.AppointmentDateTime AS DATE) = CAST(GETDATE() AS DATE)
                      ORDER BY a.AppointmentDateTime");
                dgvWalkIn.AutoGenerateColumns = true;
                dgvWalkIn.DataSource = dtWalkins;

                // Reminders (future scheduled)
                var dtReminders = DatabaseHelper.ExecuteQuery(
                    @"SELECT CAST(a.AppointmentDateTime AS DATE) AS [Date],
                             ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS PatientName,
                             FORMAT(a.AppointmentDateTime,'hh:mm tt') AS Time,
                             a.Treatment
                      FROM Appointments a
                      LEFT JOIN Patients p ON a.PatientID = p.PatientID
                      WHERE ISNULL(a.Status,'') = 'Scheduled' AND a.AppointmentDateTime > GETDATE()
                      ORDER BY a.AppointmentDateTime");
                dgvReminders.AutoGenerateColumns = true;
                dgvReminders.DataSource = dtReminders;

                // Billing summary (today)
                LoadBillingSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBillingSummary()
        {
            try
            {
                // Paid invoices today: count and sum of PaidAmount
                var dtPaid = DatabaseHelper.ExecuteQuery(
                    @"SELECT COUNT(1) AS PaidCount, ISNULL(SUM(PaidAmount),0) AS PaidSum
                      FROM Invoices
                      WHERE CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE)
                        AND ISNULL(Status,'') = 'Paid'");

                // Unpaid invoices today: count and sum of BalanceAmount (or TotalAmount - PaidAmount)
                var dtUnpaid = DatabaseHelper.ExecuteQuery(
                    @"SELECT COUNT(1) AS UnpaidCount, ISNULL(SUM(BalanceAmount),0) AS UnpaidSum
                      FROM Invoices
                      WHERE CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE)
                        AND ISNULL(Status,'') <> 'Paid'");

                int paidCount = dtPaid.Rows.Count > 0 ? Convert.ToInt32(dtPaid.Rows[0]["PaidCount"]) : 0;
                decimal paidSum = dtPaid.Rows.Count > 0 ? Convert.ToDecimal(dtPaid.Rows[0]["PaidSum"]) : 0m;

                int unpaidCount = dtUnpaid.Rows.Count > 0 ? Convert.ToInt32(dtUnpaid.Rows[0]["UnpaidCount"]) : 0;
                decimal unpaidSum = dtUnpaid.Rows.Count > 0 ? Convert.ToDecimal(dtUnpaid.Rows[0]["UnpaidSum"]) : 0m;

                lblPaid.Text = paidCount.ToString();
                lblUnpaid.Text = unpaidCount.ToString();
                lblTotalRevenue.Text = paidSum.ToString("C"); // formatted currency

                // Optionally show tooltip or small text with sums
                lblPaid.Tag = paidSum;
                lblUnpaid.Tag = unpaidSum;
            }
            catch (Exception ex)
            {
                // keep UI stable if billing summary fails
                lblPaid.Text = "0";
                lblUnpaid.Text = "0";
                lblTotalRevenue.Text = "$0.00";
            }
        }

        private void frmReceptionistDashboard_Load(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            var patientForm = new Patient_Info_Appoinment();
            patientForm.Show();
        }

        private void btnBillings_Click(object sender, EventArgs e)
        {
            // optional: open billing module
        }
    }
}

























