using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

using Lidgren.Network;
using ScapeNetLib.Packets;

/// <summary>
/// Used to handle all connections as a client. Has support for custom packet types, as well as doing special things when
/// packets are received.
/// </summary>
namespace ScapeNetLib.Networkers
{
    public class Networker_Client
    {

        protected NetClient client;
        protected NetPeerConfiguration config;     

        public virtual void Setup(string network_title)
        {
            config = new NetPeerConfiguration(network_title);

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.Data);
            client = new NetClient(config);
        }

        public virtual void StartClient(string ip, int port, string connection_approval_string)
        {
            client.Start();

            NetOutgoingMessage approval = client.CreateMessage();
            approval.Write(connection_approval_string);
            client.Connect(ip, port, approval);
        }

        public void Close()
        {
            client.Shutdown("bye");
        }


        public virtual void SendPacketToServer<T>(T packet) where T : Packet<T>
        {
            NetOutgoingMessage msg = client.CreateMessage();

            msg = PacketHelper.AddDefaultInformationToPacket(msg, typeof(T).Name, packet.Get_PacketIdentifier());
            msg = packet.PackPacketIntoMessage(msg,  packet);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void OnReceive(string packet_identifier, Func<object[], bool> function)
        {
            Packet_Register.Instance.clientPacketReceivedRegister.Add(packet_identifier, function);
        }

        public void OnReceive(string packet_identifier, Type packet_type, Func<object[], bool> function)
        {
            ScapeNet.AddPacketType(packet_type);
            Packet_Register.Instance.clientPacketReceivedRegister.Add(packet_identifier, function);
        }

        public virtual void OnConnected() { }

        public void Update()
        {
            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        break;
                    case NetIncomingMessageType.Data:
                        OnDataReceived(msg);
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        if ((NetConnectionStatus)msg.ReadByte() == NetConnectionStatus.Connected)
                            OnConnected();
                        break;
                    default:
                        break;
                }
                client.Recycle(msg);
            }
        }

        protected virtual void OnDataReceived(NetIncomingMessage msg)
        {
            string packet_name = msg.ReadString();
            string packet_identifier = msg.ReadString();

            if (Packet_Register.Instance.clientPacketReceivedRegister.ContainsKey(packet_identifier))
            {
                Object instance = Activator.CreateInstance(Packet_Register.Instance.packetTypes[packet_name], packet_identifier);
                MethodInfo openMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("OpenPacketFromMessage");
                object packet = openMethod.Invoke(instance, new object[] { msg });
                bool shouldSendBack;

                shouldSendBack = Packet_Register.Instance.clientPacketReceivedRegister[packet_identifier].Invoke(new object[] { packet, 0 });
            }
        }
    }
}

