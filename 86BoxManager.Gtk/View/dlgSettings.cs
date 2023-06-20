using System;
using System.IO;
using System.Linq;
using _86BoxManager.Core;
using _86BoxManager.Registry;
using _86BoxManager.Tools;
using _86BoxManager.Xplat;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using IOPath = System.IO.Path;
using RegistryValueKind = _86BoxManager.Registry.ValueKind;

namespace _86BoxManager.View
{
    internal sealed partial class dlgSettings : Dialog
    {
        public dlgSettings() : this(new Builder("dlgSettings.glade"))
        {
            InitializeComponent();
        }

        private dlgSettings(Builder builder) : base(builder.GetRawOwnedObject("dlgSettings"))
        {
            builder.Autoconnect(this);
            DefaultResponse = ResponseType.Cancel;

            Response += Dialog_Response;
        }

        private void Dialog_Response(object o, ResponseArgs args)
        {
            Hide();
        }

        private void dlgSettings_Load(object sender, EventArgs e)
        {
            LoadSettings();
            Get86BoxVersion();

            var txt = CurrentApp.ProductVersion.Substring(0, CurrentApp.ProductVersion.Length - 2);
            lblVersion1.Text = $"Version: {txt}";

            #if DEBUG
            lblVersion1.Text += " (Debug)";
            #endif
        }

        private void dlgSettings_FormClosing(object sender, EventArgs e)
        {
            if (!settingsChanged)
                return;

            // Unsaved changes, ask the user to confirm
            var result = (ResponseType)Dialogs.ShowMessageBox(
                "Would you like to save the changes you've made to the settings?",
                MessageType.Question, ButtonsType.YesNo, "Unsaved changes");
            if (result == ResponseType.Yes)
            {
                SaveSettings();
            }
        }

        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            var initDir = Platforms.Env.MyComputer;
            var text = "Select a folder where 86Box program files and the roms folder are located";

            var fileName = Dialogs.SelectFolder(initDir, text, parent: this);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                txtEXEdir.Text = fileName;
                if (!txtEXEdir.Text.EndsWith(IOPath.DirectorySeparatorChar)) //Just in case
                {
                    txtEXEdir.Text += IOPath.DirectorySeparatorChar;
                }
            }
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            var initDir = Platforms.Env.MyComputer;
            var text = "Select a folder where your virtual machines (configs, nvr folders, etc.) will be located";

