using System;
using System.Collections.Generic;
using ScapeNetLib.Packets;
using ScapeNetLib.Packets.MicroData;

/// <summary>
/// Stores a list of all types of packets that the client/server can use.
/// </summary>
namespace ScapeNetLib
{
    internal class Packet_Register
    {
        private static Packet_Register instance;

        public static Packet_Register Instance{
            get
            {
                if (instance == null)
                    instance = new Packet_Register();

                return instance;
            }
        }


        public Dictionary<string, Type> packetTypes;
        public Dictionary<string, Func<object[], bool>> clientPacketReceivedRegister;
        public Dictionary<string, Func<object[], bool>> serverPacketReceivedRegister;

        public Packet_Register()
        {
            packetTypes = new Dictionary<string, Type>();
            clientPacketReceivedRegister = new Dictionary<string, Func<object[], bool>>();
            serverPacketReceivedRegister = new Dictionary<string, Func<object[], bool>>();

            // Default packet types.
            AddToPacketRegister(typeof(TestPacket));
            AddToPacketRegister(typeof(ConnectionPacket));
            AddToPacketRegister(typeof(OnConnectPacket));
            AddToPacketRegister(typeof(InstantiationPacket));
            AddToPacketRegister(typeof(DeletePacket));
            AddToPacketRegister(typeof(PositionRotation));
            
            AddToPacketRegister(typeof(MDFPacket));
            AddToPacketRegister(typeof(MDIPacket));
            AddToPacketRegister(typeof(MDSPacket));
        }

        public void AddToPacketRegister(Type type)
        {
            packetTypes.Add(type.Name, type);
        }
    }
}
