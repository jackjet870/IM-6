using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    /// <summary>
    /// 数据包加密包头格式
    /// </summary>
    [Serializable]
    public class EncryptHeader
    {
        /// <summary>
        /// 压缩前包体长度
        /// </summary>        
        public int DataSize { get; set; }
        /// <summary>
        /// 是否使用压缩，1=已压缩，0=未压缩
        /// </summary>        
        public short Encrypted { get; set; }
        /// <summary>
        /// 压缩后包体长度
        /// </summary>        
        public int EncryptedSize { get; set; }

        public const int LENGTH = 9;
    }
}
