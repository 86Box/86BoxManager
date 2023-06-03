using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using _86boxManager.Core;
using _86boxManager.Models;
using _86boxManager.Tools;
using IOPath = System.IO.Path;
using ButtonsType = MessageBox.Avalonia.Enums.ButtonEnum;
using MessageType = MessageBox.Avalonia.Enums.Icon;
using ResponseType = MessageBox.Avalonia.Enums.ButtonResult;

namespace _86boxManager.Views
{
    public partial class dlgEditVM : Window
    {
        public dlgEditVM()
        {
            InitializeComponent();
        }

        private VM vm = null; //VM to be edited
        private string originalName; //Original name of the VM

        // Load the data for selected VM
        private void dlgEditVM_Load(object sender, EventArgs e)
        {
            vm = Program.Root.GetFocusedVm().Tag;
            originalName = vm.Name;
            txtName.Text = vm.Name;
            txtDesc.Text = vm.Desc;
            lblPath1.Text = vm.Path;
        }

        private void txtName_TextChanged(object sender, TextInputEventArgs e)
        {
            // Check for empty strings etc.
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnApply.IsEnabled = false;
                return;
            }

            var cfgPath = Program.Root.CfgPath;
            btnApply.IsEnabled = true;
            lblPath1.Text = cfgPath + txtName.Text;
            lblPath1.SetToolTip(cfgPath + txtName.Text);
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            // Check if a VM with this name already exists
            if (!originalName.Equals(txtName.Text) && VMCenter.CheckIfExists(txtName.Text))
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

            VMCenter.Edit(txtName.Text, txtDesc.Text);

            Close(ResponseType.Ok);
        }
    }
}