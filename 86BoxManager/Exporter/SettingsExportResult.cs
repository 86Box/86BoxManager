using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _86boxManager
{
    /// <summary>
    /// 2020-12-08  Connor Hyde (starfrost)
    /// 
    /// Settings export result enum.
    /// </summary>
    public enum SettingsExportResult
    {
        /// <summary>
        /// The export was successful.
        /// </summary>
        OK = 0,
        
        /// <summary>
        /// The user cancelled the export.
        /// </summary>
        Cancel = 1,

        /// <summary>
        /// There was an error during the export.
        /// </summary>
        Error = 2
    }
}
