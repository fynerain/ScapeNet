using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;


/// <summary>
/// A small data structure, that convert the object array received on the client or server into useful data.
/// </summary>
namespace ScapeNetLib
{
    public class PacketData<T>
    {
        public T packet;
        public int playerId;
        public NetConnection senderConnection;

        public PacketData(object[] data)
        {
            // _Unity packet , which contains a player id
            if (data.Length > 2)
            {
                packet = (T)data[0];
                playerId = (int)data[1];
                senderConnection = (NetConnection)data[2];
            }
            else
            {
                packet = (T)data[0];
                senderConnection = (NetConnection)data[1];
            }
        }
    }
}
