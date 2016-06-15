using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System.Threading.Tasks;
using IM.Core;
using System.Net;
using System.Text;
using System.Threading;

namespace IM.UnitTest
{
    [TestClass]
    public class SocketSenderTest
    {
        string ServerHost = "127.0.0.1";
        int ServerPort = 12304;
        int SocketTimeout = 3000;
        Socket Listener = null;
        bool IsListening = true;

        string Send = "Hello World!";
        string SendReceived = string.Empty;
        string Response = "Message Received.";
        string ResponseReceived = string.Empty;

        ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        [TestInitialize]
        public void Initialize()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.Listener.Bind(new IPEndPoint(IPAddress.Parse(this.ServerHost), this.ServerPort));
                    this.Listener.Listen(10);

                    while (this.IsListening)
                    {
                        this.Listener.BeginAccept(new AsyncCallback(AcceptCallback), this.Listener);
                    }
                }
                catch
                {
                    throw;
                }
            });
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.IsListening = false;
            if (this.Listener != null)
                this.Listener.Dispose();
        }

        [TestMethod]
        public void TestSocketSender()
        {
            SocketSender _sender = new SocketSender(this.ServerHost, this.ServerPort, this.SocketTimeout);
            var _data = _sender.Send(Encoding.Default.GetBytes(this.Send));

            if (_data != null && _data.Length > 0)
                this.ResponseReceived = Encoding.Default.GetString(_data);
            bool _result = ManualResetEvent.WaitOne(this.SocketTimeout);
            _sender = null;

            Assert.AreEqual(true, _result, "Listener接收socket超时");
            Assert.AreEqual(this.Send, this.SendReceived, string.Format("Client发送内容：{0}，Listener监听到内容：{1}", this.Send, this.SendReceived));
            Assert.AreEqual(this.Response, this.ResponseReceived, string.Format("Listener响应内容：{0}，Client接收到响应：{1}", this.Response, this.ResponseReceived));
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket _listener = (Socket)ar.AsyncState;
            Socket _client = _listener.EndAccept(ar);
            this.BeginRead(_client);
        }

        private void BeginRead(Socket socket)
        {
            ManualResetEvent.Reset();

            byte[] _buffer = new byte[1024];
            int _bytesRead = 0;

            IAsyncResult _ar = socket.BeginReceive(_buffer, 0, 1024, SocketFlags.None, null, null);
            bool _result = _ar.AsyncWaitHandle.WaitOne(this.SocketTimeout);
            if (_result)
                _bytesRead = socket.EndReceive(_ar);
            if (_bytesRead > 0)
            {
                byte[] _data = new byte[_bytesRead];
                Array.Copy(_buffer, 0, _data, 0, _bytesRead);
                this.SendReceived = Encoding.Default.GetString(_data);

                socket.Send(Encoding.Default.GetBytes(this.Response));
            }

            ManualResetEvent.Set();
        }
    }
}
