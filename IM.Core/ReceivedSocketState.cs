using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IM.Core
{
    public class ReceivedSocketState
    {
        public const int BUFFER_SIZE = 1024;

        private string _clientIpAddress = string.Empty;
        public string ClientIpAddress
        {
            get
            {
                if (string.IsNullOrEmpty(this._clientIpAddress))
                {
                    if (this.WorkSocket != null)
                        this._clientIpAddress = this.WorkSocket.RemoteEndPoint.ToString();
                }
                return this._clientIpAddress;
            }
        }
        public Socket WorkSocket { get; set; }
        public byte[] Buffer { get; set; }
        public byte[] DataReceived { get; set; }
        public int BytesReceived { get; set; }

        public ReceivedSocketState()
        {
            this.Buffer = new byte[BUFFER_SIZE];
            this.DataReceived = new byte[0];
        }

        public void Response(byte[] data)
        {
            if (data == null || (data != null && data.Length == 0))
                return;

            try
            {
                if (this.WorkSocket == null)
                    throw new ObjectDisposedException("workSocket", "当前连接已释放");
                if (!this.WorkSocket.Connected)
                    throw new Exception("当前连接已断开");

                this.WorkSocket.Send(data);
            }
            catch
            {
                throw;
            }
        }

        public override string ToString()
        {
            return string.Format("Receive {0}bytes message from {1}, messages are {2}", this.BytesReceived, this.ClientIpAddress, string.Join("-", this.DataReceived));
        }
    }
}
