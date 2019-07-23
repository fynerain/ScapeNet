using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScapeNetLib;
using ScapeNetLib.Networkers;
using ScapeNetLib.Packets;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Networker_Client_Unity cli = new Networker_Client_Unity();
            cli.Setup("MS");
            cli.StartClient("localhost", 7777, "secret");

            //ScapeNet.AddPacketType(typeof(TestPacket));

            cli.OnReceive("D_Test", packetObj => {
                PacketData<TestPacket> test = new PacketData<TestPacket>(packetObj);
                Console.WriteLine("Received: ");

                return true;
            });


            while (true)
            {
                cli.Update();
            }


        }
    }
}
