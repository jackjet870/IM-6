using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IM.Common
{
    public class SecurityHelper
    {
        private static string RsaPrivateKey = string.Empty;
        private static string RsaPublicKey = string.Empty;

        /// <summary>
        /// 获取RSA公钥（XML格式）
        /// </summary>
        /// <returns></returns>
        public static string GetRsaPublicKey()
        {
            if (string.IsNullOrEmpty(RsaPublicKey))
                GenerateRsaKeyPair();

            return RsaPublicKey;
        }

        /// <summary>
        /// 获取MD5
        /// </summary>
        /// <param name="data">待加密字节数组</param>
        /// <returns></returns>
        public static string GetMD5(byte[] data)
        {
            if (data == null)
                return string.Empty;

            MD5 _md5 = MD5CryptoServiceProvider.Create();
            return Convert.ToBase64String(_md5.ComputeHash(data));
        }

        /// <summary>
        /// 使用RSA加密数据
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="data">待加密字节数组</param>
        /// <returns></returns>
        public static byte[] RsaEncrypt(string publicKey, byte[] data)
        {
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException("rsaPublicKey", "密钥为空");
            if (data == null || (data != null && data.Length == 0))
                throw new ArgumentNullException("data", "没有待加密的数据");

            try
            {
                RSACryptoServiceProvider _rsa = new RSACryptoServiceProvider();
                _rsa.FromXmlString(RsaPublicKey);

                return _rsa.Encrypt(data, true);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 使用RSA解密数据
        /// </summary>
        /// <param name="data">待解密数组</param>
        /// <returns></returns>
        public static byte[] RsaDecrypt(byte[] data)
        {
            if (string.IsNullOrEmpty(RsaPrivateKey))
                GenerateRsaKeyPair();
            if (data == null || (data != null && data.Length == 0))
                throw new ArgumentNullException("data", "没有待解密的数据");

            try
            {
                RSACryptoServiceProvider _rsa = new RSACryptoServiceProvider();
                _rsa.FromXmlString(RsaPrivateKey);

                return _rsa.Decrypt(data, true);
            }
            catch
            {
                throw;
            }
        }

        private static void GenerateRsaKeyPair()
        {
            RSACryptoServiceProvider _rsa = new RSACryptoServiceProvider();
            RsaPrivateKey = _rsa.ToXmlString(true);
            RsaPublicKey = _rsa.ToXmlString(false);
            _rsa.Clear();
        }
    }
}
