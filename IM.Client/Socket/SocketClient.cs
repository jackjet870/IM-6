using IM.Common;
using IM.Core;
using IM.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Client
{
    public class SocketClient
    {
        static SocketClient() { }
        public static SocketClient Default = new SocketClient();
        private static object LockedObject = new object();

        private SocketSender SocketSender = null;

        public void Start(string serverHost, int serverPort, int socketTimeout = 3000)
        {
            try
            {
                this.SocketSender = new SocketSender(serverHost, serverPort, socketTimeout);
            }
            catch
            {
                throw;
            }
        }

        public void Stop()
        {
            this.SocketSender = null;
        }

        public T Send<T>(PacketBodyBase packageBody) where T : PacketBodyBase
        {
            try
            {
                var _data = PacketUtil.Pack(packageBody);
                var _bytesReceived = this.SocketSender.Send(_data);
                var _packageBody = PacketUtil.UnPackResponse(_bytesReceived);
                return (T)_packageBody;
            }
            catch
            {
                throw;
            }
        }
    }
}
