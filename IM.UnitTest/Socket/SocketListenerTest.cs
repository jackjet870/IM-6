using IM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace IM.UnitTest
{
    [TestClass]
    public class SocketListenerTest
    {
        string ServerHost = "127.0.0.1";
        int ServerPort = 10223;
        int SocketTimeout = 3000;
        SocketListener SocketListener = null;

        Socket ClientSocket = null;
        string Message = "Hello World!";

         ManualResetEvent ManualResetEvent1 = new ManualResetEvent(false);
         ManualResetEvent ManualResetEvent2 = new ManualResetEvent(false);

        [TestInitialize]
        public void Initialize()
        {
            this.SocketListener = new SocketListener(this.ServerHost, this.ServerPort, this.SocketTimeout);
            this.SocketListener.Start();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (this.SocketListener != null)
                this.SocketListener.Stop();
        }

        [TestMethod]
        public void TestSocketListener()
        {
            Socket _socketReceived = null;
            string _clientAddress = string.Empty;
            int _bytesReceived = 0;
            string  _messageReceived = string.Empty;

            this.SocketListener.SocketAccepted += delegate(object sender, SocketAcceptedEventArgs e)
            {
                ManualResetEvent1.Reset();
                _socketReceived = e.SocketAccepted;
                _clientAddress = _socketReceived == null ? "" : _socketReceived.RemoteEndPoint.ToString();
                ManualResetEvent1.Set();
            };
            this.SocketListener.SocketReceiveCompleted += delegate(object sender, SocketReceiveCompletedEventArgs e)
            {
                ManualResetEvent2.Reset();
                _bytesReceived = e.ReceivedSocketState.BytesReceived;
                if (_bytesReceived > 0)
                    _messageReceived = Encoding.Default.GetString(e.ReceivedSocketState.DataReceived);
                ManualResetEvent2.Set();
            };

            this.ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ClientSocket.Connect(this.ServerHost, this.ServerPort);
            Assert.AreEqual(true, this.ClientSocket.Connected, string.Format("连接SocketListener{0}:{1}失败", this.ServerHost, this.ServerPort));

            this.ClientSocket.Send(Encoding.Default.GetBytes(this.Message));

            bool _result1 = ManualResetEvent1.WaitOne(this.SocketTimeout);
            bool _result2 = ManualResetEvent2.WaitOne(this.SocketTimeout);

            Assert.AreEqual(true, _result1 && _result2, "SocketListener接收socket超时");
            Assert.AreNotEqual(null, _socketReceived, "接收socket的为空");
            Assert.AreEqual(this.ClientSocket.LocalEndPoint.ToString(), _clientAddress, string.Format("预期的Socket = {0}，实际的Socket = {1}", this.ClientSocket.ToString(), _clientAddress));
            Assert.AreNotEqual(0, _bytesReceived, "接收数据包失败");
            Assert.AreEqual(this.Message, _messageReceived, string.Format("接收数据包内容：{0}与实际发送数据包内容：{1}不一致", _messageReceived, this.Message));

            this.ClientSocket.Close();
            this.ClientSocket.Dispose();
        }
    }
}
