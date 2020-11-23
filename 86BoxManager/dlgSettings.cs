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
            ApplicationSettings.LoadSettings();
            ApplicationSettings.Get86BoxVersion();
            Display86BoxVersion(); 

            lblVersion1.Text = Application.ProductVersion.Substring(0, Application.ProductVersion.Length - 2);

            #if DEBUG
                lblVersion1.Text += " (Debug)";
            #endif
        }

        /// <summary>
        /// Update the controls to fit the new settings in the static Settings class.
        /// </summary>
        private void UpdateControlsToFitSettings()
        {

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
                    ApplicationSettings.SaveSettings();
                    
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
            bool success = ApplicationSettings.SaveSettings();

            if (!success)
            {
                return;
            }
            else
            {
                Display86BoxVersion();
                settingsChanged = ApplicationSettings.CheckForChanges();
                btnApply.Enabled = settingsChanged;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (settingsChanged)
            {
                ApplicationSettings.SaveSettings();
            }

            Close();
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEXEdir.Text) || string.IsNullOrWhiteSpace(txtCFGdir.Text) ||
                string.IsNullOrWhiteSpace(txtLaunchTimeout.Text))
            {
                btnApply.Enabled = false;
            }
            else
            {
                settingsChanged = ApplicationSettings.CheckForChanges();
                btnApply.Enabled = settingsChanged;
            }
        }

        //Obtains the 86Box version from 86Box.exe
        private void Display86BoxVersion()
        {
            try
            {
                FileVersionInfo VI = ApplicationSettings.Get86BoxVersion(); 

                FileVersionInfo vi = FileVersionInfo.GetVersionInfo(txtEXEdir.Text + @"\86Box.exe");
                if (vi.FilePrivatePart >= 2008) //Officially supported builds
                {
                    lbl86BoxVer1.Text = vi.FileMajorPart.ToString() + "." + vi.FileMinorPart.ToString() + "." + vi.FileBuildPart.ToString() + "." + vi.FilePrivatePart.ToString() + " - supported";
                    lbl86BoxVer1.ForeColor = Color.ForestGreen;
                }
                else if (vi.FilePrivatePart >= 1763 && vi.FilePrivatePart < 2008) //Should mostly work...
                {
                    lbl86BoxVer1.Text = vi.FileMajorPart.ToString() + "." + vi.FileMinorPart.ToString() + "." + vi.FileBuildPart.ToString() + "." + vi.FilePrivatePart.ToString() + " - partially supported";
                    lbl86BoxVer1.ForeColor = Color.Orange;
                }
                else //Completely unsupported, since version info can't be obtained anyway
                {
                    lbl86BoxVer1.Text = "Unknown - not supported";
                    lbl86BoxVer1.ForeColor = Color.Red;
                }
            }
            catch (FileNotFoundException)
            {
                lbl86BoxVer1.Text = "86Box.exe not found";
                lbl86BoxVer1.ForeColor = Color.Gray;
            }
        }
        

// .NET Core implements the better Vista-style folder browse dialog in the stock FolderBrowserDialog
#if NETCOREAPP
        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer,
                Description = "Select a folder where 86Box program files and the roms folder are located",
                UseDescriptionForTitle = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtEXEdir.Text  = dialog.SelectedPath;
                if (!txtEXEdir.Text.EndsWith(@"\")) //Just in case
                {
                    txtEXEdir.Text += @"\";
                }
            }
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer,
                Description = "Select a folder where your virtual machines (configs, nvr folders, etc.) will be located",
                UseDescriptionForTitle = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtCFGdir.Text = dialog.SelectedPath;
                if (!txtCFGdir.Text.EndsWith(@"\")) //Just in case
                {
                    txtCFGdir.Text += @"\";
                }
            }
        }
// A custom class is required for Vista-style folder dialogs under the original .NET Framework
#else
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
#endif

        private void btnDefaults_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("All settings will be reset to their default values. Do you wish to continue?", "Settings will be reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                ApplicationSettings.RestoreToDefaults(false);
            }
        }

        //Resets the settings to their default values
        

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
                    txtLaunchTimeout.Text != regkey.GetValue("LaunchTimeout").ToString() ||
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

        /// <summary>
        /// Runs the exporter class code. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportSettingsFile_Click(object sender, EventArgs e)
        {

        }

        private void ExportZipButton_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}