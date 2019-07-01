using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib;

public class ScapeNet_Extension : ScapeNet_Behaviour
{

   public int playersJoined = 0;

   public override void Start(){
      base.Start();

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
   }
}
