using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    public class NotSupportedPacketTypeException : NotSupportedException
    {
        public NotSupportedPacketTypeException(string message)
            : base(message)
        { }

        public NotSupportedPacketTypeException(string message, Type packetType)
            : base(message)
        {
            this.NotSupportedPacketType = packetType;
        }

        public NotSupportedPacketTypeException(string message, Type packetType, Exception innerException)
            : base(message, innerException)
        {
            this.NotSupportedPacketType = packetType;
        }

        public Type NotSupportedPacketType { get; private set; }
    }
}
