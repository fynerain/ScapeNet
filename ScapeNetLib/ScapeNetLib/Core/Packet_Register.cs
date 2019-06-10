﻿using System;
using System.Collections.Generic;

/// <summary>
/// Stores a list of all types of packets that the client and server can use.
/// </summary>
namespace ScapeNetLib
{
    public class Packet_Register
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
        public Dictionary<string, Func<object, bool>> clientPacketRecivedRegister;
        public Dictionary<string, Func<object, bool>> serverPacketRecivedRegister;

        public Packet_Register()
        {
            packetTypes = new Dictionary<string, Type>();
            clientPacketRecivedRegister = new Dictionary<string, Func<object, bool>>();
            serverPacketRecivedRegister = new Dictionary<string, Func<object, bool>>();

            packetTypes.Add("D_Test", typeof(TestPacket));
            packetTypes.Add("D_Register", typeof(RegisterPacket));
            packetTypes.Add("D_Delete", typeof(DeletePacket));
            packetTypes.Add("D_PositionRotation", typeof(PositionRotation));
        }

  




    }
}
