using System;
using _86BoxManager.Core;
using _86BoxManager.Tools;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;

namespace _86BoxManager.View
{
    internal sealed partial class dlgCloneVM : Dialog
    {
        // Path of the VM to be cloned
        private readonly string _oldPath;

        public dlgCloneVM(string oldPath) : this(new Builder("dlgCloneVM.glade"))
        {
            InitializeComponent();
            _oldPath = oldPath;
        }

        private dlgCloneVM(Builder builder) : base(builder.GetRawOwnedObject("dlgCloneVM"))
        {
            builder.Autoconnect(this);
            DefaultResponse = ResponseType.Cancel;

            Response += Dialog_Response;
        }

        private void Dialog_Response(object o, ResponseArgs args)
        {
            Hide();
        }

        private void dlgCloneVM_Load(object sender, EventArgs e)
        {
            var cfgPath = Program.Root.CfgPath;
            lblPath1.Text = cfgPath;
            lblOldVM.Text = $@"Virtual machine ""{IOPath.GetFileName(_oldPath)}"" will be cloned into:";
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnClone.Sensitive = false;
                txtName.UnsetToolTip();
                return;
            }

            if (txtName.Text.IndexOfAny(IOPath.GetInvalidFileNameChars()) >= 0)
            {
                btnClone.Sensitive = false;
                lblPath1.Text = "Invalid path";
                txtName.SetToolTip("You cannot use the following characters" +
                                   " in the name: \\ / : * ? \" < > |");
                return;
            }

            var cfgPath = Program.Root.CfgPath;
            btnClone.Sensitive = true;
            lblPath1.Text = cfgPath + txtName.Text;
            lblPath1.SetToolTip(cfgPath + txtName.Text);
        }

        private void btnClone_Click(object sender, EventArgs e)
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
            VMCenter.Import(txtName.Text, txtDescription.Text, _oldPath, cbxOpenCFG.Active, 
                cbxStartVM.Active, Program.Root);

            Respond(ResponseType.Close);
        }
    }
}