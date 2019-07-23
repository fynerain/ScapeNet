using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib.Packets
{
    public class OnConnectPacket : Packet<OnConnectPacket>
    {
        public int player_id;

        public OnConnectPacket(string packet_identifier) : base(packet_identifier) {}

        public override OnConnectPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            OnConnectPacket packet = new OnConnectPacket(packet_identifier);
            packet.player_id = msg.ReadInt32();

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg, OnConnectPacket packet)
        {
            msg.Write(packet.player_id);
            return msg;
        }
       
    }
}
