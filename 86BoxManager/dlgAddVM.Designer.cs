﻿namespace _86boxManager
{
    partial class dlgAddVM
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
            this.cbxOpenCFG = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblPath1 = new System.Windows.Forms.Label();
            this.lblPath = new System.Windows.Forms.Label();
            this.tipLblPath1 = new System.Windows.Forms.ToolTip(this.components);
            this.cbxStartVM = new System.Windows.Forms.CheckBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.tipTxtName = new System.Windows.Forms.ToolTip(this.components);
            this.txtImportPath = new System.Windows.Forms.TextBox();
            this.cbxImport = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxOpenCFG
            // 
            this.cbxOpenCFG.AutoSize = true;
            this.cbxOpenCFG.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxOpenCFG.Location = new System.Drawing.Point(235, 177);
            this.cbxOpenCFG.Margin = new System.Windows.Forms.Padding(2);
            this.cbxOpenCFG.Name = "cbxOpenCFG";
            this.cbxOpenCFG.Size = new System.Drawing.Size(247, 24);
            this.cbxOpenCFG.TabIndex = 6;
            this.cbxOpenCFG.Text = "Configure this virtual machine now";
            this.cbxOpenCFG.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(508, 11);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 30);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAdd.Enabled = false;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Location = new System.Drawing.Point(439, 11);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(65, 30);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(11, 79);
            this.lblName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(48, 19);
            this.lblName.TabIndex = 8;
            this.lblName.Text = "Name:";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(11, 113);
            this.lblDesc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(81, 19);
            this.lblDesc.TabIndex = 9;
            this.lblDesc.Text = "Description:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(96, 76);
            this.txtName.Margin = new System.Windows.Forms.Padding(2);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(477, 25);
            this.txtName.TabIndex = 3;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(96, 110);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(477, 25);
            this.txtDescription.TabIndex = 4;
            // 
            // lblPath1
            // 
            this.lblPath1.AutoEllipsis = true;
            this.lblPath1.Location = new System.Drawing.Point(55, 146);
            this.lblPath1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPath1.MaximumSize = new System.Drawing.Size(429, 17);
            this.lblPath1.Name = "lblPath1";
            this.lblPath1.Size = new System.Drawing.Size(429, 17);
            this.lblPath1.TabIndex = 12;
            this.lblPath1.Text = "<path goes here>";
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(11, 146);
            this.lblPath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(40, 19);
            this.lblPath.TabIndex = 13;
            this.lblPath.Text = "Path:";
            // 
            // cbxStartVM
            // 
            this.cbxStartVM.AutoSize = true;
            this.cbxStartVM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxStartVM.Location = new System.Drawing.Point(15, 177);
            this.cbxStartVM.Margin = new System.Windows.Forms.Padding(2);
            this.cbxStartVM.Name = "cbxStartVM";
            this.cbxStartVM.Size = new System.Drawing.Size(216, 24);
            this.cbxStartVM.TabIndex = 5;
            this.cbxStartVM.Text = "Start this virtual machine now";
            this.cbxStartVM.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnAdd);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 211);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(2);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(584, 52);
            this.pnlBottom.TabIndex = 15;
            // 
            // tipTxtName
            // 
            this.tipTxtName.Active = false;
            this.tipTxtName.AutomaticDelay = 0;
            this.tipTxtName.IsBalloon = true;
            this.tipTxtName.ShowAlways = true;
            this.tipTxtName.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            this.tipTxtName.ToolTipTitle = "Name contains invalid characters";
            // 
            // txtImportPath
            // 
            this.txtImportPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImportPath.Enabled = false;
            this.txtImportPath.Location = new System.Drawing.Point(15, 40);
            this.txtImportPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtImportPath.Name = "txtImportPath";
            this.txtImportPath.Size = new System.Drawing.Size(484, 25);
            this.txtImportPath.TabIndex = 1;
            // 
            // cbxImport
            // 
            this.cbxImport.AutoSize = true;
            this.cbxImport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxImport.Location = new System.Drawing.Point(15, 12);
            this.cbxImport.Name = "cbxImport";
            this.cbxImport.Size = new System.Drawing.Size(165, 24);
            this.cbxImport.TabIndex = 0;
            this.cbxImport.Text = "Import VM files from:";
            this.cbxImport.UseVisualStyleBackColor = true;
            this.cbxImport.CheckedChanged += new System.EventHandler(this.cbxImport_CheckedChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Enabled = false;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowse.Location = new System.Drawing.Point(503, 37);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(70, 30);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // dlgAddVM
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 263);
            this.Controls.Add(this.txtImportPath);
            this.Controls.Add(this.cbxImport);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.cbxStartVM);
            this.Controls.Add(this.cbxOpenCFG);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.lblPath1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblName);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgAddVM";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add a virtual machine";
            this.Load += new System.EventHandler(this.dlgAddVM_Load);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox cbxOpenCFG;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblPath1;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.ToolTip tipLblPath1;
        private System.Windows.Forms.CheckBox cbxStartVM;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ToolTip tipTxtName;
        private System.Windows.Forms.TextBox txtImportPath;
        private System.Windows.Forms.CheckBox cbxImport;
        private System.Windows.Forms.Button btnBrowse;
    }
}