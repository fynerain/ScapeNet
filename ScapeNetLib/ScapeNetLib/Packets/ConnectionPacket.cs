using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib
{
    public class ConnectionPacket : Packet<ConnectionPacket>
    {
        public int player_id;
        public NetConnection senderConnection;

        public ConnectionPacket(string packet_name) : base(packet_name){}

        public override ConnectionPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            ConnectionPacket packet = new ConnectionPacket(packet_name);
            packet.player_id = msg.ReadInt32();
            senderConnection = msg.SenderConnection;

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg, ConnectionPacket packet)
        {
            msg.Write(player_id);
            return msg;
        }
       
    }
}
