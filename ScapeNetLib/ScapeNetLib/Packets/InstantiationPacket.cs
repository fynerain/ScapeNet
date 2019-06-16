using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib
{
    public class InstantiationPacket : Packet<InstantiationPacket>
    {
        public string obj_name;
        public int item_net_id;
        public float x;
        public float y;
        public float z;

        public InstantiationPacket(string packet_name) : base(packet_name){}

        public override InstantiationPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            InstantiationPacket packet = new InstantiationPacket(packet_name);

            packet.obj_name = msg.ReadString();
            packet.item_net_id = msg.ReadInt32();
            packet.x = msg.ReadFloat();
            packet.y = msg.ReadFloat();
            packet.z = msg.ReadFloat();
            

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg,  InstantiationPacket packet)
        {
            msg.Write(packet.obj_name);
            msg.Write(packet.item_net_id);
            msg.Write(packet.x);
            msg.Write(packet.y);
            msg.Write(packet.z);
            return msg;
        }
       
    }
}
