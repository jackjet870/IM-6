using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    [Serializable]
    public class LoginRequest : RequestPacket
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string LoginPwd { get; set; }

        public override short GetMsgId()
        {
            return (short)PacketType.Login;
        }

        public override string ToString()
        {
            return string.Format("LoginName:{0},LoginPwd:{1}", LoginName, LoginPwd);
        }
    }
}
