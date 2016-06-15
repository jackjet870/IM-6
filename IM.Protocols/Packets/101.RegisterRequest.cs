using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    [Serializable]
    public class RegisterRequest : RequestPacket
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string LoginPwd { get; set; }

        public override short GetMsgId()
        {
            return (short)PacketType.Register;
        }

        public override string ToString()
        {
            return string.Format("LoginName:{0},DisplayName:{1},LoginPwd:{2}", LoginName, DisplayName, LoginPwd);
        }
    }
}
