using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib;
using ScapeNetLib.Packets.MicroData;

///
/// Used to allow classes to interact with networking components without being networked themselves.
///

public class ScapeNet_Game_Interface : ScapeNet_Behaviour
{
    public override void Start(){
        base.Start();
    }

    public override void Update(){
        base.Update();
    }

    public int GetPlayerID(){
        return client.clientNetworker.GetPlayerID();
    }

    public void ApplyDamage(GameObject go, float damage){
        DamagePacket packet = new DamagePacket("Damage");

        packet.damageDone = damage;
        packet.damaged_items_id = go.GetComponent<ScapeNet_Network_ID>().object_id;

        client.SendPacketToServer(packet);
    }

    public void RequestSpawn(string obj_name, Vector3 position, Vector3 rotation){
        client.SpawnRequest(obj_name, position, rotation);
    } 

     public void RequestServersideSpawn(string obj_name, Vector3 position){

        ServersideSpawnPacket packet = new ServersideSpawnPacket("ServersideSpawn");
        packet.obj_name = obj_name;
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        packet.rotX = 0;
        packet.rotY = 0;
        packet.rotZ = 0;

        client.SendPacketToServer(packet);
    } 

    public void RequestServersideSpawn(string obj_name, Vector3 position, Vector3 rotation){

        ServersideSpawnPacket packet = new ServersideSpawnPacket("ServersideSpawn");
        packet.obj_name = obj_name;
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        packet.rotX = rotation.x;
        packet.rotY = rotation.y;
        packet.rotZ = rotation.z;

        client.SendPacketToServer(packet);
    } 

    public void RequestServersideSpawnWithID(string obj_name, Vector3 position, Vector3 rotation){

        ServersideSpawnPacket packet = new ServersideSpawnPacket("ServersideSpawnWithID");
        packet.obj_name = obj_name;
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        packet.rotX = rotation.x;
        packet.rotY = rotation.y;
        packet.rotZ = rotation.z;

        client.SendPacketToServer(packet);
    } 
   
    public void DeleteNetworkedGameObject(GameObject obj){
        client.DeleteNetworkedObject(obj.GetComponent<ScapeNet_Network_ID>().object_id);
    }

}
