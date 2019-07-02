using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScapeNet_Game_Interface : ScapeNet_Behaviour
{
    public override void Start(){
        base.Start();
    }

    public override void Update(){
        base.Update();
    }

    public void ApplyDamage(GameObject go, float damage){
        DamagePacket packet = new DamagePacket("Damage");

        packet.damageDone = damage;
        packet.damaged_items_id = go.GetComponent<ScapeNet_Network_ID>().object_id;

        client.SendPacketToServer(packet);
    }

    public void RequestSpawn(string obj_name, Vector3 position){
        client.SpawnRequest(obj_name, position);
    } 

     public void RequestServersideSpawn(string obj_name, Vector3 position){

        ServersideSpawnPacket packet = new ServersideSpawnPacket("ServersideSpawn");
        packet.obj_name = obj_name;
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        client.SendPacketToServer(packet);
    } 
   
    public void DeleteNetworkedGameObject(GameObject obj){
        client.DeleteNetworkedObject(obj.GetComponent<ScapeNet_Network_ID>().object_id);
    }
}
