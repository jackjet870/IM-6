using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    public class PackException : Exception
    {
        public PackException(string message)
            : base(message)
        { }

        public PackException(string message, PacketBodyBase packetBody)
            : base(message)
        {
            this.PacketBody = packetBody.ToString();
        }

        public PackException(string message, PacketBodyBase packetBody, Exception innerException)
            : base(message, innerException)
        {
            this.PacketBody = packetBody.ToString();
        }

        public string PacketBody { get; private set; }
    }
}
