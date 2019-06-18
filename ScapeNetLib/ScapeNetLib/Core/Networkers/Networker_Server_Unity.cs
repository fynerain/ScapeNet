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

        //List of all instantiation packets sent. This will be sent to new joins to 'sync' them
        List<PacketWithId<InstantiationPacket>> registers = new List<PacketWithId<InstantiationPacket>>();

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
                Console.WriteLine("Connection packet has been received.");

                ConnectionPacket connectionPacket = (ConnectionPacket)packetObj[0];
                NetConnection senderConnection = (NetConnection)packetObj[2];
             
                //Register Player
                int newID = GetNextPlayerID();
                Console.WriteLine("Player ID To Send: " + newID);                          
                players.Add(senderConnection, newID);
                connectionPacket.player_id = newID;

                SendPacketToExistingConnection(connectionPacket, senderConnection, -1);

                return false;
            });

            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_OnConnect", packetObj =>{
                OnConnectPacket instantiate = (OnConnectPacket)packetObj[0];
                int playerId = (int)packetObj[1];
                NetConnection conn = (NetConnection)packetObj[2];
               

                foreach (PacketWithId<InstantiationPacket> ip in registers)
                {
                    SendPacketToExistingConnection(ip.packet, conn, ip.playerId);
                }

                return false;
            });


            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_Instantiate", packetObj => {
                InstantiationPacket instantiate = (InstantiationPacket)packetObj[0];
                int playerId = (int)packetObj[1];

                Console.WriteLine("Instantiate packet received with player from id: " + playerId);
                instantiate.item_net_id = GetNextItemID();        
                Console.WriteLine("Instantiate packet has been received.");
                registers.Add(new PacketWithId<InstantiationPacket>(instantiate, playerId));

                for(int i = 0; i < registers.Count; i++)
                {
                    Console.WriteLine("Register contains " + registers.Count + " values, " + "number " + i + " has a player id of " + registers[i].playerId);
                }

                SendPacketToAll(instantiate, playerId);

                return false;
            });

            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_Delete", packetObj => {
                DeletePacket instantiate = (DeletePacket)packetObj[0];
                int playerId = (int)packetObj[1];
                NetConnection conn = (NetConnection)packetObj[2];
               

                Console.WriteLine("Instantiate packet has been received.");

                int idToRemove = -1;

                for(int i = 0; i < registers.Count; i++)
                {
                    //Found packet we need to delete
                    if (registers[i].packet.item_net_id == instantiate.item_net_id)
                    {
                        idToRemove = i;
                        break;
                    }

                }

                registers.RemoveAt(idToRemove);

                SendPacketToAll(instantiate, playerId);
                return false;
            });

            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_PositionRotation", packetObj => {
                PositionRotation packet = (PositionRotation)packetObj[0];
                int playerId = (int)packetObj[1];
                NetConnection conn = (NetConnection)packetObj[2];


                Console.WriteLine("PosRot packet has been received.");


                SendPacketToAll(packet, playerId);
                return false;
            });

        }

        public void SendPacketToAll<T>(T packet, int playerID) where T : Packet<T>
        {
            NetOutgoingMessage msg = server.CreateMessage();

            msg = PacketHelper.AddDefaultInformationToPacketWithId(msg, packet.Get_PacketName(), playerID);
            msg = packet.PackPacketIntoMessage(msg, packet);
            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendPacketToExistingConnection<T>(T packet, NetConnection conn, int playerID) where T : Packet<T>
        {
            NetOutgoingMessage msg = server.CreateMessage();

            msg = PacketHelper.AddDefaultInformationToPacketWithId(msg, packet.Get_PacketName(), playerID);
            msg = packet.PackPacketIntoMessage(msg, packet);
            server.SendMessage(msg, conn, NetDeliveryMethod.ReliableOrdered);
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
                    case NetIncomingMessageType.WarningMessage:
                        break;

                   
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
                        if(status == NetConnectionStatus.Connected)
                        {

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
                            bool shouldResend = false;


                            MethodInfo openMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("OpenPacketFromMessage");
                            packet = openMethod.Invoke(instance, new object[] { msg });
                     
                            //If it needs to be adjusted then adjust the packet
                            if (Packet_Register.Instance.serverPacketReceivedRegister.ContainsKey(packet_name))
                            {
                                if (Packet_Register.Instance.serverPacketReceivedRegister[packet_name] == null)
                                    shouldResend = true;
                                else
                                    shouldResend = Packet_Register.Instance.serverPacketReceivedRegister[packet_name].Invoke(new object[] {packet, player_id, msg.SenderConnection });
                            }

                                MethodInfo packMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("PackPacketIntoMessage");


                                outMsg = PacketHelper.AddDefaultInformationToPacketWithId(outMsg, packet_name, player_id);
                                outMsg = packMethod.Invoke(instance, new object[] { outMsg, packet }) as NetOutgoingMessage;
                        
                                
                            if (shouldResend)
                            {
                                Console.WriteLine("Sent Packet To Everyone!");
                                server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                            }
                           // else
                          //  {
                            ///    Console.WriteLine("Sent Packet To Sender!");
                            //    server.SendMessage(outMsg, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                           // }
                        }

                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                server.Recycle(msg);
            }
        }

        void OnNewConnection()
        {

        }

    }
}
