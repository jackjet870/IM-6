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
    public class RequestHandlerEngine
    {
        static RequestHandlerEngine() { }

        public static RequestHandlerEngine Default = new RequestHandlerEngine();

        public void Start()
        {
            try
            {
                SocketServer.Default.SocketReceived += RequestHandlerEngine_SocketReceived;
                SocketServer.Default.SocketReceiveCompleted += RequestHandlerEngine_SocketReceiveCompleted;
            }
            catch (Exception ex)
            {
                ex.Source += ".RequestHandlerEngine.Start";
                Logger.LogError(string.Format("请求处理引擎启动失败，异常信息 = {0}", ex));
            }
        }

        public void Stop()
        {
            try
            {
                SocketServer.Default.SocketReceived -= RequestHandlerEngine_SocketReceived;
                SocketServer.Default.SocketReceiveCompleted -= RequestHandlerEngine_SocketReceiveCompleted;
            }
            catch (Exception ex)
            {
                ex.Source += ".RequestHandlerEngine.Stop";
                Logger.LogError(string.Format("请求处理引擎停止出现异常，异常信息 = {0}", ex));
            }
        }

        private void RequestHandlerEngine_SocketReceived(object sender, SocketAcceptedEventArgs e)
        {
            //if (e.SocketAccepted != null && e.SocketAccepted.RemoteEndPoint != null)
            //    Logger.LogInfo(string.Format("收到来自{0}的连接请求", e.SocketAccepted.RemoteEndPoint.ToString()));
        }

        private void RequestHandlerEngine_SocketReceiveCompleted(object sender, SocketReceiveCompletedEventArgs e)
        {
            if (e == null || (e != null && e.ReceivedSocketState == null)) return;
            if (e.ReceivedSocketState.DataReceived == null) return;
            var _bytes = e.ReceivedSocketState.DataReceived;

            try
            {
                PacketBodyBase _packetBody = PacketUtil.UnPackRequest(_bytes);
                if (_packetBody == null)
                    throw new UnPackException("解包后的数据包为空", null);

                var _packetType = (PacketType)Enum.Parse(typeof(PacketType), _packetBody.GetMsgId().ToString());
                IRequestHandler _handler = null;

                if (typeof(RequestPacket).IsAssignableFrom(_packetBody.GetType()))
                {
                    switch (_packetType)
                    {
                        case PacketType.Register:
                            _handler = new RegisterHandler(); break;
                        case PacketType.Login:
                            _handler = new LoginHandler(); break;
                        default:
                            throw new NotSupportedPacketTypeException(string.Format("暂不支持类型为{0}的数据包", _packetBody.GetType().ToString()), _packetBody.GetType());
                    }
                }
                else
                    throw new NotSupportedPacketTypeException(string.Format("暂不支持类型为{0}的数据包", _packetBody.GetType().ToString()), _packetBody.GetType());

                _handler.Execute(_packetBody, e.ReceivedSocketState);
            }
            catch (ResponseException ex)
            {
                ex.Source += ".RequestHandlerEngine.RequestHandlerEngine_SocketReceiveCompleted";
                Logger.LogError(string.Format("处理来自{0}的请求后，发回响应包时出现异常，异常信息 = {1}", e.ReceivedSocketState.ClientIpAddress, ex));
            }
            catch (NotSupportedPacketTypeException ex)
            {
                var _response = new CommErrorResponse
                {
                    Status = false,
                    Message = ResponseMessage.COMM_NOTSUPPORT_FUNCTION,
                };
                e.ReceivedSocketState.Response(PacketUtil.Pack(_response));

                ex.Source += ".RequestHandlerEngine.RequestHandlerEngine_SocketReceiveCompleted";
                Logger.LogError(string.Format("处理来自{0}的请求时，遇到未支持功能，异常信息 = {1}", e.ReceivedSocketState.ClientIpAddress, ex));
            }
            catch (UnPackException ex)
            {
                var _response = new CommErrorResponse
                {
                    Status = false,
                    Message = ResponseMessage.COMM_NETWORK_ERROR,
                };
                e.ReceivedSocketState.Response(PacketUtil.Pack(_response));

                ex.Source += ".RequestHandlerEngine.RequestHandlerEngine_SocketReceiveCompleted";
                Logger.LogError(string.Format("处理来自{0}的请求时，解包失败，异常信息 = {1}", e.ReceivedSocketState.ClientIpAddress, ex));
            }
            catch (PackException ex)
            {
                var _response = new CommErrorResponse
                {
                    Status = false,
                    Message = ResponseMessage.COMM_NETWORK_ERROR,
                };
                e.ReceivedSocketState.Response(PacketUtil.Pack(_response));

                ex.Source += ".RequestHandlerEngine.RequestHandlerEngine_SocketReceiveCompleted";
                Logger.LogError(string.Format("处理来自{0}的请求时，打包失败，异常信息 = {1}", e.ReceivedSocketState.ClientIpAddress, ex));
            }
            catch (Exception ex)
            {
                ex.Source += ".RequestHandlerEngine.RequestHandlerEngine_SocketReceiveCompleted";
                Logger.LogError(string.Format("处理来自{0}的请求时，出现异常，异常信息 = {1}", e.ReceivedSocketState.ClientIpAddress, ex));
            }
        }
    }
}
