using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    public class UnPackException : Exception
    {
        public UnPackException(string message)
            : base(message)
        { }

        public UnPackException(string message, byte[] data)
            : base(message)
        {
            this.Data = data;
        }

        public UnPackException(string message, byte[] data, Exception innerException)
            : base(message, innerException)
        {
            this.Data = data;
        }

        public byte[] Data { get; private set; }
    }
}
