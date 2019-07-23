using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib.Packets
{
    public class PositionRotation : Packet<PositionRotation>
    {
        public int item_net_id;
        public bool isRotation;
        public float x;
        public float y;
        public float z;

        public PositionRotation(string packet_identifier) : base(packet_identifier) {}

        public override PositionRotation OpenPacketFromMessage(NetIncomingMessage msg)
        {
            PositionRotation packet = new PositionRotation(packet_identifier);

            packet.item_net_id = msg.ReadInt32();
            packet.isRotation = msg.ReadBoolean();
            packet.x = msg.ReadFloat();
            packet.y = msg.ReadFloat();
            packet.z = msg.ReadFloat();
            

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg,  PositionRotation packet)
        {
            msg.Write(packet.item_net_id);
            msg.Write(packet.isRotation);
            msg.Write(packet.x);
            msg.Write(packet.y);
            msg.Write(packet.z);
            return msg;
        }
       
    }
}
