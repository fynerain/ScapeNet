using System;
using System.Collections.Generic;

/// <summary>
/// Stores a list of all types of packets.
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


        //public List<Packet> packetTypes;

        public Dictionary<string, Type> packetTypes;


        public Packet_Register()
        {
            //packetTypes = new List<Packet>();
            packetTypes = new Dictionary<string, Type>();
            packetTypes.Add("Test", typeof(TestPacket));
        }

  




    }
}
