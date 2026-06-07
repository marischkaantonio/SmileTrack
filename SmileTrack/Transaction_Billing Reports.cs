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

          
            btnGenReport.Click += BtnGenReport_Click;
            btnSummary.Click += BtnSummary_Click;

         
            btnExportCSV.Click += btnExportCSV_Click;
            btnCSV.Click += btnExportCSV_Click;

           

            btnExportCSV.Click += btnExportCSV_Click;
            btnCSV.Click += btnExportCSV_Click;

            // Print buttons
            btnPrint.Click += BtnPrint_Click;
            btnPrint1.Click += BtnPrint_Click;
                  button1.Click += Button1_Click;

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

       

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            PreparePrintText();
            printDoc = new PrintDocument();
            printDoc.PrintPage += PrintDoc_PrintPage;

            using (var dlg = new PrintDialog { Document = printDoc })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        printDoc.Print();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Print failed: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            var font = new Font("Consolas", 10);
            var margin = e.MarginBounds;
            e.Graphics.DrawString(printText, font, Brushes.Black, margin.Left, margin.Top);
        }

        private void PreparePrintText()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Transaction / Billing Report");
            sb.AppendLine($"Generated: {DateTime.Now:F}");
            sb.AppendLine(new string('-', 80));

            if (dataGridView1.Visible && dataGridView1.Rows.Count > 0)
            {
                sb.AppendLine("Transactions:");
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    if (r.IsNewRow) continue;
                    sb.AppendLine($"{r.Cells["TransactionID"].Value}\t{r.Cells["PatientName"].Value}\t{r.Cells["Amount"].Value}\t{r.Cells["DateAndTime"].Value}\t{r.Cells["ProcessedBy"].Value}");
                }
                sb.AppendLine(new string('-', 80));
            }

            if (dataGridView2.Visible && dataGridView2.Rows.Count > 0)
            {
                sb.AppendLine("Billing Summary:");
                foreach (DataGridViewRow r in dataGridView2.Rows)
                {
                    if (r.IsNewRow) continue;
                    sb.AppendLine($"{r.Cells["BillID"].Value}\t{r.Cells[1].Value}\t{r.Cells["TotalBill"].Value}\t{r.Cells[3].Value}\t{r.Cells["Balance"].Value}\t{r.Cells["Status"].Value}\t{r.Cells["BillingDate"].Value}");
                }
                sb.AppendLine(new string('-', 80));
            }

            sb.AppendLine($"Total Collected: ₱{totalCollected:#,##0.00}");
            sb.AppendLine($"Outstanding Balance: ₱{totalOutstanding:#,##0.00}");

            printText = sb.ToString();
        }

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

                dataGridView1.DataSource = transactionsTable;

                // compute totals
                totalCollected = 0m;
                foreach (DataRow row in transactionsTable.Rows)
                {
                    decimal.TryParse(row["Amount"]?.ToString(), out decimal amt);
                    totalCollected += amt;
                }

                label7.Text = $"₱{totalCollected:#,##0.00}";
                // show/hide appropriate panels
                panel1.Visible = true;
                panel2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate transactions report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

                dataGridView2.DataSource = billingTable;

                // compute totals: collected = sum(AmountPaid), outstanding = sum(Balance)
                totalCollected = 0m;
                totalOutstanding = 0m;

                foreach (DataRow row in billingTable.Rows)
                {
                    decimal.TryParse(row["AmountPaid"]?.ToString(), out decimal paid);
                    decimal.TryParse(row["Balance"]?.ToString(), out decimal bal);
                    totalCollected += paid;
                    totalOutstanding += bal;
                }

                label7.Text = $"₱{totalCollected:#,##0.00}";
                label8.Text = $"₱{totalOutstanding:#,##0.00}";

                // show/hide appropriate panels
                panel2.Visible = true;
                panel1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate billing summary: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportCurrentViewToCsv()
        {
            try
            {
                DataTable dt = null;
                string defaultName = "report";

                if (panel1.Visible && transactionsTable != null)
                {
                    dt = transactionsTable;
                    defaultName = $"transactions_{DateTime.Now:yyyyMMdd}";
                }
                else if (panel2.Visible && billingTable != null)
                {
                    dt = billingTable;
                    defaultName = $"billing_{DateTime.Now:yyyyMMdd}";
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv", FileName = defaultName + ".csv" })
                {
                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    using (var sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                    {
                        // header
                        sw.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => Quote(c.ColumnName))));
                        foreach (DataRow row in dt.Rows)
                        {
                            var values = dt.Columns.Cast<DataColumn>().Select(c => Quote(Convert.ToString(row[c]) ?? string.Empty));
                            sw.WriteLine(string.Join(",", values));
                        }
                    }
                }

                MessageBox.Show("Export completed.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message, "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Quote(string s)
        {
            if (s == null) return "\"\"";
            var escaped = s.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            // Export only the Daily Transactions view (dataGridView1 / transactionsTable)
            if (transactionsTable == null || transactionsTable.Rows.Count == 0)
            {
                MessageBox.Show("No transaction data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = $"transactions_{DateTime.Now:yyyyMMdd}.csv",
                Title = "Export Transactions to CSV"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (var sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                    {
                        // Header
                        sw.WriteLine(string.Join(",", transactionsTable.Columns.Cast<DataColumn>().Select(c => Quote(c.ColumnName))));

                        // Rows
                        foreach (DataRow row in transactionsTable.Rows)
                        {
                            var values = transactionsTable.Columns.Cast<DataColumn>()
                                .Select(c => Quote(Convert.ToString(row[c]) ?? string.Empty));
                            sw.WriteLine(string.Join(",", values));
                        }
                    }

                    MessageBox.Show("Transactions exported successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export failed: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            // Print the Daily Transactions view (dataGridView1)
    if (dataGridView1 == null || dataGridView1.Rows.Count == 0)
    {
        MessageBox.Show("No transaction data to print.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    // Build a printable text snapshot of the grid
    var sb = new StringBuilder();
    sb.AppendLine("Daily Transactions Report");
    sb.AppendLine($"Generated: {DateTime.Now:F}");
    sb.AppendLine(new string('-', 100));

    // Column headers
    var headers = dataGridView1.Columns.Cast<DataGridViewColumn>()
        .Where(c => c.Visible)
        .Select(c => c.HeaderText);
    sb.AppendLine(string.Join("\t", headers));

    // Rows
    foreach (DataGridViewRow row in dataGridView1.Rows)
    {
        if (row.IsNewRow) continue;
        var cells = dataGridView1.Columns.Cast<DataGridViewColumn>()
            .Where(c => c.Visible)
            .Select(c => Convert.ToString(row.Cells[c.Index].Value) ?? string.Empty);
        sb.AppendLine(string.Join("\t", cells));
    }

    sb.AppendLine(new string('-', 100));
    sb.AppendLine($"Total Collected: ₱{totalCollected:#,##0.00}");

    printText = sb.ToString();

    printDoc = new PrintDocument();
    printDoc.PrintPage += PrintDoc_PrintPage;

    using (var dlg = new PrintDialog { Document = printDoc })
    {
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            try
            {
                printDoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Print failed: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
    }
}