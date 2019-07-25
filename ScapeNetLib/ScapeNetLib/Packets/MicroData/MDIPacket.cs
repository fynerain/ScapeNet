using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

/// <summary>
/// A small packet for sending just a float.
/// </summary>
namespace ScapeNetLib.Packets.MicroData
{
    public class MDIPacket : Packet<MDIPacket>
    {
        public int int_value;

        public MDIPacket(string packet_identifier) : base(packet_identifier) {}

        public override MDIPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            MDIPacket packet = new MDIPacket(packet_identifier);
            packet.int_value = msg.ReadInt32();
            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg, MDIPacket packet)
        {
            msg.Write(packet.int_value);
            return msg;
        }
    }
}
