using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Lidgren.Network;
using ScapeNetLib.Packets;


namespace ScapeNetLib.Networkers
{
    public class Networker_Client_Unity : Networker_Client
    {
        int player_id = -1;
        bool isConnectedToServer = false;

        public override void Setup(string network_title)
        {
            player_id = -1;
            isConnectedToServer = false;

            base.Setup(network_title);
        }


        public override void StartClient(string ip, int port, string connection_approval_string)
        {

            AddDefaultPacketReceives();

            base.StartClient(ip, port, connection_approval_string);
        }

        public bool IsConnected()
        {
            return isConnectedToServer;
        }

        public int GetPlayerID()
        {
            return player_id;
        }

        private void AddDefaultPacketReceives()
        {
            Packet_Register.Instance.clientPacketReceivedRegister.Add("D_Connection", packetObj => {
                ConnectionPacket connectionPacket = (ConnectionPacket)packetObj[0];

                player_id = connectionPacket.player_id;
                isConnectedToServer = true;

                OnConnectPacket ocp = new OnConnectPacket("D_OnConnect");
                SendPacketToServer(ocp);

                return false;
            });
        }

        public override void SendPacketToServer<T>(T packet)
        {
            NetOutgoingMessage msg = client.CreateMessage();

            msg = PacketHelper.AddDefaultInformationToPacketWithId(msg, typeof(T).Name, packet.Get_PacketIdentifier(), player_id);
            msg = packet.PackPacketIntoMessage(msg,  packet);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public override void OnConnected()
        {
            ConnectionPacket conPacket = new ConnectionPacket("D_Connection");
            conPacket.player_id = -1;

            SendPacketToServer(conPacket);
        }

        protected override void OnDataReceived(NetIncomingMessage msg)
        {
            string packet_name = msg.ReadString();
            string packet_identifier = msg.ReadString();
            int player_id = msg.ReadInt32();

           // Console.WriteLine("Packet name of " + packet_name + " packet identifier of " + packet_identifier);


            if (Packet_Register.Instance.clientPacketReceivedRegister.ContainsKey(packet_identifier))
            {
                Console.WriteLine("Is in register");

                System.Object instance = Activator.CreateInstance(Packet_Register.Instance.packetTypes[packet_name], packet_identifier);
                MethodInfo openMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("OpenPacketFromMessage");
                object packet = openMethod.Invoke(instance, new object[] { msg });
                bool shouldSendBack;

                shouldSendBack = Packet_Register.Instance.clientPacketReceivedRegister[packet_identifier].Invoke(new object[] { packet, player_id, msg.SenderConnection });
            }
        }

      
    }
}
