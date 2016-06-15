using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace IM.Common
{
    public class SerializationHelper
    {
        private static Encoding CurrentEncoding = Encoding.UTF8;

        public static byte[] Encode(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return new byte[0];

            return CurrentEncoding.GetBytes(msg);
        }

        public static string Decode(byte[] data)
        {
            if (data == null || (data != null && data.Length == 0))
                return string.Empty;

            return CurrentEncoding.GetString(data);
        }

        public static byte[] Encode(object obj)
        {
            if (obj == null)
                return new byte[0];

            if (obj.GetType().IsValueType)
            {
                int _size = Marshal.SizeOf(obj);
                IntPtr _buffer = Marshal.AllocHGlobal(_size);
                Marshal.StructureToPtr(obj, _buffer, true);
                byte[] _bytes = new byte[_size];
                Marshal.Copy(_buffer, _bytes, 0, _size);
                Marshal.FreeHGlobal(_buffer);
                return _bytes;
            }
            else
            {
                BinaryFormatter _bf = new BinaryFormatter();
                using (var _stream = new MemoryStream())
                {
                    _bf.Serialize(_stream, obj);
                    return _stream.ToArray();
                }
            }
        }

        public static T Decode<T>(byte[] data, int position = 0)
        {
            if (data == null || (data != null && data.Length == 0))
                return default(T);

            if (typeof(T).IsValueType)
            {
                int _size = Marshal.SizeOf(typeof(T));
                if (_size > data.Length - position)
                    throw new ArgumentException(string.Format("Not enough data to fill struct. Array length from position: {0}, Struct length: {1}", data.Length - position, _size));
                IntPtr _buffer = Marshal.AllocHGlobal(_size);
                Marshal.Copy(data, position, _buffer, _size);
                T _obj = (T)Marshal.PtrToStructure(_buffer, typeof(T));
                Marshal.FreeHGlobal(_buffer);
                return _obj;
            }
            else
            {
                BinaryFormatter _bf = new BinaryFormatter();
                using (var _stream = new MemoryStream(data))
                {
                    var _obj = _bf.Deserialize(_stream);
                    if (_obj is T)
                        return (T)_obj;
                    else
                        return default(T);
                }
            }
        }
    }
}