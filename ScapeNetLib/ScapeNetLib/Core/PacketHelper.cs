using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

/// <summary>
/// Used by Networkers to quickly add the correct information to packets before sending it off.
/// </summary>
namespace ScapeNetLib
{
    internal static class PacketHelper
    {     
        public static NetOutgoingMessage AddDefaultInformationToPacket(NetOutgoingMessage msg, string packet_name, string packet_identifier)
        {
            msg.Write(packet_name);
            msg.Write(packet_identifier);
            return msg;
        }

        public static NetOutgoingMessage AddDefaultInformationToPacketWithId(NetOutgoingMessage msg, string packet_name, string packet_identifier, int player_id)
        {
            msg = AddDefaultInformationToPacket(msg, packet_name, packet_identifier);
            msg.Write(player_id);
            return msg;
        }
    }
}
