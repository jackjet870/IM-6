using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IM.Common;
using System.Text;

namespace IM.UnitTest
{
    [TestClass]
    public class SecurityTest
    {
        [TestMethod]
        public void TestMD5()
        {            
            string _str = "Hello World!"
                    , _strSame = "Hello World!"
                    , _strNotSame = "Wonderful!";

            _str = SecurityHelper.GetMD5(Encoding.Default.GetBytes(_str));
            _strSame = SecurityHelper.GetMD5(Encoding.Default.GetBytes(_strSame));
            _strNotSame = SecurityHelper.GetMD5(Encoding.Default.GetBytes(_strNotSame));

            Assert.AreEqual(_str, _strSame, string.Format("预期：{0}，实际：{1}", _str, _strSame));
            Assert.AreNotEqual(_str, _strNotSame, string.Format("预期：{0}，实际：{1}", _str, _strNotSame));
        }

        [TestMethod]
        public void TestRSA()
        {
            string _str = "Hello World!"
                    , _strDecrypted = string.Empty;
            string _publicKey = SecurityHelper.GetRsaPublicKey();

            var _bytes = SecurityHelper.RsaEncrypt(_publicKey, Encoding.Default.GetBytes(_str));
            _bytes = SecurityHelper.RsaDecrypt(_bytes);
            _strDecrypted = Encoding.Default.GetString(_bytes);

            Assert.AreEqual(_str, _strDecrypted, string.Format("原字符串：{0}，加密解密后字符串：{1}", _str, _strDecrypted));
        }
    }
}
