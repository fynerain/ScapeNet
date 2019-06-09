using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Interface used by both the client and server.
/// </summary>
namespace ScapeNetLib
{
    public abstract class Networker
    {
        public abstract void Setup(string network_title, int port);
        public abstract void Update();
    }
}
