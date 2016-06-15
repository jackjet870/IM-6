using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core
{
    public class SocketSender
    {
        protected Socket WorkSocket = null;
        protected string ServerHost = string.Empty;
        protected int ServerPort = -1;
        protected int SocketTimeout = 3000;
        protected ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        public SocketSender(string serverHost, int serverPort, int socketTimeout = 3000)
        {
            this.ServerHost = serverHost;
            this.ServerPort = serverPort;
            this.SocketTimeout = socketTimeout;
        }

        protected void CreateSocket()
        {
            try
            {
                this.WorkSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult _ar = this.WorkSocket.BeginConnect(this.ServerHost, this.ServerPort, null, null);
                if (!_ar.AsyncWaitHandle.WaitOne(this.SocketTimeout))
                    throw new TimeoutException(string.Format("连接{0}:{1}超时", this.ServerHost, this.ServerPort));
            }
            catch
            {
                throw;
            }
        }

        protected void ReleaseSocket()
        {
            if (this.WorkSocket != null)
            {
                if (this.WorkSocket.Connected)
                {
                    this.WorkSocket.Shutdown(SocketShutdown.Both);
                    this.WorkSocket.Close();
                }
                this.WorkSocket.Dispose();
            }
        }

        public byte[] Send(byte[] data)
        {
            byte[] _data = null;
            try
            {
                this.CreateSocket();
                IAsyncResult _sendAr = this.WorkSocket.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
                if (!_sendAr.AsyncWaitHandle.WaitOne(this.SocketTimeout))
                    throw new TimeoutException(string.Format("发送数据包到{0}超时", this.WorkSocket.RemoteEndPoint.ToString()));

                ManualResetEvent.Reset();
                ReceivedSocketState _state = new ReceivedSocketState
                {
                    WorkSocket = this.WorkSocket,
                };
                this.BeginRead(this.WorkSocket, _state);
                if (ManualResetEvent.WaitOne(this.SocketTimeout))
                {
                    if (_state != null)
                        _data = _state.DataReceived;
                }
                else
                    throw new TimeoutException(string.Format("接收来自{0}的数据包超时", this.WorkSocket.RemoteEndPoint.ToString()));
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_data == null) _data = new byte[0];
                this.ReleaseSocket();
            }
            return _data;
        }

        protected void BeginRead(Socket socket, ReceivedSocketState state)
        {
            if (state == null)
                throw new NullReferenceException("ReceivedSocketState为空");

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
                    ManualResetEvent.Set();
            }
        }
    }
}
