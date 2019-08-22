using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib;
using Lidgren.Network;

public class ServersideSpawnPacket : Packet<ServersideSpawnPacket>
{
        public string obj_name;
        public float x;
        public float y;
        public float z;

        public float rotX;
        public float rotY;
        public float rotZ;

        public ServersideSpawnPacket(string packet_identifier) : base(packet_identifier){}

        public override ServersideSpawnPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            ServersideSpawnPacket packet = new ServersideSpawnPacket(packet_identifier);
            packet.obj_name = msg.ReadString();
            packet.x = msg.ReadFloat();
            packet.y = msg.ReadFloat();
            packet.z = msg.ReadFloat();
            
            packet.rotX = msg.ReadFloat();
            packet.rotY = msg.ReadFloat();
            packet.rotZ = msg.ReadFloat();

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg,  ServersideSpawnPacket packet)
        {
            msg.Write(packet.obj_name);
            msg.Write(packet.x);
            msg.Write(packet.y);
            msg.Write(packet.z);

            msg.Write(packet.rotX);
            msg.Write(packet.rotY);
            msg.Write(packet.rotZ);

            return msg;
        }
}
