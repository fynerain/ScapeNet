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
            Networker_Server_Unity srv = new Networker_Server_Unity();
            srv.Setup("MS", 7777);
            srv.HostServer(100, 10, "secret");


            TestPacket t = new TestPacket("D_Test");
       
      

            while (true)
            {
                srv.Update();
                srv.SendPacketToAll(t, -1);
            }
        }
    }
}
