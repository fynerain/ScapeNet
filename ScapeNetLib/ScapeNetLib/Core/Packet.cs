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
        protected string packet_name;

        public Packet(string packet_name)
        {
            this.packet_name = packet_name;
        }

        public void Set_PacketName(string packet_name)
        {
            this.packet_name = packet_name;
        }

        public string Get_PacketName()
        {
            return packet_name;
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
