using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib.Packets
{
    public class InstantiationPacket : Packet<InstantiationPacket>
    {
        public string obj_name;
        public int item_net_id;

        public float x;
        public float y;
        public float z;

        public float rotX;
        public float rotY;
        public float rotZ;

        public InstantiationPacket(string packet_identifier) : base(packet_identifier) {}

        public override InstantiationPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            InstantiationPacket packet = new InstantiationPacket(packet_identifier);

            packet.obj_name = msg.ReadString();
            packet.item_net_id = msg.ReadInt32();
            packet.x = msg.ReadFloat();
            packet.y = msg.ReadFloat();
            packet.z = msg.ReadFloat();

            packet.rotX = msg.ReadFloat();
            packet.rotY = msg.ReadFloat();
            packet.rotZ = msg.ReadFloat();

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg,  InstantiationPacket packet)
        {
            msg.Write(packet.obj_name);
            msg.Write(packet.item_net_id);
            msg.Write(packet.x);
            msg.Write(packet.y);
            msg.Write(packet.z);

            msg.Write(packet.rotX);
            msg.Write(packet.rotY);
            msg.Write(packet.rotZ);

            return msg;
        }
       
    }
}
