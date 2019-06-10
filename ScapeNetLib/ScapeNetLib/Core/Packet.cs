using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

/// <summary>
/// Definition of a packet, as well as accompanying methods for opening, and packing them. 
/// </summary>
namespace ScapeNetLib
{
    public class Packet<T>
    {
        protected string packet_name;

        public Packet(string packet_name)
        {
            this.packet_name = packet_name;
        }

        public string Get_PacketName()
        {
            return packet_name;
        }

        public virtual T OpenPacketFromMessage(NetIncomingMessage msg) { return default(T); }
        public virtual NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg, T packet) { return null; }

        public NetOutgoingMessage RepackPacket(NetIncomingMessage msg, NetOutgoingMessage msgO)
        {
            T t = OpenPacketFromMessage(msg);
            return PackPacketIntoMessage(msgO, t);
        }

        public NetOutgoingMessage AddDefaultInformationToPacket(NetOutgoingMessage msg, string packet_name)
        {
            msg.Write(packet_name);
            return msg;
        }

        public NetOutgoingMessage AddDefaultInformationToPacket(NetOutgoingMessage msg, string packet_name, int player_id)
        {
            msg.Write(packet_name);
            msg.Write(player_id);
            return msg;
        }
    }
}
