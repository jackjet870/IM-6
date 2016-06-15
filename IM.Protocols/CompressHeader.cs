using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    /// <summary>
    /// 数据包压缩包头格式
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CompressHeader
    {
        /// <summary>
        /// 压缩前包体长度
        /// </summary>        
        public Int32 DataSize;
        /// <summary>
        /// 是否使用压缩，1=已压缩，0=未压缩
        /// </summary>        
        public short Compressed;
        /// <summary>
        /// 压缩后包体长度
        /// </summary>        
        public Int32 CompressedSize;

        public const int LENGTH = 10;
    }
}
