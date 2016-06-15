using IM.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Client.Models;

namespace IM.Client.Presenters
{
    public class LoginPresenter
    {
        public ILoginView View { get; private set; }
        public LoginHandler Handler { get; private set; }

        public LoginPresenter(ILoginView view)
        {
            this.View = view;
            this.Handler = new LoginHandler();
            this.View.LoginSubmited += OnLoginSubmited;
        }

        protected void OnLoginSubmited(object sender, LoginSubmitEventArgs e)
        {
            try
            {
                var _response = this.Handler.Login(e.LoginName, e.LoginPwd);
                this.View.HandleLoginResponse(_response);
            }
            catch (Exception ex)
            {
                this.View.HandleLoginResponse(new LoginResponse
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
    }
}
