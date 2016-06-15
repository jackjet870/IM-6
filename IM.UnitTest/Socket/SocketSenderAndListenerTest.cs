using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IM.Core;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

namespace IM.UnitTest
{
    [TestClass]
    public class SocketSenderAndListenerTest
    {
        string ServerHost = "127.0.0.1";
        int ServerPort = 12404;
        int SocketTimeout = 3000;
        SocketSender Sender = null;
        SocketListener Listener = null;

        ManualResetEvent ManualResetEvent1 = new ManualResetEvent(false);
        ManualResetEvent ManualResetEvent2 = new ManualResetEvent(false);

        [TestInitialize]
        public void Initialize()
        {
            this.Listener = new SocketListener(this.ServerHost, this.ServerPort, this.SocketTimeout);
            this.Listener.Start();
            this.Sender = new SocketSender(this.ServerHost, this.ServerPort, this.SocketTimeout);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (this.Sender != null) this.Sender = null;
            if (this.Listener != null) this.Listener.Stop();
        }

        [TestMethod]
        public void TestSocketSenderAndSocketListener()
        {
            Socket _socketReceived = null;
            string _clientAddress = string.Empty;
            int _bytesReceived = 0;
            string _message = "Hello World!"
                , _messageReceived = string.Empty
                , _response = "Message Received."
                , _responseReceived = string.Empty;

            this.Listener.SocketAccepted += delegate(object sender, SocketAcceptedEventArgs e)
            {
                ManualResetEvent1.Reset();
                _socketReceived = e.SocketAccepted;
                _clientAddress = _socketReceived == null ? "" : _socketReceived.RemoteEndPoint.ToString();
                ManualResetEvent1.Set();
            };
            this.Listener.SocketReceiveCompleted += delegate(object sender, SocketReceiveCompletedEventArgs e)
            {
                ManualResetEvent2.Reset();
                _bytesReceived = e.ReceivedSocketState.BytesReceived;
                if (_bytesReceived > 0)
                {
                    _messageReceived = Encoding.Default.GetString(e.ReceivedSocketState.DataReceived);
                    e.ReceivedSocketState.Response(Encoding.Default.GetBytes(_response));
                }
                ManualResetEvent2.Set();
            };

            var _bytes = this.Sender.Send(Encoding.Default.GetBytes(_message));

            bool _result1 = ManualResetEvent1.WaitOne(this.SocketTimeout);
            bool _result2 = ManualResetEvent2.WaitOne(this.SocketTimeout);
            _responseReceived = Encoding.Default.GetString(_bytes);

            Assert.AreEqual(true, _result1 && _result2, "接收Socket超时！");
            Assert.AreEqual(_message, _messageReceived, string.Format("SocketListener接收失败，发送信息 = {0}，接收信息 = {1}", _message, _messageReceived));
            Assert.AreEqual(_response, _responseReceived, string.Format("SocketSender接收失败，发送信息 = {0}，接收信息 = {1}", _response, _responseReceived));
        }
    }
}
