using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SmileTrack
{
    public partial class BillingForm : Form
    {
        public BillingForm()
        {
            InitializeComponent();
            this.Load += BillingForm_Load;

            // wire patient name leave to auto-populate treatments
            this.txtPname.Leave -= txtPname_Leave;
            this.txtPname.Leave += txtPname_Leave;
        }

        private void BillingForm_Load(object sender, EventArgs e)
        {
            // Ensure treatment grid has expected columns
            if (dgvTreatmentList.Columns["Amount"] == null)
            {
                var amountCol = new DataGridViewTextBoxColumn
                {
                    Name = "Amount",
                    HeaderText = "Amount",
                    ReadOnly = true
                };
                dgvTreatmentList.Columns.Add(amountCol);
            }

            // Ensure Status is a combo column
            if (!(dgvTreatmentList.Columns["Status"] is DataGridViewComboBoxColumn))
            {
                int statusIndex = dgvTreatmentList.Columns["Status"]?.Index ?? -1;
                if (statusIndex >= 0)
                {
                    dgvTreatmentList.Columns.RemoveAt(statusIndex);
                }

                var statusColumn = new DataGridViewComboBoxColumn
                {
                    Name = "Status",
                    HeaderText = "Status",
                    Items = { "Unpaid", "Partial", "Fully Paid" }
                };
                dgvTreatmentList.Columns.Add(statusColumn);
            }

            dgvTreatmentList.AllowUserToAddRows = true;
            dgvTreatmentList.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvTreatmentList.CellEndEdit -= dgvTreatmentList_CellEndEdit;
            dgvTreatmentList.CellEndEdit += dgvTreatmentList_CellEndEdit;

            // Wire invoice grid action button if not already present
            if (dgvInvoices.Columns["Action"] == null)
            {
                var actionColumn = new DataGridViewButtonColumn
                {
                    Name = "Action",
                    HeaderText = "Action",
                    Text = "Print Receipt",
                    UseColumnTextForButtonValue = true
                };
                dgvInvoices.Columns.Add(actionColumn);
            }

            // Hook text change events to recalc totals
            txtDiscount.TextChanged -= TxtFields_TextChanged;
            txtTax.TextChanged -= TxtFields_TextChanged;
            txtPayment.TextChanged -= TxtFields_TextChanged;

            txtDiscount.TextChanged += TxtFields_TextChanged;
            txtTax.TextChanged += TxtFields_TextChanged;
            txtPayment.TextChanged += TxtFields_TextChanged;

            // Initialize labels
            lblSubtotal.Text = "₱0.00";
            lblTotalAmount.Text = "₱0.00";
            lblBalance.Text = "₱0.00";

            // Kusa nitong ikokonekta at ilo-load ang mga dating invoice mula sa database papunta sa dgvInvoices
            LoadExistingInvoices();
        }

        // ========================================================
        // KONEKSYON SA DATABASE: I-LOAD ANG MGA INVOICES PATUNGO SA LOWER GRID
        // ========================================================
        private void LoadExistingInvoices()
        {
            try
            {
                string sql = @"
                    SELECT 
                        i.InvoiceID, 
                        i.InvoiceDate, 
                        ISNULL(p.FirstName, '') + ' ' + ISNULL(p.LastName, '') AS PatientName, 
                        i.TotalAmount, 
                        i.PaidAmount, 
                        i.BalanceAmount, 
                        i.Status 
                    FROM Invoices i
                    LEFT JOIN Patients p ON i.PatientID = p.PatientID
                    ORDER BY i.InvoiceDate DESC;";

                DataTable dt = DatabaseHelper.ExecuteQuery(sql);
                dgvInvoices.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    dgvInvoices.Rows.Add(
                        row["InvoiceID"],
                        Convert.ToDateTime(row["InvoiceDate"]).ToString("MM/dd/yyyy"),
                        row["PatientName"],
                        Convert.ToDecimal(row["TotalAmount"]).ToString("#,##0.00"),
                        Convert.ToDecimal(row["PaidAmount"]).ToString("#,##0.00"),
                        Convert.ToDecimal(row["BalanceAmount"]).ToString("#,##0.00"),
                        row["Status"]
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading invoices from database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                var sql = @"
                    SELECT TOP(10)
                        a.Treatment AS Treatment,
                        ISNULL(a.Notes,'') AS Description,
                        1 AS Qty,
                        0.00 AS UnitPrice
                    FROM Appointments a
                    LEFT JOIN Patients p ON a.PatientID = p.PatientID
                    WHERE (ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'')) LIKE @name
                    ORDER BY a.AppointmentDateTime DESC;";

                var dt = DatabaseHelper.ExecuteQuery(sql, new SqlParameter("@name", "%" + patientName + "%"));

                if (dt == null || dt.Rows.Count == 0) return;

                dgvTreatmentList.Rows.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    var treatment = r["Treatment"]?.ToString() ?? string.Empty;
                    var desc = r["Description"]?.ToString() ?? string.Empty;
                    var qty = Convert.ToInt32(r["Qty"]);
                    var unitPrice = Convert.ToDecimal(r["UnitPrice"]);

                    int rowIndex = dgvTreatmentList.Rows.Add();
                    var row = dgvTreatmentList.Rows[rowIndex];

                    if (dgvTreatmentList.Columns["Treatment"] != null) row.Cells["Treatment"].Value = treatment;
                    else row.Cells[0].Value = treatment;

                    if (dgvTreatmentList.Columns["Description"] != null) row.Cells["Description"].Value = desc;
                    if (dgvTreatmentList.Columns["Qty"] != null) row.Cells["Qty"].Value = qty;
                    if (dgvTreatmentList.Columns["UnitPrice"] != null) row.Cells["UnitPrice"].Value = unitPrice.ToString("#,##0.00");

                    if (dgvTreatmentList.Columns["Amount"] != null)
                    {
                        decimal amount = qty * unitPrice;
                        row.Cells["Amount"].Value = amount.ToString("#,##0.00");
                    }

                    if (dgvTreatmentList.Columns["Status"] != null)
                        row.Cells["Status"].Value = "Unpaid";
                }

                CalculateTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load treatments for patient: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtFields_TextChanged(object sender, EventArgs e) => CalculateTotals();

        private void CalculateTotals()
        {
            decimal subtotal = 0m;

            foreach (DataGridViewRow row in dgvTreatmentList.Rows)
            {
                if (row.IsNewRow) continue;

                decimal qty = 0m;
                decimal unitPrice = 0m;

                decimal.TryParse(row.Cells["Qty"].Value?.ToString(), out qty);
                decimal.TryParse(row.Cells["UnitPrice"].Value?.ToString(), out unitPrice);

                decimal amount = qty * unitPrice;

                if (row.Cells["Amount"] != null)
                    row.Cells["Amount"].Value = amount.ToString("#,##0.00");

                subtotal += amount;
            }

            decimal discountPercent = 0m;
            decimal taxPercent = 0m;

            decimal.TryParse(txtDiscount.Text, out discountPercent);
            decimal.TryParse(txtTax.Text, out taxPercent);

            decimal discountAmount = subtotal * (discountPercent / 100m);
            decimal taxAmount = subtotal * (taxPercent / 100m);

            decimal total = subtotal - discountAmount + taxAmount;

            lblSubtotal.Text = $"₱{subtotal:#,##0.00}";
            lblTotalAmount.Text = $"₱{total:#,##0.00}";

            if (decimal.TryParse(txtPayment.Text, out decimal payment))
            {
                decimal balance = total - payment;
                lblBalance.Text = $"₱{balance:#,##0.00}";
            }
            else
            {
                lblBalance.Text = $"₱{total:#,##0.00}";
            }

            if (decimal.TryParse(txtPayment.Text, out decimal paid))
            {
                string status = (paid == 0m) ? "Unpaid" : (paid < total) ? "Partial" : "Fully Paid";
                lblStatus.Text = $"Status: {status}";
            }
        }

        private void dgvTreatmentList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvTreatmentList.Rows[e.RowIndex].IsNewRow) return;

            var row = dgvTreatmentList.Rows[e.RowIndex];
            decimal qty = 0m;
            decimal unitPrice = 0m;

            decimal.TryParse(row.Cells["Qty"].Value?.ToString(), out qty);
            decimal.TryParse(row.Cells["UnitPrice"].Value?.ToString(), out unitPrice);

            decimal amount = qty * unitPrice;

            if (row.Cells["Amount"] != null)
                row.Cells["Amount"].Value = amount.ToString("#,##0.00");

            CalculateTotals();
        }

        // ========================================================
        // CRITICAL FIX: I-SAVE ANG INVOICE DIREKTA SA SQL DATABASE
        // ========================================================
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceNum.Text))
            {
                MessageBox.Show("Please enter an invoice number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPname.Text))
            {
                MessageBox.Show("Please provide a patient name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CalculateTotals();

            string invoiceNo = txtInvoiceNum.Text.Trim();
            string patientName = txtPname.Text.Trim();
            DateTime invoiceDate = dtpInvoiceDate.Value;

            decimal totalAmount = 0m;
            decimal paidAmount = 0m;

            decimal.TryParse(lblTotalAmount.Text.Replace("₱", "").Replace(",", ""), out totalAmount);
            decimal.TryParse(txtPayment.Text, out paidAmount);
            decimal balance = totalAmount - paidAmount;
            string status = (paidAmount == 0m) ? "Unpaid" : (paidAmount < totalAmount) ? "Partial" : "Fully Paid";

            try
            {
                DatabaseHelper.EnsureDatabaseAndTables();

                // 1. Kuhanin muna natin ang tamang PatientID mula sa pangalan
                string patientSql = "SELECT TOP(1) PatientID FROM Patients WHERE (FirstName + ' ' + LastName) LIKE @pname";
                DataTable pdt = DatabaseHelper.ExecuteQuery(patientSql, new SqlParameter("@pname", "%" + patientName + "%"));

                int? patientID = null;
                if (pdt != null && pdt.Rows.Count > 0)
                {
                    patientID = Convert.ToInt32(pdt.Rows[0]["PatientID"]);
                }

                // 2. I-insert ang record sa database gamit ang structure na binabasa ng system ni admin
                string insertSql = @"
                    INSERT INTO Invoices (InvoiceID, PatientID, TotalAmount, PaidAmount, BalanceAmount, Status, InvoiceDate)
                    VALUES (@InvoiceID, @PatientID, @TotalAmount, @PaidAmount, @BalanceAmount, @Status, @InvoiceDate);";

                // Gagamit tayo ng ExecuteNonQuery sa helper para sa inserts kung meron, 
                // o kaya gagamit ng standard connection pool routine:
                using (SqlConnection conn = new SqlConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@InvoiceID", invoiceNo);
                        cmd.Parameters.AddWithValue("@PatientID", (object)patientID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        cmd.Parameters.AddWithValue("@PaidAmount", paidAmount);
                        cmd.Parameters.AddWithValue("@BalanceAmount", balance);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Invoice successfully synchronized and saved to data server!", "Database Up-to-Date", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // I-refresh ang local lower table sheet grid view control
                LoadExistingInvoices();

                // 3. Pagkatapos, ituloy ang Printing routine ng Receipt gaya ng dati
                string receiptText = $"Invoice Receipt\n\n" +
                                     $"Invoice No: {invoiceNo}\n" +
                                     $"Date: {invoiceDate:MM/dd/yyyy}\n" +
                                     $"Patient: {patientName}\n" +
                                     $"Total: ₱{totalAmount:#,##0.00}\n" +
                                     $"Paid: ₱{paidAmount:#,##0.00}\n" +
                                     $"Balance: ₱{balance:#,##0.00}\n" +
                                     $"Status: {status}";

                var printDoc = new PrintDocument();
                printDoc.PrintPage += (s, ev) =>
                {
                    ev.Graphics.DrawString(receiptText, new Font("Arial", 12), Brushes.Black, new PointF(100, 100));
                };

                using (var printDialog = new PrintDialog { Document = printDoc })
                {
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        printDoc.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save and sync report to admin database: " + ex.Message, "Sync Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearchP_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchP.Text?.Trim().ToLower() ?? string.Empty;

            foreach (DataGridViewRow row in dgvInvoices.Rows)
            {
                if (row.IsNewRow) continue;

                var patientCell = row.Cells[2]; // Index mapping to avoid crash if naming mismatch
                var invoiceCell = row.Cells[0];

                string patient = patientCell?.Value?.ToString().ToLower() ?? string.Empty;
                string invoiceNo = invoiceCell?.Value?.ToString().ToLower() ?? string.Empty;

                bool match = patient.Contains(searchText) || invoiceNo.Contains(searchText);
                row.Visible = match;
            }
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStatus.SelectedItem == null) return;
            string selectedStatus = cmbStatus.SelectedItem.ToString();

            foreach (DataGridViewRow row in dgvInvoices.Rows)
            {
                if (row.IsNewRow) continue;

                string status = row.Cells[6].Value?.ToString() ?? string.Empty;
                row.Visible = string.IsNullOrEmpty(selectedStatus) ? true : status == selectedStatus;
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            txtPname.Clear();
            txtInvoiceNum.Clear();
            txtPayment.Clear();
            txtNotes.Clear();
            txtDiscount.Clear();
            txtTax.Clear();
            dgvTreatmentList.Rows.Clear();

            dtpInvoiceDate.Value = DateTime.Now;
            dtpDue.Value = DateTime.Now;

            lblSubtotal.Text = "₱0.00";
            lblTotalAmount.Text = "₱0.00";
            lblBalance.Text = "₱0.00";
            lblStatus.Text = string.Empty;
            lblInvoiceDate.Text = "Invoice Date:";
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            dgvTreatmentList.Rows.Add();
            int lastRow = dgvTreatmentList.Rows.Count - 1;
            if (lastRow >= 0)
            {
                dgvTreatmentList.CurrentCell = dgvTreatmentList.Rows[lastRow].Cells["Treatment"];
                dgvTreatmentList.BeginEdit(true);
            }
        }

        private void dgvInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void txtInvoiceNum_TextChanged(object sender, EventArgs e) { }
        private void dtpInvoiceDate_ValueChanged(object sender, EventArgs e) { }
        private void dtpDue_ValueChanged(object sender, EventArgs e) { }
        private void txtDiscount_TextChanged(object sender, EventArgs e) => CalculateTotals();
        private void txtTax_TextChanged(object sender, EventArgs e) => CalculateTotals();
        private void dgvTreatmentList_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void txtBalance_TextChanged(object sender, EventArgs e) { }
        private void dtpFrom_ValueChanged(object sender, EventArgs e) { }
        private void dtpTo_ValueChanged(object sender, EventArgs e) { }
        private void BillingForm_Load_1(object sender, EventArgs e) { }
    }
}