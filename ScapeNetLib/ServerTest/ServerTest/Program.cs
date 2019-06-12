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
            srv.Setup("MC", 7777);
            srv.HostServer(100, 2, "secret");

            while (true)
            {
                srv.Update();
            }
        }
    }
}
