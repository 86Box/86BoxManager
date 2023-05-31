using _86BoxManager.API;

namespace _86BoxManager.Common
{
    public sealed class CommonExecVars : IExecVars
    {
        public string FileName { get; set; }

        public string RomPath { get; set; }

        public string LogFile { get; set; }

        public string VmPath { get; set; }

        public IVm Vm { get; set; }

        public (string id, string hWnd)? Handle { get; set; }
    }
}