using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ScapeNetLib;

namespace ClientTest 
{
    public class Test : PacketHL<Test>
    {
        public string testStr;

        public Test() : base("Test") { }
    }
}
