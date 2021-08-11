namespace Linker
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.textBoxFrom = new System.Windows.Forms.TextBox();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.buttonAddLink = new System.Windows.Forms.Button();
            this.buttonRemoveLink = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 119);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(776, 290);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFrom.Location = new System.Drawing.Point(12, 25);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(776, 20);
            this.textBoxFrom.TabIndex = 1;
            this.textBoxFrom.Tag = "0";
            // 
            // textBoxTo
            // 
            this.textBoxTo.Location = new System.Drawing.Point(12, 64);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(776, 20);
            this.textBoxTo.TabIndex = 2;
            this.textBoxTo.Tag = "1";
            // 
            // buttonAddLink
            // 
            this.buttonAddLink.Location = new System.Drawing.Point(12, 90);
            this.buttonAddLink.Name = "buttonAddLink";
            this.buttonAddLink.Size = new System.Drawing.Size(776, 23);
            this.buttonAddLink.TabIndex = 3;
            this.buttonAddLink.Text = "Add link";
            this.buttonAddLink.UseVisualStyleBackColor = true;
            this.buttonAddLink.Click += new System.EventHandler(this.ButtonAddLink_Click);
            // 
            // buttonRemoveLink
            // 
            this.buttonRemoveLink.Location = new System.Drawing.Point(12, 415);
            this.buttonRemoveLink.Name = "buttonRemoveLink";
            this.buttonRemoveLink.Size = new System.Drawing.Size(776, 23);
            this.buttonRemoveLink.TabIndex = 4;
            this.buttonRemoveLink.Text = "Remove Link";
            this.buttonRemoveLink.UseVisualStyleBackColor = true;
            this.buttonRemoveLink.Click += new System.EventHandler(this.ButtonRemoveLink_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "To:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRemoveLink);
            this.Controls.Add(this.buttonAddLink);
            this.Controls.Add(this.textBoxTo);
            this.Controls.Add(this.textBoxFrom);
            this.Controls.Add(this.listView1);
            this.Name = "FormMain";
            this.Text = "Linker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox textBoxFrom;
        private System.Windows.Forms.TextBox textBoxTo;
        private System.Windows.Forms.Button buttonAddLink;
        private System.Windows.Forms.Button buttonRemoveLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

