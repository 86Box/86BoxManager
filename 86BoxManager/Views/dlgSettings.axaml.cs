using System;
using System.ComponentModel;
using _86boxManager.Tools;
using _86boxManager.Xplat;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.IO;
using System.Linq;
using _86boxManager.Core;
using _86boxManager.Registry;
using Avalonia.Media;
using ButtonsType = MessageBox.Avalonia.Enums.ButtonEnum;
using MessageType = MessageBox.Avalonia.Enums.Icon;
using ResponseType = MessageBox.Avalonia.Enums.ButtonResult;
using IOPath = System.IO.Path;
using RegistryValueKind = _86boxManager.Registry.ValueKind;

namespace _86boxManager.Views
{
    public partial class dlgSettings : Window
    {
        public dlgSettings()
        {
            InitializeComponent();
        }

        private bool settingsChanged = false; // Keeps track of unsaved changes

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

        private void dlgSettings_FormClosing(object sender, CancelEventArgs e)
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

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            var success = SaveSettings();
            if (!success)
            {
                return;
            }
            settingsChanged = CheckForChanges();
            btnApply.IsEnabled = settingsChanged;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (settingsChanged)
            {
                SaveSettings();
            }
            Close(ResponseType.Ok);
        }

        private async void btnBrowse3_Click(object sender, RoutedEventArgs e)
        {
            var dir = Platforms.Env.MyComputer;
            var title = "Select a file where 86Box logs will be saved";
            var filter = "Log files (*.log)|*.log";

            var fileName = await Dialogs.SaveFile(title, dir, filter, parent: this, ext: ".log");

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                txtLogPath.Text = fileName;
            }
        }

        private void btnDefaults_Click(object sender, RoutedEventArgs e)
        {
            var result = Dialogs.ShowMessageBox("All settings will be reset to their default values. " +
                                                "Do you wish to continue?",
                MessageType.Warning, ButtonsType.YesNo, "Settings will be reset");
            if (result == ResponseType.Yes)
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
            cbxMinimize.IsChecked = false;
            cbxShowConsole.IsChecked = true;
            cbxMinimizeTray.IsChecked = false;
            cbxCloseTray.IsChecked = false;
            cbxLogging.IsChecked = false;
            txtLogPath.Text = "";
            cbxGrid.IsChecked = false;
            txtLogPath.IsEditable(false);
            btnBrowse3.IsEnabled = false;

            settingsChanged = CheckForChanges();
        }

        private async void btnBrowse1_Click(object sender, RoutedEventArgs e)
        {
            var initDir = Platforms.Env.MyComputer;
            var text = "Select a folder where 86Box program files and the roms folder are located";

            var fileName = await Dialogs.SelectFolder(initDir, text, parent: this);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                txtEXEdir.Text = fileName;
                if (!txtEXEdir.Text.EndsWith(IOPath.DirectorySeparatorChar)) //Just in case
                {
                    txtEXEdir.Text += IOPath.DirectorySeparatorChar;
                }
            }
        }

        private async void btnBrowse2_Click(object sender, RoutedEventArgs e)
        {
            var initDir = Platforms.Env.MyComputer;
            var text = "Select a folder where your virtual machines (configs, nvr folders, etc.) will be located";

            var fileName = await Dialogs.SelectFolder(initDir, text, parent: this);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                txtCFGdir.Text = fileName;
                if (!txtCFGdir.Text.EndsWith(IOPath.DirectorySeparatorChar)) //Just in case
                {
                    txtCFGdir.Text += IOPath.DirectorySeparatorChar;
                }
            }
        }

        private void lnkGithub_LinkClicked(object sender, PointerPressedEventArgs e)
        {
        }

        private void lnkGithub2_LinkClicked(object sender, PointerPressedEventArgs e)
        {
        }

        private void txt_TextChanged(object sender, TextInputEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEXEdir.Text) || string.IsNullOrWhiteSpace(txtCFGdir.Text))
            {
                btnApply.IsEnabled = false;
                return;
            }

            settingsChanged = CheckForChanges();
            btnApply.IsEnabled = settingsChanged;
        }

        private void cbx_CheckedChanged(object sender, RoutedEventArgs e)
        {
            settingsChanged = CheckForChanges();
        }

        private void cbxLogging_CheckedChanged(object sender, RoutedEventArgs e)
        {
            settingsChanged = CheckForChanges();
            // Needed so the Apply button doesn't get enabled on an empty
            // logpath textbox. Too lazy to write a duplicated empty check...
            txt_TextChanged(sender, null);
            txtLogPath.IsEditable(cbxLogging.IsActive());
            btnBrowse3.IsEnabled = cbxLogging.IsActive();
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
                    lbl86BoxVer1.SetColorTxt(Brushes.ForestGreen, FontWeight.Bold, vText);
                }
                else if (vi.FilePrivatePart >= 3333 && vi.FilePrivatePart < 3541) //Should mostly work...
                {
                    var vText = $"{vi.FileMajorPart}.{vi.FileMinorPart}.{vi.FileBuildPart}.{vi.FilePrivatePart} - partially compatible";
                    lbl86BoxVer1.SetColorTxt(Brushes.Orange, FontWeight.Bold, vText);
                }
                else //Completely unsupported, since version info can't be obtained anyway
                {
                    var vText = "Unknown - may not be compatible";
                    lbl86BoxVer1.SetColorTxt(Brushes.Red, FontWeight.Bold, vText);
                }
            }
            catch
            {
                var vText = "86Box executable not found!";
                lbl86BoxVer1.SetColorTxt(Brushes.Gray, FontWeight.Bold, vText);
            }
        }

        // Save the settings to the registry
        private bool SaveSettings()
        {
            if (cbxLogging.IsActive() && string.IsNullOrWhiteSpace(txtLogPath.Text))
            {
                var result = Dialogs.ShowMessageBox(
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
                regkey.SetValue("MinimizeOnVMStart", cbxMinimize.IsActive(), RegistryValueKind.DWord);
                regkey.SetValue("ShowConsole", cbxShowConsole.IsActive(), RegistryValueKind.DWord);
                regkey.SetValue("MinimizeToTray", cbxMinimizeTray.IsActive(), RegistryValueKind.DWord);
                regkey.SetValue("CloseToTray", cbxCloseTray.IsActive(), RegistryValueKind.DWord);
                regkey.SetValue("EnableLogging", cbxLogging.IsActive(), RegistryValueKind.DWord);
                regkey.SetValue("LogPath", txtLogPath.Text, RegistryValueKind.String);
                regkey.SetValue("EnableGridLines", cbxGrid.IsActive(), RegistryValueKind.DWord);
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
                    cbxMinimize.IsChecked = false;
                    cbxShowConsole.IsChecked = true;
                    cbxMinimizeTray.IsChecked = false;
                    cbxCloseTray.IsChecked = false;
                    cbxLogging.IsChecked = false;
                    txtLogPath.Text = "";
                    cbxGrid.IsChecked = false;
                    btnBrowse3.IsEnabled = false;
                    txtLogPath.IsEnabled = false;

                    SaveSettings(); //This will write the default values to the registry
                }
                else
                {
                    txtEXEdir.Text = regkey.GetValue("EXEdir").ToString();
                    txtCFGdir.Text = regkey.GetValue("CFGdir").ToString();
                    txtLogPath.Text = regkey.GetValue("LogPath").ToString();
                    cbxMinimize.IsChecked = Convert.ToBoolean(regkey.GetValue("MinimizeOnVMStart"));
                    cbxShowConsole.IsChecked = Convert.ToBoolean(regkey.GetValue("ShowConsole"));
                    cbxMinimizeTray.IsChecked = Convert.ToBoolean(regkey.GetValue("MinimizeToTray"));
                    cbxCloseTray.IsChecked = Convert.ToBoolean(regkey.GetValue("CloseToTray"));
                    cbxLogging.IsChecked = Convert.ToBoolean(regkey.GetValue("EnableLogging"));
                    cbxGrid.IsChecked = Convert.ToBoolean(regkey.GetValue("EnableGridLines"));
                    txtLogPath.IsEditable(cbxLogging.IsActive());
                    btnBrowse3.IsEnabled = cbxLogging.IsActive();
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
                cbxMinimize.IsChecked = false;
                cbxShowConsole.IsChecked = true;
                cbxMinimizeTray.IsChecked = false;
                cbxCloseTray.IsChecked = false;
                cbxLogging.IsChecked = false;
                txtLogPath.Text = "";
                cbxGrid.IsChecked = false;
                txtLogPath.IsEditable(false);
                btnBrowse3.IsEnabled = false;
            }
        }

        // Checks if all controls match the currently saved settings to determine if any changes were made
        private bool CheckForChanges()
        {
            var regkey = Configs.Open86BoxKey();

            try
            {
                btnApply.IsEnabled = (
                    txtEXEdir.Text != regkey.GetValue("EXEdir").ToString() ||
                    txtCFGdir.Text != regkey.GetValue("CFGdir").ToString() ||
                    txtLogPath.Text != regkey.GetValue("LogPath").ToString() ||
                    cbxMinimize.IsActive() != Convert.ToBoolean(regkey.GetValue("MinimizeOnVMStart")) ||
                    cbxShowConsole.IsActive() != Convert.ToBoolean(regkey.GetValue("ShowConsole")) ||
                    cbxMinimizeTray.IsActive() != Convert.ToBoolean(regkey.GetValue("MinimizeToTray")) ||
                    cbxCloseTray.IsActive() != Convert.ToBoolean(regkey.GetValue("CloseToTray")) ||
                    cbxLogging.IsActive() != Convert.ToBoolean(regkey.GetValue("EnableLogging")) ||
                    cbxGrid.IsActive() != Convert.ToBoolean(regkey.GetValue("EnableGridLines")));

                return btnApply.IsEnabled;
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
    }
}