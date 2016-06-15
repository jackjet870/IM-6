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
    public class RegisterHandler : IRequestHandler
    {
        public void Execute(PacketBodyBase requestPacketBody, ReceivedSocketState receivedSocketState)
        {
            try
            {
                var _request = (RegisterRequest)requestPacketBody;
                var _response = this.CheckRegister(_request);

                Logger.LogInfo(string.Format("收到来自{0}的注册请求，处理结果：{1}", receivedSocketState.ClientIpAddress, _response.Message));
                receivedSocketState.Response(PacketUtil.Pack(_response));
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("收到来自{0}的注册请求，处理失败，错误信息：{1}", receivedSocketState.ClientIpAddress, ex));
                var _response = new RegisterResponse
                {
                    Status = false,
                    Message = "网络异常，请重试！"
                };
                receivedSocketState.Response(PacketUtil.Pack(_response));
            }
        }

        private RegisterResponse CheckRegister(RegisterRequest request)
        {
            return null;
        }
    }
}
