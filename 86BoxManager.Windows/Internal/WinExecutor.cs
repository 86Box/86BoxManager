using System.Diagnostics;
using _86BoxManager.API;
using _86BoxManager.Common;

namespace _86BoxManager.Windows.Internal
{
    internal sealed class WinExecutor : CommonExecutor
    {
        public override ProcessStartInfo BuildStartInfo(IExecVars args)
        {
            var info = base.BuildStartInfo(args);
            var ops = info.ArgumentList;
            if (args.Handle != null)
            {
                ops.Add("--hwnd");
                var (idString, hWndHex) = args.Handle.Value;
                ops.Add($"{idString},{hWndHex}");
            }
            return info;
        }
    }
}