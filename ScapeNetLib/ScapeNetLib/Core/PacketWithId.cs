using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Small packet class that also contains an ID. Used for the Unity variations of the networkers.
/// </summary>
namespace ScapeNetLib
{
    internal class PacketWithId<T> where T : Packet<T>
    {
        public T packet = null;
        public int playerId = -1;

        public PacketWithId(T packet, int playerId){
            this.packet = packet;
            this.playerId = playerId;
        }
    }
}
