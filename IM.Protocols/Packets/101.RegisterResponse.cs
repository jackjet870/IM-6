﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Protocols
{
    [Serializable]
    public class RegisterResponse : ResponsePacket
    {
        public override short GetMsgId()
        {
            return (short)PacketType.Register;
        }

        public override string ToString()
        {
            return string.Format("UserId:{0},Status:{1},Message:{2}", UserId, Status, Message);
        }
    }
}
