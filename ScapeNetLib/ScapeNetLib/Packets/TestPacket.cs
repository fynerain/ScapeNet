using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib
{
    public class TestPacket : Packet<TestPacket>
    {
        public int testInt;

        public TestPacket(string packet_name) : base(packet_name){}

        public override TestPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            TestPacket packet = new TestPacket(packet_name)
            {
                testInt = msg.ReadInt32()
            };

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg,  TestPacket packet)
        {
            msg.Write(packet.testInt);
            return msg;
        }
       
    }
}
