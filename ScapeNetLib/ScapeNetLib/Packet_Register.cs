using System;
using System.Collections.Generic;

/// <summary>
/// Stores a list of all types of packets. That the server accepts.
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
        public Dictionary<string, Func<object, bool>> packetRecivedRegister;

        public Packet_Register()
        {
            packetTypes = new Dictionary<string, Type>();
            packetRecivedRegister = new Dictionary<string, Func<object, bool>>();


            packetTypes.Add("Test", typeof(TestPacket));
        }

  




    }
}
