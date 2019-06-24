using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib;
using Lidgren.Network;

public class ScapeNet_Server : MonoBehaviour
{

    Networker_Server_Unity server;

    void Awake(){
        DontDestroyOnLoad(this);

        server = new Networker_Server_Unity();

        server.Setup("Forts", 7777);
        server.HostServer(100,10, "secret");
    }
   
    // Update is called once per frame
    void Update()
    {
        server.Update();
    }
}
