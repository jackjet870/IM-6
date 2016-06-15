using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Core
{
    /// <summary>
    /// socket监听器接收客户端发送的消息完毕时
    /// </summary>
    public class SocketReceiveCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// 接收到的数据状态
        /// </summary>
        public ReceivedSocketState ReceivedSocketState { get; set; }
    }
}
