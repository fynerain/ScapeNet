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

        public ServersideSpawnPacket(string packet_name) : base(packet_name){}

        public override ServersideSpawnPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            ServersideSpawnPacket packet = new ServersideSpawnPacket(packet_name);
            packet.obj_name = msg.ReadString();
            packet.x = msg.ReadFloat();
            packet.y = msg.ReadFloat();
            packet.z = msg.ReadFloat();

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg,  ServersideSpawnPacket packet)
        {
            msg.Write(packet.obj_name);
            msg.Write(packet.x);
            msg.Write(packet.y);
            msg.Write(packet.z);

            return msg;
        }
}
