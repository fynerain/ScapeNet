using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

using Lidgren.Network;

/// <summary>
/// Used to handle all connections as a server. Has support for custom packet types, as well as doing special things when
/// packets are received.
/// </summary>
namespace ScapeNetLib
{
    public class Networker_Server_Unity : INetworker
    {
        NetServer server;
        NetPeerConfiguration config;

        private string connection_approval_string;

        int currentPlayerID = -1;
        int currentItemID = -1;

        //List of all players, with their id, and respective connection
        Dictionary<NetConnection, int> players;

        public void Setup(string network_title, int port)
        {
            config = new NetPeerConfiguration(network_title);
            config.Port = port;

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.Data);
        }

        public void OnReceive(string packet_name, Func<object, bool> function)
        {
            Packet_Register.Instance.serverPacketRecivedRegister.Add(packet_name, function);
        }

        public void SendPacket<T>(T packet, NetConnection conn) where T : Packet<T>
        {
            NetOutgoingMessage msg = server.CreateMessage();

            msg = packet.AddDefaultInformationToPacket(msg, packet.Get_PacketName());
            msg = packet.PackPacketIntoMessage(msg, packet);
            server.SendMessage(msg, conn, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendPacket<T>(T packet) where T : Packet<T>
        {
            NetOutgoingMessage msg = server.CreateMessage();

            msg = packet.AddDefaultInformationToPacket(msg, packet.Get_PacketName());
            msg = packet.PackPacketIntoMessage(msg, packet);
            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void HostServer(float connection_timeout, int maximum_connections, string connection_approval_string)
        {
            config.ConnectionTimeout = connection_timeout;
            config.MaximumConnections = maximum_connections;

            this.connection_approval_string = connection_approval_string;

            players = new Dictionary<NetConnection, int>();

            AddDefaultPacketReceives();


            server = new NetServer(config);
            server.Start();
        }

        private void AddDefaultPacketReceives()
        {
            Packet_Register.Instance.serverPacketRecivedRegister.Add("D_Connection", packetObj => {
                ConnectionPacket connectionPacket = (ConnectionPacket)packetObj;
             
                //Register Player
                int newID = GetNextPlayerID();
                players.Add(connectionPacket.senderConnection, newID);

                ConnectionPacket packet = new ConnectionPacket("D_Connection");
                packet.id = newID;

                connectionPacket = packet;

                return false;
            });


            //NEED TO COMPLETE
            Packet_Register.Instance.serverPacketRecivedRegister.Add("D_Instantiate", packetObj => {
                InstantiationPacket instantiate = (InstantiationPacket)packetObj;

                instantiate.item_id = GetNextItemID();


                return true;
            });


        }

        private int GetNextPlayerID()
        {
            currentPlayerID++;
            return currentPlayerID;
        }


        private int GetNextItemID()
        {
            currentItemID++;
            return currentItemID;
        }

        public void Update()
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
                        int player_id = msg.ReadInt32();
                        NetOutgoingMessage outMsg = server.CreateMessage();

                        Console.WriteLine("Message Received In Server: " + packet_name + " packet");

                        if (Packet_Register.Instance.packetTypes.ContainsKey(packet_name))
                        {
                            Object instance = Activator.CreateInstance(Packet_Register.Instance.packetTypes[packet_name], packet_name);

                            MethodInfo openMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("OpenPacketFromMessage");
                            object packet = openMethod.Invoke(instance, new object[] { msg });
                            bool shouldSendToClients = false;

                            //If it needs to be adjusted then adjust the packet
                            if (Packet_Register.Instance.serverPacketRecivedRegister.ContainsKey(packet_name))
                            {
                               shouldSendToClients = Packet_Register.Instance.serverPacketRecivedRegister[packet_name].Invoke(packet);
                            }

                                MethodInfo packMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("PackPacketIntoMessage");
                                MethodInfo defaultInfoMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("AddDefaultInformationToPacket");

                                outMsg = defaultInfoMethod.Invoke(instance, new object[] { outMsg, packet_name, player_id }) as NetOutgoingMessage;
                                outMsg = packMethod.Invoke(instance, new object[] { outMsg, packet }) as NetOutgoingMessage;


                            if (shouldSendToClients)
                            {
                                server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                            }
                            else
                            {
                                server.SendMessage(outMsg, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                            }
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
