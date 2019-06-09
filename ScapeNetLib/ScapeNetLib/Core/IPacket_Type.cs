using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib
{
   // public interface IPacket_Type{}
    public interface IPacket_Type<T>
    {
    //    object OpenPacketFromMessage(NetIncomingMessage msg);
      //  NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg, T packet) { return null; }
    }
}
