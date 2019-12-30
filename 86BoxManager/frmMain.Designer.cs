namespace _86boxManager
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.lstVMs = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmsVM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetCTRLALTDELETEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hardResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.killToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wipeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createADesktopShortcutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.img86box = new System.Windows.Forms.ImageList(this.components);
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnConfigure = new System.Windows.Forms.Button();
            this.imgStatus = new System.Windows.Forms.ImageList(this.components);
            this.btnPause = new System.Windows.Forms.Button();
            this.btnCtrlAltDel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsTrayIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.open86BoxManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblVMCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmsVM.SuspendLayout();
            this.cmsTrayIcon.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnEdit.Location = new System.Drawing.Point(58, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(45, 30);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.toolTip.SetToolTip(this.btnEdit, "Edit the properties of this virtual machine");
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDelete.Location = new System.Drawing.Point(109, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Remove";
            this.toolTip.SetToolTip(this.btnDelete, "Remove this virtual machine");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnStart.Location = new System.Drawing.Point(197, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(45, 30);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.toolTip.SetToolTip(this.btnStart, "Start this virtual machine");
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSettings.Location = new System.Drawing.Point(607, 12);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(65, 30);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "Settings";
            this.toolTip.SetToolTip(this.btnSettings, "Open 86Box Manager settings");
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // lstVMs
            // 
            this.lstVMs.AllowColumnReorder = true;
            this.lstVMs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstVMs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmStatus,
            this.clmPath});
            this.lstVMs.ContextMenuStrip = this.cmsVM;
            this.lstVMs.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstVMs.FullRowSelect = true;
            this.lstVMs.HideSelection = false;
            this.lstVMs.Location = new System.Drawing.Point(12, 52);
            this.lstVMs.Name = "lstVMs";
            this.lstVMs.ShowGroups = false;
            this.lstVMs.ShowItemToolTips = true;
            this.lstVMs.Size = new System.Drawing.Size(660, 407);
            this.lstVMs.SmallImageList = this.img86box;
            this.lstVMs.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstVMs.TabIndex = 10;
            this.lstVMs.UseCompatibleStateImageBehavior = false;
            this.lstVMs.View = System.Windows.Forms.View.Details;
            this.lstVMs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstVMs_ColumnClick);
            this.lstVMs.SelectedIndexChanged += new System.EventHandler(this.lstVMs_SelectedIndexChanged);
            this.lstVMs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstVMs_KeyDown);
            this.lstVMs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstVMs_MouseDoubleClick);
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 184;
            // 
            // clmStatus
            // 
            this.clmStatus.Text = "Status";
            this.clmStatus.Width = 107;
            // 
            // clmPath
            // 
            this.clmPath.Text = "Path";
            this.clmPath.Width = 359;
            // 
            // cmsVM
            // 
            this.cmsVM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.configureToolStripMenuItem,
            this.pauseToolStripMenuItem,
            this.resetCTRLALTDELETEToolStripMenuItem,
            this.hardResetToolStripMenuItem,
            this.toolStripSeparator3,
            this.killToolStripMenuItem,
            this.wipeToolStripMenuItem,
            this.toolStripSeparator1,
            this.editToolStripMenuItem,
            this.cloneToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.openFolderToolStripMenuItem,
            this.createADesktopShortcutToolStripMenuItem});
            this.cmsVM.Name = "cmsVM";
            this.cmsVM.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmsVM.Size = new System.Drawing.Size(210, 280);
            this.cmsVM.Opening += new System.ComponentModel.CancelEventHandler(this.cmsVM_Opening);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.ToolTipText = "Start this virtual machine";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.configureToolStripMenuItem.Text = "Configure";
            this.configureToolStripMenuItem.ToolTipText = "Open settings for this virtual machine";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            this.pauseToolStripMenuItem.ToolTipText = "Pause this virtual machine";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // resetCTRLALTDELETEToolStripMenuItem
            // 
            this.resetCTRLALTDELETEToolStripMenuItem.Name = "resetCTRLALTDELETEToolStripMenuItem";
            this.resetCTRLALTDELETEToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.resetCTRLALTDELETEToolStripMenuItem.Text = "Send CTRL+ALT+DEL";
            this.resetCTRLALTDELETEToolStripMenuItem.ToolTipText = "Send the CTRL+ALT+DEL keystroke to this virtual machine";
            this.resetCTRLALTDELETEToolStripMenuItem.Click += new System.EventHandler(this.resetCTRLALTDELETEToolStripMenuItem_Click);
            // 
            // hardResetToolStripMenuItem
            // 
            this.hardResetToolStripMenuItem.Name = "hardResetToolStripMenuItem";
            this.hardResetToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.hardResetToolStripMenuItem.Text = "Hard reset";
            this.hardResetToolStripMenuItem.ToolTipText = "Reset this virtual machine by simulating a power cycle";
            this.hardResetToolStripMenuItem.Click += new System.EventHandler(this.hardResetToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(206, 6);
            // 
            // killToolStripMenuItem
            // 
            this.killToolStripMenuItem.Name = "killToolStripMenuItem";
            this.killToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.killToolStripMenuItem.Text = "Kill";
            this.killToolStripMenuItem.ToolTipText = "Kill this virtual machine";
            this.killToolStripMenuItem.Click += new System.EventHandler(this.killToolStripMenuItem_Click);
            // 
            // wipeToolStripMenuItem
            // 
            this.wipeToolStripMenuItem.Name = "wipeToolStripMenuItem";
            this.wipeToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.wipeToolStripMenuItem.Text = "Wipe";
            this.wipeToolStripMenuItem.ToolTipText = "Delete configuration and nvr for this virtual machine";
            this.wipeToolStripMenuItem.Click += new System.EventHandler(this.wipeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(206, 6);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.ToolTipText = "Edit the properties of this virtual machine";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // cloneToolStripMenuItem
            // 
            this.cloneToolStripMenuItem.Name = "cloneToolStripMenuItem";
            this.cloneToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.cloneToolStripMenuItem.Text = "Clone";
            this.cloneToolStripMenuItem.ToolTipText = "Clone this virtual machine";
            this.cloneToolStripMenuItem.Click += new System.EventHandler(this.cloneToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.deleteToolStripMenuItem.Text = "Remove";
            this.deleteToolStripMenuItem.ToolTipText = "Remove this virtual machine";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.openFolderToolStripMenuItem.Text = "Open folder in Explorer";
            this.openFolderToolStripMenuItem.ToolTipText = "Open the folder for this virtual machine in Windows Explorer";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // createADesktopShortcutToolStripMenuItem
            // 
            this.createADesktopShortcutToolStripMenuItem.Name = "createADesktopShortcutToolStripMenuItem";
            this.createADesktopShortcutToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.createADesktopShortcutToolStripMenuItem.Text = "Create a desktop shortcut";
            this.createADesktopShortcutToolStripMenuItem.ToolTipText = "Create a shortcut for this virtual machine on the desktop";
            this.createADesktopShortcutToolStripMenuItem.Click += new System.EventHandler(this.createADesktopShortcutToolStripMenuItem_Click);
            // 
            // img86box
            // 
            this.img86box.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img86box.ImageStream")));
            this.img86box.TransparentColor = System.Drawing.Color.Transparent;
            this.img86box.Images.SetKeyName(0, "86box_16x16.png");
            this.img86box.Images.SetKeyName(1, "86box_16x16_green.png");
            this.img86box.Images.SetKeyName(2, "86box_16x16_yellow.png");
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAdd.Location = new System.Drawing.Point(12, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(40, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.toolTip.SetToolTip(this.btnAdd, "Add a new virtual machine");
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnConfigure
            // 
            this.btnConfigure.Enabled = false;
            this.btnConfigure.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnConfigure.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnConfigure.Location = new System.Drawing.Point(248, 12);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new System.Drawing.Size(70, 30);
            this.btnConfigure.TabIndex = 4;
            this.btnConfigure.Text = "Configure";
            this.toolTip.SetToolTip(this.btnConfigure, "Open settings for this virtual machine");
            this.btnConfigure.UseVisualStyleBackColor = true;
            this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
            // 
            // imgStatus
            // 
            this.imgStatus.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgStatus.ImageSize = new System.Drawing.Size(16, 16);
            this.imgStatus.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btnPause
            // 
            this.btnPause.Enabled = false;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPause.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnPause.Location = new System.Drawing.Point(324, 12);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(55, 30);
            this.btnPause.TabIndex = 5;
            this.btnPause.Text = "Pause";
            this.toolTip.SetToolTip(this.btnPause, "Pause this virtual machine");
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnCtrlAltDel
            // 
            this.btnCtrlAltDel.Enabled = false;
            this.btnCtrlAltDel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCtrlAltDel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCtrlAltDel.Location = new System.Drawing.Point(385, 12);
            this.btnCtrlAltDel.Name = "btnCtrlAltDel";
            this.btnCtrlAltDel.Size = new System.Drawing.Size(60, 30);
            this.btnCtrlAltDel.TabIndex = 6;
            this.btnCtrlAltDel.Text = "C+A+D";
            this.toolTip.SetToolTip(this.btnCtrlAltDel, "Send the CTRL+ALT+DEL keystroke to this virtual machine");
            this.btnCtrlAltDel.UseVisualStyleBackColor = true;
            this.btnCtrlAltDel.Click += new System.EventHandler(this.btnCtrlAltDel_Click);
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnReset.Location = new System.Drawing.Point(451, 12);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(50, 30);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Reset";
            this.toolTip.SetToolTip(this.btnReset, "Reset this virtual machine by simulating a power cycle");
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.cmsTrayIcon;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "86Box Manager";
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // cmsTrayIcon
            // 
            this.cmsTrayIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open86BoxManagerToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.cmsTrayIcon.Name = "cmsVM";
            this.cmsTrayIcon.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmsTrayIcon.Size = new System.Drawing.Size(189, 76);
            // 
            // open86BoxManagerToolStripMenuItem
            // 
            this.open86BoxManagerToolStripMenuItem.Name = "open86BoxManagerToolStripMenuItem";
            this.open86BoxManagerToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.open86BoxManagerToolStripMenuItem.Text = "Show 86Box Manager";
            this.open86BoxManagerToolStripMenuItem.ToolTipText = "Restore the 86Box Manager window";
            this.open86BoxManagerToolStripMenuItem.Click += new System.EventHandler(this.open86BoxManagerToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.ToolTipText = "Open 86Box Manager settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.ToolTipText = "Close 86Box Manager";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblVMCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 473);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(684, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblVMCount
            // 
            this.lblVMCount.BackColor = System.Drawing.Color.Transparent;
            this.lblVMCount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblVMCount.Name = "lblVMCount";
            this.lblVMCount.Size = new System.Drawing.Size(121, 17);
            this.lblVMCount.Text = "# of virtual machines:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 495);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnCtrlAltDel);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnConfigure);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lstVMs);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "86Box Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.cmsVM.ResumeLayout(false);
            this.cmsTrayIcon.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmStatus;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnConfigure;
        private System.Windows.Forms.ColumnHeader clmPath;
        private System.Windows.Forms.ContextMenuStrip cmsVM;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetCTRLALTDELETEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hardResetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ImageList img86box;
        private System.Windows.Forms.ImageList imgStatus;
        public System.Windows.Forms.ListView lstVMs;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnCtrlAltDel;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createADesktopShortcutToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip cmsTrayIcon;
        private System.Windows.Forms.ToolStripMenuItem open86BoxManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem killToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem wipeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cloneToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblVMCount;
    }
}

