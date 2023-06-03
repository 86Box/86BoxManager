using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using _86boxManager.Tools;
using _86boxManager.Xplat;
using _86boxManager.Core;
using IOPath = System.IO.Path;
using ButtonsType = MessageBox.Avalonia.Enums.ButtonEnum;
using MessageType = MessageBox.Avalonia.Enums.Icon;
using ResponseType = MessageBox.Avalonia.Enums.ButtonResult;

namespace _86boxManager.Views
{
    public partial class dlgAddVM : Window
    {
        public dlgAddVM()
        {
            InitializeComponent();
        }

        private bool existingVM = false; // Is this importing an existing VM or not

        private void dlgAddVM_Load(object sender, EventArgs e)
        {
            lblPath1.Text = Program.Root.CfgPath;

            // Disable on start
            cbxImport_CheckedChanged(sender, null);
            txtName_TextChanged(sender, null);
        }

        private void cbxImport_CheckedChanged(object sender, RoutedEventArgs e)
        {
            var status = cbxImport.IsActive();
            existingVM = status;
            txtImportPath.Focusable = txtImportPath.IsEditable(status);
            btnBrowse.IsEnabled = btnBrowse.Focusable = status;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
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

        private void txtName_TextChanged(object sender, TextInputEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnAdd.IsEnabled = false;
                txtName.UnsetToolTip();
                return;
            }

            if (txtName.Text.IndexOfAny(IOPath.GetInvalidFileNameChars()) >= 0)
            {
                btnAdd.IsEnabled = false;
                lblPath1.Text = "Invalid path";
                txtName.SetToolTip("You cannot use the following characters" +
                                   " in the name: \\ / : * ? \" < > |");
                return;
            }

            var cfgPath = Program.Root.CfgPath;
            btnAdd.IsEnabled = true;
            lblPath1.Text = cfgPath + txtName.Text;
            lblPath1.SetToolTip(cfgPath + txtName.Text);
        }

        // Check if VM with this name already exists, and send the data to the main form for processing if it doesn't
        private void btnAdd_Click(object sender, RoutedEventArgs e)
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
                    cbxOpenCFG.IsActive(), cbxStartVM.IsActive(), Program.Root);
            }
            else
            {
                VMCenter.Add(txtName.Text, txtDescription.Text, cbxOpenCFG.IsActive(), cbxStartVM.IsActive());
            }

            Close(ResponseType.Ok);
        }

        private void btnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close(ResponseType.Cancel);
        }
    }
}