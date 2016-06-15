using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Data
{
    public class Account : EntityBase
    {
        public string UserId { get; private set; }
        public string LoginName { get; private set; }
        public string DisplayName { get; set; }
        public string LoginPwd { get; private set; }

        public Account(string loginName, string loginPwd, string displayName = "新用户")
        {
            this.UserId = Guid.NewGuid().ToString();
            this.LoginName = loginName;
            this.DisplayName = displayName;
            this.LoginPwd = loginPwd;
        }

        public bool CheckLoginPwd(string loginPwd)
        {
            return this.LoginPwd == loginPwd;
        }
    }
}
