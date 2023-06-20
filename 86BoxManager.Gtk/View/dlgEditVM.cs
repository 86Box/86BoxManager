using System;
using _86BoxManager.Core;
using _86BoxManager.Models;
using _86BoxManager.Tools;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;

namespace _86BoxManager.View
{
    internal sealed partial class dlgEditVM : Dialog
    {
        public dlgEditVM() : this(new Builder("dlgEditVM.glade"))
        {
            InitializeComponent();
        }

        private dlgEditVM(Builder builder) : base(builder.GetRawOwnedObject("dlgEditVM"))
        {
            builder.Autoconnect(this);
            DefaultResponse = ResponseType.Cancel;

            Response += Dialog_Response;
        }

        private void Dialog_Response(object o, ResponseArgs args)
        {
            Hide();
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

        private void btnApply_Click(object sender, EventArgs e)
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

            Respond(ResponseType.Close);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            // Check for empty strings etc.
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnApply.Sensitive = false;
                return;
            }

            var cfgPath = Program.Root.CfgPath;
            btnApply.Sensitive = true;
            lblPath1.Text = cfgPath + txtName.Text;
            lblPath1.SetToolTip(cfgPath + txtName.Text);
        }
    }
}