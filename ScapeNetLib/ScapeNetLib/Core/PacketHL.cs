using System;
using System.Collections.Generic;
using System.Reflection;

using Lidgren.Network;

namespace ScapeNetLib
{
    public class PacketHL<T> : Packet<T> where T : Packet<T>, new()
    {
        public PacketHL(string packet_name) : base(packet_name){}

        public override T OpenPacketFromMessage(NetIncomingMessage msg) 
        {
            T packet = new T();
            packet.Set_PacketName(packet_name);

            var properties = packet.GetType().GetProperties();
            foreach (var p in properties)
            {
                if (p.GetType() == typeof(int))
                   p.SetValue(packet, msg.ReadInt32());
                else if (p.GetType() == typeof(string))
                   p.SetValue(packet, msg.ReadString());
                else if (p.GetType() == typeof(bool))
                   p.SetValue(packet, msg.ReadBoolean());
                else if (p.GetType() == typeof(float))
                   p.SetValue(packet, msg.ReadFloat());
                else
                   p.SetValue(packet, msg.ReadByte());
            }


                return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg, T packet)
        {
            //loop through all properties, and then write each one depending on its type
            var properties = packet.GetType().GetProperties();
            foreach (var p in properties)
            {
                //string name = p.Name;

                if (p.GetType() == typeof(int))
                    msg.Write((int)p.GetValue(packet, null));
                else if (p.GetType() == typeof(string))
                    msg.Write((string)p.GetValue(packet, null));
                else if (p.GetType() == typeof(bool))
                    msg.Write((bool)p.GetValue(packet, null));
                else if (p.GetType() == typeof(float))
                    msg.Write((float)p.GetValue(packet, null));
                else
                    msg.Write((byte)p.GetValue(packet, null));
            }

            return msg;
        }

    }
}
