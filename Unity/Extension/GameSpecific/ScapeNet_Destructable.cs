using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib;

public class ScapeNet_Destructable : ScapeNet_Behaviour
{
    public float health = 100;

    public override void Update(){
        base.Update();


        if(isServer){
            if(health <= 0){

                DeletePacket packet = new DeletePacket("D_Delete");
                packet.item_net_id = GetComponent<ScapeNet_Network_ID>().object_id;            

                server.SendPacketToAll(packet);
                Destroy(gameObject);
            }
        }
    }
}
