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
    public class Networker_Server_Unity : Networker_Server
    {

        int currentPlayerID = -1;
        int currentItemID = -1;

        //List of all players, with their id, and respective connection.
        Dictionary<NetConnection, int> players = new Dictionary<NetConnection, int>();

        //List of all instantiation packets sent. This will be sent to new joins to 'sync' them.
        List<PacketWithId<InstantiationPacket>> registers = new List<PacketWithId<InstantiationPacket>>();

        Func<object[], bool> funcOnNewConnection = null;
        Func<object[], bool> funcOnLeaveConnection = null;

        //Start up server after adding definitions of default packets.
        public override void HostServer(float connection_timeout, int maximum_connections, string connection_approval_string)
        {
            AddDefaultPacketReceives();

            base.HostServer(connection_timeout, maximum_connections, connection_approval_string);
        }

        private void AddDefaultPacketReceives()
        {
            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_Connection", packetObj =>
            {

                ConnectionPacket connectionPacket = (ConnectionPacket)packetObj[0];
                NetConnection senderConnection = (NetConnection)packetObj[2];

                //Register Player
                int newID = GetNextPlayerID();
                players.Add(senderConnection, newID);
                connectionPacket.player_id = newID;

                SendPacketToExistingConnection(connectionPacket, senderConnection, -1);

                return false;
            });

            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_OnConnect", packetObj =>
            {
                OnConnectPacket instantiate = (OnConnectPacket)packetObj[0];
                int playerId = (int)packetObj[1];
                NetConnection conn = (NetConnection)packetObj[2];


                foreach (PacketWithId<InstantiationPacket> ip in registers)
                {
                    SendPacketToExistingConnection(ip.packet, conn, ip.playerId);
                }

                if (funcOnNewConnection != null)
                    funcOnNewConnection.Invoke(packetObj);

                return false;
            });


            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_Instantiate", packetObj =>
            {
                InstantiationPacket instantiate = (InstantiationPacket)packetObj[0];
                int playerId = (int)packetObj[1];

                instantiate.item_net_id = GetNextItemID();
                registers.Add(new PacketWithId<InstantiationPacket>(instantiate, playerId));

                SendPacketToAll(instantiate, playerId);

                return false;
            });

            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_Delete", packetObj =>
            {
                DeletePacket instantiate = (DeletePacket)packetObj[0];
                int playerId = (int)packetObj[1];
                NetConnection conn = (NetConnection)packetObj[2];

                int idToRemove = -1;

                for (int i = 0; i < registers.Count; i++)
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

            Packet_Register.Instance.serverPacketReceivedRegister.Add("D_PositionRotation", packetObj =>
            {
                PositionRotation packet = (PositionRotation)packetObj[0];
                int playerId = (int)packetObj[1];
                NetConnection conn = (NetConnection)packetObj[2];

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

        public int GetNextPlayerID()
        {
            currentPlayerID++;
            return currentPlayerID;
        }


        public int GetNextItemID()
        {
            currentItemID++;
            return currentItemID;
        }

        public void AddRegister(InstantiationPacket packet, int playerId)
        {
            registers.Add(new PacketWithId<InstantiationPacket>(packet, playerId));
        }

        public void OnNewConnection(Func<object[], bool> func)
        {
            funcOnNewConnection = func;
        }

        public void OnLeaveConnection(Func<object[], bool> func)
        {
            funcOnLeaveConnection = func;
        }

        public override void PlayerLeft(NetConnection lostConnection)
        {
            int playerLeftId = players[lostConnection];

            List<PacketWithId<InstantiationPacket>> registersToRemove = new List<PacketWithId<InstantiationPacket>>();

            foreach (PacketWithId<InstantiationPacket> packet in registers)
            {
                if (packet.playerId == playerLeftId)
                {
                    registersToRemove.Add(packet);
                }
            }

            foreach (PacketWithId<InstantiationPacket> packet in registersToRemove)
            {
                registers.Remove(packet);
            }

            if (funcOnLeaveConnection != null)
                funcOnLeaveConnection.Invoke(new object[] { null, playerLeftId, lostConnection });
        }

        public override void OnDataReceived(NetIncomingMessage msg)
        {
            string packet_name = msg.ReadString();
            int player_id = msg.ReadInt32();
            NetOutgoingMessage outMsg = server.CreateMessage();


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
                        shouldResend = Packet_Register.Instance.serverPacketReceivedRegister[packet_name].Invoke(new object[] { packet, player_id, msg.SenderConnection });
                }

                MethodInfo packMethod = Packet_Register.Instance.packetTypes[packet_name].GetMethod("PackPacketIntoMessage");
                outMsg = PacketHelper.AddDefaultInformationToPacketWithId(outMsg, packet_name, player_id);
                outMsg = packMethod.Invoke(instance, new object[] { outMsg, packet }) as NetOutgoingMessage;


                if (shouldResend)
                {
                    Console.WriteLine("Sent Packet To Everyone!");
                    server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                }
            }

        }
    }
}
