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
    public interface INetworker
    {
       void Setup(string network_title, int port);
       void Update();
    }
}
