using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmileTrack
{
    public partial class Transaction_Billing_Reports : Form
    {
        private DataTable transactionsTable;
        private DataTable billingTable;
        private decimal totalCollected = 0m;
        private decimal totalOutstanding = 0m;
        private PrintDocument printDoc;
        private string printText;

        public Transaction_Billing_Reports()
        {
            InitializeComponent();

            // Wire up internal functional click handlers
            btnGenReport.Click += BtnGenReport_Click;
            btnSummary.Click += BtnSummary_Click;

            // Wire up CSV export buttons safely to separate triggers
            btnExportCSV.Click += btnExportCSV_Click; // Daily Transactions CSV
            btnCSV.Click += btnCSV_Billing_Click;    // Billing Summary CSV

            // Wire up printing document routines
            btnPrint.Click += BtnPrint_Transactions_Click; // Daily Transactions Print
            btnPrint1.Click += BtnPrint_Billing_Click;     // Billing Summary Print

            button1.Click += Button1_Click;

            // Set system date controls initially to current date timestamp
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
            dtpDateFrom.Value = DateTime.Today;
            dtpDateTo.Value = DateTime.Today;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnGenReport_Click(object sender, EventArgs e)
        {
            GenerateTransactionsReport();
        }

        private void BtnSummary_Click(object sender, EventArgs e)
        {
            GenerateBillingSummary();
        }

        // ========================================================
        // 1. GENERATE & BIND DAILY TRANSACTIONS REPORT
        // ========================================================
        private void GenerateTransactionsReport()
        {
            try
            {
                DatabaseHelper.EnsureDatabaseAndTables();

                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date.AddDays(1).AddTicks(-1);

                const string sql = @"
                    SELECT 
                        i.InvoiceID AS TransactionID,
                        ISNULL(p.FirstName, '') + ' ' + ISNULL(p.LastName, '') AS PatientName,
                        i.PaidAmount AS Amount,
                        i.InvoiceDate AS DateAndTime,
                        ISNULL(i.Status, '') AS ProcessedBy
                    FROM Invoices i
                    LEFT JOIN Patients p ON i.PatientID = p.PatientID
                    WHERE i.InvoiceDate BETWEEN @from AND @to
                    ORDER BY i.InvoiceDate DESC, i.InvoiceID;";

                transactionsTable = DatabaseHelper.ExecuteQuery(sql,
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to));

                // CRITICAL FIX: Explicitly binding database source to user interface view component
                dataGridView1.DataSource = transactionsTable;

                // Reset and calculate system totals
                totalCollected = 0m;
                foreach (DataRow row in transactionsTable.Rows)
                {
                    decimal.TryParse(row["Amount"]?.ToString(), out decimal amt);
                    totalCollected += amt;
                }

                // Push calculation straight into the green panel collection card display
                label7.Text = $"₱ {totalCollected:#,##0.00}";

                // CRITICAL FIX: Ensure panels remain visible to prevent UI rendering dropouts
                if (panel1 != null) panel1.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate transactions report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========================================================
        // 2. GENERATE & BIND BILLING SUMMARY & STATUS
        // ========================================================
        private void GenerateBillingSummary()
        {
            try
            {
                DatabaseHelper.EnsureDatabaseAndTables();

                DateTime from = dtpDateFrom.Value.Date;
                DateTime to = dtpDateTo.Value.Date.AddDays(1).AddTicks(-1);

                const string sql = @"
                    SELECT 
                        i.InvoiceID AS BillID,
                        ISNULL(p.FirstName,'') + ' ' + ISNULL(p.LastName,'') AS PatientName,
                        i.TotalAmount AS TotalBill,
                        i.PaidAmount AS AmountPaid,
                        i.BalanceAmount AS Balance,
                        i.Status,
                        i.InvoiceDate AS BillingDate
                    FROM Invoices i
                    LEFT JOIN Patients p ON i.PatientID = p.PatientID
                    WHERE i.InvoiceDate BETWEEN @from AND @to
                    ORDER BY i.InvoiceDate DESC;";

                billingTable = DatabaseHelper.ExecuteQuery(sql,
                    new SqlParameter("@from", from),
                    new SqlParameter("@to", to));

                // CRITICAL FIX: Binding datatable directly into the bottom DataGridView
                dataGridView2.DataSource = billingTable;

                // Reset and aggregate computing parameters
                totalCollected = 0m;
                totalOutstanding = 0m;

                foreach (DataRow row in billingTable.Rows)
                {
                    decimal.TryParse(row["AmountPaid"]?.ToString(), out decimal paid);
                    decimal.TryParse(row["Balance"]?.ToString(), out decimal bal);
                    totalCollected += paid;
                    totalOutstanding += bal;
                }

                // Render metrics simultaneously onto both color summary boxes
                label7.Text = $"₱ {totalCollected:#,##0.00}";     // Green Panel
                label8.Text = $"₱ {totalOutstanding:#,##0.00}";   // Red Panel

                // CRITICAL FIX: Ensure validation frames stay visible on execution
                if (panel2 != null) panel2.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate billing summary: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========================================================
        // 3. EXPORT MODULE CONVERT ENGINES (CSV)
        // ========================================================
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            ExportDataTableToCSV(transactionsTable, "Daily_Transactions_Report");
        }

        private void btnCSV_Billing_Click(object sender, EventArgs e)
        {
            ExportDataTableToCSV(billingTable, "Billing_Summary_Report");
        }

        private void ExportDataTableToCSV(DataTable dt, string defaultPrefixName)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No transaction records available to save.", "Export Request", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv", FileName = $"{defaultPrefixName}_{DateTime.Now:yyyyMMdd}.csv" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            sw.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => Quote(c.ColumnName))));
                            foreach (DataRow row in dt.Rows)
                            {
                                var values = dt.Columns.Cast<DataColumn>().Select(c => Quote(Convert.ToString(row[c]) ?? string.Empty));
                                sw.WriteLine(string.Join(",", values));
                            }
                        }
                        MessageBox.Show("Data snapshot exported successfully to file spreadsheet!", "Export Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex) { MessageBox.Show("Failed exporting: " + ex.Message); }
                }
            }
        }

        private static string Quote(string s)
        {
            if (s == null) return "\"\"";
            return $"\"{s.Replace("\"", "\"\"")}\"";
        }

        // ========================================================
        // 4. NATIVE PRINT MANAGEMENT ROUTINES
        // ========================================================
        private void BtnPrint_Transactions_Click(object sender, EventArgs e)
        {
            ExecutePrintProcess(dataGridView1, "Daily Transactions Report", true);
        }

        private void BtnPrint_Billing_Click(object sender, EventArgs e)
        {
            ExecutePrintProcess(dataGridView2, "Billing Summary Report", false);
        }

        private void ExecutePrintProcess(DataGridView dgv, string title, bool isTrans)
        {
            if (dgv == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("No table report logs to print.", "Print Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("=====================================================================");
            sb.AppendLine($"                    SMILETRACK CLINIC REPORT                        ");
            sb.AppendLine($"                 {title.ToUpper()}                  ");
            sb.AppendLine("=====================================================================");
            sb.AppendLine($"Generated on: {DateTime.Now:F}");
            sb.AppendLine(new string('-', 75));

            var headings = dgv.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).Select(c => c.HeaderText);
            sb.AppendLine(string.Join("\t", headings));
            sb.AppendLine(new string('-', 75));

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                var cells = dgv.Columns.Cast<DataGridViewColumn>()
                    .Where(c => c.Visible)
                    .Select(c => Convert.ToString(row.Cells[c.Index].Value) ?? string.Empty);
                sb.AppendLine(string.Join("\t", cells));
            }

            sb.AppendLine(new string('-', 75));
            sb.AppendLine($"Summary Total Collected Amount: ₱ {totalCollected:#,##0.00}");
            if (!isTrans)
            {
                sb.AppendLine($"Summary Outstanding Balance   : ₱ {totalOutstanding:#,##0.00}");
            }
            sb.AppendLine("=====================================================================");

            printText = sb.ToString();
            printDoc = new PrintDocument();
            printDoc.PrintPage += PrintDoc_PrintPage;

            using (var dlg = new PrintDialog { Document = printDoc })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try { printDoc.Print(); }
                    catch (Exception ex) { MessageBox.Show("Print runtime exception error: " + ex.Message); }
                }
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var font = new Font("Consolas", 9);
            var margin = e.MarginBounds;
            e.Graphics.DrawString(printText, font, Brushes.Black, margin.Left, margin.Top);
        }
    }
}