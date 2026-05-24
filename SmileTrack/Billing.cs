using System;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Drawing;

namespace SmileTrack
{
    public partial class BillingForm : Form
    {
        public BillingForm()
        {
            InitializeComponent();
        }

        // Calculate subtotal, discount, tax, total, and balance
        private void CalculateTotals()
        {
            decimal subtotal = 0;

            foreach (DataGridViewRow row in dgvTreatmentList.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["Amount"].Value != null)
                {
                    string amountText = row.Cells["Amount"].Value.ToString()
                        .Replace("₱", "")
                        .Replace(",", "")
                        .Trim();

                    if (decimal.TryParse(amountText, out decimal amount))
                        subtotal += amount;
                }
            }

            decimal discountPercent = 0;
            decimal taxPercent = 0;

            decimal.TryParse(txtDiscount.Text, out discountPercent);
            decimal.TryParse(txtTax.Text, out taxPercent);

            decimal discount = subtotal * (discountPercent / 100);
            decimal tax = subtotal * (taxPercent / 100);
            decimal total = subtotal - discount + tax;

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
        }

        private void BillingForm_Load(object sender, EventArgs e)
        {
            dgvTreatmentList.AllowUserToAddRows = true;
            dgvTreatmentList.EditMode = DataGridViewEditMode.EditOnEnter;

            dgvTreatmentList.CellEndEdit += dgvTreatmentList_CellEndEdit;

            dgvTreatmentList.Columns.Clear();
            dgvTreatmentList.Columns.Add("Treatment", "Treatment");
            dgvTreatmentList.Columns.Add("Description", "Description");
            dgvTreatmentList.Columns.Add("Qty", "Qty");
            dgvTreatmentList.Columns.Add("UnitPrice", "Unit Price");
            dgvTreatmentList.Columns.Add("Amount", "Amount");

            DataGridViewComboBoxColumn statusColumn = new DataGridViewComboBoxColumn();
            statusColumn.Name = "Status";
            statusColumn.HeaderText = "Status";
            statusColumn.Items.AddRange("Unpaid", "Partial", "Fully Paid");
            dgvTreatmentList.Columns.Add(statusColumn);

            // Hook discount/tax/payment events
            txtDiscount.TextChanged += (s, ev) => CalculateTotals();
            txtTax.TextChanged += (s, ev) => CalculateTotals();
            txtPayment.TextChanged += (s, ev) => CalculateTotals();

            DataGridViewButtonColumn actionColumn = new DataGridViewButtonColumn();
            actionColumn.Name = "Action";
            actionColumn.HeaderText = "Action";
            actionColumn.Text = "Print Receipt";
            actionColumn.UseColumnTextForButtonValue = true;
            dgvInvoices.Columns.Add(actionColumn);
        }

