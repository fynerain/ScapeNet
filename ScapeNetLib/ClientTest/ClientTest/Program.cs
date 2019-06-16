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

            cli.OnReceive("D_Test", packetObj => {

                TestPacket testPacket = (TestPacket)packetObj[0];
                Console.WriteLine("Packet stored the value: " + testPacket.testInt);

                return true;
            });

            cli.OnReceive("D_Register", received => {
                RegisterPacket rp = (RegisterPacket)received[0];
                int players_id = (int)received[1];

                string obj_name = rp.obj_name;
                int obj_net_id = rp.item_net_id;

                if (obj_net_id != -1)
                {
                    //SpawnLocalCopyOfObject(players_id, obj_name, obj_net_id, new Vector3(rp.x, rp.y, rp.z));
                    Console.WriteLine("Reigster Packet Information: " + players_id + " " + obj_net_id + " XYZ: " + rp.x + "," + rp.y + "," + rp.z);
                }

                return false; //We don't want this packet to be send to the server.
            });

            RegisterPacket pack = new RegisterPacket("D_Register");
            pack.

            while (true)
            {
                cli.Update();
                cli.TestSend();
            }


        }
    }
}
