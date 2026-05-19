namespace SmileTrack
{
    partial class DentistDashboard
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnMySched = new System.Windows.Forms.Button();
            this.btnTreatments = new System.Windows.Forms.Button();
            this.btnNotes = new System.Windows.Forms.Button();
            this.btnRepots = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtpReportDate = new System.Windows.Forms.DateTimePicker();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.TreatmentDone = new System.Windows.Forms.Panel();
            this.lbTreatment = new System.Windows.Forms.ListBox();
            this.txtNotes = new System.Windows.Forms.RichTextBox();
            this.cmbTreatmentType = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label6 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastVisit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvSched = new System.Windows.Forms.DataGridView();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Patient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Treatment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTreatmentDone = new System.Windows.Forms.RichTextBox();
            this.txtPatientSeen = new System.Windows.Forms.RichTextBox();
            this.txtTodaysAppoinment = new System.Windows.Forms.RichTextBox();
            this.txtUpApp = new System.Windows.Forms.RichTextBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.TreatmentDone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSched)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.Teal;
            this.Panel1.Controls.Add(this.pictureBox2);
            this.Panel1.Controls.Add(this.button1);
            this.Panel1.Controls.Add(this.btnMySched);
            this.Panel1.Controls.Add(this.btnTreatments);
            this.Panel1.Controls.Add(this.btnNotes);
            this.Panel1.Controls.Add(this.btnRepots);
            this.Panel1.Controls.Add(this.btnSettings);
            this.Panel1.Controls.Add(this.pictureBox1);
            this.Panel1.Controls.Add(this.panel2);
            this.Panel1.Controls.Add(this.btnLogOut);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.Panel1.Location = new System.Drawing.Point(10, 10);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(185, 714);
            this.Panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Aquamarine;
            this.button1.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(3, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 38);
            this.button1.TabIndex = 1;
            this.button1.Text = "Dashboard";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // btnMySched
            // 
            this.btnMySched.BackColor = System.Drawing.Color.Aquamarine;
            this.btnMySched.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMySched.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnMySched.Location = new System.Drawing.Point(3, 181);
            this.btnMySched.Name = "btnMySched";
            this.btnMySched.Size = new System.Drawing.Size(145, 38);
            this.btnMySched.TabIndex = 2;
            this.btnMySched.Text = "MySchedule";
            this.btnMySched.UseVisualStyleBackColor = false;
            this.btnMySched.Click += new System.EventHandler(this.btnMySched_Click);
            // 
            // btnTreatments
            // 
            this.btnTreatments.BackColor = System.Drawing.Color.Aquamarine;
            this.btnTreatments.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTreatments.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnTreatments.Location = new System.Drawing.Point(3, 225);
            this.btnTreatments.Name = "btnTreatments";
            this.btnTreatments.Size = new System.Drawing.Size(145, 38);
            this.btnTreatments.TabIndex = 3;
            this.btnTreatments.Text = "Treatments";
            this.btnTreatments.UseVisualStyleBackColor = false;
            // 
            // btnNotes
            // 
            this.btnNotes.BackColor = System.Drawing.Color.Aquamarine;
            this.btnNotes.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNotes.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnNotes.Location = new System.Drawing.Point(3, 269);
            this.btnNotes.Name = "btnNotes";
            this.btnNotes.Size = new System.Drawing.Size(145, 38);
            this.btnNotes.TabIndex = 5;
            this.btnNotes.Text = "Notes";
            this.btnNotes.UseVisualStyleBackColor = false;
            // 
            // btnRepots
            // 
            this.btnRepots.BackColor = System.Drawing.Color.Aquamarine;
            this.btnRepots.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRepots.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRepots.Location = new System.Drawing.Point(3, 313);
            this.btnRepots.Name = "btnRepots";
            this.btnRepots.Size = new System.Drawing.Size(145, 38);
            this.btnRepots.TabIndex = 6;
            this.btnRepots.Text = "Reports";
            this.btnRepots.UseVisualStyleBackColor = false;
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.Aquamarine;
            this.btnSettings.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSettings.Location = new System.Drawing.Point(3, 357);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(145, 38);
            this.btnSettings.TabIndex = 7;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(3, 528);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 10;
            // 
            // btnLogOut
            // 
            this.btnLogOut.AllowDrop = true;
            this.btnLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLogOut.BackColor = System.Drawing.Color.Aquamarine;
            this.btnLogOut.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOut.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnLogOut.Location = new System.Drawing.Point(3, 634);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(135, 38);
            this.btnLogOut.TabIndex = 9;
            this.btnLogOut.Text = "Log-out";
            this.btnLogOut.UseVisualStyleBackColor = false;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.dtpReportDate);
            this.panel3.Controls.Add(this.textBox2);
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.pictureBox6);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.TreatmentDone);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.chart1);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.dataGridView1);
            this.panel3.Controls.Add(this.pictureBox7);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.dgvSched);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.pictureBox3);
            this.panel3.Controls.Add(this.pictureBox4);
            this.panel3.Controls.Add(this.pictureBox5);
            this.panel3.Controls.Add(this.txtTreatmentDone);
            this.panel3.Controls.Add(this.txtPatientSeen);
            this.panel3.Controls.Add(this.txtTodaysAppoinment);
            this.panel3.Controls.Add(this.txtUpApp);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(195, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1077, 714);
            this.panel3.TabIndex = 1;
            // 
            // dtpReportDate
            // 
            this.dtpReportDate.Location = new System.Drawing.Point(28, 448);
            this.dtpReportDate.Name = "dtpReportDate";
            this.dtpReportDate.Size = new System.Drawing.Size(200, 22);
            this.dtpReportDate.TabIndex = 11;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(464, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(137, 22);
            this.textBox2.TabIndex = 28;
            this.textBox2.Text = " 🔍Search patient";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 27;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DarkOrange;
            this.label11.Location = new System.Drawing.Point(607, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(27, 18);
            this.label11.TabIndex = 26;
            this.label11.Text = "🔔";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(714, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 16);
            this.label10.TabIndex = 25;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(278, 473);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(118, 16);
            this.label9.TabIndex = 20;
            this.label9.Text = "Treatment Done";
            // 
            // TreatmentDone
            // 
            this.TreatmentDone.Controls.Add(this.lbTreatment);
            this.TreatmentDone.Controls.Add(this.txtNotes);
            this.TreatmentDone.Controls.Add(this.cmbTreatmentType);
            this.TreatmentDone.Controls.Add(this.btnSave);
            this.TreatmentDone.Location = new System.Drawing.Point(273, 494);
            this.TreatmentDone.Name = "TreatmentDone";
            this.TreatmentDone.Size = new System.Drawing.Size(299, 193);
            this.TreatmentDone.TabIndex = 19;
            // 
            // lbTreatment
            // 
            this.lbTreatment.FormattingEnabled = true;
            this.lbTreatment.ItemHeight = 16;
            this.lbTreatment.Location = new System.Drawing.Point(21, 134);
            this.lbTreatment.Name = "lbTreatment";
            this.lbTreatment.Size = new System.Drawing.Size(251, 36);
            this.lbTreatment.TabIndex = 13;
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(21, 51);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(251, 33);
            this.txtNotes.TabIndex = 12;
            this.txtNotes.Text = "";
            // 
            // cmbTreatmentType
            // 
            this.cmbTreatmentType.FormattingEnabled = true;
            this.cmbTreatmentType.Items.AddRange(new object[] {
            "Cleaning",
            "Crown/Denture",
            "DentalExtraction",
            "Brace"});
            this.cmbTreatmentType.Location = new System.Drawing.Point(21, 20);
            this.cmbTreatmentType.Name = "cmbTreatmentType";
            this.cmbTreatmentType.Size = new System.Drawing.Size(156, 24);
            this.cmbTreatmentType.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Aquamarine;
            this.btnSave.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSave.Location = new System.Drawing.Point(21, 90);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(156, 38);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save Treatment";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(415, 106);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 16);
            this.label8.TabIndex = 17;
            this.label8.Text = "Treatment Done";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(25, 473);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(145, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Treatment Summary";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            legend1.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide;
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(37, 501);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(238, 172);
            this.chart1.TabIndex = 14;
            this.chart1.Text = "chart1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(549, 275);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Patient Overview";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.LastVisit});
            this.dataGridView1.Location = new System.Drawing.Point(552, 297);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(303, 150);
            this.dataGridView1.TabIndex = 12;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Patient";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 125;
            // 
            // LastVisit
            // 
            this.LastVisit.HeaderText = "Last Visit";
            this.LastVisit.MinimumWidth = 6;
            this.LastVisit.Name = "LastVisit";
            this.LastVisit.Width = 125;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(25, 278);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Schedule table";
            // 
            // dgvSched
            // 
            this.dgvSched.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSched.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.Patient,
            this.Treatment,
            this.Status});
            this.dgvSched.Location = new System.Drawing.Point(28, 297);
            this.dgvSched.Name = "dgvSched";
            this.dgvSched.RowHeadersWidth = 51;
            this.dgvSched.RowTemplate.Height = 24;
            this.dgvSched.Size = new System.Drawing.Size(505, 150);
            this.dgvSched.TabIndex = 10;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.MinimumWidth = 6;
            this.Time.Name = "Time";
            this.Time.Width = 125;
            // 
            // Patient
            // 
            this.Patient.HeaderText = "Patient";
            this.Patient.MinimumWidth = 6;
            this.Patient.Name = "Patient";
            this.Patient.Width = 125;
            // 
            // Treatment
            // 
            this.Treatment.HeaderText = "Treatment";
            this.Treatment.MinimumWidth = 6;
            this.Treatment.Name = "Treatment";
            this.Treatment.Width = 125;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.Width = 125;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(612, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Upcoming Appoinment";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(226, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Patient\'s Seen";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Today\'s Appointment";
            // 
            // txtTreatmentDone
            // 
            this.txtTreatmentDone.Location = new System.Drawing.Point(411, 117);
            this.txtTreatmentDone.Name = "txtTreatmentDone";
            this.txtTreatmentDone.Size = new System.Drawing.Size(161, 101);
            this.txtTreatmentDone.TabIndex = 21;
            this.txtTreatmentDone.Text = "";
            // 
            // txtPatientSeen
            // 
            this.txtPatientSeen.Location = new System.Drawing.Point(215, 117);
            this.txtPatientSeen.Name = "txtPatientSeen";
            this.txtPatientSeen.Size = new System.Drawing.Size(150, 101);
            this.txtPatientSeen.TabIndex = 22;
            this.txtPatientSeen.Text = "";
            // 
            // txtTodaysAppoinment
            // 
            this.txtTodaysAppoinment.Location = new System.Drawing.Point(28, 117);
            this.txtTodaysAppoinment.Name = "txtTodaysAppoinment";
            this.txtTodaysAppoinment.Size = new System.Drawing.Size(150, 101);
            this.txtTodaysAppoinment.TabIndex = 23;
            this.txtTodaysAppoinment.Text = "";
            // 
            // txtUpApp
            // 
            this.txtUpApp.Location = new System.Drawing.Point(603, 121);
            this.txtUpApp.Name = "txtUpApp";
            this.txtUpApp.Size = new System.Drawing.Size(180, 101);
            this.txtUpApp.TabIndex = 24;
            this.txtUpApp.Text = "";
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackgroundImage = global::SmileTrack.Properties.Resources.pngtree_this_is_a_cute_cartoon_tooth_icon_vector_png_image_12285812;
            this.pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox6.Location = new System.Drawing.Point(418, 137);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(39, 38);
            this.pictureBox6.TabIndex = 18;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.Azure;
            this.pictureBox7.BackgroundImage = global::SmileTrack.Properties.Resources._4209059;
            this.pictureBox7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox7.Location = new System.Drawing.Point(855, 12);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Padding = new System.Windows.Forms.Padding(5);
            this.pictureBox7.Size = new System.Drawing.Size(64, 77);
            this.pictureBox7.TabIndex = 11;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::SmileTrack.Properties.Resources._5539724;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Location = new System.Drawing.Point(37, 137);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(37, 38);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::SmileTrack.Properties.Resources.dentist_icon_health_checkups_icon_dental_icon_dentistry_patient_oral_hygiene_health_care_therapy_iq_dental_implant_centre_medicine_png_clipart;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox4.Location = new System.Drawing.Point(229, 137);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(38, 38);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackgroundImage = global::SmileTrack.Properties.Resources._3d_realistic_clock_icon_illustration_vector;
            this.pictureBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox5.Location = new System.Drawing.Point(615, 137);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(41, 38);
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::SmileTrack.Properties.Resources._7fbf6c74_85d6_441b_b2e0_5faf4c8635f6;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(180, 128);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Azure;
            this.pictureBox1.BackgroundImage = global::SmileTrack.Properties.Resources._4209059;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(3, 401);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(5);
            this.pictureBox1.Size = new System.Drawing.Size(126, 121);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // DentistDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 734);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.Panel1);
            this.Name = "DentistDashboard";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "DentistDashboard";
            this.Load += new System.EventHandler(this.DentistDashboard_Load);
            this.Panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.TreatmentDone.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSched)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel Panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnMySched;
        private System.Windows.Forms.Button btnTreatments;
        private System.Windows.Forms.Button btnNotes;
        private System.Windows.Forms.Button btnRepots;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.DataGridView dgvSched;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Patient;
        private System.Windows.Forms.DataGridViewTextBoxColumn Treatment;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastVisit;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel TreatmentDone;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbTreatmentType;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox lbTreatment;
        private System.Windows.Forms.RichTextBox txtNotes;
        private System.Windows.Forms.RichTextBox txtTreatmentDone;
        private System.Windows.Forms.RichTextBox txtPatientSeen;
        private System.Windows.Forms.RichTextBox txtTodaysAppoinment;
        private System.Windows.Forms.RichTextBox txtUpApp;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DateTimePicker dtpReportDate;
    }
}