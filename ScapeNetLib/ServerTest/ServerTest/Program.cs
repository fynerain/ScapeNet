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
            Networker_Server srv = new Networker_Server();
            srv.Setup("Test", 7777);
            srv.HostServer(100, 2, "secret");

            while (true)
            {
                srv.Update();
            }
        }
    }
}
