using System;
using System.IO;
using System.Windows.Forms;

namespace _86boxManager
{
    public partial class dlgEditVM : Form
    {
        private frmMain main = (frmMain)Application.OpenForms["frmMain"]; //Instance of frmMain
        private VM vm = null; //VM to be edited
        private string originalName; //Original name of the VM

        public dlgEditVM()
        {
            InitializeComponent();
        }

        private void dlgEditVM_Load(object sender, EventArgs e)
        {
            VMLoadData();
        }

        //Load the data for selected VM
        private void VMLoadData()
        {
            vm = (VM)main.lstVMs.FocusedItem.Tag;
            originalName = vm.Name;
            txtName.Text = vm.Name;
            txtDesc.Text = vm.Desc;
            lblPath1.Text = vm.Path;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //Check if a VM with this name already exists
            if (!originalName.Equals(txtName.Text) && main.VMCheckIfExists(txtName.Text))
            {
                MessageBox.Show("A virtual machine with this name already exists. Please pick a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                MessageBox.Show("There are invalid characters in the name you specified. You can't use the following characters: \\ / : * ? \" < > |", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                main.VMEdit(txtName.Text, txtDesc.Text);
                Close();
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            //Check for empty strings etc.
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnApply.Enabled = false;
            }
            else
            {
                btnApply.Enabled = true;
                lblPath1.Text = main.cfgpath + txtName.Text;
                tipLblPath1.SetToolTip(lblPath1, main.cfgpath + txtName.Text);
            }
        }
    }
}
