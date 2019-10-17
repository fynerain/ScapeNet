using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

using Lidgren.Network;
using ScapeNetLib.Packets;

/// <summary>
/// Used to handle all connections as a server. Has support for custom packet types, as well as doing special things when
/// packets are received.
/// </summary>
namespace ScapeNetLib.Networkers
{
    public class Networker_Server 
    {
        protected NetServer server;
        protected NetPeerConfiguration config;
        protected string connection_approval_string;    

        public void Setup(string network_title, int port)
        {
            config = new NetPeerConfiguration(network_title);
            config.AutoExpandMTU = true;
            config.Port = port;
           
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.Data);
        }

        public virtual void HostServer(float connection_timeout, int maximum_connections, string connection_approval_string)
        {
            config.ConnectionTimeout = connection_timeout;
            config.MaximumConnections = maximum_connections;

            this.connection_approval_string = connection_approval_string;

            server = new NetServer(config);
            server.Start();

            Console.WriteLine("[ScapeNet] Server is up.");
        }

        public void Close()
        {
            server.Shutdown("bye");
        }

        public void OnReceive(string packet_identifier, Func<object[], bool> function)
        {
            Packet_Register.Instance.serverPacketReceivedRegister.Add(packet_identifier, function);
        }

        public void OnReceive(string packet_identifier, Type packet_type, Func<object[], bool> function)
        {
            ScapeNet.AddPacketType(packet_type);
            Packet_Register.Instance.serverPacketReceivedRegister.Add(packet_identifier, function);
        }

        public void SendPacketToAll<T>(T packet) where T : Packet<T>
        {
            NetOutgoingMessage msg = server.CreateMessage();

            msg = PacketHelper.AddDefaultInformationToPacket(msg, typeof(T).Name, packet.Get_PacketIdentifier());
            msg = packet.PackPacketIntoMessage(msg, packet);
            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void SendPacketToExistingConnection<T>(T packet, NetConnection conn) where T : Packet<T>
        {
            NetOutgoingMessage msg = server.CreateMessage();

            msg = PacketHelper.AddDefaultInformationToPacket(msg, typeof(T).Name, packet.Get_PacketIdentifier());
            msg = packet.PackPacketIntoMessage(msg, packet);
            server.SendMessage(msg, conn, NetDeliveryMethod.ReliableOrdered);
        }

        public virtual void Update()
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
                            PlayerLeft(lostConnection);
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
                        OnDataReceived(msg);
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                server.Recycle(msg);
            }
        }

        //Remove all player information once the player leaves.
        public virtual void PlayerLeft(NetConnection lostConnection){}

        //When server receives data.
        protected virtual void OnDataReceived(NetIncomingMessage msg)
        {
            string packet_name = msg.ReadString();
            string packet_identifier = msg.ReadString();
            NetOutgoingMessage outMsg = server.CreateMessage();

            Console.WriteLine("Message Received In Server: " + packet_name + " packet");

            if (Packet_Register.Instance.packetTypes.ContainsKey(packet_name))
            {
                Object instance = Activator.CreateInstance(Packet_Register.Instance.packetTypes[packet_name], packet_identifier);

                MethodInfo openMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("OpenPacketFromMessage");
                object packet = openMethod.Invoke(instance, new object[] { msg });
                bool shouldResend = false;

                //If it needs to be adjusted then adjust the packet
                if (Packet_Register.Instance.serverPacketReceivedRegister.ContainsKey(packet_identifier))
                {
                    shouldResend = Packet_Register.Instance.serverPacketReceivedRegister[packet_identifier].Invoke(new object[] { packet, msg.SenderConnection });
                }

                MethodInfo packMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("PackPacketIntoMessage");

                outMsg = PacketHelper.AddDefaultInformationToPacket(outMsg, packet_name, packet_identifier);
                outMsg = packMethod.Invoke(instance, new object[] { outMsg, packet }) as NetOutgoingMessage;

                if (shouldResend)
                {
                    server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                }
            }
        }


        public int GetConnections()
        {
            return server.Connections.Count;
        }
    }


}

