using IM.Common;
using IM.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Client.Models
{
    public class LoginHandler
    {
        public LoginResponse Login(string loginName, string loginPwd)
        {
            var _packetBody = new LoginRequest
            {
                LoginName = loginName,
                LoginPwd = SecurityHelper.GetMD5(SerializationHelper.Encode(loginPwd))
            };
            return SocketClient.Default.Send<LoginResponse>(_packetBody);
        }
    }
}
