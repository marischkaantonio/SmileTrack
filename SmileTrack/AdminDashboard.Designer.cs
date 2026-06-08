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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Panel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnUserMngt = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.AppoinmentData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblViewTitle = new System.Windows.Forms.Label();
            this.btnHome = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dgvAuditLogs = new System.Windows.Forms.DataGridView();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Details = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDailyTransaction = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblBillingSummary = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panelUManagement = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.dgvUserMngt = new System.Windows.Forms.DataGridView();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Role = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelDashboard = new System.Windows.Forms.Panel();
            this.panelReports = new System.Windows.Forms.Panel();
            this.lblAppoinmentToday = new System.Windows.Forms.Label();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AppoinmentData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditLogs)).BeginInit();
            this.panelUManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserMngt)).BeginInit();
            this.panelDashboard.SuspendLayout();
            this.panelReports.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.Teal;
            this.Panel1.Controls.Add(this.pictureBox2);
            this.Panel1.Controls.Add(this.btnDashboard);
            this.Panel1.Controls.Add(this.btnUserMngt);
            this.Panel1.Controls.Add(this.btnReports);
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
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Azure;
            this.pictureBox1.BackgroundImage = global::SmileTrack.Properties.Resources._4209059;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(2, 215);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Size = new System.Drawing.Size(94, 98);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(2, 317);
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
            this.btnLogOut.Location = new System.Drawing.Point(2, 402);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(101, 31);
            this.btnLogOut.TabIndex = 9;
            this.btnLogOut.Text = "Log-out";
            this.btnLogOut.UseVisualStyleBackColor = false;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // AppoinmentData
            // 
            chartArea1.Name = "ChartArea1";
            this.AppoinmentData.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            legend1.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide;
            this.AppoinmentData.Legends.Add(legend1);
            this.AppoinmentData.Location = new System.Drawing.Point(648, 364);
            this.AppoinmentData.Margin = new System.Windows.Forms.Padding(2);
            this.AppoinmentData.Name = "AppoinmentData";
            this.AppoinmentData.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.AppoinmentData.Series.Add(series1);
            this.AppoinmentData.Size = new System.Drawing.Size(333, 229);
            this.AppoinmentData.TabIndex = 32;
            this.AppoinmentData.Text = "chart1";
            // 
            // lblViewTitle
            // 
            this.lblViewTitle.AutoSize = true;
            this.lblViewTitle.BackColor = System.Drawing.Color.PaleTurquoise;
            this.lblViewTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblViewTitle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblViewTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblViewTitle.Location = new System.Drawing.Point(64, 25);
            this.lblViewTitle.Name = "lblViewTitle";
            this.lblViewTitle.Size = new System.Drawing.Size(2, 17);
            this.lblViewTitle.TabIndex = 27;
            // 
            // btnHome
            // 
            this.btnHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.Location = new System.Drawing.Point(15, 21);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(43, 23);
            this.btnHome.TabIndex = 43;
            this.btnHome.Text = "🏠";
            this.btnHome.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(207, 21);
            this.textBox1.TabIndex = 40;
            this.textBox1.Text = "   Reports";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 59);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Daily Transaction";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(9, 184);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Billing Summary";
            // 
            // dgvAuditLogs
            // 
            this.dgvAuditLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAuditLogs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Date,
            this.User,
            this.Action,
            this.Details});
            this.dgvAuditLogs.Location = new System.Drawing.Point(596, 107);
            this.dgvAuditLogs.Margin = new System.Windows.Forms.Padding(2);
            this.dgvAuditLogs.Name = "dgvAuditLogs";
            this.dgvAuditLogs.RowHeadersWidth = 51;
            this.dgvAuditLogs.RowTemplate.Height = 24;
            this.dgvAuditLogs.Size = new System.Drawing.Size(475, 106);
            this.dgvAuditLogs.TabIndex = 29;
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
            // lblDailyTransaction
            // 
            this.lblDailyTransaction.AutoSize = true;
            this.lblDailyTransaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDailyTransaction.ForeColor = System.Drawing.Color.Blue;
            this.lblDailyTransaction.Location = new System.Drawing.Point(9, 128);
            this.lblDailyTransaction.Name = "lblDailyTransaction";
            this.lblDailyTransaction.Size = new System.Drawing.Size(0, 18);
            this.lblDailyTransaction.TabIndex = 36;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Aquamarine;
            this.btnRefresh.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRefresh.Location = new System.Drawing.Point(685, 245);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(117, 28);
            this.btnRefresh.TabIndex = 41;
            this.btnRefresh.Text = "♻️ Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblBillingSummary
            // 
            this.lblBillingSummary.AutoSize = true;
            this.lblBillingSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillingSummary.ForeColor = System.Drawing.Color.Blue;
            this.lblBillingSummary.Location = new System.Drawing.Point(49, 546);
            this.lblBillingSummary.Name = "lblBillingSummary";
            this.lblBillingSummary.Size = new System.Drawing.Size(13, 18);
            this.lblBillingSummary.TabIndex = 37;
            this.lblBillingSummary.Text = " ";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Aquamarine;
            this.btnClear.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClear.Location = new System.Drawing.Point(913, 245);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(117, 28);
            this.btnClear.TabIndex = 39;
            this.btnClear.Text = " 🗑️ Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(593, 92);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Audit Logs";
            // 
            // panelUManagement
            // 
            this.panelUManagement.Controls.Add(this.button1);
            this.panelUManagement.Controls.Add(this.label1);
            this.panelUManagement.Controls.Add(this.btnAdd);
            this.panelUManagement.Controls.Add(this.btnEdit);
            this.panelUManagement.Controls.Add(this.btnRemove);
            this.panelUManagement.Controls.Add(this.dgvUserMngt);
            this.panelUManagement.Location = new System.Drawing.Point(15, 65);
            this.panelUManagement.Name = "panelUManagement";
            this.panelUManagement.Size = new System.Drawing.Size(558, 216);
            this.panelUManagement.TabIndex = 45;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 23);
            this.button1.TabIndex = 45;
            this.button1.Text = "🏠";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 44;
            this.label1.Text = "User Management";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Aquamarine;
            this.btnAdd.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.Location = new System.Drawing.Point(390, 176);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(117, 28);
            this.btnAdd.TabIndex = 31;
            this.btnAdd.Text = "+ Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Aquamarine;
            this.btnEdit.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnEdit.Location = new System.Drawing.Point(39, 176);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(2);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(117, 28);
            this.btnEdit.TabIndex = 35;
            this.btnEdit.Text = "🖊️Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.Aquamarine;
            this.btnRemove.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemove.Location = new System.Drawing.Point(199, 176);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(117, 28);
            this.btnRemove.TabIndex = 38;
            this.btnRemove.Text = " 🗑️ Remove";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // dgvUserMngt
            // 
            this.dgvUserMngt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserMngt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName,
            this.Role,
            this.Password,
            this.Status});
            this.dgvUserMngt.Location = new System.Drawing.Point(24, 38);
            this.dgvUserMngt.Margin = new System.Windows.Forms.Padding(2);
            this.dgvUserMngt.Name = "dgvUserMngt";
            this.dgvUserMngt.RowHeadersWidth = 51;
            this.dgvUserMngt.RowTemplate.Height = 24;
            this.dgvUserMngt.Size = new System.Drawing.Size(523, 106);
            this.dgvUserMngt.TabIndex = 28;
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
            // panelDashboard
            // 
            this.panelDashboard.BackColor = System.Drawing.Color.White;
            this.panelDashboard.Controls.Add(this.panelReports);
            this.panelDashboard.Controls.Add(this.panelUManagement);
            this.panelDashboard.Controls.Add(this.label6);
            this.panelDashboard.Controls.Add(this.btnClear);
            this.panelDashboard.Controls.Add(this.lblBillingSummary);
            this.panelDashboard.Controls.Add(this.btnRefresh);
            this.panelDashboard.Controls.Add(this.dgvAuditLogs);
            this.panelDashboard.Controls.Add(this.btnHome);
            this.panelDashboard.Controls.Add(this.lblViewTitle);
            this.panelDashboard.Controls.Add(this.lblAppoinmentToday);
            this.panelDashboard.Controls.Add(this.AppoinmentData);
            this.panelDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDashboard.Location = new System.Drawing.Point(181, 0);
            this.panelDashboard.Name = "panelDashboard";
            this.panelDashboard.Size = new System.Drawing.Size(1104, 645);
            this.panelDashboard.TabIndex = 3;
            // 
            // panelReports
            // 
            this.panelReports.Controls.Add(this.lblDailyTransaction);
            this.panelReports.Controls.Add(this.label9);
            this.panelReports.Controls.Add(this.label7);
            this.panelReports.Controls.Add(this.textBox1);
            this.panelReports.Location = new System.Drawing.Point(40, 328);
            this.panelReports.Name = "panelReports";
            this.panelReports.Size = new System.Drawing.Size(231, 235);
            this.panelReports.TabIndex = 46;
            // 
            // lblAppoinmentToday
            // 
            this.lblAppoinmentToday.AutoSize = true;
            this.lblAppoinmentToday.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppoinmentToday.Location = new System.Drawing.Point(606, 331);
            this.lblAppoinmentToday.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAppoinmentToday.Name = "lblAppoinmentToday";
            this.lblAppoinmentToday.Size = new System.Drawing.Size(116, 13);
            this.lblAppoinmentToday.TabIndex = 42;
            this.lblAppoinmentToday.Text = "Appointment Today";
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
            this.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AppoinmentData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditLogs)).EndInit();
            this.panelUManagement.ResumeLayout(false);
            this.panelUManagement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserMngt)).EndInit();
            this.panelDashboard.ResumeLayout(false);
            this.panelDashboard.PerformLayout();
            this.panelReports.ResumeLayout(false);
            this.panelReports.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel Panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnUserMngt;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.DataVisualization.Charting.Chart AppoinmentData;
        private System.Windows.Forms.Label lblViewTitle;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView dgvAuditLogs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn Details;
        private System.Windows.Forms.Label lblDailyTransaction;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblBillingSummary;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panelUManagement;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.DataGridView dgvUserMngt;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Role;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Panel panelDashboard;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelReports;
        private System.Windows.Forms.Label lblAppoinmentToday;
    }
}