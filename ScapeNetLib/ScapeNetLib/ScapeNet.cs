using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScapeNetLib
{
    public static class ScapeNet
    {
        public static void AddPacketType(Type packet_type)
        {
            Packet_Register.Instance.AddToPacketRegister(packet_type);
        }
    }
}
