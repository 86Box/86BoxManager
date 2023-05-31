using _86BoxManager.API;
using _86BoxManager.Common;

namespace _86BoxManager.Unix
{
    public abstract class UnixManager : CommonManager
    {
        private readonly UnixExecutor _exec;

        protected UnixManager(string tempDir)
        {
            _exec = new UnixExecutor(tempDir);
        }

        public override IMessageLoop GetLoop(IMessageReceiver callback)
        {
            var loop = new UnixLoop(callback, _exec);
            return loop;
        }

        public override IMessageSender GetSender()
        {
            var loop = new UnixLoop(null, _exec);
            return loop;
        }

        public override IExecutor GetExecutor()
        {
            return _exec;
        }
    }
}