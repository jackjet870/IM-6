using IM.Common;
using IM.Core;
using IM.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Server.Models
{
    public class LoginHandler : IRequestHandler
    {
        public void Execute(PacketBodyBase requestPacketBody, ReceivedSocketState receivedSocketState)
        {
            LoginResponse _response = null;
            string _exception = string.Empty;

            try
            {
                var _request = (LoginRequest)requestPacketBody;
                _response = this.CheckLogin(_request);
            }
            catch (Exception ex)
            {
                _response = new LoginResponse
                 {
                     Status = false,
                     Message = ResponseMessage.COMM_NETWORK_ERROR,
                 };
                _exception = ex.ToString();
            }

            try
            {
                receivedSocketState.Response(PacketUtil.Pack(_response));
                Logger.LogInfo(string.Format("收到来自{0}的请求，Request = {1}，Response = {2}，Exception = {3}"
                    , receivedSocketState.ClientIpAddress, requestPacketBody.ToString(), _response.ToString(), _exception));
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex.ToString());
            }
        }

        private LoginResponse CheckLogin(LoginRequest request)
        {
            LoginResponse _response = new LoginResponse { Status = false };

            if (request != null && request.LoginName == "chenyongbin"
                && request.LoginPwd == SecurityHelper.GetMD5(SerializationHelper.Encode("123456")))
            {
                _response.Status = true;
                _response.Message = ResponseMessage.LOGIN_SUCCESS;
            }
            else
                _response.Message = ResponseMessage.LOGIN_INCORRECT;

            return _response;
        }
    }
}
