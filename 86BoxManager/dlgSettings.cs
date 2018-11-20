using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace _86boxManager
{
    public partial class dlgSettings : Form
    {
        private bool settingsChanged = false; //Keeps track of unsaved changes

        public dlgSettings()
        {
            InitializeComponent();
        }

        private void dlgSettings_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void dlgSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Unsaved changes, ask the user to confirm
            if (settingsChanged == true)
            {
                e.Cancel = true;
                DialogResult result = MessageBox.Show("Would you like to save the changes you've made to the settings?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SaveSettings();
                }
                if (result != DialogResult.Cancel)
                {
                    e.Cancel = false;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveSettings();
            btnApply.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCFGdir.Text) || string.IsNullOrWhiteSpace(txtEXEdir.Text))
            {
                btnApply.Enabled = false;
                btnOK.Enabled = false;
            }
            else
            {
                btnApply.Enabled = true;
                settingsChanged = true;
                btnOK.Enabled = true;
            }
        }

        //Save the settings to the registry
        private void SaveSettings()
        {
            try
            {
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true); //Try to open the key first (in read-write mode) to see if it already exists
                if (regkey == null) //Regkey doesn't exist yet, must be created first and then reopened
                {
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\86Box");
                    regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);
                }

                //Store the new values, close the key, changes are saved
                regkey.SetValue("EXEdir", txtEXEdir.Text, RegistryValueKind.String);
                regkey.SetValue("CFGdir", txtCFGdir.Text, RegistryValueKind.String);
                regkey.SetValue("MinimizeOnVMStart", cbxMinimize.Checked, RegistryValueKind.DWord);
                regkey.SetValue("ShowConsole", cbxShowConsole.Checked, RegistryValueKind.DWord);
                regkey.SetValue("MinimizeToTray", cbxMinimizeTray.Checked, RegistryValueKind.DWord);
                regkey.SetValue("CloseToTray", cbxCloseTray.Checked, RegistryValueKind.DWord);
                regkey.Close();
                settingsChanged = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error has occurred. Please provide the following information to the developer:\n" + ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Read the settings from the registry
        private void LoadSettings()
        {
            try
            {
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box"); //Open the key as read only
                if (regkey == null)
                { //Key doesn't exist yet, fallback to defaults
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\86Box");
                    txtCFGdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\86Box VMs";
                    txtEXEdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\86Box";
                    cbxMinimize.Checked = false;
                    cbxShowConsole.Checked = true;
                    cbxMinimizeTray.Checked = false;
                    cbxCloseTray.Checked = false;
                }
                else
                {
                    txtEXEdir.Text = regkey.GetValue("EXEdir").ToString();
                    txtCFGdir.Text = regkey.GetValue("CFGdir").ToString();
                    cbxMinimize.Checked = Convert.ToBoolean(regkey.GetValue("MinimizeOnVMStart"));
                    cbxShowConsole.Checked = Convert.ToBoolean(regkey.GetValue("ShowConsole"));
                    cbxMinimizeTray.Checked = Convert.ToBoolean(regkey.GetValue("MinimizeToTray"));
                    cbxCloseTray.Checked = Convert.ToBoolean(regkey.GetValue("CloseToTray"));

                    //These two lines are needed because storing the values into the textboxes (above code) triggers textchanged event
                    settingsChanged = false;
                    btnApply.Enabled = false;

                    regkey.Close();
                }
            }
            catch(Exception ex)
            {
                txtCFGdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\86Box Virtual Machines";
                txtEXEdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\86Box";
            }
        }

        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a folder where 86Box program files and the roms folder are located.";
            fbd.ShowNewFolderButton = true;
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtEXEdir.Text = fbd.SelectedPath;
                if (!txtEXEdir.Text.EndsWith(@"\")) //Just in case
                {
                    txtEXEdir.Text += @"\";
                }
            }
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a folder where your virtual machines (configs, nvr folders, etc.) will be located.";
            fbd.ShowNewFolderButton = true;
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtCFGdir.Text = fbd.SelectedPath;
                if (!txtCFGdir.Text.EndsWith(@"\")) //Just in case
                {
                    txtCFGdir.Text += @"\";
                }
            }
        }

        private void cbxMinimize_CheckedChanged(object sender, EventArgs e)
        {
            settingsChanged = true;
            btnApply.Enabled = true;
        }

        private void cbxShowConsole_CheckedChanged(object sender, EventArgs e)
        {
            settingsChanged = true;
            btnApply.Enabled = true;
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            ResetSettings();
        }

        //Resets the settings to their default values
        private void ResetSettings()
        {
            try
            {
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE", true); //Open the key as read only
                Registry.CurrentUser.DeleteSubKeyTree(@"86Box");
            }
            catch(Exception ex){/*Do nothing, key doesn't exist anyway*/}

            txtCFGdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\86Box VMs";
            txtEXEdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\86Box";
            cbxMinimize.Checked = false;
            cbxShowConsole.Checked = true;
            cbxMinimizeTray.Checked = false;
            cbxCloseTray.Checked = false;
        }

        private void cbxCloseTray_CheckedChanged(object sender, EventArgs e)
        {
            settingsChanged = true;
            btnApply.Enabled = true;
        }

        private void cbxMinimizeTray_CheckedChanged(object sender, EventArgs e)
        {
            settingsChanged = true;
            btnApply.Enabled = true;
        }
    }
}
