using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ScapeNetLib;

namespace ServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Networker_Server_Unity srv = new Networker_Server_Unity();
            srv.Setup("MS", 7777);
            srv.HostServer(100, 10, "secret");

            //srv.OnReceive("D_Register", received => {
            //    RegisterPacket rp = (RegisterPacket)received[0];
             //   int players_id = (int)received[1];
              //  Console.WriteLine(rp.obj_name);

             //   return true; //We don't want this packet to be send to the server.
        //    });

            while (true)
            {
                srv.Update();
            }
        }
    }
}
