using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScapeNetLib;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Networker_Client_Unity cli = new Networker_Client_Unity();
            cli.Setup("MS", 7777);
            cli.StartClient("localhost", 7777, "secret");

            ScapeNet.AddPacketType("Test", typeof(Test));

            cli.OnReceive("Test", packetObj => {
                PacketData<Test> test = new PacketData<Test>(packetObj);
                Console.WriteLine("Received: " + test.packet.testStr);

                return true;
            });


            while (true)
            {
                cli.Update();
            }


        }
    }
}
