namespace SmileTrack
{
    partial class FormAdminDashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panelDashboard = new System.Windows.Forms.Panel();
            this.panelAuditLogs = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvAuditLogs = new System.Windows.Forms.DataGridView();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Details = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelUserManagement = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvUserMngt = new System.Windows.Forms.DataGridView();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Role = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblAppoinmentToday = new System.Windows.Forms.Label();
            this.panelReports = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblBillingSummary = new System.Windows.Forms.Label();
            this.lblDailyTransaction = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.DailyTransaction = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Panel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnUserMngt = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnAuditLogs = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelDashboard.SuspendLayout();
            this.panelAuditLogs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditLogs)).BeginInit();
            this.panelUserManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserMngt)).BeginInit();
            this.panelReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DailyTransaction)).BeginInit();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelDashboard
            // 
            this.panelDashboard.BackColor = System.Drawing.Color.White;
            this.panelDashboard.Controls.Add(this.panelAuditLogs);
            this.panelDashboard.Controls.Add(this.panelUserManagement);
            this.panelDashboard.Controls.Add(this.lblAppoinmentToday);
            this.panelDashboard.Controls.Add(this.panelReports);
            this.panelDashboard.Controls.Add(this.DailyTransaction);
            this.panelDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDashboard.Location = new System.Drawing.Point(181, 0);
            this.panelDashboard.Margin = new System.Windows.Forms.Padding(2);
            this.panelDashboard.Name = "panelDashboard";
            this.panelDashboard.Size = new System.Drawing.Size(1104, 645);
            this.panelDashboard.TabIndex = 3;
            // 
            // panelAuditLogs
            // 
            this.panelAuditLogs.Controls.Add(this.btnRefresh);
            this.panelAuditLogs.Controls.Add(this.btnClear);
            this.panelAuditLogs.Controls.Add(this.label6);
            this.panelAuditLogs.Controls.Add(this.dgvAuditLogs);
            this.panelAuditLogs.Location = new System.Drawing.Point(577, 20);
            this.panelAuditLogs.Name = "panelAuditLogs";
            this.panelAuditLogs.Size = new System.Drawing.Size(490, 207);
            this.panelAuditLogs.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(2, 3);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Audit Logs";
            // 
            // dgvAuditLogs
            // 
            this.dgvAuditLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAuditLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Date,
            this.User,
            this.Action,
            this.Details});
            this.dgvAuditLogs.Location = new System.Drawing.Point(5, 22);
            this.dgvAuditLogs.Margin = new System.Windows.Forms.Padding(2);
            this.dgvAuditLogs.Name = "dgvAuditLogs";
            this.dgvAuditLogs.RowHeadersWidth = 51;
            this.dgvAuditLogs.RowTemplate.Height = 24;
            this.dgvAuditLogs.Size = new System.Drawing.Size(475, 122);
            this.dgvAuditLogs.TabIndex = 12;
            // 
            // Date
            // 
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 6;
            this.Date.Name = "Date";
            this.Date.Width = 125;
            // 
            // User
            // 
            this.User.HeaderText = "User";
            this.User.MinimumWidth = 6;
            this.User.Name = "User";
            this.User.Width = 125;
            // 
            // Action
            // 
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            // 
            // Details
            // 
            this.Details.HeaderText = "Details";
            this.Details.Name = "Details";
            // 
            // panelUserManagement
            // 
            this.panelUserManagement.Controls.Add(this.btnRemove);
            this.panelUserManagement.Controls.Add(this.btnEdit);
            this.panelUserManagement.Controls.Add(this.btnAdd);
            this.panelUserManagement.Controls.Add(this.label5);
            this.panelUserManagement.Controls.Add(this.dgvUserMngt);
            this.panelUserManagement.Location = new System.Drawing.Point(0, 25);
            this.panelUserManagement.Name = "panelUserManagement";
            this.panelUserManagement.Size = new System.Drawing.Size(571, 202);
            this.panelUserManagement.TabIndex = 25;
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.Aquamarine;
            this.btnRemove.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemove.Location = new System.Drawing.Point(277, 155);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(117, 31);
            this.btnRemove.TabIndex = 22;
            this.btnRemove.Text = " 🗑️ Remove";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Aquamarine;
            this.btnEdit.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnEdit.Location = new System.Drawing.Point(141, 155);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(2);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(117, 31);
            this.btnEdit.TabIndex = 21;
            this.btnEdit.Text = "🖊️Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Aquamarine;
            this.btnAdd.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.Location = new System.Drawing.Point(11, 155);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(117, 31);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "+ Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 2);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "User Management";
            // 
            // dgvUserMngt
            // 
            this.dgvUserMngt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserMngt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName,
            this.Role,
            this.Password,
            this.Status});
            this.dgvUserMngt.Location = new System.Drawing.Point(4, 17);
            this.dgvUserMngt.Margin = new System.Windows.Forms.Padding(2);
            this.dgvUserMngt.Name = "dgvUserMngt";
            this.dgvUserMngt.RowHeadersWidth = 51;
            this.dgvUserMngt.RowTemplate.Height = 24;
            this.dgvUserMngt.Size = new System.Drawing.Size(523, 122);
            this.dgvUserMngt.TabIndex = 10;
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
            // lblAppoinmentToday
            // 
            this.lblAppoinmentToday.AutoSize = true;
            this.lblAppoinmentToday.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppoinmentToday.Location = new System.Drawing.Point(440, 243);
            this.lblAppoinmentToday.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAppoinmentToday.Name = "lblAppoinmentToday";
            this.lblAppoinmentToday.Size = new System.Drawing.Size(116, 13);
            this.lblAppoinmentToday.TabIndex = 24;
            this.lblAppoinmentToday.Text = "Appointment Today";
            // 
            // panelReports
            // 
            this.panelReports.Controls.Add(this.textBox1);
            this.panelReports.Controls.Add(this.lblBillingSummary);
            this.panelReports.Controls.Add(this.lblDailyTransaction);
            this.panelReports.Controls.Add(this.label7);
            this.panelReports.Controls.Add(this.label9);
            this.panelReports.Location = new System.Drawing.Point(21, 255);
            this.panelReports.Name = "panelReports";
            this.panelReports.Size = new System.Drawing.Size(270, 248);
            this.panelReports.TabIndex = 23;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(267, 21);
            this.textBox1.TabIndex = 23;
            this.textBox1.Text = "   Reports";
            // 
            // lblBillingSummary
            // 
            this.lblBillingSummary.AutoSize = true;
            this.lblBillingSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillingSummary.ForeColor = System.Drawing.Color.Blue;
            this.lblBillingSummary.Location = new System.Drawing.Point(26, 195);
            this.lblBillingSummary.Name = "lblBillingSummary";
            this.lblBillingSummary.Size = new System.Drawing.Size(52, 18);
            this.lblBillingSummary.TabIndex = 22;
            this.lblBillingSummary.Text = "label1";
            // 
            // lblDailyTransaction
            // 
            this.lblDailyTransaction.AutoSize = true;
            this.lblDailyTransaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDailyTransaction.ForeColor = System.Drawing.Color.Blue;
            this.lblDailyTransaction.Location = new System.Drawing.Point(12, 92);
            this.lblDailyTransaction.Name = "lblDailyTransaction";
            this.lblDailyTransaction.Size = new System.Drawing.Size(52, 18);
            this.lblDailyTransaction.TabIndex = 21;
            this.lblDailyTransaction.Text = "label1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 54);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Daily Transaction";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 162);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Billing Summary";
            // 
            // DailyTransaction
            // 
            chartArea3.Name = "ChartArea1";
            this.DailyTransaction.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            legend3.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide;
            this.DailyTransaction.Legends.Add(legend3);
            this.DailyTransaction.Location = new System.Drawing.Point(402, 258);
            this.DailyTransaction.Margin = new System.Windows.Forms.Padding(2);
            this.DailyTransaction.Name = "DailyTransaction";
            this.DailyTransaction.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.DailyTransaction.Series.Add(series3);
            this.DailyTransaction.Size = new System.Drawing.Size(333, 245);
            this.DailyTransaction.TabIndex = 14;
            this.DailyTransaction.Text = "chart1";
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.Teal;
            this.Panel1.Controls.Add(this.pictureBox2);
            this.Panel1.Controls.Add(this.btnDashboard);
            this.Panel1.Controls.Add(this.btnUserMngt);
            this.Panel1.Controls.Add(this.btnReports);
            this.Panel1.Controls.Add(this.btnAuditLogs);
            this.Panel1.Controls.Add(this.btnSettings);
            this.Panel1.Controls.Add(this.pictureBox1);
            this.Panel1.Controls.Add(this.panel2);
            this.Panel1.Controls.Add(this.btnLogOut);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Margin = new System.Windows.Forms.Padding(2);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(181, 645);
            this.Panel1.TabIndex = 2;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::SmileTrack.Properties.Resources._7fbf6c74_85d6_441b_b2e0_5faf4c8635f6;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(2, 2);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(135, 104);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.Aquamarine;
            this.btnDashboard.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnDashboard.Location = new System.Drawing.Point(2, 110);
            this.btnDashboard.Margin = new System.Windows.Forms.Padding(2);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(150, 31);
            this.btnDashboard.TabIndex = 1;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click_1);
            // 
            // btnUserMngt
            // 
            this.btnUserMngt.BackColor = System.Drawing.Color.Aquamarine;
            this.btnUserMngt.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUserMngt.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnUserMngt.Location = new System.Drawing.Point(2, 145);
            this.btnUserMngt.Margin = new System.Windows.Forms.Padding(2);
            this.btnUserMngt.Name = "btnUserMngt";
            this.btnUserMngt.Size = new System.Drawing.Size(150, 31);
            this.btnUserMngt.TabIndex = 2;
            this.btnUserMngt.Text = "UserManagement";
            this.btnUserMngt.UseVisualStyleBackColor = false;
            this.btnUserMngt.Click += new System.EventHandler(this.btnUserMngt_Click);
            // 
            // btnReports
            // 
            this.btnReports.BackColor = System.Drawing.Color.Aquamarine;
            this.btnReports.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReports.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnReports.Location = new System.Drawing.Point(2, 180);
            this.btnReports.Margin = new System.Windows.Forms.Padding(2);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(150, 31);
            this.btnReports.TabIndex = 3;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = false;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnAuditLogs
            // 
            this.btnAuditLogs.BackColor = System.Drawing.Color.Aquamarine;
            this.btnAuditLogs.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuditLogs.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAuditLogs.Location = new System.Drawing.Point(2, 215);
            this.btnAuditLogs.Margin = new System.Windows.Forms.Padding(2);
            this.btnAuditLogs.Name = "btnAuditLogs";
            this.btnAuditLogs.Size = new System.Drawing.Size(150, 31);
            this.btnAuditLogs.TabIndex = 6;
            this.btnAuditLogs.Text = "AuditLogs";
            this.btnAuditLogs.UseVisualStyleBackColor = false;
            this.btnAuditLogs.Click += new System.EventHandler(this.btnAuditLogs_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.Aquamarine;
            this.btnSettings.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSettings.Location = new System.Drawing.Point(2, 250);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(2);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(150, 31);
            this.btnSettings.TabIndex = 7;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Azure;
            this.pictureBox1.BackgroundImage = global::SmileTrack.Properties.Resources._4209059;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(2, 285);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Size = new System.Drawing.Size(94, 98);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(2, 387);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(150, 81);
            this.panel2.TabIndex = 10;
            // 
            // btnLogOut
            // 
            this.btnLogOut.AllowDrop = true;
            this.btnLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLogOut.BackColor = System.Drawing.Color.Aquamarine;
            this.btnLogOut.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOut.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnLogOut.Location = new System.Drawing.Point(2, 472);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(101, 31);
            this.btnLogOut.TabIndex = 9;
            this.btnLogOut.Text = "Log-out";
            this.btnLogOut.UseVisualStyleBackColor = false;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Aquamarine;
            this.btnClear.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClear.Location = new System.Drawing.Point(345, 160);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(117, 31);
            this.btnClear.TabIndex = 23;
            this.btnClear.Text = " 🗑️ Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Aquamarine;
            this.btnRefresh.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRefresh.Location = new System.Drawing.Point(109, 160);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(117, 31);
            this.btnRefresh.TabIndex = 24;
            this.btnRefresh.Text = "♻️ Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // FormAdminDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1285, 645);
            this.Controls.Add(this.panelDashboard);
            this.Controls.Add(this.Panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormAdminDashboard";
            this.Text = "AdminDashboard";
            this.Load += new System.EventHandler(this.FormAdminDashboard_Load);
            this.panelDashboard.ResumeLayout(false);
            this.panelDashboard.PerformLayout();
            this.panelAuditLogs.ResumeLayout(false);
            this.panelAuditLogs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditLogs)).EndInit();
            this.panelUserManagement.ResumeLayout(false);
            this.panelUserManagement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserMngt)).EndInit();
            this.panelReports.ResumeLayout(false);
            this.panelReports.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DailyTransaction)).EndInit();
            this.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDashboard;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataVisualization.Charting.Chart DailyTransaction;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvAuditLogs;
        private System.Windows.Forms.FlowLayoutPanel Panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnUserMngt;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnAuditLogs;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn Details;
        private System.Windows.Forms.Panel panelReports;
        private System.Windows.Forms.Label lblDailyTransaction;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblBillingSummary;
        private System.Windows.Forms.Label lblAppoinmentToday;
        private System.Windows.Forms.Panel panelUserManagement;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvUserMngt;
        private System.Windows.Forms.Panel panelAuditLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRefresh;
    }
}