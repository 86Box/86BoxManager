using System;
using _86BoxManager.Core;
using _86BoxManager.Tools;
using _86BoxManager.Xplat;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;

namespace _86BoxManager.View
{
    internal sealed partial class dlgAddVM : Dialog
    {
        public dlgAddVM() : this(new Builder("dlgAddVM.glade"))
        {
            InitializeComponent();
        }

        private dlgAddVM(Builder builder) : base(builder.GetRawOwnedObject("dlgAddVM"))
        {
            builder.Autoconnect(this);
            DefaultResponse = ResponseType.Cancel;

            Response += Dialog_Response;
        }

        private void Dialog_Response(object o, ResponseArgs args)
        {
            Hide();
        }

        private bool existingVM = false; // Is this importing an existing VM or not

        private void dlgAddVM_Load(object sender, EventArgs e)
        {
            lblPath1.Text = Program.Root.CfgPath;

            // Disable on start
            cbxImport_CheckedChanged(sender, e);
            txtName_TextChanged(sender, e);
        }

        private void cbxImport_CheckedChanged(object sender, EventArgs e)
        {
            var status = cbxImport.Active;
            existingVM = status;
            txtImportPath.IsEditable = txtImportPath.CanFocus = status;
            btnBrowse.Sensitive = btnBrowse.CanFocus = status;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var initDir = Platforms.Env.MyComputer;
            var text = "Select a folder where your virtual machine (configs, nvr folders, etc.) will be located";

            var fileName = Dialogs.SelectFolder(text, initDir, this);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                txtImportPath.Text = fileName;
                txtName.Text = IOPath.GetFileName(fileName);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnAdd.Sensitive = false;
                txtName.UnsetToolTip();
                return;
            }

            if (txtName.Text.IndexOfAny(IOPath.GetInvalidFileNameChars()) >= 0)
            {
                btnAdd.Sensitive = false;
                lblPath1.Text = "Invalid path";
                txtName.SetToolTip("You cannot use the following characters" +
                                   " in the name: \\ / : * ? \" < > |");
                return;
            }

            var cfgPath = Program.Root.CfgPath;
            btnAdd.Sensitive = true;
            lblPath1.Text = cfgPath + txtName.Text;
            lblPath1.SetToolTip(cfgPath + txtName.Text);
        }

        // Check if VM with this name already exists, and send the data to the main form for processing if it doesn't
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (VMCenter.CheckIfExists(txtName.Text))
            {
                Dialogs.ShowMessageBox("A virtual machine with this name already exists. Please pick a different name.",
                    MessageType.Error, ButtonsType.Ok, "Error");
                return;
            }

            if (existingVM && string.IsNullOrWhiteSpace(txtImportPath.Text))
            {
                Dialogs.ShowMessageBox("If you wish to import VM files, you must specify a path.",
                    MessageType.Error, ButtonsType.Ok, "Error");
                return;
            }

            if (existingVM)
            {
                VMCenter.Import(txtName.Text, txtDescription.Text, txtImportPath.Text,
                    cbxOpenCFG.Active, cbxStartVM.Active, Program.Root);
            }
            else
            {
                VMCenter.Add(txtName.Text, txtDescription.Text, cbxOpenCFG.Active, cbxStartVM.Active);
            }

            Respond(ResponseType.Close);
        }
    }
}