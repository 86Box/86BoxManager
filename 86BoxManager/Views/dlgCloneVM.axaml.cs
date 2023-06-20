using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using _86BoxManager.Core;
using _86BoxManager.Tools;
using IOPath = System.IO.Path;
using ButtonsType = MessageBox.Avalonia.Enums.ButtonEnum;
using MessageType = MessageBox.Avalonia.Enums.Icon;
using ResponseType = MessageBox.Avalonia.Enums.ButtonResult;

namespace _86BoxManager.Views
{
    public partial class dlgCloneVM : Window
    {
        // Path of the VM to be cloned
        private readonly string _oldPath;

        public dlgCloneVM()
        {
            InitializeComponent();
            txtName.OnTextChanged(txtName_TextChanged);
        }

        public dlgCloneVM(string oldPath) : this()
        {
            _oldPath = oldPath;
        }

        private void dlgCloneVM_Load(object sender, EventArgs e)
        {
            var cfgPath = Program.Root.CfgPath;
            lblPath1.Text = cfgPath;
            lblOldVM.Text = $@"Virtual machine ""{IOPath.GetFileName(_oldPath)}"" will be cloned into:";
        }

        private void txtName_TextChanged(object sender, TextInputEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnClone.IsEnabled = false;
                txtName.UnsetToolTip();
                return;
            }

            if (txtName.Text.IndexOfAny(IOPath.GetInvalidFileNameChars()) >= 0)
            {
                btnClone.IsEnabled = false;
                lblPath1.Text = "Invalid path";
                txtName.SetToolTip("You cannot use the following characters" +
                                   " in the name: \\ / : * ? \" < > |");
                return;
            }

            var cfgPath = Program.Root.CfgPath;
            btnClone.IsEnabled = true;
            lblPath1.Text = cfgPath + txtName.Text;
            lblPath1.SetToolTip(cfgPath + txtName.Text);
        }

        private void btnClone_Click(object sender, RoutedEventArgs e)
        {
            if (VMCenter.CheckIfExists(txtName.Text))
            {
                Dialogs.ShowMessageBox("A virtual machine with this name already exists. Please pick a different name.",
                    MessageType.Error, ButtonsType.Ok, "Error");
                return;
            }

            if (txtName.Text.IndexOfAny(IOPath.GetInvalidFileNameChars()) >= 0)
            {
                Dialogs.ShowMessageBox("There are invalid characters in the name you specified. " +
                                       "You can't use the following characters: \\ / : * ? \" < > |",
                    MessageType.Error, ButtonsType.Ok, "Error");
                return;
            }

            // Just import stuff from the existing VM
            VMCenter.Import(txtName.Text, txtDescription.Text, _oldPath, cbxOpenCFG.IsActive(),
                cbxStartVM.IsActive(), Program.Root);

            Close(ResponseType.Ok);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close(ResponseType.Cancel);
        }
    }
}