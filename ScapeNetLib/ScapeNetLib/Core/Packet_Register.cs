using System;
using System.Collections.Generic;

/// <summary>
/// Stores a list of all types of packets that the client and server can use.
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

            packetTypes.Add("D_Test", typeof(TestPacket));
            packetTypes.Add("D_Connection", typeof(ConnectionPacket));
            packetTypes.Add("D_Register", typeof(RegisterPacket));
            packetTypes.Add("D_Instantiate", typeof(InstantiationPacket));
            packetTypes.Add("D_Delete", typeof(DeletePacket));
            packetTypes.Add("D_PositionRotation", typeof(PositionRotation));
           
        }

  




    }
}
