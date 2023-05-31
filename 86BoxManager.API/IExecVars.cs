namespace _86BoxManager.API
{
    public interface IExecVars
    {
        string FileName { get; }

        string RomPath { get; }

        string LogFile { get; }

        string VmPath { get; }

        string VmName { get; }

        (string id, string hWnd)? Handle { get; }
    }
}