using System;
using System.IO;
using System.Windows.Forms;

namespace _86boxManager
{
    public partial class dlgAddVM : Form
    {
        private frmMain main = (frmMain)Application.OpenForms["frmMain"]; //Instance of frmMain
        private bool existingVM = false; //Is this importing an existing VM or not

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
                DialogResult = DialogResult.None;
                return;
            }
            if (existingVM && string.IsNullOrWhiteSpace(txtImportPath.Text))
            {
                MessageBox.Show("If you wish to import VM files, you must specify a path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            if (existingVM)
            {
                main.VMImport(txtName.Text, txtDescription.Text, txtImportPath.Text, cbxOpenCFG.Checked, cbxStartVM.Checked);
            }
            else
            {
                main.VMAdd(txtName.Text, txtDescription.Text, cbxOpenCFG.Checked, cbxStartVM.Checked);
            }
            Close();
        }

        private void dlgAddVM_Load(object sender, EventArgs e)
        {
            lblPath1.Text = main.cfgpath;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                btnAdd.Enabled = false;
                tipTxtName.Active = false;
            }
            else
            {
                if (txtName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    btnAdd.Enabled = false;
                    lblPath1.Text = "Invalid path";
                    tipTxtName.Active = true;
                    tipTxtName.Show("You cannot use the following characters in the name: \\ / : * ? \" < > |", txtName, 20000);
                }
                else
                {
                    btnAdd.Enabled = true;
                    lblPath1.Text = main.cfgpath + txtName.Text;
                    tipLblPath1.SetToolTip(lblPath1, main.cfgpath + txtName.Text);
                }
            }
        }

// .NET Core implements the better Vista-style folder browse dialog in the stock FolderBrowserDialog
#if NETCOREAPP
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer,
                Description = "Select a folder where your virtual machines (configs, nvr folders, etc.) will be located",
                UseDescriptionForTitle = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtImportPath.Text = dialog.SelectedPath;
                txtName.Text = Path.GetFileName(dialog.SelectedPath);
            }
        }
// A custom class is required for Vista-style folder dialogs under the original .NET Framework
#else
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderSelectDialog dialog = new FolderSelectDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Title = "Select a folder where your virtual machines (configs, nvr folders, etc.) will be located"
            };

            if (dialog.Show(Handle))
            {
                txtImportPath.Text = dialog.FileName;
                txtName.Text = Path.GetFileName(dialog.FileName);
            }
        }
#endif

        private void cbxImport_CheckedChanged(object sender, EventArgs e)
        {
            existingVM = !existingVM;
            txtImportPath.Enabled = cbxImport.Checked;
            btnBrowse.Enabled = cbxImport.Checked;
        }
    }
}