            var fileName = Dialogs.SelectFolder(initDir, text, parent: this);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                txtCFGdir.Text = fileName;
                if (!txtCFGdir.Text.EndsWith(IOPath.DirectorySeparatorChar)) //Just in case
                {
                    txtCFGdir.Text += IOPath.DirectorySeparatorChar;
                }
            }
        }

        private void btnBrowse3_Click(object sender, EventArgs e)
        {
            var dir = Platforms.Env.MyComputer;
            var title = "Select a file where 86Box logs will be saved";
            var filter = "Log files (*.log)|*.log";

            var fileName = Dialogs.SaveFile(title, dir, filter, parent: this);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                txtLogPath.Text = fileName;
            }
        }

        private void lnkGithub2_LinkClicked(object sender, EventArgs e)
        {
            lnkGithub2.Visited = true;
        }

        private void lnkGithub_LinkClicked(object sender, EventArgs e)
        {
            lnkGithub.Visited = true;
        }

        private bool settingsChanged = false; // Keeps track of unsaved changes

        private void btnApply_Click(object sender, EventArgs e)
        {
            var success = SaveSettings();
            if (!success)
            {
                return;
            }
            settingsChanged = CheckForChanges();
            btnApply.Sensitive = settingsChanged;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (settingsChanged)
            {
                SaveSettings();
            }
            Respond(ResponseType.Close);
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEXEdir.Text) || string.IsNullOrWhiteSpace(txtCFGdir.Text))
            {
                btnApply.Sensitive = false;
                return;
            }

            settingsChanged = CheckForChanges();
            btnApply.Sensitive = settingsChanged;
        }

        // Obtains the 86Box version from 86Box executable
        private void Get86BoxVersion()
        {
            try
            {
                var vi = Platforms.Manager.GetBoxVersion(txtEXEdir.Text);
                if (vi.FilePrivatePart >= 3541) //Officially supported builds
                {
                    var vText = $"{vi.FileMajorPart}.{vi.FileMinorPart}.{vi.FileBuildPart}.{vi.FilePrivatePart} - fully compatible";
                    lbl86BoxVer1.Markup = $@"<span weight=""bold"" foreground=""ForestGreen"">{vText}</span>";
                }
                else if (vi.FilePrivatePart >= 3333 && vi.FilePrivatePart < 3541) //Should mostly work...
                {
                    var vText = $"{vi.FileMajorPart}.{vi.FileMinorPart}.{vi.FileBuildPart}.{vi.FilePrivatePart} - partially compatible";
                    lbl86BoxVer1.Markup = $@"<span weight=""bold"" foreground=""Orange"">{vText}</span>";
                }
                else //Completely unsupported, since version info can't be obtained anyway
                {
                    var vText = "Unknown - may not be compatible";
                    lbl86BoxVer1.Markup = $@"<span weight=""bold"" foreground=""Red"">{vText}</span>";
                }
            }
            catch
            {
                var vText = "86Box executable not found!";
                lbl86BoxVer1.Markup = $@"<span weight=""bold"" foreground=""Gray"">{vText}</span>";
            }
        }

        // Save the settings to the registry
        private bool SaveSettings()
        {
            if (cbxLogging.Active && string.IsNullOrWhiteSpace(txtLogPath.Text))
            {
                var result = (ResponseType)Dialogs.ShowMessageBox(
                    "Using an empty or whitespace string for the log path will " +
                    "prevent 86Box from logging anything. Are you sure you want to use" +
                    " this path?",
                    MessageType.Warning, ButtonsType.YesNo, "Warning");
                if (result == ResponseType.No)
                {
                    return false;
                }
            }

            var exeName = Platforms.Env.ExeNames.First();
            var boxExe = IOPath.Combine(txtEXEdir.Text, exeName);
            if (!File.Exists(boxExe))
            {
                var result = (ResponseType)Dialogs.ShowMessageBox(
                    "86Box executable could not be found in the directory you specified, so " +
                    "you won't be able to use any virtual machines. Are you sure you want " +
                    "to use this path?",
                    MessageType.Warning, ButtonsType.YesNo, "Warning");
                if (result == ResponseType.No)
                {
                    return false;
                }
            }

            try
            {
                //Try to open the key first (in read-write mode) to see if it already exists
                var regkey = Configs.Open86BoxKey(true);

                //Regkey doesn't exist yet, must be created first and then reopened
                if (regkey == null)
                {
                    Configs.Create86BoxKey();
                    regkey = Configs.Open86BoxKey(true);
                    Configs.Create86BoxVmKey();
                }

                //Store the new values, close the key, changes are saved
                regkey.SetValue("EXEdir", txtEXEdir.Text, RegistryValueKind.String);
                regkey.SetValue("CFGdir", txtCFGdir.Text, RegistryValueKind.String);
                regkey.SetValue("MinimizeOnVMStart", cbxMinimize.Active, RegistryValueKind.DWord);
                regkey.SetValue("ShowConsole", cbxShowConsole.Active, RegistryValueKind.DWord);
                regkey.SetValue("MinimizeToTray", cbxMinimizeTray.Active, RegistryValueKind.DWord);
                regkey.SetValue("CloseToTray", cbxCloseTray.Active, RegistryValueKind.DWord);
                regkey.SetValue("EnableLogging", cbxLogging.Active, RegistryValueKind.DWord);
                regkey.SetValue("LogPath", txtLogPath.Text, RegistryValueKind.String);
                regkey.SetValue("EnableGridLines", cbxGrid.Active, RegistryValueKind.DWord);
                regkey.Close();

                settingsChanged = CheckForChanges();
            }
            catch (Exception ex)
            {
                Dialogs.ShowMessageBox("An error has occurred. Please provide the following information" +
                                       $" to the developer:\n{ex.Message}\n{ex.StackTrace}",
                    MessageType.Error, ButtonsType.Ok, "Error");
                return false;
            }
            finally
            {
                Get86BoxVersion(); //Get the new exe version in any case
            }
            return true;
        }
        
        // Read the settings from the registry
        private void LoadSettings()
        {
            try
            {
                var regkey = Configs.Open86BoxKey(false); //Open the key as read only

                //If the key doesn't exist yet, fallback to defaults
                if (regkey == null)
                {
                    Dialogs.ShowMessageBox("86Box Manager settings could not be loaded. This " +
                                           "is normal if you're running 86Box Manager for the first " +
                                           "time. Default values will be used.",
                        MessageType.Warning, ButtonsType.Ok, "Warning");

                    //Create the key and reopen it for write access
                    Configs.Create86BoxKey();
                    regkey = Configs.Open86BoxKey(true);
                    Configs.Create86BoxVmKey();

                    var (cfgPath, exePath) = VMCenter.FindPaths();
                    txtCFGdir.Text = cfgPath;
                    txtEXEdir.Text = exePath;
                    cbxMinimize.Active = false;
                    cbxShowConsole.Active = true;
                    cbxMinimizeTray.Active = false;
                    cbxCloseTray.Active = false;
                    cbxLogging.Active = false;
                    txtLogPath.Text = "";
                    cbxGrid.Active = false;
                    btnBrowse3.Sensitive = false;
                    txtLogPath.Sensitive = false;

                    SaveSettings(); //This will write the default values to the registry
                }
                else
                {
                    txtEXEdir.Text = regkey.GetValue("EXEdir").ToString();
                    txtCFGdir.Text = regkey.GetValue("CFGdir").ToString();
                    txtLogPath.Text = regkey.GetValue("LogPath").ToString();
                    cbxMinimize.Active = Convert.ToBoolean(regkey.GetValue("MinimizeOnVMStart"));
                    cbxShowConsole.Active = Convert.ToBoolean(regkey.GetValue("ShowConsole"));
                    cbxMinimizeTray.Active = Convert.ToBoolean(regkey.GetValue("MinimizeToTray"));
                    cbxCloseTray.Active = Convert.ToBoolean(regkey.GetValue("CloseToTray"));
                    cbxLogging.Active = Convert.ToBoolean(regkey.GetValue("EnableLogging"));
                    cbxGrid.Active = Convert.ToBoolean(regkey.GetValue("EnableGridLines"));
                    txtLogPath.IsEditable = cbxLogging.Active;
                    btnBrowse3.Sensitive = cbxLogging.Active;
                }

                regkey.Close();
            }
            catch 
            {
                Dialogs.ShowMessageBox("86Box Manager settings could not be loaded, because an " +
                                       "error occured trying to load the registry keys and/or values. " +
                                       "Make sure you have the required permissions and try again. " +
                                       "Default values will be used now.",
                    MessageType.Warning, ButtonsType.Ok, "Warning");

                var (_, exePath) = VMCenter.FindPaths();
                txtCFGdir.Text = IOPath.Combine(Platforms.Env.MyDocuments, "86Box VMs");
                txtEXEdir.Text = exePath;
                cbxMinimize.Active = false;
                cbxShowConsole.Active = true;
                cbxMinimizeTray.Active = false;
                cbxCloseTray.Active = false;
                cbxLogging.Active = false;
                txtLogPath.Text = "";
                cbxGrid.Active = false;
                txtLogPath.IsEditable = false;
                btnBrowse3.Sensitive = false;
            }
        }
        
        // Checks if all controls match the currently saved settings to determine if any changes were made
        private bool CheckForChanges()
        {
            var regkey = Configs.Open86BoxKey();

            try
            {
                btnApply.Sensitive = (
                    txtEXEdir.Text != regkey.GetValue("EXEdir").ToString() ||
                    txtCFGdir.Text != regkey.GetValue("CFGdir").ToString() ||
                    txtLogPath.Text != regkey.GetValue("LogPath").ToString() ||
                    cbxMinimize.Active != Convert.ToBoolean(regkey.GetValue("MinimizeOnVMStart")) ||
                    cbxShowConsole.Active != Convert.ToBoolean(regkey.GetValue("ShowConsole")) ||
                    cbxMinimizeTray.Active != Convert.ToBoolean(regkey.GetValue("MinimizeToTray")) ||
                    cbxCloseTray.Active != Convert.ToBoolean(regkey.GetValue("CloseToTray")) || 
                    cbxLogging.Active != Convert.ToBoolean(regkey.GetValue("EnableLogging")) ||
                    cbxGrid.Active != Convert.ToBoolean(regkey.GetValue("EnableGridLines")));

                return btnApply.Sensitive;
            }
            catch (Exception ex)
            {
                Dialogs.ShowMessageBox($"Error: {ex.Message}", MessageType.Error);
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
            // Needed so the Apply button doesn't get enabled on an empty
            // logpath textbox. Too lazy to write a duplicated empty check...
            txt_TextChanged(sender, e); 
            txtLogPath.IsEditable = cbxLogging.Active;
            btnBrowse3.Sensitive = cbxLogging.Active;
        }
        
        private void btnDefaults_Click(object sender, EventArgs e)
        {
            var result = Dialogs.ShowMessageBox("All settings will be reset to their default values. " +
                                                "Do you wish to continue?",
                MessageType.Warning, ButtonsType.YesNo, "Settings will be reset");
            if (result == (int)ResponseType.Yes)
            {
                ResetSettings();
            }
        }
        
        // Resets the settings to their default values
        private void ResetSettings()
        {
            var regkey = Configs.Open86BoxKey(true);
            if (regkey == null)
            {
                Configs.Create86BoxKey();
                regkey = Configs.Open86BoxKey(true);
                Configs.Create86BoxVmKey();
            }
            regkey.Close();

            var (cfgPath, exePath) = VMCenter.FindPaths();
            txtCFGdir.Text = cfgPath;
            txtEXEdir.Text = exePath;
            cbxMinimize.Active = false;
            cbxShowConsole.Active = true;
            cbxMinimizeTray.Active = false;
            cbxCloseTray.Active = false;
            cbxLogging.Active = false;
            txtLogPath.Text = "";
            cbxGrid.Active = false;
            txtLogPath.IsEditable = false;
            btnBrowse3.Sensitive = false;

            settingsChanged = CheckForChanges();
        }
    }
}