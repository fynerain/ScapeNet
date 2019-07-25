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
    public class MDSPacket : Packet<MDSPacket>
    {
        public string string_value;

        public MDSPacket(string packet_identifier) : base(packet_identifier) { }

        public override MDSPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            MDSPacket packet = new MDSPacket(packet_identifier);
            packet.string_value = msg.ReadString();
            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg, MDSPacket packet)
        {
            msg.Write(packet.string_value);
            return msg;
        }
    }
}
