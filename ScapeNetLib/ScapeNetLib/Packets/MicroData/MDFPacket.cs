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
    public class MDFPacket : Packet<MDFPacket>
    {
        public float float_value;

        public MDFPacket(string packet_identifier) : base(packet_identifier) {}

        public override MDFPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            MDFPacket packet = new MDFPacket(packet_identifier);
            packet.float_value = msg.ReadFloat();
            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg, MDFPacket packet)
        {
            msg.Write(packet.float_value);
            return msg;
        }
    }
}
