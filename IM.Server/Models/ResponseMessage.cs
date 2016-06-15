using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Server.Models
{
    public class ResponseMessage
    {        
        public const string COMM_NETWORK_ERROR = "网络异常，请稍后重试！";
        public const string COMM_NOTSUPPORT_FUNCTION = "暂不支持该功能！";
        public const string COMM_INNER_ERROR = "内部错误，请稍后重试！";        

        public const string REGISTER_EXISTS_SAMEACCOUNT = "该账户已存在！";
        public const string REGISTER_SUCCESS = "注册成功！";

        public const string LOGIN_INCORRECT = "账户或密码不对！";
        public const string LOGIN_SUCCESS = "登陆成功！";
    }
}
