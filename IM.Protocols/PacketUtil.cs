using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Common;
using System.Runtime.Serialization;

namespace IM.Protocols
{
    public class PacketUtil
    {
        public static byte[] Pack(PacketBodyBase packetBody, bool compressed = true)
        {
            try
            {
                byte[] _head, _compress, _body;
                string _md5;

                //编码包体            
                _body = SerializationHelper.Encode(packetBody);
                _md5 = SecurityHelper.GetMD5(_body);

                //编码压缩包头
                CompressHeader _compressHeader = new CompressHeader();
                _compressHeader.Compressed = (short)(compressed ? 1 : 0);
                _compressHeader.DataSize = _body.Length;
                if (compressed)
                    _body = CompressHelper.Compress(_body);
                _compressHeader.CompressedSize = _body.Length;
                _compress = SerializationHelper.Encode(_compressHeader);
                _body = _compress.Concat(_body).ToArray();

                //编码数据包包头
                if (typeof(RequestPacket).IsAssignableFrom(packetBody.GetType()))
                {
                    RequestHeader _requestHeader = new RequestHeader
                    {
                        MsgId = packetBody.GetMsgId(),
                        PkgSize = _body.Length,
                        MD5 = _md5,
                    };
                    _head = SerializationHelper.Encode(_requestHeader);
                }
                else if (typeof(ResponsePacket).IsAssignableFrom(packetBody.GetType()))
                {
                    ResponseHeader _responseHeader = new ResponseHeader
                    {
                        MsgId = packetBody.GetMsgId(),
                        PkgSize = _body.Length,
                        MD5 = _md5,
                    };
                    _head = SerializationHelper.Encode(_responseHeader);
                }
                else
                    throw new NotSupportedPacketTypeException(string.Format("暂不支持类型为{0}的数据包", packetBody.GetType().ToString()), packetBody.GetType());

                return _head.Concat(_body).ToArray();
            }
            catch (NotSupportedPacketTypeException ex)
            {
                ex.Source += ".PacketUtil.Pack";
                throw;
            }
            catch (Exception ex)
            {
                ex.Source += ".PacketUtil.Pack";
                throw new PackException("数据包打包出现异常", packetBody, ex);
            }
        }

        public static PacketBodyBase UnPackRequest(byte[] data)
        {
            return UnPack<RequestPacket>(data);
        }

        public static PacketBodyBase UnPackResponse(byte[] data)
        {
            return UnPack<ResponsePacket>(data);
        }

        private static TResult UnPack<TResult>(byte[] data) where TResult : PacketBodyBase
        {
            try
            {
                if (data == null || (data != null && data.Length == 0))
                    throw new UnPackException("数据包为空");

                var _type = typeof(TResult);
                int _msgId = -1;
                string _md5 = "";

                //解码数据包头部
                if (_type == typeof(RequestPacket))
                {
                    if (data.Length < RequestHeader.LENGTH)
                        throw new UnPackException("请求数据包数据不全", data);

                    var _request = data.Take(RequestHeader.LENGTH);
                    var _requestHeader = SerializationHelper.Decode<RequestHeader>(_request.ToArray());
                    if ((data.Length - RequestHeader.LENGTH) < _requestHeader.PkgSize)
                        throw new UnPackException("请求数据包包体不全");
                    data = data.Skip(RequestHeader.LENGTH).Take(_requestHeader.PkgSize).ToArray();
                    _msgId = _requestHeader.MsgId;
                    _md5 = _requestHeader.MD5;
                }
                else if (_type == typeof(ResponsePacket))
                {
                    if (data.Length < ResponseHeader.LENGTH)
                        throw new UnPackException("响应数据包数据不全", data);

                    var _response = data.Take(ResponseHeader.LENGTH);
                    var _responseHeader = SerializationHelper.Decode<ResponseHeader>(_response.ToArray());
                    if ((data.Length - ResponseHeader.LENGTH) < _responseHeader.PkgSize)
                        throw new UnPackException("响应数据包包体不全");
                    data = data.Skip(ResponseHeader.LENGTH).Take(_responseHeader.PkgSize).ToArray();
                    _msgId = _responseHeader.MsgId;
                    _md5 = _responseHeader.MD5;
                }
                else
                    throw new NotSupportedPacketTypeException(string.Format("暂不支持类型为{0}的数据包", _type.ToString()), _type);

                //解码数据包压缩头部
                var _compress = data.Take(CompressHeader.LENGTH);
                var _compressHeader = SerializationHelper.Decode<CompressHeader>(_compress.ToArray());
                if ((data.Length - CompressHeader.LENGTH) < _compressHeader.CompressedSize)
                    throw new UnPackException("数据包压缩包体不全");
                data = data.Skip(CompressHeader.LENGTH).Take(_compressHeader.CompressedSize).ToArray();
                if (_compressHeader.Compressed == 1)
                    data = CompressHelper.Decompress(data);

                //验证接收到的数据是否完整
                var _newMd5 = SecurityHelper.GetMD5(data);
                if (_md5 != _newMd5)
                    throw new UnPackException("MD5校验失败，接收到的数据包包体不完整");

                return (TResult)GetPacketBody(_type, _msgId, data);
            }
            catch (Exception ex)
            {
                ex.Source += ".PacketUtil.UnPack";
                throw new UnPackException("数据包解包出现异常", data, ex);
            }
        }

        private static PacketBodyBase GetPacketBody(Type packetBodyType, int msgId, byte[] data)
        {
            PacketBodyBase _packetBody = null;
            try
            {
                PacketType _packetType = (PacketType)Enum.Parse(typeof(PacketType), msgId.ToString());
                if (typeof(RequestPacket).IsAssignableFrom(packetBodyType))
                {
                    switch (_packetType)
                    {
                        case PacketType.Register:
                            _packetBody = SerializationHelper.Decode<RegisterResponse>(data); break;
                        case PacketType.Login:
                            _packetBody = SerializationHelper.Decode<LoginRequest>(data); break;
                        default:
                            throw new NotSupportedPacketTypeException(string.Format("暂不支持类型为{0}的数据包", _packetType), packetBodyType);
                    }
                }
                else if (typeof(ResponsePacket).IsAssignableFrom(packetBodyType))
                {
                    switch (_packetType)
                    {
                        case PacketType.Register:
                            _packetBody = SerializationHelper.Decode<RegisterResponse>(data); break;
                        case PacketType.Login:
                            _packetBody = SerializationHelper.Decode<LoginResponse>(data); break;
                        default:
                            throw new NotSupportedPacketTypeException(string.Format("暂不支持类型为{0}的数据包", _packetType), packetBodyType);
                    }
                }
            }
            catch
            {
                throw;
            }

            return _packetBody;
        }
    }
}
