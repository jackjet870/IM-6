using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using IM.Common;

namespace IM.UnitTest
{
    [TestClass]
    public class CompressTest
    {
        [TestMethod]
        public void TestCompress()
        {
            string _testString = "Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!", _decompressString;
            int _rawBytesLength = 0, _compressBytesLength = 0, _decompressBytesLength = 0;

            var _bytes = Encoding.Default.GetBytes(_testString);
            _rawBytesLength = _bytes.Length;

            _bytes = CompressHelper.Compress(_bytes);
            _compressBytesLength = _bytes.Length;

            _bytes = CompressHelper.Decompress(_bytes);
            _decompressBytesLength = _bytes.Length;

            _decompressString = Encoding.Default.GetString(_bytes);

            Assert.IsTrue(_rawBytesLength > _compressBytesLength, string.Format("压缩不成功，压缩前字节数:{0}，压缩后字节数:{1}",_rawBytesLength,_compressBytesLength));
            Assert.AreEqual(_testString, _decompressString, string.Format("压缩、解压失败，压缩前:{0}，压缩解压后:{1}", _testString, _decompressString));
        }
    }
}
