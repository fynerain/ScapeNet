using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace ScapeNetLib.Packets
{
    public class TestPacket : Packet<TestPacket>
    {
        public string testString;

        public TestPacket(string packet_name) : base(packet_name){}

        public override TestPacket OpenPacketFromMessage(NetIncomingMessage msg)
        {
            TestPacket packet = new TestPacket(packet_name)
            {
                testString = msg.ReadString()
            };

            return packet;
        }

        public override NetOutgoingMessage PackPacketIntoMessage(NetOutgoingMessage msg,  TestPacket packet)
        {
            msg.Write(packet.testString);
            return msg;
        }
       
    }
}
