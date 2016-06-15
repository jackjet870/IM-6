using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    public abstract class ResponsePacket : PacketBodyBase
    {
        /// <summary>
        /// 唯一值，在网络中用来精确定位
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 请求的处理结果
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }
    }
}
