namespace _86boxManager
{
    partial class dlgSettings
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
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblEXEdir = new System.Windows.Forms.Label();
            this.lblCFGdir = new System.Windows.Forms.Label();
            this.txtEXEdir = new System.Windows.Forms.TextBox();
            this.txtCFGdir = new System.Windows.Forms.TextBox();
            this.btnBrowse1 = new System.Windows.Forms.Button();
            this.btnBrowse2 = new System.Windows.Forms.Button();
            this.cbxMinimize = new System.Windows.Forms.CheckBox();
            this.cbxShowConsole = new System.Windows.Forms.CheckBox();
            this.btnDefaults = new System.Windows.Forms.Button();
            this.cbxMinimizeTray = new System.Windows.Forms.CheckBox();
            this.cbxCloseTray = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(567, 164);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 30);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(486, 164);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(405, 164);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblEXEdir
            // 
            this.lblEXEdir.AutoSize = true;
            this.lblEXEdir.Location = new System.Drawing.Point(12, 15);
            this.lblEXEdir.Name = "lblEXEdir";
            this.lblEXEdir.Size = new System.Drawing.Size(91, 21);
            this.lblEXEdir.TabIndex = 3;
            this.lblEXEdir.Text = "86Box path:";
            // 
            // lblCFGdir
            // 
            this.lblCFGdir.AutoSize = true;
            this.lblCFGdir.Location = new System.Drawing.Point(12, 51);
            this.lblCFGdir.Name = "lblCFGdir";
            this.lblCFGdir.Size = new System.Drawing.Size(94, 21);
            this.lblCFGdir.TabIndex = 4;
            this.lblCFGdir.Text = "Config path:";
            // 
            // txtEXEdir
            // 
            this.txtEXEdir.Location = new System.Drawing.Point(112, 12);
            this.txtEXEdir.Name = "txtEXEdir";
            this.txtEXEdir.Size = new System.Drawing.Size(439, 29);
            this.txtEXEdir.TabIndex = 5;
            this.txtEXEdir.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // txtCFGdir
            // 
            this.txtCFGdir.Location = new System.Drawing.Point(112, 48);
            this.txtCFGdir.Name = "txtCFGdir";
            this.txtCFGdir.Size = new System.Drawing.Size(439, 29);
            this.txtCFGdir.TabIndex = 6;
            this.txtCFGdir.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // btnBrowse1
            // 
            this.btnBrowse1.Location = new System.Drawing.Point(557, 12);
            this.btnBrowse1.Name = "btnBrowse1";
            this.btnBrowse1.Size = new System.Drawing.Size(85, 30);
            this.btnBrowse1.TabIndex = 7;
            this.btnBrowse1.Text = "Browse...";
            this.btnBrowse1.UseVisualStyleBackColor = true;
            this.btnBrowse1.Click += new System.EventHandler(this.btnBrowse1_Click);
            // 
            // btnBrowse2
            // 
            this.btnBrowse2.Location = new System.Drawing.Point(557, 48);
            this.btnBrowse2.Name = "btnBrowse2";
            this.btnBrowse2.Size = new System.Drawing.Size(85, 30);
            this.btnBrowse2.TabIndex = 8;
            this.btnBrowse2.Text = "Browse...";
            this.btnBrowse2.UseVisualStyleBackColor = true;
            this.btnBrowse2.Click += new System.EventHandler(this.btnBrowse2_Click);
            // 
            // cbxMinimize
            // 
            this.cbxMinimize.AutoSize = true;
            this.cbxMinimize.Location = new System.Drawing.Point(16, 94);
            this.cbxMinimize.Name = "cbxMinimize";
            this.cbxMinimize.Size = new System.Drawing.Size(355, 25);
            this.cbxMinimize.TabIndex = 9;
            this.cbxMinimize.Text = "Minimize 86Box Manager when a VM is started";
            this.cbxMinimize.UseVisualStyleBackColor = true;
            this.cbxMinimize.CheckedChanged += new System.EventHandler(this.cbxMinimize_CheckedChanged);
            // 
            // cbxShowConsole
            // 
            this.cbxShowConsole.AutoSize = true;
            this.cbxShowConsole.Location = new System.Drawing.Point(377, 94);
            this.cbxShowConsole.Name = "cbxShowConsole";
            this.cbxShowConsole.Size = new System.Drawing.Size(269, 25);
            this.cbxShowConsole.TabIndex = 10;
            this.cbxShowConsole.Text = "Show the console window for VMs";
            this.cbxShowConsole.UseVisualStyleBackColor = true;
            this.cbxShowConsole.CheckedChanged += new System.EventHandler(this.cbxShowConsole_CheckedChanged);
            // 
            // btnDefaults
            // 
            this.btnDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDefaults.Location = new System.Drawing.Point(12, 164);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(75, 30);
            this.btnDefaults.TabIndex = 11;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // cbxMinimizeTray
            // 
            this.cbxMinimizeTray.AutoSize = true;
            this.cbxMinimizeTray.Location = new System.Drawing.Point(16, 125);
            this.cbxMinimizeTray.Name = "cbxMinimizeTray";
            this.cbxMinimizeTray.Size = new System.Drawing.Size(288, 25);
            this.cbxMinimizeTray.TabIndex = 12;
            this.cbxMinimizeTray.Text = "Minimize 86Box Manager to tray icon";
            this.cbxMinimizeTray.UseVisualStyleBackColor = true;
            this.cbxMinimizeTray.CheckedChanged += new System.EventHandler(this.cbxMinimizeTray_CheckedChanged);
            // 
            // cbxCloseTray
            // 
            this.cbxCloseTray.AutoSize = true;
            this.cbxCloseTray.Location = new System.Drawing.Point(377, 125);
            this.cbxCloseTray.Name = "cbxCloseTray";
            this.cbxCloseTray.Size = new System.Drawing.Size(262, 25);
            this.cbxCloseTray.TabIndex = 13;
            this.cbxCloseTray.Text = "Close 86Box Manager to tray icon";
            this.cbxCloseTray.UseVisualStyleBackColor = true;
            this.cbxCloseTray.CheckedChanged += new System.EventHandler(this.cbxCloseTray_CheckedChanged);
            // 
            // dlgSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(654, 206);
            this.Controls.Add(this.cbxCloseTray);
            this.Controls.Add(this.cbxMinimizeTray);
            this.Controls.Add(this.btnDefaults);
            this.Controls.Add(this.cbxShowConsole);
            this.Controls.Add(this.cbxMinimize);
            this.Controls.Add(this.btnBrowse2);
            this.Controls.Add(this.btnBrowse1);
            this.Controls.Add(this.txtCFGdir);
            this.Controls.Add(this.txtEXEdir);
            this.Controls.Add(this.lblCFGdir);
            this.Controls.Add(this.lblEXEdir);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dlgSettings_FormClosing);
            this.Load += new System.EventHandler(this.dlgSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblEXEdir;
        private System.Windows.Forms.Label lblCFGdir;
        private System.Windows.Forms.TextBox txtEXEdir;
        private System.Windows.Forms.TextBox txtCFGdir;
        private System.Windows.Forms.Button btnBrowse1;
        private System.Windows.Forms.Button btnBrowse2;
        private System.Windows.Forms.CheckBox cbxMinimize;
        private System.Windows.Forms.CheckBox cbxShowConsole;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.CheckBox cbxMinimizeTray;
        private System.Windows.Forms.CheckBox cbxCloseTray;
    }
}