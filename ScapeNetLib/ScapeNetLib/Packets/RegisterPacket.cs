using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib
{
    public class RegisterPacket : Packet<RegisterPacket>
    {
        public string obj_name;
        public int item_net_id;
        public float x;
        public float y;
        public float z;

        public RegisterPacket(string packet_name) : base(packet_name){}

        public override RegisterPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            RegisterPacket packet = new RegisterPacket(packet_name);

            packet.obj_name = msg.ReadString();
            packet.item_net_id = msg.ReadInt32();
            packet.x = msg.ReadFloat();
            packet.y = msg.ReadFloat();
            packet.z = msg.ReadFloat();
            

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg, RegisterPacket packet)
        {
            msg.Write(packet_name);
            msg.Write(packet.testInt);
            return msg;
        }
       
    }
}
