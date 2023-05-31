using _86BoxManager.API;
using _86BoxManager.Common;

namespace _86BoxManager.Unix
{
    public abstract class UnixManager : CommonManager
    {
        public override IMessageLoop GetLoop(IMessageReceiver callback)
        {
            var loop = new UnixLoop(callback);
            return loop;
        }

        public override IMessageSender GetSender()
        {
            var loop = new UnixLoop(null);
            return loop;
        }

        public override IExecutor GetExecutor()
        {
            var exec = new UnixExecutor();
            return exec;
        }
    }
}