using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    public enum PacketType
    {
        /// <summary>
        /// 交换RSA密钥数据包
        /// </summary>
        ExchangeRsaKey = 90,
        /// <summary>
        /// 登陆数据包
        /// </summary>
        Login = 100,
        /// <summary>
        ///注册数据包
        /// </summary>
        Register = 101,
        /// <summary>
        /// 普通文本数据包
        /// </summary>
        Text = 200,
        /// <summary>
        /// 发送文件概要
        /// </summary>
        FileInfo = 300,
        /// <summary>
        /// 文件数据包
        /// </summary>
        File = 400,
        /// <summary>
        /// 通用错误包
        /// </summary>
        CommError = 900,
    }
}
