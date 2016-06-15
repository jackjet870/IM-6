using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    [Serializable]
    public abstract class PacketBodyBase
    {
        /// <summary>
        /// 数据包目标ID
        /// </summary>
        public string TargetId;

        /// <summary>
        /// 获取功能号
        /// </summary>
        /// <returns></returns>
        public abstract short GetMsgId();
        /// <summary>
        /// 将数据包转换成字符串
        /// </summary>
        /// <returns></returns>
        public abstract string ToString();
    }
}
