using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Common
{
    public class CompressHelper
    {
        /// <summary>
        /// 压缩字节流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            if (data == null || (data != null && data.Length == 0))
                return new byte[0];

            using (var _src = new MemoryStream(data))
            {
                using (var _stream = new MemoryStream())
                {
                    using (GZipStream _dest = new GZipStream(_stream, CompressionMode.Compress))
                    {
                        CopyTo(_src, _dest);
                    }
                    return _stream.ToArray();
                }
            }
        }

        /// <summary>
        /// 解压字节流
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            if (data == null || (data != null && data.Length == 0))
                return new byte[0];

            using (var _dest = new MemoryStream())
            {
                using (var _stream = new MemoryStream(data))
                {
                    using (GZipStream _src = new GZipStream(_stream, CompressionMode.Decompress, true))
                    {
                        CopyTo(_src, _dest);
                    }                    
                }
                return _dest.ToArray();
            }
        }

        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] _buffer = new byte[4096];
            int _bytesRead = 0;
            while ((_bytesRead = src.Read(_buffer, 0, _buffer.Length)) != 0)
                dest.Write(_buffer, 0, _bytesRead);
        }
    }
}
