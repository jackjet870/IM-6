using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IM.Protocols;
using IM.Common;

namespace IM.UnitTest
{
    [TestClass]
    public class PacketTest
    {
        [TestMethod]
        public void TestPacketUtil()
        {
            string _publicKey = SecurityHelper.GetRsaPublicKey();
            LoginRequest _request = new LoginRequest();
            _request.LoginName = "Albin";
            _request.LoginPwd = "123456";

            var _bytes = PacketUtil.Pack(_request);
            var _packetBody = PacketUtil.UnPackRequest(_bytes);

            string _expected = _request.ToString()
                , _actual = _packetBody.ToString();

            Assert.AreEqual(_expected, _actual, string.Format("数据包打包、解包失败，打包前是：{0}，打包、解包后是：{1}", _expected, _actual));
        }
    }
}
