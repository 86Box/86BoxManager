using Microsoft.Win32; 
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _86boxManager
{
    /// <summary>
    /// 2020-11-23  Connor Hyde (starfrost013)
    /// </summary>
    public class SettingsExporter
    {
        public bool ZipUpRegFile { get; set; }
        
        // we probably need input validation support and result classes,
        // but i already wrote too much code for this pull request
        public bool ExportSettings()
        {
            SaveFileDialog SFD = new SaveFileDialog();
            
            // If the user has chosen to zip up the reg file, modify the dialog accordingly
            if (ZipUpRegFile)
            {
                SFD.Title = "Export to Registry File";
                SFD.Filter = "Registry file (*.reg)|*.reg";
            }
            else
            {
                SFD.Title = "Export to Zipped Registry File";
                SFD.Filter = "ZIP archive (*.zip)|*.zip";
            }

            SFD.ShowDialog();

            if (SFD.FileName == "")
            {
                return true;
            }
            else
            {
                // Export to .reg
                if (ZipUpRegFile)
                {
                    ExportReg(SFD.FileName);
                }
                else
                {
                    ExportZip(SFD.FileName); 
                }
            }

            return false; 
        }

        private bool ExportReg(string FileName)
        {
            return WriteRegFile(FileName);
        }

        private bool ExportZip(string FileName)
        {
            try
            {
                bool Result = WriteRegFile(FileName);

                if (!Result)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        return CompressRegFile(FileName);
                    }
                    // some more specific error messages for specific scenarios...
                    catch (DirectoryNotFoundException)
                    {
                        MessageBox.Show($"Directory not found!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    catch (PathTooLongException)
                    {
                        MessageBox.Show($"Path too long - please make sure the path to the file is less than 260 characters!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    catch (IOException err)
                    {
#if DEBUG
                        MessageBox.Show($"[Debug] An error occurred while exporting: {err.Message}\n\n{err.StackTrace}");
#endif
                        return false;
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show($"Directory not found!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (PathTooLongException)
            {
                MessageBox.Show($"Path too long - please make sure the path to the file is less than 260 characters!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (IOException err)
            {
#if DEBUG
                MessageBox.Show($"[Debug] An error occurred while exporting: {err.Message}\n\n{err.StackTrace}");
#endif
                return false;
            }
        }
     
        /// <summary>
        /// Write a .REG file from the settings
        /// </summary>
        /// <returns></returns>
        private bool WriteRegFile(string FileName)
        {
            using (StreamWriter SW = new StreamWriter(new FileStream(FileName, FileMode.OpenOrCreate)))
            {

                string BoxRoot = @"SOFTWARE\86Box";

                WriteRegFileHeader(SW);

                RegistryKey BoxKey = Registry.CurrentUser.OpenSubKey(BoxRoot);

                SW.WriteLine($@"[HKEY_CURRENT_USER\{BoxRoot}]");
                // 86box main key

                WriteRegKeyValues(SW, BoxKey);

                // loop through all subkeys of the 86box key    

                foreach (string BoxSubkeyName in BoxKey.GetSubKeyNames())
                {
                    RegistryKey BoxSubkey = BoxKey.OpenSubKey(BoxSubkeyName);

                    SW.WriteLine($@"[HKEY_CURRENT_USER\{BoxRoot}\{BoxSubkeyName}]");

                    WriteRegKeyValues(SW, BoxSubkey); 
                }

                SW.Close();
                return true; 
            }

        }

        private void WriteRegKeyValues(StreamWriter SW, RegistryKey BoxKey)
        {
            foreach (string BoxValueName in BoxKey.GetValueNames())
            {
                object BoxValue = BoxKey.GetValue(BoxValueName);

                if (BoxValue is int)
                {
                    // write a DWORD
                    WriteRegFileDwordValue(SW, BoxValueName, (int)BoxValue);
                }
                else if (BoxValue is string)
                {
                    WriteRegFileStringValue(SW, BoxValueName, (string)BoxValue);
                }
            }
        }

        // maybe best not in this class? idk
        private void WriteRegFileHeader(StreamWriter SW) => SW.WriteLine("Windows Registry Editor Version 5.00\n");

        /// <summary>
        /// Write a registry file dword value.
        /// </summary>
        /// <param name="SW"></param>
        /// <param name="KeyName"></param>
        /// <param name="DWordValue"></param>
        /// <returns></returns>
        private bool WriteRegFileDwordValue(StreamWriter SW, string KeyName, int DWordValue)
        {
            SW.Write($"\"{KeyName}\"=dword:");
            string HexString = DWordValue.ToString("X");
            
            HexString = HexString.ToLower(); // safety purposes

            if (HexString.Length > 8)
            {
                // dword moment
                return false;
            }
            else
            {
                // best to split this off into another function
                // just not worth doing it unless this code would be used multiple times
                int LeadingZeros = 8 - HexString.Length;

                // write the leading zeros
                for (int i = 0; i < LeadingZeros; i++) SW.Write("0");

                // write the hex string
                SW.Write(HexString);
                SW.Write("\n"); 

                return true; 
            }

        }

        /// <summary>
        /// Writes a string value to a .reg file. 
        /// </summary>
        /// <param name="SW"></param>
        /// <param name="KeyName"></param>
        /// <param name="KeyValue"></param>
        private void WriteRegFileStringValue(StreamWriter SW, string KeyName, string KeyValue) => SW.Write($"\"{KeyName}\"=\"{KeyValue}\"\n");

        private bool CompressRegFile(string RegFileName)
        {
            // lazy
            Directory.CreateDirectory(RegFileName);
            File.Copy(RegFileName, $@"{RegFileName}\{RegFileName}");
            ZipFile.CreateFromDirectory(RegFileName, $"{RegFileName}.zip");
            return true; 
        }
    }
}
