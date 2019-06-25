using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScapeNet_Spawner : ScapeNet_Behaviour
{
    [Header("Item must be in clients networkable list.")]
    public GameObject itemToSpawn;

    public bool serverside = false;

    public override void OnNetworkConnect(){
        if(!serverside && client.IsClientConnected())
            client.SpawnServersideRequest(itemToSpawn.name, gameObject.transform.position);      
    }

    public override void OnServerStart(){
        if(serverside && isServer)
            server.SpawnObject(itemToSpawn.name, gameObject.transform.position);
    }
}
