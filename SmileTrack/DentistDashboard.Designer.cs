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
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnMySched = new System.Windows.Forms.Button();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.txtTodaysAppoinment = new System.Windows.Forms.RichTextBox();
            this.txtPatientSeen = new System.Windows.Forms.RichTextBox();
            this.txtTreatmentDone = new System.Windows.Forms.RichTextBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.dgvSched = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSched)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnDashboard.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnDashboard.Location = new System.Drawing.Point(75, 234);
            this.btnDashboard.Margin = new System.Windows.Forms.Padding(2);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(138, 37);
            this.btnDashboard.TabIndex = 1;
            this.btnDashboard.Text = "🏠Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = false;
            // 
            // btnMySched
            // 
            this.btnMySched.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnMySched.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMySched.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnMySched.Location = new System.Drawing.Point(75, 302);
            this.btnMySched.Margin = new System.Windows.Forms.Padding(2);
            this.btnMySched.Name = "btnMySched";
            this.btnMySched.Size = new System.Drawing.Size(138, 35);
            this.btnMySched.TabIndex = 2;
            this.btnMySched.Text = "MySchedule";
            this.btnMySched.UseVisualStyleBackColor = false;
            // 
            // btnLogOut
            // 
            this.btnLogOut.AllowDrop = true;
            this.btnLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLogOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnLogOut.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOut.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnLogOut.Location = new System.Drawing.Point(75, 541);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(138, 37);
            this.btnLogOut.TabIndex = 9;
            this.btnLogOut.Text = "Log-out";
            this.btnLogOut.UseVisualStyleBackColor = false;
            // 
            // txtTodaysAppoinment
            // 
            this.txtTodaysAppoinment.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTodaysAppoinment.Location = new System.Drawing.Point(444, 55);
            this.txtTodaysAppoinment.Margin = new System.Windows.Forms.Padding(2);
            this.txtTodaysAppoinment.Name = "txtTodaysAppoinment";
            this.txtTodaysAppoinment.Size = new System.Drawing.Size(114, 83);
            this.txtTodaysAppoinment.TabIndex = 23;
            this.txtTodaysAppoinment.Text = "";
            // 
            // txtPatientSeen
            // 
            this.txtPatientSeen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatientSeen.Location = new System.Drawing.Point(593, 46);
            this.txtPatientSeen.Margin = new System.Windows.Forms.Padding(2);
            this.txtPatientSeen.Name = "txtPatientSeen";
            this.txtPatientSeen.Size = new System.Drawing.Size(114, 88);
            this.txtPatientSeen.TabIndex = 22;
            this.txtPatientSeen.Text = "";
            // 
            // txtTreatmentDone
            // 
            this.txtTreatmentDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTreatmentDone.Location = new System.Drawing.Point(754, 51);
            this.txtTreatmentDone.Margin = new System.Windows.Forms.Padding(2);
            this.txtTreatmentDone.Name = "txtTreatmentDone";
            this.txtTreatmentDone.Size = new System.Drawing.Size(122, 83);
            this.txtTreatmentDone.TabIndex = 21;
            this.txtTreatmentDone.Text = "";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox4.Location = new System.Drawing.Point(661, 65);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(28, 31);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Location = new System.Drawing.Point(517, 65);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(28, 31);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(441, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Today\'s Appointment";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(600, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Patient\'s Seen";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(766, 46);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Treatment Done";
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox6.Location = new System.Drawing.Point(835, 65);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(29, 31);
            this.pictureBox6.TabIndex = 18;
            this.pictureBox6.TabStop = false;
            // 
            // dgvSched
            // 
            this.dgvSched.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dgvSched.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvSched.Location = new System.Drawing.Point(485, 276);
            this.dgvSched.Name = "dgvSched";
            this.dgvSched.Size = new System.Drawing.Size(506, 108);
            this.dgvSched.TabIndex = 0;
            this.dgvSched.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSched_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(497, 260);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Upcoming Appointment";
            // 
            // DentistDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SmileTrack.Properties.Resources.ChatGPT_Image_Jun_10__2026__12_47_54_PM;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1261, 645);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvSched);
            this.Controls.Add(this.btnDashboard);
            this.Controls.Add(this.btnLogOut);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnMySched);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTreatmentDone);
            this.Controls.Add(this.txtTodaysAppoinment);
            this.Controls.Add(this.txtPatientSeen);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DentistDashboard";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Text = "DentistDashboard";
            this.Load += new System.EventHandler(this.DentistDashboard_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSched)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnMySched;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.RichTextBox txtTodaysAppoinment;
        private System.Windows.Forms.RichTextBox txtPatientSeen;
        private System.Windows.Forms.RichTextBox txtTreatmentDone;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.DataGridView dgvSched;
        private System.Windows.Forms.Label label1;
    }
}