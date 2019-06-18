using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

using Lidgren.Network;

/// <summary>
/// Used to handle all connections as a client. Has support for custom packet types, as well as doing special things when
/// packets are received.
/// </summary>
namespace ScapeNetLib
{
    public class Networker_Client : INetworker
    {

        NetClient client;
        NetPeerConfiguration config;     

        public void Setup(string network_title, int port)
        {
            config = new NetPeerConfiguration(network_title);

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.Data);
        }

        public void StartClient(string ip, int port, string connection_approval_string)
        {
            client = new NetClient(config);
            client.Start();

            NetOutgoingMessage approval = client.CreateMessage();
            approval.Write(connection_approval_string);
            client.Connect(ip, port, approval);
        }

        public void SendPacketToServer<T>(T packet) where T : Packet<T>
        {
            NetOutgoingMessage msg = client.CreateMessage();

            msg = PacketHelper.AddDefaultInformationToPacket(msg, packet.Get_PacketName());
            msg = packet.PackPacketIntoMessage( msg,  packet);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void TestSend()
        {
            NetOutgoingMessage msg = client.CreateMessage();
            TestPacket pak = new TestPacket("D_Test");
            pak.testInt = 100;

            Console.WriteLine("Test packet has been sent.");

            msg = pak.PackPacketIntoMessage( msg, pak);
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public void OnReceive(string packet_name, Func<object[], bool> function)
        {
            Packet_Register.Instance.clientPacketReceivedRegister.Add(packet_name, function);
        }

        public void Update()
        {
          //  if (client == null)
              //  Debug.LogError("Client object does not exist, cannot instantiate network communications.");

            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                       // Debug.Log(msg.ReadString());
                        break;
                    case NetIncomingMessageType.Data:
                        Console.WriteLine("MESSAGE RECEIVED IN CLIENT");
                        string packet_name = msg.ReadString();


                        if (Packet_Register.Instance.clientPacketReceivedRegister.ContainsKey(packet_name))
                        {
                            Object instance = Activator.CreateInstance(Packet_Register.Instance.packetTypes[packet_name], packet_name);
                            MethodInfo openMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("OpenPacketFromMessage");
                            object packet = openMethod.Invoke(instance, new object[] { msg });
                            bool shouldSendBack;

                            shouldSendBack = Packet_Register.Instance.clientPacketReceivedRegister[packet_name].Invoke(new object[] { packet, 0 });
                          
                        }
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        break;
                    default:
                      //  Debug.Log("Unhandled type: " + msg.MessageType);
                        break;
                }
                client.Recycle(msg);
            }
        }
    }
}

