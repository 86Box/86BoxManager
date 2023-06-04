using System.Diagnostics;
using _86BoxManager.API;

namespace _86BoxManager.Common
{
    public abstract class CommonExecutor : IExecutor
    {
        public virtual ProcessStartInfo BuildStartInfo(IExecVars args)
        {
            var info = new ProcessStartInfo(args.FileName);
            var ops = info.ArgumentList;
            if (!string.IsNullOrWhiteSpace(args.RomPath))
            {
                ops.Add("-R");
                ops.Add(args.RomPath);
            }
            if (!string.IsNullOrWhiteSpace(args.LogFile))
            {
                ops.Add("-L");
                ops.Add(args.LogFile);
            }
            ops.Add("-P");
            ops.Add(args.VmPath);
            ops.Add("-V");
            ops.Add(args.Vm.Name);
            return info;
        }

        public virtual ProcessStartInfo BuildConfigInfo(IExecVars args)
        {
            var info = new ProcessStartInfo(args.FileName);
            var ops = info.ArgumentList;
            ops.Add("--settings");
            ops.Add("-P");
            ops.Add(args.VmPath);
            return info;
        }
    }
}