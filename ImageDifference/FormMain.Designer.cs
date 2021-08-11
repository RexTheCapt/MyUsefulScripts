namespace ImageDifference
{
    partial class FormMain
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
            this.pictureBoxCurrent = new System.Windows.Forms.PictureBox();
            this.pictureBoxCheck = new System.Windows.Forms.PictureBox();
            this.pictureBoxCurrent16 = new System.Windows.Forms.PictureBox();
            this.pictureBoxCheck16 = new System.Windows.Forms.PictureBox();
            this.buttonManual = new System.Windows.Forms.Button();
            this.buttonAuto = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSkip = new System.Windows.Forms.Button();
            this.numericUpDownCurrent = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCheck = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPercent = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheck16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPercent)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxCurrent
            // 
            this.pictureBoxCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxCurrent.Location = new System.Drawing.Point(12, 38);
            this.pictureBoxCurrent.Name = "pictureBoxCurrent";
            this.pictureBoxCurrent.Size = new System.Drawing.Size(962, 811);
            this.pictureBoxCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCurrent.TabIndex = 0;
            this.pictureBoxCurrent.TabStop = false;
            // 
            // pictureBoxCheck
            // 
            this.pictureBoxCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCheck.Location = new System.Drawing.Point(1050, 38);
            this.pictureBoxCheck.Name = "pictureBoxCheck";
            this.pictureBoxCheck.Size = new System.Drawing.Size(957, 811);
            this.pictureBoxCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCheck.TabIndex = 1;
            this.pictureBoxCheck.TabStop = false;
            // 
            // pictureBoxCurrent16
            // 
            this.pictureBoxCurrent16.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxCurrent16.Location = new System.Drawing.Point(980, 38);
            this.pictureBoxCurrent16.Name = "pictureBoxCurrent16";
            this.pictureBoxCurrent16.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxCurrent16.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCurrent16.TabIndex = 2;
            this.pictureBoxCurrent16.TabStop = false;
            // 
            // pictureBoxCheck16
            // 
            this.pictureBoxCheck16.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxCheck16.Location = new System.Drawing.Point(980, 108);
            this.pictureBoxCheck16.Name = "pictureBoxCheck16";
            this.pictureBoxCheck16.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxCheck16.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCheck16.TabIndex = 3;
            this.pictureBoxCheck16.TabStop = false;
            // 
            // buttonManual
            // 
            this.buttonManual.Location = new System.Drawing.Point(980, 178);
            this.buttonManual.Name = "buttonManual";
            this.buttonManual.Size = new System.Drawing.Size(64, 23);
            this.buttonManual.TabIndex = 4;
            this.buttonManual.Text = "Manual";
            this.buttonManual.UseVisualStyleBackColor = true;
            this.buttonManual.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonAuto
            // 
            this.buttonAuto.Location = new System.Drawing.Point(980, 207);
            this.buttonAuto.Name = "buttonAuto";
            this.buttonAuto.Size = new System.Drawing.Size(64, 23);
            this.buttonAuto.TabIndex = 5;
            this.buttonAuto.Text = "Auto";
            this.buttonAuto.UseVisualStyleBackColor = true;
            this.buttonAuto.Click += new System.EventHandler(this.buttonAuto_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1047, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "label2";
            // 
            // buttonSkip
            // 
            this.buttonSkip.Location = new System.Drawing.Point(980, 236);
            this.buttonSkip.Name = "buttonSkip";
            this.buttonSkip.Size = new System.Drawing.Size(64, 23);
            this.buttonSkip.TabIndex = 8;
            this.buttonSkip.Text = "Skip";
            this.buttonSkip.UseVisualStyleBackColor = true;
            this.buttonSkip.Click += new System.EventHandler(this.buttonSkip_Click);
            // 
            // numericUpDownCurrent
            // 
            this.numericUpDownCurrent.Location = new System.Drawing.Point(854, 12);
            this.numericUpDownCurrent.Name = "numericUpDownCurrent";
            this.numericUpDownCurrent.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCurrent.TabIndex = 10;
            this.numericUpDownCurrent.ValueChanged += new System.EventHandler(this.SelectionIndex);
            // 
            // numericUpDownCheck
            // 
            this.numericUpDownCheck.Location = new System.Drawing.Point(1886, 12);
            this.numericUpDownCheck.Name = "numericUpDownCheck";
            this.numericUpDownCheck.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCheck.TabIndex = 11;
            this.numericUpDownCheck.ValueChanged += new System.EventHandler(this.SelectionIndex);
            // 
            // numericUpDownPercent
            // 
            this.numericUpDownPercent.DecimalPlaces = 2;
            this.numericUpDownPercent.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownPercent.Location = new System.Drawing.Point(980, 265);
            this.numericUpDownPercent.Name = "numericUpDownPercent";
            this.numericUpDownPercent.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownPercent.TabIndex = 12;
            this.numericUpDownPercent.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDownPercent.ValueChanged += new System.EventHandler(this.numericUpDownPercent_ValueChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(980, 291);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(53, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Show";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2018, 858);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.numericUpDownPercent);
            this.Controls.Add(this.numericUpDownCheck);
            this.Controls.Add(this.numericUpDownCurrent);
            this.Controls.Add(this.buttonSkip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAuto);
            this.Controls.Add(this.buttonManual);
            this.Controls.Add(this.pictureBoxCheck16);
            this.Controls.Add(this.pictureBoxCurrent16);
            this.Controls.Add(this.pictureBoxCheck);
            this.Controls.Add(this.pictureBoxCurrent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Difference";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheck16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPercent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxCurrent;
        private System.Windows.Forms.PictureBox pictureBoxCheck;
        private System.Windows.Forms.PictureBox pictureBoxCurrent16;
        private System.Windows.Forms.PictureBox pictureBoxCheck16;
        private System.Windows.Forms.Button buttonManual;
        private System.Windows.Forms.Button buttonAuto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSkip;
        private System.Windows.Forms.NumericUpDown numericUpDownCurrent;
        private System.Windows.Forms.NumericUpDown numericUpDownCheck;
        private System.Windows.Forms.NumericUpDown numericUpDownPercent;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

