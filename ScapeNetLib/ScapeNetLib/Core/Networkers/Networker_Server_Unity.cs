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
        Dictionary<NetConnection, int> players = new Dictionary<NetConnection, int>();

        public void Setup(string network_title, int port)
        {
            config = new NetPeerConfiguration(network_title);
            config.Port = port;

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.Data);
        }

        public void OnReceive(string packet_name, Func<object[], bool> function)
        {
            Packet_Register.Instance.serverPacketReceivedRegister.Add(packet_name, function);
        }

        public void SendPacket<T>(T packet, NetConnection conn) where T : Packet<T>
        {
            NetOutgoingMessage msg = server.CreateMessage();

            msg = packet.AddDefaultInformationToPacket( msg, packet.Get_PacketName());
            msg = packet.PackPacketIntoMessage( msg,  packet);
            server.SendMessage(msg, conn, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendPacket<T>(T packet) where T : Packet<T>
        {
            NetOutgoingMessage msg = server.CreateMessage();

            msg = packet.AddDefaultInformationToPacket( msg, packet.Get_PacketName());
            msg = packet.PackPacketIntoMessage( msg,  packet);
            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void HostServer(float connection_timeout, int maximum_connections, string connection_approval_string)
        {
            config.ConnectionTimeout = connection_timeout;
            config.MaximumConnections = maximum_connections;

            this.connection_approval_string = connection_approval_string;

            AddDefaultPacketReceives();


            server = new NetServer(config);
            server.Start();

            Console.WriteLine("Server is up.");
        }

        private void AddDefaultPacketReceives()
        {
            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_Connection", packetObj => {
                ConnectionPacket connectionPacket = (ConnectionPacket)packetObj[0];
                NetConnection senderConnection = (NetConnection)packetObj[2];
             
                //Register Player
                int newID = GetNextPlayerID();

                Console.WriteLine("Player ID To Send: " + newID);
                    

                players.Add(senderConnection, newID);

                ConnectionPacket packet = new ConnectionPacket("D_Connection");
                packet.player_id = newID;

                connectionPacket = packet;

                Console.WriteLine("Connection packet has been received.");

                return false;
            });


            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_Instantiate", packetObj => {
                InstantiationPacket instantiate = (InstantiationPacket)packetObj[0];

                instantiate.item_id = GetNextItemID();

                Console.WriteLine("Instantiate packet has been received.");
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
                Console.WriteLine("Bytes : " + msg.LengthBytes);

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
                            msg.SenderConnection.Approve();
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
                            object packet = null;
                            bool shouldSendToAll = false;

                            try
                            {
                                MethodInfo openMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("OpenPacketFromMessage");
                                packet = openMethod.Invoke(instance, new object[] { msg });
                                
                            }catch(System.IndexOutOfRangeException e)
                            {
                                Console.WriteLine("Packet has failed to open due to an exception: " + e.Message);
                            }

                            //If it needs to be adjusted then adjust the packet
                            if (Packet_Register.Instance.serverPacketReceivedRegister.ContainsKey(packet_name))
                            {
                                shouldSendToAll = Packet_Register.Instance.serverPacketReceivedRegister[packet_name].Invoke(new object[] { packet, player_id, msg.SenderConnection });
                            }

                             
                                MethodInfo defaultInfoMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("AddDefaultInformationToPacketWithId");
                                MethodInfo packMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("PackPacketIntoMessage");

                                outMsg = defaultInfoMethod.Invoke(instance, new object[] { outMsg, packet_name, player_id }) as NetOutgoingMessage;
                                outMsg = packMethod.Invoke(instance, new object[] { outMsg, packet }) as NetOutgoingMessage;


                            if (shouldSendToAll)
                            {
                                server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                            }
                            else
                            {
                                Console.WriteLine("Sent Packet!");
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
