using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
