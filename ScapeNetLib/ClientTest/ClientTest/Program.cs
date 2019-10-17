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
            // Setup a client on the localhost.
            // Argument passed to cli.Setup is the title of the network, this must match the server.
            // "secret" is the approval code which must match the server.

            Networker_Client cli = new Networker_Client();
            cli.Setup("ClientServerTest");
            cli.StartClient("localhost", 7777, "secret");
           
            // Upon receving a packet called "Test", make some PacketData which is casted to the type we expect.
            // Collect and print the string inside the packet.

            cli.OnReceive("Test", packetObj => {
                PacketData<TestPacket> test = new PacketData<TestPacket>(packetObj);
                Console.WriteLine("Received: " + test.packet.testString);

                return true;
            });

            // We need to call .Update() to allow for polling.
            while (true)
            {
                cli.Update();
            }


        }
    }
}
