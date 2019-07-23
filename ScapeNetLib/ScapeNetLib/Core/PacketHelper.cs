using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib
{
    internal static class PacketHelper
    {     
        public static NetOutgoingMessage AddDefaultInformationToPacket(NetOutgoingMessage msg, string packet_name)
        {
            msg.Write(packet_name);
            return msg;
        }

        public static NetOutgoingMessage AddDefaultInformationToPacketWithId(NetOutgoingMessage msg, string packet_name, int player_id)
        {
            msg.Write(packet_name);
            msg.Write(player_id);
            return msg;
        }
    }
}
