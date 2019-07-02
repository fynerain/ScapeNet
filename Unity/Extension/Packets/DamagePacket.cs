using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib;
using Lidgren.Network;

public class DamagePacket : Packet<DamagePacket>
{
        public int damaged_items_id;
        public float damageDone;

        public DamagePacket(string packet_name) : base(packet_name){}

        public override DamagePacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            DamagePacket packet = new DamagePacket(packet_name);
            packet.damaged_items_id = msg.ReadInt32();
            packet.damageDone = msg.ReadFloat();


            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg,  DamagePacket packet)
        {
            msg.Write(packet.damaged_items_id);
            msg.Write(packet.damageDone);

            
            return msg;
        }
}
