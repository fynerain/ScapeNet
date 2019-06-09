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
            Networker_Client cli = new Networker_Client();
            cli.Setup("Test", 0);
            cli.StartClient("localhost", 7777, "secret");

            while (true)
            {
                cli.Update();
                cli.TestSend();
            }


        }
    }
}
