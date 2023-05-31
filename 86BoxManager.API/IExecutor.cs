using System.Diagnostics;

namespace _86BoxManager.API
{
    public interface IExecutor
    {
        ProcessStartInfo BuildStartInfo(IExecVars args);

        ProcessStartInfo BuildConfigInfo(IExecVars args);
    }
}