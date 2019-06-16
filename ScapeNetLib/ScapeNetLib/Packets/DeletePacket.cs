using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib
{
    public class DeletePacket : Packet<DeletePacket>
    {
        public int item_net_id;

        public DeletePacket(string packet_name) : base(packet_name){}

        public override DeletePacket OpenPacketFromMessage( NetIncomingMessage msg)
        {
            DeletePacket packet = new DeletePacket(packet_name);
            packet.item_net_id = msg.ReadInt32();

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage( NetOutgoingMessage msg,  DeletePacket packet)
        {
            msg.Write(packet.item_net_id);
            return msg;
        }
       
    }
}
