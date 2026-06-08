using System;
using System.Data;
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
            // Ensure treatment grid has expected columns (don't duplicate if designer already provided them)
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
        }

        // When user finishes typing patient name, attempt to load recent treatments into the invoice items grid
        private void txtPname_Leave(object sender, EventArgs e)
        {
            var name = txtPname.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name)) return;
            PopulateTreatmentsForPatient(name);
        }

        // Populate dgvTreatmentList with most recent treatments for the given patient name
        private void PopulateTreatmentsForPatient(string patientName)
        {
            try
            {
                // Use simple name matching (FirstName + ' ' + LastName LIKE @name)
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

                var dt = DatabaseHelper.ExecuteQuery(sql, new System.Data.SqlClient.SqlParameter("@name", "%" + patientName + "%"));

                if (dt == null || dt.Rows.Count == 0)
                {
                    // no recent treatments found — do nothing
                    return;
                }

                // Clear existing invoice items and add treatments
                dgvTreatmentList.Rows.Clear();
                foreach (System.Data.DataRow r in dt.Rows)
                {
                    var treatment = r["Treatment"]?.ToString() ?? string.Empty;
                    var desc = r["Description"]?.ToString() ?? string.Empty;
                    var qty = Convert.ToInt32(r["Qty"]);
                    var unitPrice = Convert.ToDecimal(r["UnitPrice"]);

                    int rowIndex = dgvTreatmentList.Rows.Add();
                    var row = dgvTreatmentList.Rows[rowIndex];

                    // Ensure columns exist before assigning
                    if (dgvTreatmentList.Columns["Treatment"] != null) row.Cells["Treatment"].Value = treatment;
                    else row.Cells[0].Value = treatment;

                    if (dgvTreatmentList.Columns["Description"] != null) row.Cells["Description"].Value = desc;
                    if (dgvTreatmentList.Columns["Qty"] != null) row.Cells["Qty"].Value = qty;
                    if (dgvTreatmentList.Columns["UnitPrice"] != null) row.Cells["UnitPrice"].Value = unitPrice.ToString("#,##0.00");

                    // Amount will be calculated by CalculateTotals / CellEndEdit
                    if (dgvTreatmentList.Columns["Amount"] != null)
                    {
                        decimal amount = qty * unitPrice;
                        row.Cells["Amount"].Value = amount.ToString("#,##0.00");
                    }

                    // Default status for invoice item
                    if (dgvTreatmentList.Columns["Status"] != null)
                        row.Cells["Status"].Value = "Unpaid";
                }

                // Recalculate totals after populating
                CalculateTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load treatments for patient: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Recalculate totals when discount/tax/payment change
        private void TxtFields_TextChanged(object sender, EventArgs e) => CalculateTotals();

        // Calculate subtotal, discount, tax, total and balance from dgvTreatmentList
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

                // Store numeric amount (formatted) in Amount cell
                if (row.Cells["Amount"] != null)
                    row.Cells["Amount"].Value = amount.ToString("#,##0.00");

                subtotal += amount;
            }

            // Parse discount and tax as percentages (if user enters percent) or absolute if they want — treat input as percent by default
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

            // Update status label
            if (decimal.TryParse(txtPayment.Text, out decimal paid))
            {
                string status = (paid == 0m) ? "Unpaid" : (paid < total) ? "Partial" : "Fully Paid";
                lblStatus.Text = $"Status: {status}";
            }
        }

        private void dgvTreatmentList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvTreatmentList.Rows[e.RowIndex].IsNewRow) return;

            // Recalculate amount for changed row and update totals
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

        private void txtInvoiceNum_TextChanged(object sender, EventArgs e)
        {
            lblStatus.Text = txtInvoiceNum.Text.Length < 3 ? "Invoice number must be at least 3 characters." : string.Empty;
        }

        private void dtpInvoiceDate_ValueChanged(object sender, EventArgs e)
        {
            // Keep label friendly; actual dtp value used for storage
            lblInvoiceDate.Text = $"Invoice Date: {dtpInvoiceDate.Value:MMMM dd, yyyy}";
        }

        private void dtpDue_ValueChanged(object sender, EventArgs e)
        {
            lblStatus.Text = dtpDue.Value < dtpInvoiceDate.Value ? "Due date cannot be earlier than invoice date." : string.Empty;
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
            // Add an empty row so user can input a new item
            dgvTreatmentList.Rows.Add();
            // Optionally move focus to the new row's Treatment cell
            int lastRow = dgvTreatmentList.Rows.Count - 1;
            if (lastRow >= 0)
            {
                dgvTreatmentList.CurrentCell = dgvTreatmentList.Rows[lastRow].Cells["Treatment"];
                dgvTreatmentList.BeginEdit(true);
            }
        }

        private void txtSearchP_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchP.Text?.Trim().ToLower() ?? string.Empty;

            foreach (DataGridViewRow row in dgvInvoices.Rows)
            {
                if (row.IsNewRow) continue;

                var patientCell = row.Cells["colPatientName"];
                var invoiceCell = row.Cells["colInvoiceNo"];

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

                string status = row.Cells["colStatus"].Value?.ToString() ?? string.Empty;
                row.Visible = string.IsNullOrEmpty(selectedStatus) ? true : status == selectedStatus;
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            // optional: implement filtering by date range
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            // optional: implement filtering by date range
        }

        private void dgvInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvInvoices.Columns[e.ColumnIndex].Name == "Action")
            {
                var row = dgvInvoices.Rows[e.RowIndex];

                string invoiceNo = row.Cells["colInvoiceNo"].Value?.ToString() ?? string.Empty;
                string invoiceDate = row.Cells["colInvoiceDate"].Value?.ToString() ?? string.Empty;
                string patientName = row.Cells["colPatientName"].Value?.ToString() ?? string.Empty;
                string totalAmount = row.Cells["colTotal"].Value?.ToString() ?? string.Empty;
                string paidAmount = row.Cells["colPaidAmount"].Value?.ToString() ?? string.Empty;
                string balance = row.Cells["colBalance"].Value?.ToString() ?? string.Empty;
                string status = row.Cells["colStatus"].Value?.ToString() ?? string.Empty;

                string receiptText = $"Invoice Receipt\n\n" +
                                     $"Invoice No: {invoiceNo}\n" +
                                     $"Date: {invoiceDate}\n" +
                                     $"Patient: {patientName}\n" +
                                     $"Total: {totalAmount}\n" +
                                     $"Paid: {paidAmount}\n" +
                                     $"Balance: {balance}\n" +
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
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtInvoiceNum.Text))
            {
                MessageBox.Show("Please enter an invoice number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CalculateTotals();

            string invoiceNo = txtInvoiceNum.Text.Trim();
            string patientName = txtPname.Text.Trim();
            string invoiceDate = dtpInvoiceDate.Value.ToString("MM/dd/yyyy");

            // Parse total and paid from labels/textboxes
            decimal totalAmount = 0m;
            decimal paidAmount = 0m;

            decimal.TryParse(lblTotalAmount.Text.Replace("₱", "").Replace(",", ""), out totalAmount);
            decimal.TryParse(txtPayment.Text, out paidAmount);

            decimal balance = totalAmount - paidAmount;
            string status = (paidAmount == 0m) ? "Unpaid" : (paidAmount < totalAmount) ? "Partial" : "Fully Paid";

            // Add to dgvInvoices using designer column names
            dgvInvoices.Rows.Add(invoiceNo,
                                 invoiceDate,
                                 patientName,
                                 totalAmount.ToString("#,##0.00"),
                                 paidAmount.ToString("#,##0.00"),
                                 balance.ToString("#,##0.00"),
                                 status);

            // Prepare receipt text
            string receiptText = $"Invoice Receipt\n\n" +
                                 $"Invoice No: {invoiceNo}\n" +
                                 $"Date: {invoiceDate}\n" +
                                 $"Patient: {patientName}\n" +
                                 $"Total: {totalAmount:#,##0.00}\n" +
                                 $"Paid: {paidAmount:#,##0.00}\n" +
                                 $"Balance: {balance:#,##0.00}\n" +
                                 $"Status: {status}";

            // Ask user to print
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

            // Save a copy to application folder
            try
            {
                string safeFileName = string.Concat(invoiceNo.Split(Path.GetInvalidFileNameChars()));
                string path = Path.Combine(Application.StartupPath, $"{safeFileName}.txt");
                File.WriteAllText(path, receiptText);
                MessageBox.Show($"Invoice saved and a copy stored as {path}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Invoice printed but failed to save copy: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvTreatmentList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // no-op for now
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            // reuse centralized calculation
            CalculateTotals();
        }

        private void txtTax_TextChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void txtBalance_TextChanged(object sender, EventArgs e)
        {
            // not used by current UI
        }

        private void BillingForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}