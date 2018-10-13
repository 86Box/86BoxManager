using System;
using System.Windows.Forms;

namespace _86boxManager
{
    public partial class dlgAddVM : Form
    {
        private frmMain main = (frmMain)Application.OpenForms["frmMain"]; //Instance of frmMain

        public dlgAddVM()
        {
            InitializeComponent();
        }

        //Check if VM with this name already exists, and send the data to the main form for processing if it doesn't
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (main.VMCheckIfExists(txtName.Text))
            {
                MessageBox.Show("A virtual machine with this name already exists. Please pick a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                main.VMAdd(txtName.Text, txtDescription.Text, cbxOpenCFG.Checked);
                Close();
            }
        }

        private void dlgAddVM_Load(object sender, EventArgs e)
        {
            lblPath1.Text = main.cfgpath;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnAdd.Enabled = false;
            }
            else
            {
                btnAdd.Enabled = true;
                lblPath1.Text = main.cfgpath + txtName.Text;
            }
        }
    }
}
