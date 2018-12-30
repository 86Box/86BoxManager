namespace _86boxManager
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
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxOpenCFG
            // 
            this.cbxOpenCFG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxOpenCFG.AutoSize = true;
            this.cbxOpenCFG.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxOpenCFG.Location = new System.Drawing.Point(263, 123);
            this.cbxOpenCFG.Name = "cbxOpenCFG";
            this.cbxOpenCFG.Size = new System.Drawing.Size(278, 26);
            this.cbxOpenCFG.TabIndex = 2;
            this.cbxOpenCFG.Text = "Configure this virtual machine now";
            this.cbxOpenCFG.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(537, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Enabled = false;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Location = new System.Drawing.Point(456, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 30);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(55, 21);
            this.lblName.TabIndex = 8;
            this.lblName.Text = "Name:";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(12, 50);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(92, 21);
            this.lblDesc.TabIndex = 9;
            this.lblDesc.Text = "Description:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(73, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(539, 29);
            this.txtName.TabIndex = 10;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(110, 47);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(502, 29);
            this.txtDescription.TabIndex = 11;
            // 
            // lblPath1
            // 
            this.lblPath1.AutoEllipsis = true;
            this.lblPath1.Location = new System.Drawing.Point(61, 88);
            this.lblPath1.MaximumSize = new System.Drawing.Size(551, 21);
            this.lblPath1.Name = "lblPath1";
            this.lblPath1.Size = new System.Drawing.Size(551, 21);
            this.lblPath1.TabIndex = 12;
            this.lblPath1.Text = "<path goes here>";
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(12, 88);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(43, 21);
            this.lblPath.TabIndex = 13;
            this.lblPath.Text = "Path:";
            // 
            // cbxStartVM
            // 
            this.cbxStartVM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxStartVM.AutoSize = true;
            this.cbxStartVM.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxStartVM.Location = new System.Drawing.Point(16, 123);
            this.cbxStartVM.Name = "cbxStartVM";
            this.cbxStartVM.Size = new System.Drawing.Size(241, 26);
            this.cbxStartVM.TabIndex = 14;
            this.cbxStartVM.Text = "Start this virtual machine now";
            this.cbxStartVM.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Controls.Add(this.btnAdd);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 164);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(624, 54);
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
            // dlgAddVM
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(624, 218);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.cbxStartVM);
            this.Controls.Add(this.cbxOpenCFG);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.lblPath1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.lblName);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
    }
}