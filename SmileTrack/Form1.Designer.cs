namespace SmileTrack
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panelDashboard = new System.Windows.Forms.Panel();
            this.btnHome = new System.Windows.Forms.Button();
            this.lblViewTitle = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lblAppoinmentToday = new System.Windows.Forms.Label();
            this.dgvUserMngt = new System.Windows.Forms.DataGridView();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Role = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DailyTransaction = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label9 = new System.Windows.Forms.Label();
            this.lblBillingSummary = new System.Windows.Forms.Label();
            this.lblDailyTransaction = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dgvAuditLogs = new System.Windows.Forms.DataGridView();
            this.Details = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panelDashboard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserMngt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DailyTransaction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // panelDashboard
            // 
            this.panelDashboard.BackColor = System.Drawing.Color.White;
            this.panelDashboard.Controls.Add(this.label6);
            this.panelDashboard.Controls.Add(this.btnClear);
            this.panelDashboard.Controls.Add(this.lblBillingSummary);
            this.panelDashboard.Controls.Add(this.btnRefresh);
            this.panelDashboard.Controls.Add(this.lblDailyTransaction);
            this.panelDashboard.Controls.Add(this.dgvAuditLogs);
            this.panelDashboard.Controls.Add(this.label9);
            this.panelDashboard.Controls.Add(this.label7);
            this.panelDashboard.Controls.Add(this.textBox1);
            this.panelDashboard.Controls.Add(this.btnHome);
            this.panelDashboard.Controls.Add(this.lblViewTitle);
            this.panelDashboard.Controls.Add(this.btnAdd);
            this.panelDashboard.Controls.Add(this.btnEdit);
            this.panelDashboard.Controls.Add(this.btnRemove);
            this.panelDashboard.Controls.Add(this.lblAppoinmentToday);
            this.panelDashboard.Controls.Add(this.dgvUserMngt);
            this.panelDashboard.Controls.Add(this.DailyTransaction);
            this.panelDashboard.Location = new System.Drawing.Point(17, 11);
            this.panelDashboard.Margin = new System.Windows.Forms.Padding(2);
            this.panelDashboard.Name = "panelDashboard";
            this.panelDashboard.Size = new System.Drawing.Size(1100, 569);
            this.panelDashboard.TabIndex = 4;
            // 
            // btnHome
            // 
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Location = new System.Drawing.Point(4, 2);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(43, 23);
            this.btnHome.TabIndex = 26;
            this.btnHome.Text = "🏠";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // lblViewTitle
            // 
            this.lblViewTitle.AutoSize = true;
            this.lblViewTitle.BackColor = System.Drawing.Color.PaleTurquoise;
            this.lblViewTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblViewTitle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblViewTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblViewTitle.Location = new System.Drawing.Point(53, 6);
            this.lblViewTitle.Name = "lblViewTitle";
            this.lblViewTitle.Size = new System.Drawing.Size(2, 17);
            this.lblViewTitle.TabIndex = 4;
            this.lblViewTitle.Click += new System.EventHandler(this.lblViewTitle_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Aquamarine;
            this.btnAdd.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.Location = new System.Drawing.Point(388, 196);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(117, 28);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "+ Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Aquamarine;
            this.btnEdit.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnEdit.Location = new System.Drawing.Point(37, 196);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(2);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(117, 28);
            this.btnEdit.TabIndex = 21;
            this.btnEdit.Text = "🖊️Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.Aquamarine;
            this.btnRemove.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemove.Location = new System.Drawing.Point(197, 196);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(117, 28);
            this.btnRemove.TabIndex = 22;
            this.btnRemove.Text = " 🗑️ Remove";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lblAppoinmentToday
            // 
            this.lblAppoinmentToday.AutoSize = true;
            this.lblAppoinmentToday.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppoinmentToday.Location = new System.Drawing.Point(576, 282);
            this.lblAppoinmentToday.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAppoinmentToday.Name = "lblAppoinmentToday";
            this.lblAppoinmentToday.Size = new System.Drawing.Size(116, 13);
            this.lblAppoinmentToday.TabIndex = 24;
            this.lblAppoinmentToday.Text = "Appointment Today";
            this.lblAppoinmentToday.Click += new System.EventHandler(this.lblAppoinmentToday_Click);
            // 
            // dgvUserMngt
            // 
            this.dgvUserMngt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserMngt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName,
            this.Role,
            this.Password,
            this.Status});
            this.dgvUserMngt.Location = new System.Drawing.Point(22, 58);
            this.dgvUserMngt.Margin = new System.Windows.Forms.Padding(2);
            this.dgvUserMngt.Name = "dgvUserMngt";
            this.dgvUserMngt.RowHeadersWidth = 51;
            this.dgvUserMngt.RowTemplate.Height = 24;
            this.dgvUserMngt.Size = new System.Drawing.Size(523, 106);
            this.dgvUserMngt.TabIndex = 10;
            this.dgvUserMngt.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUserMngt_CellContentClick);
            // 
            // UserName
            // 
            this.UserName.HeaderText = "UserName";
            this.UserName.MinimumWidth = 6;
            this.UserName.Name = "UserName";
            this.UserName.Width = 125;
            // 
            // Role
            // 
            this.Role.HeaderText = "Role";
            this.Role.MinimumWidth = 6;
            this.Role.Name = "Role";
            this.Role.Width = 125;
            // 
            // Password
            // 
            this.Password.HeaderText = "Password";
            this.Password.Name = "Password";
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.Width = 125;
            // 
            // DailyTransaction
            // 
            chartArea1.Name = "ChartArea1";
            this.DailyTransaction.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            legend1.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide;
            this.DailyTransaction.Legends.Add(legend1);
            this.DailyTransaction.Location = new System.Drawing.Point(618, 315);
            this.DailyTransaction.Margin = new System.Windows.Forms.Padding(2);
            this.DailyTransaction.Name = "DailyTransaction";
            this.DailyTransaction.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.DailyTransaction.Series.Add(series1);
            this.DailyTransaction.Size = new System.Drawing.Size(333, 229);
            this.DailyTransaction.TabIndex = 14;
            this.DailyTransaction.Text = "chart1";
            this.DailyTransaction.Click += new System.EventHandler(this.DailyTransaction_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(19, 463);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Billing Summary";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // lblBillingSummary
            // 
            this.lblBillingSummary.AutoSize = true;
            this.lblBillingSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillingSummary.ForeColor = System.Drawing.Color.Blue;
            this.lblBillingSummary.Location = new System.Drawing.Point(19, 497);
            this.lblBillingSummary.Name = "lblBillingSummary";
            this.lblBillingSummary.Size = new System.Drawing.Size(52, 18);
            this.lblBillingSummary.TabIndex = 22;
            this.lblBillingSummary.Text = "label1";
            this.lblBillingSummary.Click += new System.EventHandler(this.lblBillingSummary_Click);
            // 
            // lblDailyTransaction
            // 
            this.lblDailyTransaction.AutoSize = true;
            this.lblDailyTransaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDailyTransaction.ForeColor = System.Drawing.Color.Blue;
            this.lblDailyTransaction.Location = new System.Drawing.Point(19, 407);
            this.lblDailyTransaction.Name = "lblDailyTransaction";
            this.lblDailyTransaction.Size = new System.Drawing.Size(52, 18);
            this.lblDailyTransaction.TabIndex = 21;
            this.lblDailyTransaction.Text = "label1";
            this.lblDailyTransaction.Click += new System.EventHandler(this.lblDailyTransaction_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(19, 338);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Daily Transaction";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(22, 282);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(207, 21);
            this.textBox1.TabIndex = 23;
            this.textBox1.Text = "   Reports";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // dgvAuditLogs
            // 
            this.dgvAuditLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAuditLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Date,
            this.User,
            this.Action,
            this.Details});
            this.dgvAuditLogs.Location = new System.Drawing.Point(566, 58);
            this.dgvAuditLogs.Margin = new System.Windows.Forms.Padding(2);
            this.dgvAuditLogs.Name = "dgvAuditLogs";
            this.dgvAuditLogs.RowHeadersWidth = 51;
            this.dgvAuditLogs.RowTemplate.Height = 24;
            this.dgvAuditLogs.Size = new System.Drawing.Size(475, 106);
            this.dgvAuditLogs.TabIndex = 12;
            this.dgvAuditLogs.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAuditLogs_CellContentClick);
            // 
            // Details
            // 
            this.Details.HeaderText = "Details";
            this.Details.Name = "Details";
            // 
            // Action
            // 
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            // 
            // User
            // 
            this.User.HeaderText = "User";
            this.User.MinimumWidth = 6;
            this.User.Name = "User";
            this.User.Width = 125;
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 6;
            this.Date.Name = "Date";
            this.Date.Width = 125;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Aquamarine;
            this.btnRefresh.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRefresh.Location = new System.Drawing.Point(655, 196);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(117, 28);
            this.btnRefresh.TabIndex = 24;
            this.btnRefresh.Text = "♻️ Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Aquamarine;
            this.btnClear.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClear.Location = new System.Drawing.Point(883, 196);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(117, 28);
            this.btnClear.TabIndex = 23;
            this.btnClear.Text = " 🗑️ Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(563, 43);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Audit Logs";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1135, 609);
            this.Controls.Add(this.panelDashboard);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelDashboard.ResumeLayout(false);
            this.panelDashboard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserMngt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DailyTransaction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditLogs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDashboard;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Label lblViewTitle;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label lblAppoinmentToday;
        private System.Windows.Forms.DataGridView dgvUserMngt;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataVisualization.Charting.Chart DailyTransaction;
        private System.Windows.Forms.Label lblBillingSummary;
        private System.Windows.Forms.Label lblDailyTransaction;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvAuditLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn Details;
    }
}