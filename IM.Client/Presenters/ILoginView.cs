using IM.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Client.Presenters
{
    public interface ILoginView
    {
        event EventHandler<LoginSubmitEventArgs> LoginSubmited;
        void HandleLoginResponse(LoginResponse response);
    }
}
