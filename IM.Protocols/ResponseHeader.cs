using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    /// <summary>
    /// 数据包响应包头格式
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ResponseHeader
    {
        /// <summary>
        /// 数据包大小
        /// </summary>        
        public Int32 PkgSize;
        /// <summary>
        /// 数据包功能号
        /// </summary>
        public Int16 MsgId;
        /// <summary>
        /// 包体MD5
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string MD5;

        public const int LENGTH = 38;
    }
}
