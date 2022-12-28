using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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
            Get86BoxVersion();

            lblVersion1.Text = Application.ProductVersion.Substring(0, Application.ProductVersion.Length - 2);

            #if DEBUG
                lblVersion1.Text += " (Debug)";
            #endif
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
            bool success = SaveSettings();
            if (!success)
            {
                return;
            }
            settingsChanged = CheckForChanges();
            btnApply.Enabled = settingsChanged;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (settingsChanged)
            {
                SaveSettings();
            }
            Close();
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEXEdir.Text) || string.IsNullOrWhiteSpace(txtCFGdir.Text))
            {
                btnApply.Enabled = false;
            }
            else
            {
                settingsChanged = CheckForChanges();
                btnApply.Enabled = settingsChanged;
            }
        }

        //Obtains the 86Box version from 86Box.exe
        private void Get86BoxVersion()
        {
            try
            {
                FileVersionInfo vi = FileVersionInfo.GetVersionInfo(txtEXEdir.Text + @"\86Box.exe");
                if (vi.FilePrivatePart >= 3541) //Officially supported builds
                {
                    lbl86BoxVer1.Text = vi.FileMajorPart.ToString() + "." + vi.FileMinorPart.ToString() + "." + vi.FileBuildPart.ToString() + "." + vi.FilePrivatePart.ToString() + " - fully compatible";
                    lbl86BoxVer1.ForeColor = Color.ForestGreen;
                }
                else if (vi.FilePrivatePart >= 3333 && vi.FilePrivatePart < 3541) //Should mostly work...
                {
                    lbl86BoxVer1.Text = vi.FileMajorPart.ToString() + "." + vi.FileMinorPart.ToString() + "." + vi.FileBuildPart.ToString() + "." + vi.FilePrivatePart.ToString() + " - partially compatible";
                    lbl86BoxVer1.ForeColor = Color.Orange;
                }
                else //Completely unsupported, since version info can't be obtained anyway
                {
                    lbl86BoxVer1.Text = "Unknown - may not be compatible";
                    lbl86BoxVer1.ForeColor = Color.Red;
                }
            }
            catch(FileNotFoundException ex)
            {
                lbl86BoxVer1.Text = "86Box.exe not found";
                lbl86BoxVer1.ForeColor = Color.Gray;
            }
        }
        
        //TODO: Rewrite
        //Save the settings to the registry
        private bool SaveSettings()
        {
            if (cbxLogging.Checked && string.IsNullOrWhiteSpace(txtLogPath.Text))
            {
                DialogResult result = MessageBox.Show("Using an empty or whitespace string for the log path will prevent 86Box from logging anything. Are you sure you want to use this path?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return false;
                }
            }
            if (!File.Exists(txtEXEdir.Text + "86Box.exe") && !File.Exists(txtEXEdir.Text + @"\86Box.exe"))
            {
                DialogResult result = MessageBox.Show("86Box.exe could not be found in the directory you specified, so you won't be able to use any virtual machines. Are you sure you want to use this path?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return false;
                }
            }
            try
            {
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true); //Try to open the key first (in read-write mode) to see if it already exists
                if (regkey == null) //Regkey doesn't exist yet, must be created first and then reopened
                {
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\86Box");
                    regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);
                    regkey.CreateSubKey("Virtual Machines");
                }

                //Store the new values, close the key, changes are saved
                regkey.SetValue("EXEdir", txtEXEdir.Text, RegistryValueKind.String);
                regkey.SetValue("CFGdir", txtCFGdir.Text, RegistryValueKind.String);
                regkey.SetValue("MinimizeOnVMStart", cbxMinimize.Checked, RegistryValueKind.DWord);
                regkey.SetValue("ShowConsole", cbxShowConsole.Checked, RegistryValueKind.DWord);
                regkey.SetValue("MinimizeToTray", cbxMinimizeTray.Checked, RegistryValueKind.DWord);
                regkey.SetValue("CloseToTray", cbxCloseTray.Checked, RegistryValueKind.DWord);
                regkey.SetValue("EnableLogging", cbxLogging.Checked, RegistryValueKind.DWord);
                regkey.SetValue("LogPath", txtLogPath.Text, RegistryValueKind.String);
                regkey.SetValue("EnableGridLines", cbxGrid.Checked, RegistryValueKind.DWord);
                regkey.Close();

                settingsChanged = CheckForChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred. Please provide the following information to the developer:\n" + ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Get86BoxVersion(); //Get the new exe version in any case
            }
            return true;
        }

        //TODO: Rewrite
        //Read the settings from the registry
        private void LoadSettings()
        {
            try
            {
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", false); //Open the key as read only

                //If the key doesn't exist yet, fallback to defaults
                if (regkey == null)
                {
                    MessageBox.Show("86Box Manager settings could not be loaded. This is normal if you're running 86Box Manager for the first time. Default values will be used.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //Create the key and reopen it for write access
                    Registry.CurrentUser.CreateSubKey(@"SOFTWARE\86Box");
                    regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);
                    regkey.CreateSubKey("Virtual Machines");

                    txtCFGdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\86Box VMs\";
                    txtEXEdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\86Box\";
                    cbxMinimize.Checked = false;
                    cbxShowConsole.Checked = true;
                    cbxMinimizeTray.Checked = false;
                    cbxCloseTray.Checked = false;
                    cbxLogging.Checked = false;
                    txtLogPath.Text = "";
                    cbxGrid.Checked = false;
                    btnBrowse3.Enabled = false;
                    txtLogPath.Enabled = false;

                    SaveSettings(); //This will write the default values to the registry
                }
                else
                {
                    txtEXEdir.Text = regkey.GetValue("EXEdir").ToString();
                    txtCFGdir.Text = regkey.GetValue("CFGdir").ToString();
                    txtLogPath.Text = regkey.GetValue("LogPath").ToString();
                    cbxMinimize.Checked = Convert.ToBoolean(regkey.GetValue("MinimizeOnVMStart"));
                    cbxShowConsole.Checked = Convert.ToBoolean(regkey.GetValue("ShowConsole"));
                    cbxMinimizeTray.Checked = Convert.ToBoolean(regkey.GetValue("MinimizeToTray"));
                    cbxCloseTray.Checked = Convert.ToBoolean(regkey.GetValue("CloseToTray"));
                    cbxLogging.Checked = Convert.ToBoolean(regkey.GetValue("EnableLogging"));
                    cbxGrid.Checked = Convert.ToBoolean(regkey.GetValue("EnableGridLines"));
                    txtLogPath.Enabled = cbxLogging.Checked;
                    btnBrowse3.Enabled = cbxLogging.Checked;
                }

                regkey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("86Box Manager settings could not be loaded, because an error occured trying to load the registry keys and/or values. Make sure you have the required permissions and try again. Default values will be used now.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtCFGdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\86Box VMs";
                txtEXEdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\86Box";
                cbxMinimize.Checked = false;
                cbxShowConsole.Checked = true;
                cbxMinimizeTray.Checked = false;
                cbxCloseTray.Checked = false;
                cbxLogging.Checked = false;
                txtLogPath.Text = "";
                cbxGrid.Checked = false;
                txtLogPath.Enabled = false;
                btnBrowse3.Enabled = false;
            }
        }

        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            FolderSelectDialog dialog = new FolderSelectDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Title = "Select a folder where 86Box program files and the roms folder are located"
            };

            if (dialog.Show(Handle))
            {
                txtEXEdir.Text  = dialog.FileName;
                if (!txtEXEdir.Text.EndsWith(@"\")) //Just in case
                {
                    txtEXEdir.Text += @"\";
                }
            }
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            FolderSelectDialog dialog = new FolderSelectDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Title = "Select a folder where your virtual machines (configs, nvr folders, etc.) will be located"
            };

            if (dialog.Show(Handle))
            {
                txtCFGdir.Text = dialog.FileName;
                if (!txtCFGdir.Text.EndsWith(@"\")) //Just in case
                {
                    txtCFGdir.Text += @"\";
                }
            }
        }

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("All settings will be reset to their default values. Do you wish to continue?", "Settings will be reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                ResetSettings();
            }
        }

        //Resets the settings to their default values
        private void ResetSettings()
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);
            if (regkey == null)
            {
                Registry.CurrentUser.CreateSubKey(@"SOFTWARE\86Box");
                regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box", true);
                regkey.CreateSubKey("Virtual Machines");
            }
            regkey.Close();

            txtCFGdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\86Box VMs\";
            txtEXEdir.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\86Box\";
            cbxMinimize.Checked = false;
            cbxShowConsole.Checked = true;
            cbxMinimizeTray.Checked = false;
            cbxCloseTray.Checked = false;
            cbxLogging.Checked = false;
            txtLogPath.Text = "";
            cbxGrid.Checked = false;
            txtLogPath.Enabled = false;
            btnBrowse3.Enabled = false;

            settingsChanged = CheckForChanges();
        }

        //Checks if all controls match the currently saved settings to determine if any changes were made
        private bool CheckForChanges()
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\86Box");

            try
            {
                btnApply.Enabled = (
                    txtEXEdir.Text != regkey.GetValue("EXEdir").ToString() ||
                    txtCFGdir.Text != regkey.GetValue("CFGdir").ToString() ||
                    txtLogPath.Text != regkey.GetValue("LogPath").ToString() ||
                    cbxMinimize.Checked != Convert.ToBoolean(regkey.GetValue("MinimizeOnVMStart")) ||
                    cbxShowConsole.Checked != Convert.ToBoolean(regkey.GetValue("ShowConsole")) ||
                    cbxMinimizeTray.Checked != Convert.ToBoolean(regkey.GetValue("MinimizeToTray")) ||
                    cbxCloseTray.Checked != Convert.ToBoolean(regkey.GetValue("CloseToTray")) || 
                    cbxLogging.Checked != Convert.ToBoolean(regkey.GetValue("EnableLogging")) ||
                    cbxGrid.Checked != Convert.ToBoolean(regkey.GetValue("EnableGridLines")));

                return btnApply.Enabled;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return true; //For now let's just return true if anything goes wrong
            }
            finally
            {
                regkey.Close();
            }
        }

        private void cbx_CheckedChanged(object sender, EventArgs e)
        {
            settingsChanged = CheckForChanges();
        }

        private void cbxLogging_CheckedChanged(object sender, EventArgs e)
        {
            settingsChanged = CheckForChanges();
            txt_TextChanged(sender, e); //Needed so the Apply button doesn't get enabled on an empty logpath textbox. Too lazy to write a duplicated empty check...
            txtLogPath.Enabled = cbxLogging.Checked;
            btnBrowse3.Enabled = cbxLogging.Checked;
        }

        private void btnBrowse3_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.DefaultExt = "log";
            ofd.Title = "Select a file where 86Box logs will be saved";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            ofd.Filter = "Log files (*.log)|*.log";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtLogPath.Text = ofd.FileName;
            }

            ofd.Dispose();
        }

        private void lnkGithub2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkGithub2.LinkVisited = true;
            Process.Start("https://github.com/86Box/86Box");
        }

        private void lnkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkGithub.LinkVisited = true;
            Process.Start("https://github.com/86Box/86BoxManager");
        }
    }
}