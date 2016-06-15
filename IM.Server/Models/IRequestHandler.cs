using IM.Core;
using IM.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Server.Models
{
    interface IRequestHandler
    {
        void Execute(PacketBodyBase requestPacketBody,ReceivedSocketState receivedSocketState);
    }
}