        private void dgvTreatmentList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvTreatmentList.Rows[e.RowIndex].IsNewRow) return;

            var row = dgvTreatmentList.Rows[e.RowIndex];

            int qty = 0;
            decimal unitPrice = 0;

            if (row.Cells["Qty"].Value != null)
                int.TryParse(row.Cells["Qty"].Value.ToString(), out qty);

            if (row.Cells["UnitPrice"].Value != null)
                decimal.TryParse(row.Cells["UnitPrice"].Value.ToString(), out unitPrice);

            decimal amount = qty * unitPrice;
            row.Cells["Amount"].Value = amount.ToString("₱#,##0.00");

            CalculateTotals();
        }

        private void txtInvoiceNum_TextChanged(object sender, EventArgs e)
        {
            if (txtInvoiceNum.Text.Length < 3)
                lblStatus.Text = "Invoice number must be at least 3 characters.";
            else
                lblStatus.Text = "";
        }

        private void dtpInvoiceDate_ValueChanged(object sender, EventArgs e)
        {
            lblInvoiceDate.Text = $"Invoice Date: {dtpInvoiceDate.Value:MMMM dd, yyyy}";
        }

        private void dtpDue_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDue.Value < dtpInvoiceDate.Value)
            {
                lblStatus.Text = "Due date cannot be earlier than invoice date.";
            }
            else
            {
                lblStatus.Text = "";

            }
        }

        private void btnSaveInvoice_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Invoice saved successfully!");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPname.Clear();
            txtInvoiceNum.Clear();
            txtPayment.Clear();
            txtNotes.Clear();
            txtDiscount.Clear();
            txtTax.Clear();
            dgvTreatmentList.Rows.Clear();

            lblSubtotal.Text = "₱0.00";
            lblTotalAmount.Text = "₱0.00";
            lblBalance.Text = "₱0.00";
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            decimal subtotal = 0;

            foreach (DataGridViewRow row in dgvTreatmentList.Rows)
            {
                if (row.IsNewRow) continue;

                // Get Qty and Unit Price
                int qty = 0;
                decimal unitPrice = 0;

                int.TryParse(row.Cells["Qty"].Value?.ToString(), out qty);
                decimal.TryParse(row.Cells["UnitPrice"].Value?.ToString(), out unitPrice);

                // Calculate Amount
                decimal amount = qty * unitPrice;

                // Display amount in Amount column
                

                // Add to subtotal
                subtotal += amount;
            }

            // Display subtotal
            lblSubtotal.Text = subtotal.ToString("0.00");

            // Tax (example = 2%)
            decimal tax = subtotal * 0.02m;
            txtTax.Text = tax.ToString("0.00");

            // Discount
            decimal discount = 0;
            decimal.TryParse(txtDiscount.Text, out discount);

            // Total Amount
            decimal total = subtotal + tax - discount;
            lblTotalAmount.Text = total.ToString("0.00");

            // Payment
            decimal payment = 0;
            decimal.TryParse(txtPayment.Text, out payment);

            // Balance
            decimal balance = total - payment;
            lblBalance.Text = balance.ToString("0.00");

            // Status
            string status;
            if (payment == 0)
                status = "Unpaid";
            else if (payment < total)
                status = "Partial";
            else
                status = "Fully Paid";

            // Optional: update a Status label or column
            lblStatus.Text = $"Status: {status}";
        }





        private void txtSearchP_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchP.Text.ToLower();

            foreach (DataGridViewRow row in dgvInvoices.Rows)
            {
                if (row.IsNewRow) continue;

                bool match = row.Cells["PatientName"].Value.ToString().ToLower().Contains(searchText)
                          || row.Cells["InvoiceNo"].Value.ToString().ToLower().Contains(searchText);

                row.Visible = match;
            }
        }


        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = cmbStatus.SelectedItem.ToString();

            foreach (DataGridViewRow row in dgvInvoices.Rows)
            {
                if (row.IsNewRow) continue;

                bool match = row.Cells["Status"].Value != null &&
                             row.Cells["Status"].Value.ToString() == selectedStatus;

                row.Visible = match;
            }
        }


        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {

        }


        private void dgvInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvInvoices.Columns[e.ColumnIndex].Name == "Action")
            {
                var row = dgvInvoices.Rows[e.RowIndex];

                string invoiceNo = row.Cells["InvoiceNo"].Value?.ToString();
                string invoiceDate = row.Cells["InvoiceDate"].Value?.ToString();
                string patientName = row.Cells["PatientName"].Value?.ToString();
                string totalAmount = row.Cells["TotalAmount"].Value?.ToString();
                string paidAmount = row.Cells["PaidAmount"].Value?.ToString();
                string balance = row.Cells["Balance"].Value?.ToString();
                string status = row.Cells["Status"].Value?.ToString();

                string receiptText = $"Invoice Receipt\n\n" +
                                     $"Invoice No: {invoiceNo}\n" +
                                     $"Date: {invoiceDate}\n" +
                                     $"Patient: {patientName}\n" +
                                     $"Total: {totalAmount}\n" +
                                     $"Paid: {paidAmount}\n" +
                                     $"Balance: {balance}\n" +
                                     $"Status: {status}";

                PrintDocument printDoc = new PrintDocument();
                printDoc.PrintPage += (s, ev) =>
                {
                    ev.Graphics.DrawString(receiptText, new Font("Arial", 12), Brushes.Black, new PointF(100, 100));
                };

                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDoc;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDoc.Print();
                }
            }
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // Collect values
            string invoiceNo = txtInvoiceNum.Text;
            string patientName = txtPname.Text;
            string invoiceDate = dtpInvoiceDate.Value.ToString("MM/dd/yyyy");

            decimal totalAmount = 0;
            decimal paidAmount = 0;
            decimal.TryParse(lblTotalAmount.Text.Replace("₱", "").Replace(",", ""), out totalAmount);
            decimal.TryParse(txtPayment.Text, out paidAmount);

            decimal balance = totalAmount - paidAmount;
            string status = (paidAmount == 0) ? "Unpaid" :
                            (paidAmount < totalAmount) ? "Partial" : "Fully Paid";

            // Add to dgvInvoices
            dgvInvoices.Rows.Add(invoiceNo, invoiceDate, patientName,
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

            // Print automatically
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (s, ev) =>
            {
                ev.Graphics.DrawString(receiptText, new Font("Arial", 12), Brushes.Black, new PointF(100, 100));
            };
            printDoc.Print();

            // Save copy automatically (text file example)
            string path = $"`{invoiceNo}.txt";
            System.IO.File.WriteAllText(path, receiptText);

            MessageBox.Show($"Invoice saved, printed, and copy stored as {path}");
        }


        private void dgvTreatmentList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            decimal subtotal = 0;

            foreach (DataGridViewRow row in dgvTreatmentList.Rows)
            {
                if (row.IsNewRow) continue;

                decimal amount = 0;
                decimal.TryParse(row.Cells["Amount"].Value?.ToString(), out amount);
                subtotal += amount;
            }

            lblSubtotal.Text = subtotal.ToString("0.00");

            // Tax
            decimal tax = 0;
            decimal.TryParse(txtTax.Text, out tax);

            // Discount %
            decimal discountPercent = 0;
            decimal.TryParse(txtDiscount.Text, out discountPercent);

            decimal discount = subtotal * (discountPercent / 100);

            // Total
            decimal total = subtotal - discount + tax;
            lblTotalAmount.Text = total.ToString("0.00");

            // Payment & Balance
            decimal payment = 0;
            decimal.TryParse(txtPayment.Text, out payment);

            decimal balance = total - payment;
            lblBalance.Text = balance.ToString("0.00");

            // Status
            string status;
            if (payment == 0)
                status = "Unpaid";
            else if (payment < total)
                status = "Partial";
            else
                status = "Fully Paid";

            lblStatus.Text = $"Status: {status}";
        }


        private void txtTax_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBalance_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            // Clear textboxes
            txtPname.Clear();
            txtInvoiceNum.Clear();
            txtPayment.Clear();
            txtNotes.Clear();
            txtDiscount.Clear();
            txtTax.Clear();

            // Reset DatePickers
            dtpInvoiceDate.Value = DateTime.Now;
            dtpDue.Value = DateTime.Now;

            // Clear DataGridView
            dgvTreatmentList.Rows.Clear();

            // Reset labels
            lblSubtotal.Text = "₱0.00";
            lblTotalAmount.Text = "₱0.00";
            lblBalance.Text = "₱0.00";
            lblStatus.Text = "";

            // Optional: reset invoice date label
            lblInvoiceDate.Text = "Invoice Date:";
        }


    }
}