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
            this.components = new System.ComponentModel.Container();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbxShowConsole = new System.Windows.Forms.CheckBox();
            this.btnDefaults = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblLaunchTimeout = new System.Windows.Forms.Label();
            this.txtLaunchTimeout = new System.Windows.Forms.TextBox();
            this.tbcSettings = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.gbxBehaviour = new System.Windows.Forms.GroupBox();
            this.cbxMinimizeTray = new System.Windows.Forms.CheckBox();
            this.cbxCloseTray = new System.Windows.Forms.CheckBox();
            this.cbxMinimize = new System.Windows.Forms.CheckBox();
            this.gbxPaths = new System.Windows.Forms.GroupBox();
            this.lbl86BoxVer1 = new System.Windows.Forms.Label();
            this.lbl86BoxVer = new System.Windows.Forms.Label();
            this.lblCFGdir = new System.Windows.Forms.Label();
            this.txtCFGdir = new System.Windows.Forms.TextBox();
            this.txtEXEdir = new System.Windows.Forms.TextBox();
            this.btnBrowse2 = new System.Windows.Forms.Button();
            this.lblEXEdir = new System.Windows.Forms.Label();
            this.btnBrowse1 = new System.Windows.Forms.Button();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.gbxMisc = new System.Windows.Forms.GroupBox();
            this.cbxGrid = new System.Windows.Forms.CheckBox();
            this.btnBrowse3 = new System.Windows.Forms.Button();
            this.txtLogPath = new System.Windows.Forms.TextBox();
            this.cbxLogging = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlBottom.SuspendLayout();
            this.tbcSettings.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.gbxBehaviour.SuspendLayout();
            this.gbxPaths.SuspendLayout();
            this.tabAdvanced.SuspendLayout();
            this.gbxMisc.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Enabled = false;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnApply.Location = new System.Drawing.Point(633, 11);
            this.btnApply.Margin = new System.Windows.Forms.Padding(2);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(65, 30);
            this.btnApply.TabIndex = 17;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(564, 11);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 30);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(495, 11);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 30);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbxShowConsole
            // 
            this.cbxShowConsole.AutoSize = true;
            this.cbxShowConsole.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxShowConsole.Location = new System.Drawing.Point(285, 146);
            this.cbxShowConsole.Margin = new System.Windows.Forms.Padding(2);
            this.cbxShowConsole.Name = "cbxShowConsole";
            this.cbxShowConsole.Size = new System.Drawing.Size(217, 24);
            this.cbxShowConsole.TabIndex = 13;
            this.cbxShowConsole.Text = "Enable 86Box console window";
            this.cbxShowConsole.UseVisualStyleBackColor = true;
            this.cbxShowConsole.CheckedChanged += new System.EventHandler(this.cbx_CheckedChanged);
            // 
            // btnDefaults
            // 
            this.btnDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDefaults.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDefaults.Location = new System.Drawing.Point(15, 11);
            this.btnDefaults.Margin = new System.Windows.Forms.Padding(2);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(65, 30);
            this.btnDefaults.TabIndex = 14;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnApply);
            this.pnlBottom.Controls.Add(this.btnDefaults);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 284);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(2);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(709, 52);
            this.pnlBottom.TabIndex = 14;
            // 
            // lblLaunchTimeout
            // 
            this.lblLaunchTimeout.AutoSize = true;
            this.lblLaunchTimeout.Location = new System.Drawing.Point(5, 33);
            this.lblLaunchTimeout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLaunchTimeout.Name = "lblLaunchTimeout";
            this.lblLaunchTimeout.Size = new System.Drawing.Size(254, 19);
            this.lblLaunchTimeout.TabIndex = 15;
            this.lblLaunchTimeout.Text = "Launch timeout:                   miliseconds";
            // 
            // txtLaunchTimeout
            // 
            this.txtLaunchTimeout.Location = new System.Drawing.Point(117, 30);
            this.txtLaunchTimeout.Margin = new System.Windows.Forms.Padding(2);
            this.txtLaunchTimeout.MaxLength = 5;
            this.txtLaunchTimeout.Name = "txtLaunchTimeout";
            this.txtLaunchTimeout.Size = new System.Drawing.Size(56, 25);
            this.txtLaunchTimeout.TabIndex = 8;
            this.txtLaunchTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLaunchTimeout.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // tbcSettings
            // 
            this.tbcSettings.Controls.Add(this.tabGeneral);
            this.tbcSettings.Controls.Add(this.tabAdvanced);
            this.tbcSettings.Location = new System.Drawing.Point(12, 12);
            this.tbcSettings.Name = "tbcSettings";
            this.tbcSettings.SelectedIndex = 0;
            this.tbcSettings.Size = new System.Drawing.Size(689, 263);
            this.tbcSettings.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.gbxBehaviour);
            this.tabGeneral.Controls.Add(this.gbxPaths);
            this.tabGeneral.Location = new System.Drawing.Point(4, 26);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(681, 233);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // gbxBehaviour
            // 
            this.gbxBehaviour.Controls.Add(this.cbxMinimizeTray);
            this.gbxBehaviour.Controls.Add(this.cbxCloseTray);
            this.gbxBehaviour.Controls.Add(this.cbxMinimize);
            this.gbxBehaviour.Location = new System.Drawing.Point(6, 141);
            this.gbxBehaviour.Name = "gbxBehaviour";
            this.gbxBehaviour.Size = new System.Drawing.Size(669, 86);
            this.gbxBehaviour.TabIndex = 6;
            this.gbxBehaviour.TabStop = false;
            this.gbxBehaviour.Text = "Behaviour";
            // 
            // cbxMinimizeTray
            // 
            this.cbxMinimizeTray.AutoSize = true;
            this.cbxMinimizeTray.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxMinimizeTray.Location = new System.Drawing.Point(9, 23);
            this.cbxMinimizeTray.Margin = new System.Windows.Forms.Padding(2);
            this.cbxMinimizeTray.Name = "cbxMinimizeTray";
            this.cbxMinimizeTray.Size = new System.Drawing.Size(264, 24);
            this.cbxMinimizeTray.TabIndex = 5;
            this.cbxMinimizeTray.Text = "Minimize 86Box Manager to tray icon";
            this.cbxMinimizeTray.UseVisualStyleBackColor = true;
            this.cbxMinimizeTray.CheckedChanged += new System.EventHandler(this.cbx_CheckedChanged);
            // 
            // cbxCloseTray
            // 
            this.cbxCloseTray.AutoSize = true;
            this.cbxCloseTray.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxCloseTray.Location = new System.Drawing.Point(337, 23);
            this.cbxCloseTray.Margin = new System.Windows.Forms.Padding(2);
            this.cbxCloseTray.Name = "cbxCloseTray";
            this.cbxCloseTray.Size = new System.Drawing.Size(242, 24);
            this.cbxCloseTray.TabIndex = 6;
            this.cbxCloseTray.Text = "Close 86Box Manager to tray icon";
            this.cbxCloseTray.UseVisualStyleBackColor = true;
            this.cbxCloseTray.CheckedChanged += new System.EventHandler(this.cbx_CheckedChanged);
            // 
            // cbxMinimize
            // 
            this.cbxMinimize.AutoSize = true;
            this.cbxMinimize.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxMinimize.Location = new System.Drawing.Point(9, 51);
            this.cbxMinimize.Margin = new System.Windows.Forms.Padding(2);
            this.cbxMinimize.Name = "cbxMinimize";
            this.cbxMinimize.Size = new System.Drawing.Size(324, 24);
            this.cbxMinimize.TabIndex = 7;
            this.cbxMinimize.Text = "Minimize 86Box Manager when a VM is started";
            this.cbxMinimize.UseVisualStyleBackColor = true;
            this.cbxMinimize.CheckedChanged += new System.EventHandler(this.cbx_CheckedChanged);
            // 
            // gbxPaths
            // 
            this.gbxPaths.Controls.Add(this.lbl86BoxVer1);
            this.gbxPaths.Controls.Add(this.lbl86BoxVer);
            this.gbxPaths.Controls.Add(this.lblCFGdir);
            this.gbxPaths.Controls.Add(this.txtCFGdir);
            this.gbxPaths.Controls.Add(this.txtEXEdir);
            this.gbxPaths.Controls.Add(this.btnBrowse2);
            this.gbxPaths.Controls.Add(this.lblEXEdir);
            this.gbxPaths.Controls.Add(this.btnBrowse1);
            this.gbxPaths.Location = new System.Drawing.Point(6, 6);
            this.gbxPaths.Name = "gbxPaths";
            this.gbxPaths.Size = new System.Drawing.Size(669, 129);
            this.gbxPaths.TabIndex = 5;
            this.gbxPaths.TabStop = false;
            this.gbxPaths.Text = "Paths";
            // 
            // lbl86BoxVer1
            // 
            this.lbl86BoxVer1.AutoSize = true;
            this.lbl86BoxVer1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbl86BoxVer1.Location = new System.Drawing.Point(100, 59);
            this.lbl86BoxVer1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl86BoxVer1.Name = "lbl86BoxVer1";
            this.lbl86BoxVer1.Size = new System.Drawing.Size(69, 19);
            this.lbl86BoxVer1.TabIndex = 6;
            this.lbl86BoxVer1.Text = "Unknown";
            // 
            // lbl86BoxVer
            // 
            this.lbl86BoxVer.AutoSize = true;
            this.lbl86BoxVer.Location = new System.Drawing.Point(5, 59);
            this.lbl86BoxVer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl86BoxVer.Name = "lbl86BoxVer";
            this.lbl86BoxVer.Size = new System.Drawing.Size(98, 19);
            this.lbl86BoxVer.TabIndex = 5;
            this.lbl86BoxVer.Text = "86Box version:";
            // 
            // lblCFGdir
            // 
            this.lblCFGdir.AutoSize = true;
            this.lblCFGdir.Location = new System.Drawing.Point(5, 93);
            this.lblCFGdir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCFGdir.Name = "lblCFGdir";
            this.lblCFGdir.Size = new System.Drawing.Size(66, 19);
            this.lblCFGdir.TabIndex = 4;
            this.lblCFGdir.Text = "VM path:";
            // 
            // txtCFGdir
            // 
            this.txtCFGdir.Location = new System.Drawing.Point(93, 90);
            this.txtCFGdir.Margin = new System.Windows.Forms.Padding(2);
            this.txtCFGdir.Name = "txtCFGdir";
            this.txtCFGdir.Size = new System.Drawing.Size(497, 25);
            this.txtCFGdir.TabIndex = 3;
            this.txtCFGdir.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // txtEXEdir
            // 
            this.txtEXEdir.Location = new System.Drawing.Point(93, 23);
            this.txtEXEdir.Margin = new System.Windows.Forms.Padding(2);
            this.txtEXEdir.Name = "txtEXEdir";
            this.txtEXEdir.Size = new System.Drawing.Size(497, 25);
            this.txtEXEdir.TabIndex = 1;
            this.txtEXEdir.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // btnBrowse2
            // 
            this.btnBrowse2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse2.Location = new System.Drawing.Point(594, 87);
            this.btnBrowse2.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse2.Name = "btnBrowse2";
            this.btnBrowse2.Size = new System.Drawing.Size(70, 30);
            this.btnBrowse2.TabIndex = 4;
            this.btnBrowse2.Text = "Browse...";
            this.btnBrowse2.UseVisualStyleBackColor = true;
            this.btnBrowse2.Click += new System.EventHandler(this.btnBrowse2_Click);
            // 
            // lblEXEdir
            // 
            this.lblEXEdir.AutoSize = true;
            this.lblEXEdir.Location = new System.Drawing.Point(5, 26);
            this.lblEXEdir.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEXEdir.Name = "lblEXEdir";
            this.lblEXEdir.Size = new System.Drawing.Size(82, 19);
            this.lblEXEdir.TabIndex = 3;
            this.lblEXEdir.Text = "86Box path:";
            // 
            // btnBrowse1
            // 
            this.btnBrowse1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnBrowse1.Location = new System.Drawing.Point(594, 20);
            this.btnBrowse1.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse1.Name = "btnBrowse1";
            this.btnBrowse1.Size = new System.Drawing.Size(70, 30);
            this.btnBrowse1.TabIndex = 2;
            this.btnBrowse1.Text = "Browse...";
            this.btnBrowse1.UseVisualStyleBackColor = true;
            this.btnBrowse1.Click += new System.EventHandler(this.btnBrowse1_Click);
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Controls.Add(this.gbxMisc);
            this.tabAdvanced.Location = new System.Drawing.Point(4, 26);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdvanced.Size = new System.Drawing.Size(681, 233);
            this.tabAdvanced.TabIndex = 1;
            this.tabAdvanced.Text = "Advanced";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // gbxMisc
            // 
            this.gbxMisc.Controls.Add(this.cbxGrid);
            this.gbxMisc.Controls.Add(this.txtLaunchTimeout);
            this.gbxMisc.Controls.Add(this.btnBrowse3);
            this.gbxMisc.Controls.Add(this.cbxShowConsole);
            this.gbxMisc.Controls.Add(this.txtLogPath);
            this.gbxMisc.Controls.Add(this.lblLaunchTimeout);
            this.gbxMisc.Controls.Add(this.cbxLogging);
            this.gbxMisc.Location = new System.Drawing.Point(6, 6);
            this.gbxMisc.Name = "gbxMisc";
            this.gbxMisc.Size = new System.Drawing.Size(669, 221);
            this.gbxMisc.TabIndex = 17;
            this.gbxMisc.TabStop = false;
            this.gbxMisc.Text = "Miscellaneous";
            // 
            // cbxGrid
            // 
            this.cbxGrid.AutoSize = true;
            this.cbxGrid.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxGrid.Location = new System.Drawing.Point(9, 146);
            this.cbxGrid.Margin = new System.Windows.Forms.Padding(2);
            this.cbxGrid.Name = "cbxGrid";
            this.cbxGrid.Size = new System.Drawing.Size(272, 24);
            this.cbxGrid.TabIndex = 12;
            this.cbxGrid.Text = "Enable grid lines in virtual machines list";
            this.cbxGrid.UseVisualStyleBackColor = true;
            this.cbxGrid.CheckedChanged += new System.EventHandler(this.cbx_CheckedChanged);
            // 
            // btnBrowse3
            // 
            this.btnBrowse3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse3.Location = new System.Drawing.Point(594, 101);
            this.btnBrowse3.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse3.Name = "btnBrowse3";
            this.btnBrowse3.Size = new System.Drawing.Size(70, 30);
            this.btnBrowse3.TabIndex = 11;
            this.btnBrowse3.Text = "Browse...";
            this.btnBrowse3.UseVisualStyleBackColor = true;
            this.btnBrowse3.Click += new System.EventHandler(this.btnBrowse3_Click);
            // 
            // txtLogPath
            // 
            this.txtLogPath.Location = new System.Drawing.Point(9, 104);
            this.txtLogPath.Name = "txtLogPath";
            this.txtLogPath.Size = new System.Drawing.Size(580, 25);
            this.txtLogPath.TabIndex = 10;
            this.txtLogPath.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // cbxLogging
            // 
            this.cbxLogging.AutoSize = true;
            this.cbxLogging.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxLogging.Location = new System.Drawing.Point(9, 76);
            this.cbxLogging.Margin = new System.Windows.Forms.Padding(2);
            this.cbxLogging.Name = "cbxLogging";
            this.cbxLogging.Size = new System.Drawing.Size(207, 24);
            this.cbxLogging.TabIndex = 9;
            this.cbxLogging.Text = "Enable 86Box logging to file:";
            this.cbxLogging.UseVisualStyleBackColor = true;
            this.cbxLogging.CheckedChanged += new System.EventHandler(this.cbxLogging_CheckedChanged);
            // 
            // dlgSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(709, 336);
            this.Controls.Add(this.tbcSettings);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dlgSettings_FormClosing);
            this.Load += new System.EventHandler(this.dlgSettings_Load);
            this.pnlBottom.ResumeLayout(false);
            this.tbcSettings.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.gbxBehaviour.ResumeLayout(false);
            this.gbxBehaviour.PerformLayout();
            this.gbxPaths.ResumeLayout(false);
            this.gbxPaths.PerformLayout();
            this.tabAdvanced.ResumeLayout(false);
            this.gbxMisc.ResumeLayout(false);
            this.gbxMisc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox cbxShowConsole;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblLaunchTimeout;
        private System.Windows.Forms.TextBox txtLaunchTimeout;
        private System.Windows.Forms.TabControl tbcSettings;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.GroupBox gbxBehaviour;
        private System.Windows.Forms.CheckBox cbxMinimizeTray;
        private System.Windows.Forms.CheckBox cbxCloseTray;
        private System.Windows.Forms.CheckBox cbxMinimize;
        private System.Windows.Forms.GroupBox gbxPaths;
        private System.Windows.Forms.Label lblCFGdir;
        private System.Windows.Forms.TextBox txtCFGdir;
        private System.Windows.Forms.TextBox txtEXEdir;
        private System.Windows.Forms.Button btnBrowse2;
        private System.Windows.Forms.Label lblEXEdir;
        private System.Windows.Forms.Button btnBrowse1;
        private System.Windows.Forms.TabPage tabAdvanced;
        private System.Windows.Forms.Label lbl86BoxVer1;
        private System.Windows.Forms.Label lbl86BoxVer;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox gbxMisc;
        private System.Windows.Forms.CheckBox cbxLogging;
        private System.Windows.Forms.Button btnBrowse3;
        private System.Windows.Forms.TextBox txtLogPath;
        private System.Windows.Forms.CheckBox cbxGrid;
    }
}