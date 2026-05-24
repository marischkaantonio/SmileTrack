namespace SmileTrack
{
    partial class BillingForm
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
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.dgvInvoiceItems = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPatients = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDiscount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Duedat = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.dgvTreatmentList = new System.Windows.Forms.DataGridView();
            this.colTreatment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpDue = new System.Windows.Forms.DateTimePicker();
            this.dtpInvoiceDate = new System.Windows.Forms.DateTimePicker();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.txtInvoiceNum = new System.Windows.Forms.TextBox();
            this.txtPayment = new System.Windows.Forms.TextBox();
            this.txtPname = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.lblInvoiceDate = new System.Windows.Forms.Label();
            this.lblDuedate = new System.Windows.Forms.Label();
            this.lblInvoiceNum = new System.Windows.Forms.Label();
            this.lblPayment = new System.Windows.Forms.Label();
            this.lblPname = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.colInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaidAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label20 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.dgvInvoiceItems.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTreatmentList)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvInvoiceItems
            // 
            this.dgvInvoiceItems.BackColor = System.Drawing.Color.Gray;
            this.dgvInvoiceItems.Controls.Add(this.button1);
            this.dgvInvoiceItems.Controls.Add(this.btnPatients);
            this.dgvInvoiceItems.Controls.Add(this.panel3);
            this.dgvInvoiceItems.Controls.Add(this.panel2);
            this.dgvInvoiceItems.Controls.Add(this.panel1);
            this.dgvInvoiceItems.Controls.Add(this.label1);
            this.dgvInvoiceItems.Location = new System.Drawing.Point(12, 53);
            this.dgvInvoiceItems.Name = "dgvInvoiceItems";
            this.dgvInvoiceItems.Size = new System.Drawing.Size(1521, 370);
            this.dgvInvoiceItems.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Aquamarine;
            this.button1.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Location = new System.Drawing.Point(1254, 317);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 38);
            this.button1.TabIndex = 5;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // btnPatients
            // 
            this.btnPatients.BackColor = System.Drawing.Color.Aquamarine;
            this.btnPatients.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPatients.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnPatients.Location = new System.Drawing.Point(1068, 317);
            this.btnPatients.Name = "btnPatients";
            this.btnPatients.Size = new System.Drawing.Size(145, 38);
            this.btnPatients.TabIndex = 4;
            this.btnPatients.Text = "Save Invoice";
            this.btnPatients.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.txtTax);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.txtDiscount);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.lblSubtotal);
            this.panel3.Controls.Add(this.lbl3);
            this.panel3.Location = new System.Drawing.Point(1222, 40);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(299, 271);
            this.panel3.TabIndex = 3;
            
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel6.Controls.Add(this.lblTotalAmount);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Location = new System.Drawing.Point(3, 181);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(275, 104);
            this.panel6.TabIndex = 5;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.ForeColor = System.Drawing.Color.Blue;
            this.lblTotalAmount.Location = new System.Drawing.Point(146, 23);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(70, 25);
            this.lblTotalAmount.TabIndex = 18;
            this.lblTotalAmount.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "Total Amount";
            // 
            // txtTax
            // 
            this.txtTax.Location = new System.Drawing.Point(166, 106);
            this.txtTax.Name = "txtTax";
            this.txtTax.Size = new System.Drawing.Size(100, 22);
            this.txtTax.TabIndex = 17;
            this.txtTax.TextChanged += new System.EventHandler(this.txtTax_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Tax";
            // 
            // txtDiscount
            // 
            this.txtDiscount.Location = new System.Drawing.Point(166, 52);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.Size = new System.Drawing.Size(100, 22);
            this.txtDiscount.TabIndex = 15;
            this.txtDiscount.TextChanged += new System.EventHandler(this.txtDiscount_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "Discount %";
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.Location = new System.Drawing.Point(163, 16);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(50, 16);
            this.lblSubtotal.TabIndex = 13;
            this.lblSubtotal.Text = "label3";
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl3.Location = new System.Drawing.Point(14, 16);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(74, 16);
            this.lbl3.TabIndex = 12;
            this.lbl3.Text = "Sub Total";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.Duedat);
            this.panel2.Controls.Add(this.lblStatus);
            this.panel2.Controls.Add(this.btnAddItem);
            this.panel2.Controls.Add(this.dgvTreatmentList);
            this.panel2.Location = new System.Drawing.Point(446, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(767, 271);
            this.panel2.TabIndex = 2;
            // 
            // Duedat
            // 
            this.Duedat.AutoSize = true;
            this.Duedat.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Duedat.Location = new System.Drawing.Point(748, 247);
            this.Duedat.Name = "Duedat";
            this.Duedat.Size = new System.Drawing.Size(11, 16);
            this.Duedat.TabIndex = 13;
            this.Duedat.Text = ".";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 243);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(11, 16);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = ".";
            // 
            // btnAddItem
            // 
            this.btnAddItem.BackColor = System.Drawing.Color.DarkGray;
            this.btnAddItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAddItem.Location = new System.Drawing.Point(636, 231);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(106, 32);
            this.btnAddItem.TabIndex = 1;
            this.btnAddItem.Text = "+ Add Item";
            this.btnAddItem.UseVisualStyleBackColor = false;
            // 
            // dgvTreatmentList
            // 
            this.dgvTreatmentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTreatmentList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTreatment,
            this.colDescription,
            this.colQty,
            this.colUnitPrice,
            this.colAmount,
            this.Action});
            this.dgvTreatmentList.Location = new System.Drawing.Point(3, 3);
            this.dgvTreatmentList.Name = "dgvTreatmentList";
            this.dgvTreatmentList.RowHeadersWidth = 51;
            this.dgvTreatmentList.RowTemplate.Height = 24;
            this.dgvTreatmentList.Size = new System.Drawing.Size(756, 150);
            this.dgvTreatmentList.TabIndex = 0;
            this.dgvTreatmentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // colTreatment
            // 
            this.colTreatment.HeaderText = "Treatment";
            this.colTreatment.MaxInputLength = 300;
            this.colTreatment.MinimumWidth = 6;
            this.colTreatment.Name = "colTreatment";
            this.colTreatment.Width = 125;
            // 
            // colDescription
            // 
            this.colDescription.HeaderText = "Description";
            this.colDescription.MaxInputLength = 300;
            this.colDescription.MinimumWidth = 6;
            this.colDescription.Name = "colDescription";
            this.colDescription.Width = 125;
            // 
            // colQty
            // 
            this.colQty.HeaderText = "Qty";
            this.colQty.MaxInputLength = 5;
            this.colQty.MinimumWidth = 6;
            this.colQty.Name = "colQty";
            this.colQty.Width = 125;
            // 
            // colUnitPrice
            // 
            this.colUnitPrice.HeaderText = "Unit Price";
            this.colUnitPrice.MaxInputLength = 100;
            this.colUnitPrice.MinimumWidth = 6;
            this.colUnitPrice.Name = "colUnitPrice";
            this.colUnitPrice.Width = 125;
            // 
            // colAmount
            // 
            this.colAmount.HeaderText = "Amount";
            this.colAmount.MaxInputLength = 100;
            this.colAmount.MinimumWidth = 6;
            this.colAmount.Name = "colAmount";
            this.colAmount.Width = 125;
            // 
            // Action
            // 
            this.Action.HeaderText = "Action";
            this.Action.MaxInputLength = 100;
            this.Action.MinimumWidth = 6;
            this.Action.Name = "Action";
            this.Action.Width = 125;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.dtpDue);
            this.panel1.Controls.Add(this.dtpInvoiceDate);
            this.panel1.Controls.Add(this.txtNotes);
            this.panel1.Controls.Add(this.txtInvoiceNum);
            this.panel1.Controls.Add(this.txtPayment);
            this.panel1.Controls.Add(this.txtPname);
            this.panel1.Controls.Add(this.lblNotes);
            this.panel1.Controls.Add(this.lblInvoiceDate);
            this.panel1.Controls.Add(this.lblDuedate);
            this.panel1.Controls.Add(this.lblInvoiceNum);
            this.panel1.Controls.Add(this.lblPayment);
            this.panel1.Controls.Add(this.lblPname);
            this.panel1.Location = new System.Drawing.Point(4, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(436, 271);
            this.panel1.TabIndex = 1;
            // 
            // dtpDue
            // 
            this.dtpDue.Location = new System.Drawing.Point(9, 160);
            this.dtpDue.Name = "dtpDue";
            this.dtpDue.Size = new System.Drawing.Size(173, 22);
            this.dtpDue.TabIndex = 11;
            this.dtpDue.ValueChanged += new System.EventHandler(this.dtpDue_ValueChanged);
            // 
            // dtpInvoiceDate
            // 
            this.dtpInvoiceDate.Location = new System.Drawing.Point(9, 86);
            this.dtpInvoiceDate.Name = "dtpInvoiceDate";
            this.dtpInvoiceDate.Size = new System.Drawing.Size(173, 22);
            this.dtpInvoiceDate.TabIndex = 10;
            this.dtpInvoiceDate.ValueChanged += new System.EventHandler(this.dtpInvoiceDate_ValueChanged);
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(227, 146);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(164, 82);
            this.txtNotes.TabIndex = 9;
            // 
            // txtInvoiceNum
            // 
            this.txtInvoiceNum.Location = new System.Drawing.Point(227, 36);
            this.txtInvoiceNum.Name = "txtInvoiceNum";
            this.txtInvoiceNum.Size = new System.Drawing.Size(164, 22);
            this.txtInvoiceNum.TabIndex = 8;
            this.txtInvoiceNum.TextChanged += new System.EventHandler(this.txtInvoiceNum_TextChanged);
            // 
            // txtPayment
            // 
            this.txtPayment.Location = new System.Drawing.Point(227, 85);
            this.txtPayment.Name = "txtPayment";
            this.txtPayment.Size = new System.Drawing.Size(164, 22);
            this.txtPayment.TabIndex = 7;
            this.txtPayment.TextChanged += new System.EventHandler(this.txtPayment_TextChanged);
            // 
            // txtPname
            // 
            this.txtPname.Location = new System.Drawing.Point(9, 36);
            this.txtPname.Name = "txtPname";
            this.txtPname.Size = new System.Drawing.Size(173, 22);
            this.txtPname.TabIndex = 6;
            this.txtPname.TextChanged += new System.EventHandler(this.TxtPname_TextChanged);
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotes.Location = new System.Drawing.Point(227, 127);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(58, 16);
            this.lblNotes.TabIndex = 5;
            this.lblNotes.Text = "Notes *";
            // 
            // lblInvoiceDate
            // 
            this.lblInvoiceDate.AutoSize = true;
            this.lblInvoiceDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvoiceDate.Location = new System.Drawing.Point(6, 66);
            this.lblInvoiceDate.Name = "lblInvoiceDate";
            this.lblInvoiceDate.Size = new System.Drawing.Size(104, 16);
            this.lblInvoiceDate.TabIndex = 4;
            this.lblInvoiceDate.Text = "Invoice Date *";
            // 
            // lblDuedate
            // 
            this.lblDuedate.AutoSize = true;
            this.lblDuedate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuedate.Location = new System.Drawing.Point(6, 127);
            this.lblDuedate.Name = "lblDuedate";
            this.lblDuedate.Size = new System.Drawing.Size(82, 16);
            this.lblDuedate.TabIndex = 3;
            this.lblDuedate.Text = "Due Date *";
            // 
            // lblInvoiceNum
            // 
            this.lblInvoiceNum.AutoSize = true;
            this.lblInvoiceNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvoiceNum.Location = new System.Drawing.Point(224, 16);
            this.lblInvoiceNum.Name = "lblInvoiceNum";
            this.lblInvoiceNum.Size = new System.Drawing.Size(125, 16);
            this.lblInvoiceNum.TabIndex = 2;
            this.lblInvoiceNum.Text = "Invoice Number *";
            // 
            // lblPayment
            // 
            this.lblPayment.AutoSize = true;
            this.lblPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayment.Location = new System.Drawing.Point(224, 66);
            this.lblPayment.Name = "lblPayment";
            this.lblPayment.Size = new System.Drawing.Size(77, 16);
            this.lblPayment.TabIndex = 1;
            this.lblPayment.Text = "Payment *";
            // 
            // lblPname
            // 
            this.lblPname.AutoSize = true;
            this.lblPname.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPname.Location = new System.Drawing.Point(6, 16);
            this.lblPname.Name = "lblPname";
            this.lblPname.Size = new System.Drawing.Size(110, 16);
            this.lblPname.TabIndex = 0;
            this.lblPname.Text = "Patient Name *";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Create New Invoice";
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.Gray;
            this.panel11.Controls.Add(this.panel12);
            this.panel11.Controls.Add(this.label19);
            this.panel11.Location = new System.Drawing.Point(12, 429);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(1521, 288);
            this.panel11.TabIndex = 5;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.White;
            this.panel12.Controls.Add(this.dataGridView3);
            this.panel12.Controls.Add(this.label20);
            this.panel12.Controls.Add(this.comboBox1);
            this.panel12.Controls.Add(this.label24);
            this.panel12.Controls.Add(this.cmbStatus);
            this.panel12.Controls.Add(this.label23);
            this.panel12.Controls.Add(this.dtpTo);
            this.panel12.Controls.Add(this.dtpFrom);
            this.panel12.Controls.Add(this.textBox8);
            this.panel12.Controls.Add(this.textBox11);
            this.panel12.Controls.Add(this.label21);
            this.panel12.Controls.Add(this.label22);
            this.panel12.Location = new System.Drawing.Point(0, 39);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(1263, 284);
            this.panel12.TabIndex = 12;
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInvoiceNo,
            this.colInvoiceDate,
            this.colPatientName,
            this.colTotal,
            this.colPaidAmount,
            this.colBalance,
            this.colStatus});
            this.dataGridView3.Location = new System.Drawing.Point(8, 78);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowHeadersWidth = 51;
            this.dataGridView3.RowTemplate.Height = 24;
            this.dataGridView3.Size = new System.Drawing.Size(926, 150);
            this.dataGridView3.TabIndex = 2;
            // 
            // colInvoiceNo
            // 
            this.colInvoiceNo.HeaderText = "Invoice No.";
            this.colInvoiceNo.MaxInputLength = 300;
            this.colInvoiceNo.MinimumWidth = 6;
            this.colInvoiceNo.Name = "colInvoiceNo";
            this.colInvoiceNo.Width = 125;
            // 
            // colInvoiceDate
            // 
            this.colInvoiceDate.HeaderText = "Invoice Date";
            this.colInvoiceDate.MaxInputLength = 300;
            this.colInvoiceDate.MinimumWidth = 6;
            this.colInvoiceDate.Name = "colInvoiceDate";
            this.colInvoiceDate.Width = 125;
            // 
            // colPatientName
            // 
            this.colPatientName.HeaderText = "Patient Name";
            this.colPatientName.MaxInputLength = 5;
            this.colPatientName.MinimumWidth = 6;
            this.colPatientName.Name = "colPatientName";
            this.colPatientName.Width = 125;
            // 
            // colTotal
            // 
            this.colTotal.HeaderText = "Total Amount";
            this.colTotal.MaxInputLength = 100;
            this.colTotal.MinimumWidth = 6;
            this.colTotal.Name = "colTotal";
            this.colTotal.Width = 125;
            // 
            // colPaidAmount
            // 
            this.colPaidAmount.HeaderText = "Paid Amount";
            this.colPaidAmount.MaxInputLength = 100;
            this.colPaidAmount.MinimumWidth = 6;
            this.colPaidAmount.Name = "colPaidAmount";
            this.colPaidAmount.Width = 125;
            // 
            // colBalance
            // 
            this.colBalance.HeaderText = "Balance";
            this.colBalance.MaxInputLength = 100;
            this.colBalance.MinimumWidth = 6;
            this.colBalance.Name = "colBalance";
            this.colBalance.Width = 125;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.Width = 125;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.Blue;
            this.label20.Location = new System.Drawing.Point(767, 17);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(42, 16);
            this.label20.TabIndex = 16;
            this.label20.Text = "Filter";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(770, 40);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 15;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(361, 17);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(42, 16);
            this.label24.TabIndex = 14;
            this.label24.Text = "From";
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(216, 36);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(121, 24);
            this.cmbStatus.TabIndex = 13;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(213, 17);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(50, 16);
            this.label23.TabIndex = 12;
            this.label23.Text = "Status";
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(563, 38);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(173, 22);
            this.dtpTo.TabIndex = 11;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(364, 37);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(173, 22);
            this.dtpFrom.TabIndex = 10;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(227, 146);
            this.textBox8.Multiline = true;
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(164, 82);
            this.textBox8.TabIndex = 9;
            // 
            // textBox11
            // 
            this.textBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox11.ForeColor = System.Drawing.Color.DimGray;
            this.textBox11.Location = new System.Drawing.Point(4, 36);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(173, 22);
            this.textBox11.TabIndex = 6;
            this.textBox11.Text = "Search Name/ Invoice No.";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(9, 17);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(108, 16);
            this.label21.TabIndex = 4;
            this.label21.Text = "Search Patient";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(560, 17);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(26, 16);
            this.label22.TabIndex = 3;
            this.label22.Text = "To";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label19.Location = new System.Drawing.Point(10, 9);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(150, 16);
            this.label19.TabIndex = 0;
            this.label19.Text = "Today\'s Appoinment";
            // 
            // btnLast
            // 
            this.btnLast.BackColor = System.Drawing.Color.Blue;
            this.btnLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLast.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnLast.Location = new System.Drawing.Point(907, 713);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(33, 31);
            this.btnLast.TabIndex = 34;
            this.btnLast.Text = ">";
            this.btnLast.UseVisualStyleBackColor = false;
            // 
            // btnFirst
            // 
            this.btnFirst.BackColor = System.Drawing.Color.Blue;
            this.btnFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFirst.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnFirst.Location = new System.Drawing.Point(853, 713);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(35, 31);
            this.btnFirst.TabIndex = 33;
            this.btnFirst.Text = "1";
            this.btnFirst.UseVisualStyleBackColor = false;
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Blue;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnNext.Location = new System.Drawing.Point(692, 713);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(33, 31);
            this.btnNext.TabIndex = 32;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = false;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.Blue;
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnPrev.Location = new System.Drawing.Point(640, 713);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(32, 31);
            this.btnPrev.TabIndex = 31;
            this.btnPrev.Text = "<";
            this.btnPrev.UseVisualStyleBackColor = false;
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(745, 720);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(76, 16);
            this.lblPageInfo.TabIndex = 30;
            this.lblPageInfo.Text = "Page 1 0f N";
            // 
            // BillingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1542, 597);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.dgvInvoiceItems);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblPageInfo);
            this.Controls.Add(this.btnPrev);
            this.Name = "BillingForm";
            this.Text = "Billing";
            this.dgvInvoiceItems.ResumeLayout(false);
            this.dgvInvoiceItems.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTreatmentList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.Panel dgvInvoiceItems;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblPname;
        private System.Windows.Forms.Label lblInvoiceDate;
        private System.Windows.Forms.Label lblDuedate;
        private System.Windows.Forms.Label lblInvoiceNum;
        private System.Windows.Forms.Label lblPayment;
        private System.Windows.Forms.DateTimePicker dtpDue;
        private System.Windows.Forms.DateTimePicker dtpInvoiceDate;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.TextBox txtInvoiceNum;
        private System.Windows.Forms.TextBox txtPayment;
        private System.Windows.Forms.TextBox txtPname;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvTreatmentList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTreatment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Button btnPatients;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDiscount;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaidAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label Duedat;
    }
}