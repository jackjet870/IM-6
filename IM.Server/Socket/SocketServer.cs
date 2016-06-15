using IM.Common;
using IM.Core;
using IM.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Server
{
    public class SocketServer
    {
        static SocketServer() { }
        public static SocketServer Default = new SocketServer();
        private object LockedObject = new object();

        private SocketListener SocketListener = null;

        public event EventHandler<SocketAcceptedEventArgs> SocketReceived;
        public event EventHandler<SocketReceiveCompletedEventArgs> SocketReceiveCompleted;

        public void Start(string listeningHost, int listeningPort, int socketTimeout = 3000)
        {
            try
            {
                //启动监听器
                string _host = listeningHost;
                int _port = listeningPort;
                this.SocketListener = new SocketListener(_host, _port, socketTimeout);
                this.SocketListener.SocketAccepted += SocketListener_SocketReceived;
                this.SocketListener.SocketReceiveCompleted += SocketListener_SocketReceiveCompleted;
                this.SocketListener.Start();
                //启动请求处理引擎
                RequestHandlerEngine.Default.Start();
            }
            catch
            {
                throw;
            }
        }

        public void Stop()
        {
            if (this.SocketListener != null)
            {
                this.SocketListener.Stop();
            }
            //关闭请求处理引擎
            RequestHandlerEngine.Default.Stop();
        }

        private void SocketListener_SocketReceived(object sender, SocketAcceptedEventArgs e)
        {
            if (this.SocketReceived != null)
                this.SocketReceived(this, e);
        }

        private void SocketListener_SocketReceiveCompleted(object sender, SocketReceiveCompletedEventArgs e)
        {
            if (this.SocketReceiveCompleted != null)
                this.SocketReceiveCompleted(this, e);
        }
    }
}
