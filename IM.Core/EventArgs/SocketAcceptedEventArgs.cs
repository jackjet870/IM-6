using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace IM.Core
{
    /// <summary>
    /// socket监听器收到一个客户端的socket连接请求
    /// </summary>
    public class SocketAcceptedEventArgs
    {
        /// <summary>
        /// 收到的socket连接
        /// </summary>
        public Socket SocketAccepted { get; set; }
    }
}
