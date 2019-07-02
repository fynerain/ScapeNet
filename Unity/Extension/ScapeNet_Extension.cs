using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib;

public class ScapeNet_Extension : ScapeNet_Behaviour
{

   public int playersJoined = 0;

   public override void Start(){
      base.Start();

      ScapeNet.AddPacketType("Damage", typeof(DamagePacket));
      ScapeNet.AddPacketType("ServersideSpawn", typeof(ServersideSpawnPacket));

      if(isServer){
         server.serverNetworker.OnNewConnection(received => {
            PacketData<OnConnectPacket> data = new PacketData<OnConnectPacket>(received);           
            playersJoined++;

            ScapeNet_Manual_Spawner[] spawners = GameObject.FindObjectOfType<ScapeNet_Team_Manager>().teamSpawners.ToArray(); 

            if(playersJoined%2 == 0)
              spawners[1].SpawnObjectServerside(data.playerId);
            else
              spawners[0].SpawnObjectServerside(data.playerId);

            return false;
        });


        server.serverNetworker.OnReceive("Damage", received => {
            PacketData<DamagePacket> data = new PacketData<DamagePacket>(received);

            server.FindSpawnedNetObject(data.packet.damaged_items_id).GetComponent<ScapeNet_Destructable>().health += data.packet.damageDone;
            return false;
        });

          server.serverNetworker.OnReceive("ServersideSpawn", received => {
            PacketData<ServersideSpawnPacket> data = new PacketData<ServersideSpawnPacket>(received);

            Console.WriteLine("Serverside packet received, with name" + data.packet.obj_name);

            server.SpawnServerside(data.packet.obj_name, new Vector3(data.packet.x, data.packet.y, data.packet.z));
            return false;
        });
      }
   }
}
