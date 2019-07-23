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
    public abstract class Packet<T> 
    {
        protected string packet_identifier;

        public Packet(){}

        public Packet(string packet_identifier)
        {
            this.packet_identifier = packet_identifier;
        }

        public void Set_PacketIdentifer(string packet_identifier)
        {
            this.packet_identifier = packet_identifier;
        }

        public string Get_PacketIdentifier()
        {
            return packet_identifier;
        }

        public abstract T OpenPacketFromMessage(NetIncomingMessage msg);
        public abstract NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg, T packet);

        public NetOutgoingMessage RepackPacket(NetIncomingMessage msg,  NetOutgoingMessage msgO)
        {
            T t = OpenPacketFromMessage(msg);
            return PackPacketIntoMessage(msgO,  t);
        }
    }
}
