using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SmileTrack
{
    public partial class BillingForm : Form
    {
        public BillingForm()
        {
            InitializeComponent();
            this.Load += BillingForm_Load;
            this.txtPname.Leave += txtPname_Leave;

            // CRITICAL: Ensure the event handler is connected
            this.dgvInvoices.CellContentClick += dgvInvoices_CellContentClick;
        }

        private void BillingForm_Load(object sender, EventArgs e)
        {
            // Setup Treatment List Grid
            if (dgvTreatmentList.Columns["Amount"] == null)
                dgvTreatmentList.Columns.Add(new DataGridViewTextBoxColumn { Name = "Amount", HeaderText = "Amount", ReadOnly = true });

            if (!(dgvTreatmentList.Columns["Status"] is DataGridViewComboBoxColumn))
            {
                int statusIndex = dgvTreatmentList.Columns["Status"]?.Index ?? -1;
                if (statusIndex >= 0) dgvTreatmentList.Columns.RemoveAt(statusIndex);
                var statusColumn = new DataGridViewComboBoxColumn { Name = "Status", HeaderText = "Status", Items = { "Unpaid", "Partial", "Fully Paid" } };
                dgvTreatmentList.Columns.Add(statusColumn);
            }

            dgvTreatmentList.AllowUserToAddRows = true;
            dgvTreatmentList.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvTreatmentList.CellEndEdit += dgvTreatmentList_CellEndEdit;

            // Setup Invoice Grid
            if (dgvInvoices.Columns["Action"] == null)
            {
                dgvInvoices.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "Action",
                    HeaderText = "Action",
                    Text = "Print Receipt",
                    UseColumnTextForButtonValue = true
                });
            }

            txtDiscount.TextChanged += TxtFields_TextChanged;
            txtTax.TextChanged += TxtFields_TextChanged;
            txtPayment.TextChanged += TxtFields_TextChanged;

            lblSubtotal.Text = "₱0.00";
            lblTotalAmount.Text = "₱0.00";
            lblBalance.Text = "₱0.00";

            LoadExistingInvoices();
        }

        private void txtPname_Leave(object sender, EventArgs e)
        {
            var name = txtPname.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)) return;
            PopulateTreatmentsForPatient(name);
        }

        private void PopulateTreatmentsForPatient(string patientName)
        {
            try
            {
                var sql = @"SELECT TOP(10) a.Treatment AS Treatment, ISNULL(a.Notes,'') AS Description, 1 AS Qty, 0.00 AS UnitPrice
                            FROM Appointments a
                            LEFT JOIN Patients p ON a.PatientID = p.PatientID
                            WHERE (ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'')) LIKE @name
                            ORDER BY a.AppointmentDateTime DESC;";

                var dt = DatabaseHelper.ExecuteQuery(sql, new SqlParameter("@name", "%" + patientName + "%"));
                if (dt == null || dt.Rows.Count == 0) return;

                dgvTreatmentList.Rows.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    int rowIndex = dgvTreatmentList.Rows.Add();
                    var row = dgvTreatmentList.Rows[rowIndex];
                    row.Cells["Treatment"].Value = r["Treatment"];
                    row.Cells["Description"].Value = r["Description"];
                    row.Cells["Qty"].Value = r["Qty"];
                    row.Cells["UnitPrice"].Value = Convert.ToDecimal(r["UnitPrice"]).ToString("#,##0.00");
                    row.Cells["Amount"].Value = (Convert.ToInt32(r["Qty"]) * Convert.ToDecimal(r["UnitPrice"])).ToString("#,##0.00");
                    row.Cells["Status"].Value = "Unpaid";
                }
                CalculateTotals();
            }
            catch (Exception ex) { MessageBox.Show("Failed to load treatments: " + ex.Message); }
        }

        private void TxtFields_TextChanged(object sender, EventArgs e) => CalculateTotals();

        private void CalculateTotals()
        {
            decimal subtotal = 0m;
            foreach (DataGridViewRow row in dgvTreatmentList.Rows)
            {
                if (row.IsNewRow) continue;
                decimal.TryParse(row.Cells["Qty"].Value?.ToString(), out decimal qty);
                decimal.TryParse(row.Cells["UnitPrice"].Value?.ToString(), out decimal unitPrice);
                decimal amount = qty * unitPrice;
                row.Cells["Amount"].Value = amount.ToString("#,##0.00");
                subtotal += amount;
            }

            decimal.TryParse(txtDiscount.Text, out decimal discountPercent);
            decimal.TryParse(txtTax.Text, out decimal taxPercent);
            decimal total = subtotal - (subtotal * (discountPercent / 100m)) + (subtotal * (taxPercent / 100m));

            lblSubtotal.Text = $"₱{subtotal:#,##0.00}";
            lblTotalAmount.Text = $"₱{total:#,##0.00}";
            decimal.TryParse(txtPayment.Text, out decimal payment);
            lblBalance.Text = $"₱{total - payment:#,##0.00}";
            lblStatus.Text = $"Status: {(payment == 0m ? "Unpaid" : payment < total ? "Partial" : "Fully Paid")}";
        }

        private void dgvTreatmentList_CellEndEdit(object sender, DataGridViewCellEventArgs e) => CalculateTotals();

        private void LoadExistingInvoices()
        {
            try
            {
                string sql = @"SELECT i.InvoiceNo, i.InvoiceDate, (p.FirstName + ' ' + p.LastName) AS PatientName, 
                               i.TotalAmount, i.PaidAmount, i.BalanceAmount, i.Status
                               FROM Invoices i
                               LEFT JOIN Patients p ON i.PatientID = p.PatientID
                               ORDER BY i.InvoiceDate DESC";
                DataTable dt = DatabaseHelper.ExecuteQuery(sql);
                dgvInvoices.DataSource = null;
                dgvInvoices.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Failed to refresh grid: " + ex.Message); }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceNum.Text)) { MessageBox.Show("Please enter an invoice number."); return; }

            CalculateTotals();
            string invoiceNo = txtInvoiceNum.Text.Trim();
            string patientName = txtPname.Text.Trim();
            decimal.TryParse(lblTotalAmount.Text.Replace("₱", "").Replace(",", ""), out decimal totalAmount);
            decimal.TryParse(txtPayment.Text, out decimal paidAmount);
            decimal balance = totalAmount - paidAmount;
            string status = (paidAmount == 0m) ? "Unpaid" : (paidAmount < totalAmount) ? "Partial" : "Fully Paid";

            try
            {
                DatabaseHelper.EnsureDatabaseAndTables();
                string patientSql = "SELECT TOP(1) PatientID FROM Patients WHERE (FirstName + ' ' + LastName) LIKE @pname";
                DataTable pdt = DatabaseHelper.ExecuteQuery(patientSql, new SqlParameter("@pname", "%" + patientName + "%"));
                object patientID = (pdt != null && pdt.Rows.Count > 0) ? pdt.Rows[0]["PatientID"] : DBNull.Value;

                string insertSql = @"INSERT INTO Invoices (InvoiceNo, PatientID, TotalAmount, PaidAmount, BalanceAmount, Status, InvoiceDate) 
                                     VALUES (@InvoiceNo, @PatientID, @TotalAmount, @PaidAmount, @BalanceAmount, @Status, @InvoiceDate);";

                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@InvoiceNo", invoiceNo);
                        cmd.Parameters.AddWithValue("@PatientID", patientID);
                        cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        cmd.Parameters.AddWithValue("@PaidAmount", paidAmount);
                        cmd.Parameters.AddWithValue("@BalanceAmount", balance);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@InvoiceDate", dtpInvoiceDate.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Invoice saved successfully!");
                LoadExistingInvoices();
            }
            catch (Exception ex) { MessageBox.Show("Sync Error: " + ex.Message); }
        }

        private void dgvInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvInvoices.Columns[e.ColumnIndex].Name == "Action")
            {
                DataGridViewRow row = dgvInvoices.Rows[e.RowIndex];
                string receiptText = $"--- OFFICIAL RECEIPT ---\n" +
                                     $"Invoice No: {row.Cells["InvoiceNo"].Value}\n" +
                                     $"Date: {row.Cells["InvoiceDate"].Value}\n" +
                                     $"Patient: {row.Cells["PatientName"].Value}\n" +
                                     $"Total Amount: ₱{row.Cells["TotalAmount"].Value}\n" +
                                     $"Paid Amount: ₱{row.Cells["PaidAmount"].Value}\n" +
                                     $"Balance: ₱{row.Cells["BalanceAmount"].Value}\n" +
                                     $"Status: {row.Cells["Status"].Value}";
                PrintReceipt(receiptText);
            }
        }

        private void PrintReceipt(string text)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (s, ev) => { ev.Graphics.DrawString(text, new Font("Arial", 12), Brushes.Black, new PointF(100, 100)); };
            using (PrintDialog printDialog = new PrintDialog())
            {
                printDialog.Document = printDoc;
                if (printDialog.ShowDialog() == DialogResult.OK) printDoc.Print();
            }
        }

        private void txtSearchP_TextChanged(object sender, EventArgs e)
        {
           
            string searchText = txtSearchP.Text.Trim();

            // Check if the data source is a DataTable
            if (dgvInvoices.DataSource is DataTable dt)
            {
                // Use DataView to filter the table
                DataView dv = dt.DefaultView;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    dv.RowFilter = string.Empty;
                }
                else
                {
                    // Filter by InvoiceNo or PatientName
                    dv.RowFilter = $"InvoiceNo LIKE '%{searchText}%' OR PatientName LIKE '%{searchText}%'";
                }
            }
        }
       

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string status = cmbStatus.SelectedItem?.ToString();
            if (dgvInvoices.DataSource is DataTable dt)
            {
                DataView dv = dt.DefaultView;
                if (string.IsNullOrWhiteSpace(status) || status == "All")
                {
                    dv.RowFilter = string.Empty;
                }
                else
                {
                    dv.RowFilter = $"Status = '{status}'";
                }
            }
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (dgvInvoices.DataSource is DataTable dt)
            {
                DataView dv = dt.DefaultView;
                // Assuming your column is 'InvoiceDate'
                string fromDate = dtpFrom.Value.ToString("yyyy-MM-dd");
                string toDate = dtpTo.Value.ToString("yyyy-MM-dd");

                dv.RowFilter = $"InvoiceDate >= '{fromDate}' AND InvoiceDate <= '{toDate}'";
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Hide();
            var receptionistForm = new frmReceptionistDashboard();
            receptionistForm.Show();
        }
    }
    }
