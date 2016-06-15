using IM.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Client.Presenters
{
    public class LoginSubmitEventArgs : EventArgs
    {
        public LoginSubmitEventArgs()
        {
            this.LoginTime = DateTime.Now;
        }

        public string LoginName { get; set; }
        public string LoginPwd { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
