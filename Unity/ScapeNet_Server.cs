using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib;
using Lidgren.Network;

public class ScapeNet_Server : MonoBehaviour
{

    public Networker_Server_Unity serverNetworker;

    void Awake(){
        DontDestroyOnLoad(this);

        serverNetworker = new Networker_Server_Unity();

        serverNetworker.Setup("Forts", 7777);
        serverNetworker.HostServer(100,10, "secret");    
    }
   
    // Update is called once per frame
    void Update()
    {
        serverNetworker.Update();
    }

    public void SpawnObject(string obj_name, Vector3 position){

        InstantiationPacket packet = new InstantiationPacket("D_Instantiate");
        packet.obj_name = obj_name;
        packet.item_net_id = serverNetworker.GetNextItemID();
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        serverNetworker.SendPacketToAll(packet, 999);
        serverNetworker.AddRegister(packet, 999);
    }

     public void SpawnObject(string obj_name, Vector3 position, int playerId){

        InstantiationPacket packet = new InstantiationPacket("D_Instantiate");
        packet.obj_name = obj_name;
        packet.item_net_id = serverNetworker.GetNextItemID();
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        serverNetworker.SendPacketToAll(packet, playerId);
        serverNetworker.AddRegister(packet, playerId);
    }

    public void OnApplicationQuit()
    {
        serverNetworker.Close();
    }
}
