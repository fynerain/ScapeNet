using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

using ScapeNetLib;
using ScapeNetLib.Networkers;
using ScapeNetLib.Packets;

using Lidgren.Network;

[RequireComponent(typeof(ScapeNet_Identifier))]
public class ScapeNet_Server : MonoBehaviour
{
    public int port = 7777;
    public bool autoStartServer = false;

    [HideInInspector]
    public Networker_Server_Unity serverNetworker;
    private ScapeNet_Identifier identifier;

    private bool serverRunning = false;

    void Awake(){
        DontDestroyOnLoad(this);

        serverNetworker = new Networker_Server_Unity();
        identifier = GetComponent<ScapeNet_Identifier>();

        serverNetworker.Setup("Forts", port);
      
        serverNetworker.OnReceive("ServersideSpawn", received => {
            PacketData<ServersideSpawnPacket> data = new PacketData<ServersideSpawnPacket>(received);

            Console.WriteLine("[ScapeNet] Serverside packet received, with name " + data.packet.obj_name);
            GameObject spawned = SpawnServerside(data.packet.obj_name, new Vector3(data.packet.x, data.packet.y, data.packet.z), new Vector3(data.packet.rotX, data.packet.rotY, data.packet.rotZ));

            return false;
        });   

        serverNetworker.OnReceive("ServersideSpawnWithID", received => {
            PacketData<ServersideSpawnPacket> data = new PacketData<ServersideSpawnPacket>(received);

            Console.WriteLine("[ScapeNet] Serverside packet received, with name " + data.packet.obj_name);
            GameObject spawned = SpawnServerside(data.packet.obj_name, data.playerId, new Vector3(data.packet.x, data.packet.y, data.packet.z), new Vector3(data.packet.rotX, data.packet.rotY, data.packet.rotZ));

            return false;
        }); 
        
    }

    public void HostServer(){
          serverNetworker.HostServer(100,10, "secret"); 
          serverRunning = true;
    }
   
    void Update()
    {
        if(serverRunning)
            serverNetworker.Update();
    }

     public void SendPacketToAll<T>(T packet) where T : Packet<T>{
        serverNetworker.SendPacketToAll(packet, 999);
    }

    public void IssueSpawnCommand(string obj_name, Vector3 position){

        InstantiationPacket packet = new InstantiationPacket("D_Instantiate");
        packet.obj_name = obj_name;
        packet.item_net_id = serverNetworker.GetNextItemID();
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        serverNetworker.SendPacketToAll(packet, 999);
        serverNetworker.AddRegister(packet, 999);
    }

     public void IssueSpawnCommand(string obj_name, Vector3 position, Vector3 rotation, int playerId){

        InstantiationPacket packet = new InstantiationPacket("D_Instantiate");
        packet.obj_name = obj_name;
        packet.item_net_id = serverNetworker.GetNextItemID();
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        packet.rotX = rotation.x;
        packet.rotY = rotation.y;
        packet.rotZ = rotation.z;

        serverNetworker.SendPacketToAll(packet, playerId);
        serverNetworker.AddRegister(packet, playerId);
    }

    public GameObject SpawnServerside(string obj_name, Vector3 position, Vector3 rotation){

        int itemId = serverNetworker.GetNextItemID();

        GameObject newObj = null;
        newObj = Instantiate(FindNetObject(obj_name), position, Quaternion.Euler(rotation));

        if(newObj.GetComponent<ScapeNet_Network_ID>() == null)
            Debug.LogError("Object " + newObj + " cannot be networked, as it does not have a Network_ID component.");

        newObj.GetComponent<ScapeNet_Network_ID>().players_id = 999;
        newObj.GetComponent<ScapeNet_Network_ID>().object_id = itemId;

        InstantiationPacket packet = new InstantiationPacket("D_Instantiate");
        packet.obj_name = obj_name;
        packet.item_net_id = itemId;
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        packet.rotX = rotation.x;
        packet.rotY = rotation.y;
        packet.rotZ = rotation.z;
        
        //If editor, running client and server on once instance. Then do not respawn on clients
        if(GetComponent<ScapeNet_Identifier>().isServer && !GetComponent<ScapeNet_Identifier>().isClient){
              serverNetworker.SendPacketToAll(packet, 999);
              Console.WriteLine("Sent");
              serverNetworker.AddRegister(packet, 999);
        }

        identifier.currentAliveNetworkedObjects.Add(newObj); 

        return newObj;   
    }

    public GameObject SpawnServerside(string obj_name, int playerId, Vector3 position, Vector3 rotation){

        int itemId = serverNetworker.GetNextItemID();

        GameObject newObj = null;
        newObj = Instantiate(FindNetObject(obj_name), position, Quaternion.Euler(rotation));

        if(newObj.GetComponent<ScapeNet_Network_ID>() == null)
            Debug.LogError("Object " + newObj + " cannot be networked, as it does not have a Network_ID component.");

        newObj.GetComponent<ScapeNet_Network_ID>().players_id = playerId;
        newObj.GetComponent<ScapeNet_Network_ID>().object_id = itemId;

        InstantiationPacket packet = new InstantiationPacket("D_Instantiate");
        packet.obj_name = obj_name;
        packet.item_net_id = itemId;
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        packet.rotX = rotation.x;
        packet.rotY = rotation.y;
        packet.rotZ = rotation.z;
        
        //If editor, running client and server on once instance. Then do not respawn on clients
        if(GetComponent<ScapeNet_Identifier>().isServer && !GetComponent<ScapeNet_Identifier>().isClient){
              serverNetworker.SendPacketToAll(packet, 999);
              Console.WriteLine("Sent");
              serverNetworker.AddRegister(packet, 999);
        }

        identifier.currentAliveNetworkedObjects.Add(newObj); 

        return newObj;   
    }


    public GameObject FindNetObject(string object_name){
        foreach(GameObject go in identifier.networkableObjects)
            if(go != null && go.name == object_name)
                return go;

        return null;
    }

     public GameObject FindSpawnedNetObject(int obj_net_id){
          foreach(GameObject go in identifier.currentAliveNetworkedObjects)
            if(go != null && go.GetComponent<ScapeNet_Network_ID>().object_id == obj_net_id)
                return go;

        return null;
    }

    public void OnApplicationQuit()
    {
        if(serverNetworker != null && serverRunning)
            serverNetworker.Close();
    }
}
