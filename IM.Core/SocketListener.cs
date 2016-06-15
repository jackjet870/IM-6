using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core
{
    public class SocketListener
    {
        protected Socket WorkSocket = null;
        protected string Host = string.Empty;
        protected int Port = -1;
        protected int SocketTimeout = 3000;
        protected bool IsListening = false;
        protected ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        public event EventHandler<SocketAcceptedEventArgs> SocketAccepted;
        public event EventHandler<SocketReceiveCompletedEventArgs> SocketReceiveCompleted;

        public SocketListener(string host, int port, int socketTimeout = 3000)
        {
            this.Host = host;
            this.Port = port;
            this.SocketTimeout = socketTimeout;
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.WorkSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.WorkSocket.Bind(new IPEndPoint(IPAddress.Parse(this.Host), this.Port));
                    this.WorkSocket.Listen(100);
                    this.IsListening = true;

                    while (this.IsListening)
                    {
                        manualResetEvent.Reset();
                        this.WorkSocket.BeginAccept(new AsyncCallback(this.AcceptCallback), this.WorkSocket);
                        manualResetEvent.WaitOne();
                    }
                }
                catch
                {
                    throw;
                }
            });
        }

        public void Stop()
        {
            this.IsListening = false;
            if (this.WorkSocket != null)
            {
                this.WorkSocket.Close();
                this.WorkSocket.Dispose();
            }
        }

        protected void AcceptCallback(IAsyncResult ar)
        {
            manualResetEvent.Set();
            if (!this.IsListening) return;

            Socket _listener = (Socket)ar.AsyncState;
            Socket _socket = _listener.EndAccept(ar);

            //通知注册SocketAccepted事件的客户
            this.OnSocketAccepted(this, new SocketAcceptedEventArgs
            {
                SocketAccepted = _socket,
            });
            this.Receive(_socket);
        }

        public virtual void Receive(Socket socket)
        {
            this.BeginRead(socket);
        }

        protected void BeginRead(Socket socket, ReceivedSocketState state = null)
        {
            if (state == null)
            {
                state = new ReceivedSocketState();
                state.WorkSocket = socket;
            }
            socket.BeginReceive(state.Buffer, 0, ReceivedSocketState.BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReadCallback), state);
        }

        protected void ReadCallback(IAsyncResult ar)
        {
            if (ar.AsyncState == null) return;

            ReceivedSocketState _state = (ReceivedSocketState)ar.AsyncState;
            int _bytesRead = _state.WorkSocket.EndReceive(ar);
            if (_bytesRead > 0)
            {
                _state.BytesReceived += _bytesRead;
                _state.DataReceived = _state.DataReceived.Concat(_state.Buffer.Take(_bytesRead)).ToArray();
                if (_state.WorkSocket.Available > 0)
                    this.BeginRead(_state.WorkSocket, _state);
                else
                    this.OnSocketReceiveCompleted(this, new SocketReceiveCompletedEventArgs
                    {
                        ReceivedSocketState = _state,
                    });
            }
        }

        protected void OnSocketReceiveCompleted(object sender, SocketReceiveCompletedEventArgs e)
        {
            if (this.SocketReceiveCompleted != null)
            {
                this.SocketReceiveCompleted(sender, e);
            }
        }

        protected void OnSocketAccepted(object sender, SocketAcceptedEventArgs e)
        {
            if (this.SocketAccepted != null)
            {
                this.SocketAccepted(sender, e);
            }
        }
    }
}
