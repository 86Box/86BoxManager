using System;
using System.IO;
using System.Windows.Forms;

namespace _86boxManager
{
    public partial class dlgCloneVM : Form
    {
        private string oldPath; //Path of the VM to be cloned
        private frmMain main = (frmMain)Application.OpenForms["frmMain"]; //Instance of frmMain

        public dlgCloneVM()
        {
            InitializeComponent();
        }

        public dlgCloneVM(string oldPath)
        {
            InitializeComponent();
            this.oldPath = oldPath;
        }

        private void dlgCloneVM_Load(object sender, EventArgs e)
        {
            lblPath1.Text = main.cfgpath;
            lblOldVM.Text = "Virtual machine \"" + Path.GetFileName(oldPath) + "\" will be cloned into:";
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnClone.Enabled = false;
                tipTxtName.Active = false;
            }
            else
            {
                if (txtName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    btnClone.Enabled = false;
                    lblPath1.Text = "Invalid path";
                    tipTxtName.Active = true;
                    tipTxtName.Show("You cannot use the following characters in the name: \\ / : * ? \" < > |", txtName, 20000);
                }
                else
                {
                    btnClone.Enabled = true;
                    lblPath1.Text = main.cfgpath + txtName.Text;
                    tipLblPath1.SetToolTip(lblPath1, main.cfgpath + txtName.Text);
                }
            }
        }

        private void btnClone_Click(object sender, EventArgs e)
        {
            if (main.VMCheckIfExists(txtName.Text))
            {
                MessageBox.Show("A virtual machine with this name already exists. Please pick a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                MessageBox.Show("There are invalid characters in the name you specified. You can't use the following characters: \\ / : * ? \" < > |", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Just import stuff from the existing VM
            main.VMImport(txtName.Text, txtDescription.Text, oldPath, cbxOpenCFG.Checked, cbxStartVM.Checked);
            Close();
        }
    }
}
