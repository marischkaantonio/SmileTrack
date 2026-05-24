using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmileTrack
{
    public partial class BillingForm : Form
    {
        public BillingForm()
        {
            InitializeComponent();
        }

        

      


private void dgvTreatmentList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore the new blank row
            if (e.RowIndex < 0 || dgvTreatmentList.Rows[e.RowIndex].IsNewRow)
                return;

            DataGridViewRow row = dgvTreatmentList.Rows[e.RowIndex];

            // Read Qty and Unit Price safely
            int qty = 0;
            decimal unitPrice = 0;

            if (row.Cells["Qty"].Value != null)
                int.TryParse(row.Cells["Qty"].Value.ToString(), out qty);

            if (row.Cells["UnitPrice"].Value != null)
            {
                string priceText = row.Cells["UnitPrice"].Value.ToString()
                    .Replace("₱", "")
                    .Replace(",", "")
                    .Trim();

                decimal.TryParse(priceText, out unitPrice);
            }

            // Compute Amount
            decimal amount = qty * unitPrice;
            row.Cells["Amount"].Value = amount.ToString("₱#,##0.00");

            // Recalculate overall totals
            CalculateTotals();
        }

        // Recalculate subtotal, discount, tax, and total
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

            // Read discount and tax percentages
            decimal discountPercent = 0;
            decimal taxPercent = 0;

            if (!string.IsNullOrWhiteSpace(txtDiscount.Text))
                decimal.TryParse(txtDiscount.Text, out discountPercent);

            if (!string.IsNullOrWhiteSpace(txtTax.Text))
                decimal.TryParse(txtTax.Text, out taxPercent);

            // Compute totals
            decimal discount = subtotal * (discountPercent / 100);
            decimal tax = subtotal * (taxPercent / 100);
            decimal total = subtotal - discount + tax;

            // Display results
            lblSubtotal.Text = $"₱{subtotal:#,##0.00}";
            txtDiscount.Text = $"{discountPercent}";
            txtTax.Text = $"{taxPercent}";
            lblTotalAmount.Text = $"₱{total:#,##0.00}";
        }

        // Register the event in your form constructor or Load event
        private void BillingForm_Load(object sender, EventArgs e)
        {
            dgvTreatmentList.CellEndEdit += dgvTreatmentList_CellEndEdit;
        }
        
       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TxtPname_TextChanged(object sender, EventArgs e)
        {
            // Validate patient name is not empty
            if (string.IsNullOrWhiteSpace(txtPname.Text))
            {
                lblStatus.Text = "Patient name is required.";
            }
            else
            {
                lblStatus.Text = "";
            }
        }

        private void txtInvoiceNum_TextChanged(object sender, EventArgs e)
        {
            // Ensure invoice number is unique (pseudo-check)
            if (txtInvoiceNum.Text.Length < 3)
            {
                lblStatus.Text = "Invoice number must be at least 3 characters.";
            }
            else
            {
                lblStatus.Text = "";
            }
        }

        private void txtPayment_TextChanged(object sender, EventArgs e)
        {
            // Validate numeric payment input
            if (!decimal.TryParse(txtPayment.Text, out decimal payment))
            {
                lblStatus.Text = "Payment must be a valid number.";
            }
            else
            {
                lblStatus.Text = "";
            }
        }

        private void dtpInvoiceDate_ValueChanged(object sender, EventArgs e)
        {
            // Show selected invoice date
            lblInvoiceDate.Text = $"Invoice Date: {dtpInvoiceDate.Value:MMMM dd, yyyy}";
        }

        private void dtpDue_ValueChanged(object sender, EventArgs e)
        {
            // Ensure due date is not before invoice date
            if (dtpDue.Value < dtpInvoiceDate.Value)
            {
                lblStatus.Text = "Due date cannot be earlier than invoice date.";
            }
            else
            {
                lblStatus.Text = "";
                Duedat.Text = $"Due Date: {dtpDue.Value:MMMM dd, yyyy}";
            }
        }



        private void txtTax_TextChanged(object sender, EventArgs e)
        {
            // Validate numeric input
            if (!decimal.TryParse(txtTax.Text, out decimal taxPercent))
            {
                lblStatus.Text = "Tax must be a valid number.";
                return;
            }
            lblStatus.Text = "";

            // Recalculate totals automatically
            CalculateTotals();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            // Validate numeric input
            if (!decimal.TryParse(txtDiscount.Text, out decimal discountPercent))
            {
                lblStatus.Text = "Discount must be a valid number.";
                return;
            }
            lblStatus.Text = "";

            // Recalculate totals automatically
            CalculateTotals();
        }

        

    }
}


