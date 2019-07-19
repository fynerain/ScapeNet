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
            client.SpawnRequest(itemToSpawn.name, gameObject.transform.position, gameObject.transform.rotation.eulerAngles);      
    }

    public override void OnServerStart(){
        if(serverside && isServer)
            server.IssueSpawnCommand(itemToSpawn.name, gameObject.transform.position);
    }
}
