using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using _86BoxManager.API;
using _86BoxManager.Common;

namespace _86BoxManager.Unix
{
    public sealed class UnixExecutor : CommonExecutor, IDisposable
    {
        private readonly string _tempDir;
        private readonly IDictionary<string, SocketInfo> _runningVm;

        public UnixExecutor(string tempDir)
        {
            _tempDir = tempDir;
            _runningVm = new Dictionary<string, SocketInfo>();
        }

        public void Dispose()
        {
            foreach (var info in _runningVm.Values)
                info.Dispose();
            _runningVm.Clear();
        }

        ~UnixExecutor()
        {
            Dispose();
        }

        public override ProcessStartInfo BuildStartInfo(IExecVars args)
        {
            var info = base.BuildStartInfo(args);

            var name = args.Vm.Name;
            var socketName = name + Environment.ProcessId;

            var server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            var socketPath = Path.Combine(_tempDir, socketName);
            server.Bind(new UnixDomainSocketEndPoint(socketPath));
            server.Listen();

            _runningVm[name] = new SocketInfo { Server = server };
            args.Vm.OnExit = OnVmExit;

            var opEnv = info.Environment;
            opEnv["86BOX_MANAGER_SOCKET"] = socketName;

            if (server.IsBound)
                server.BeginAccept(OnSocketConnect, (server, name));

            return info;
        }

        private void OnSocketConnect(IAsyncResult result)
        {
            var (server, name) = (ValueTuple<Socket, string>)result.AsyncState!;
            try
            {
                var client = server.EndAccept(result);
                _runningVm[name].Client = client;
            }
            catch
            {
                // Simply ignore!
            }
        }

        private void OnVmExit(IVm vm)
        {
            var name = vm.Name;
            if (!_runningVm.TryGetValue(name, out var info))
                return;
            info.Dispose();
            _runningVm.Remove(name);
        }

        private sealed class SocketInfo : IDisposable
        {
            public Socket Server { get; set; }
            public Socket Client { get; set; }

            public void Dispose()
            {
                Server?.Dispose();
                Client?.Dispose();
            }
        }

        internal Socket GetClient(string name)
        {
            return _runningVm.TryGetValue(name, out var info) ? info.Client : null;
        }
    }
}