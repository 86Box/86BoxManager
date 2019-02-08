namespace _86BoxManager
{
    partial class dlgAbout
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lnkGithub = new System.Windows.Forms.LinkLabel();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lnkGithub2 = new System.Windows.Forms.LinkLabel();
            this.lblVersion1 = new System.Windows.Forms.Label();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(135, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(154, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "86Box Manager";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnOK.Location = new System.Drawing.Point(162, 11);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Close";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDesc.Location = new System.Drawing.Point(12, 52);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(307, 19);
            this.lblDesc.TabIndex = 2;
            this.lblDesc.Text = "A configuration manager for the 86Box emulator";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVersion.Location = new System.Drawing.Point(12, 80);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(57, 19);
            this.lblVersion.TabIndex = 3;
            this.lblVersion.Text = "Version:";
            // 
            // lnkGithub
            // 
            this.lnkGithub.AutoSize = true;
            this.lnkGithub.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lnkGithub.Location = new System.Drawing.Point(12, 197);
            this.lnkGithub.Name = "lnkGithub";
            this.lnkGithub.Size = new System.Drawing.Size(264, 19);
            this.lnkGithub.TabIndex = 5;
            this.lnkGithub.TabStop = true;
            this.lnkGithub.Text = "https://github.com/86Box/86BoxManager";
            this.lnkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGithub_LinkClicked);
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCopyright.Location = new System.Drawing.Point(12, 108);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(368, 57);
            this.lblCopyright.TabIndex = 6;
            this.lblCopyright.Text = "Copyright © 2018-2019 David Simunič\r\nLicensed under the MIT license.  See the LIC" +
    "ENSE file for\r\nlicense information and AUTHORS for a list of contributors.";
            // 
            // lnkGithub2
            // 
            this.lnkGithub2.AutoSize = true;
            this.lnkGithub2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lnkGithub2.Location = new System.Drawing.Point(12, 174);
            this.lnkGithub2.Name = "lnkGithub2";
            this.lnkGithub2.Size = new System.Drawing.Size(209, 19);
            this.lnkGithub2.TabIndex = 7;
            this.lnkGithub2.TabStop = true;
            this.lnkGithub2.Text = "https://github.com/86Box/86Box";
            this.lnkGithub2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGuthub2_LinkClicked);
            // 
            // lblVersion1
            // 
            this.lblVersion1.AutoSize = true;
            this.lblVersion1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVersion1.Location = new System.Drawing.Point(65, 80);
            this.lblVersion1.Name = "lblVersion1";
            this.lblVersion1.Size = new System.Drawing.Size(137, 19);
            this.lblVersion1.TabIndex = 8;
            this.lblVersion1.Text = "<version goes here>";
            // 
            // imgLogo
            // 
            this.imgLogo.Image = global::_86BoxManager.Properties.Resources._86Box;
            this.imgLogo.Location = new System.Drawing.Point(100, 11);
            this.imgLogo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(32, 32);
            this.imgLogo.TabIndex = 9;
            this.imgLogo.TabStop = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 232);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(389, 52);
            this.pnlBottom.TabIndex = 15;
            // 
            // dlgAbout
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(389, 284);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.lnkGithub);
            this.Controls.Add(this.imgLogo);
            this.Controls.Add(this.lblVersion1);
            this.Controls.Add(this.lnkGithub2);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About 86Box Manager";
            this.Load += new System.EventHandler(this.dlgAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.LinkLabel lnkGithub;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.LinkLabel lnkGithub2;
        private System.Windows.Forms.Label lblVersion1;
        private System.Windows.Forms.PictureBox imgLogo;
        private System.Windows.Forms.Panel pnlBottom;
    }
}