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

    public void SpawnObject(string obj_name, Vector3 position){

        InstantiationPacket packet = new InstantiationPacket("D_Instantiate");
        packet.obj_name = obj_name;
        packet.item_net_id = server.GetNextItemID();
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        server.SendPacketToAll(packet, 99);
        server.AddRegister(packet, 99);
    }
}
