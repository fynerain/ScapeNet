using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

using Lidgren.Network;

namespace ScapeNetLib
{
    public class Networker_Server : Networker
    {
        NetServer server;
        NetPeerConfiguration config;

        private string connection_approval_string;

        public override void Setup(string network_title, int port)
        {
            config = new NetPeerConfiguration(network_title);
            config.Port = port;
           
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.Data);
        }

        public void HostServer(float connection_timeout, int maximum_connections, string connection_approval_string)
        {
            config.ConnectionTimeout = connection_timeout;
            config.MaximumConnections = maximum_connections;

            this.connection_approval_string = connection_approval_string;

            server = new NetServer(config);
            server.Start();
        }

        public override void Update()
        {
            NetIncomingMessage msg;
            while ((msg = server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                        break;

                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        throw new Exception("Error: " + msg.ReadString());

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        Console.WriteLine("Status: " + status);
                        if (status == NetConnectionStatus.Disconnected)
                        {
                            NetConnection lostConnection = msg.SenderConnection;
                            //PlayerLeft(lostConnection);
                        }
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        string s = msg.ReadString();
                        if (s == connection_approval_string)
                        {
                            msg.SenderConnection.Approve();
                        }
                        else
                            msg.SenderConnection.Deny();
                        break;
                    case NetIncomingMessageType.Data:
                        string packet_name = msg.ReadString();
                        NetOutgoingMessage outMsg = server.CreateMessage();

                        Console.WriteLine("Message Received In Server: " + packet_name + " packet");
                        ///////////
                        //Type t = typeof(ScapeNetLib.Packets.TestPacket);
                        ////////
                        ///


                        if (Packet_Register.Instance.packetTypes.ContainsKey(packet_name)) { 
                            Object instance = Activator.CreateInstance(Packet_Register.Instance.packetTypes[packet_name], packet_name);
                            MethodInfo openMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("RepackPacket");
                            outMsg = openMethod.Invoke(instance, new object[] { msg, outMsg }) as NetOutgoingMessage;
                            server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                        }
                       
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                server.Recycle(msg);
            }
        }


    }
}
