using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScapeNetLib;
using ScapeNetLib.Networkers;
using ScapeNetLib.Packets;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {

            // Setup a server on the localhost.
            // Argument passed to server.Setup is the title of the network, this must match the client. The port is then sent over.
            // "secret" is the approval code which must match the client.

            Networker_Server srv = new Networker_Server();
            srv.Setup("ClientServerTest", 7777);
            srv.HostServer(100, 10, "secret");

            // Make a test packet called "Test"and input the string "Hello".
            TestPacket t = new TestPacket("Test");
            t.testString = "Hello";
       
            // Allow the server to poll and then sent the packet to all connected clients.
            while (true)
            {
                srv.Update();
                srv.SendPacketToAll(t);
            }
        }
    }
}
