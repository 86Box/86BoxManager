using System.Collections.Generic;
using System.Linq;
using _86boxManager.Registry;
using _86boxManager.Tools;
using _86boxManager.Xplat;
using _86boxManager.Model;
using ButtonsType = MessageBox.Avalonia.Enums.ButtonEnum;
using MessageType = MessageBox.Avalonia.Enums.Icon;
using ResponseType = MessageBox.Avalonia.Enums.ButtonResult;
using IOPath = System.IO.Path;
using RegistryValueKind = _86boxManager.Registry.ValueKind;

// ReSharper disable InconsistentNaming
namespace _86boxManager.Core
{
    internal static class VMCenter
    {
        private static VMWatch _watch;

        public static (string cfgpath, string exepath) FindPaths()
        {
            var cfgPath = IOPath.Combine(Platforms.Env.UserProfile, "86Box VMs").CheckTrail();
            var exeFolders = Platforms.Env.GetProgramFiles("86Box");
            var exeFound = Search.Find(exeFolders, Platforms.Env.ExeNames);
            if (exeFound == null)
            {
                // The old code did that, so... reproduce
                exeFound = exeFolders.First();
            }
            var exePath = exeFound.CheckTrail();
            return (cfgPath, exePath);
        }

        // Checks if a VM with this name already exists
        public static bool CheckIfExists(string name)
        {
            try
            {
                var regkey = Configs.Open86BoxVmKey(true);
                if (regkey == null) //Regkey doesn't exist yet
                {
                    regkey = Configs.Open86BoxKey(true);
                    Configs.Create86BoxVmKey();
                    return false;
                }

                //VM's registry value doesn't exist yet
                if (regkey.GetValue(name) == null)
                {
                    regkey.Close();
                    return false;
                }

                //It really exists
                regkey.Close();
                return true;
            }
            catch
            {
                Dialogs.ShowMessageBox("Could not load the virtual machine information from the " +
                                       "registry. Make sure you have the required permissions " +
                                       "and try again.",
                    MessageType.Error, ButtonsType.Ok, "Error");
                return false;
            }
        }

        public static void OpenConfig(IEnumerable<VMRow> selected)
        {
            foreach (var lvi in selected)
            {
                var vm = lvi.Tag;
                try
                {
                    var file = IOPath.Combine(vm.Path, "86box.cfg");
                    Platforms.Shell.EditFile(file);
                }
                catch
                {
                    Dialogs.ShowMessageBox($@"The config file for the virtual machine ""{vm.Name}"" could" +
                                           " not be opened. Make sure it still exists and that you have " +
                                           "sufficient privileges to access it.",
                        MessageType.Error, ButtonsType.Ok, "Error");
                }
            }
        }

        // Opens the folder containing the selected VM
        public static void OpenFolder(IEnumerable<VMRow> selected)
        {
            foreach (var lvi in selected)
            {
                var vm = lvi.Tag;
                try
                {
                    Platforms.Shell.OpenFolder(vm.Path);
                }
                catch
                {
                    Dialogs.ShowMessageBox($@"The folder for the virtual machine ""{vm.Name}"" could" +
                                           " not be opened. Make sure it still exists and that you have " +
                                           "sufficient privileges to access it.",
                        MessageType.Error, ButtonsType.Ok, "Error");
                }
            }
        }
    }
}