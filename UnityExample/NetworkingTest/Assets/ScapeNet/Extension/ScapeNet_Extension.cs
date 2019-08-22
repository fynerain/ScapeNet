using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib;
using ScapeNetLib.Packets;
using ScapeNetLib.Packets.MicroData;

///
/// Used to begin to seperate 'user' code from the 'base' code
///

public class ScapeNet_Extension : ScapeNet_Behaviour
{

   public int playersJoined = 0;

 

   public void AddExtension(){
         ScapeNet.AddPacketType(typeof(DamagePacket));
         ScapeNet.AddPacketType(typeof(ServersideSpawnPacket));

         Debug.Log("EXTENSION ADDED");

         if(isServer){
        
            server.serverNetworker.OnNewConnection(received => {
               PacketData<OnConnectPacket> data = new PacketData<OnConnectPacket>(received);      

               SpawnPlayer(data);
               Debug.Log("ID " + data.playerId);

               return false;
         });


         server.serverNetworker.OnReceive("Damage", received => {
               PacketData<DamagePacket> data = new PacketData<DamagePacket>(received);
               server.FindSpawnedNetObject(data.packet.damaged_items_id).GetComponent<ScapeNet_Destructable>().health += data.packet.damageDone;
               return false;
         });

      }
   }

   public override void Start(){
      base.Start();  
   }

    public override void Update(){
      base.Update();
    }



   public void SpawnPlayer(PacketData<OnConnectPacket> data){

            playersJoined++;

            ScapeNet_Manual_Spawner[] spawners = GameObject.FindObjectOfType<ScapeNet_Team_Manager>().teamSpawners.ToArray(); 

            if(playersJoined%2 == 0)
              spawners[1].SpawnObjectServerside(data.playerId);
            else
              spawners[0].SpawnObjectServerside(data.playerId);
   }

  
}
