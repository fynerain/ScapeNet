using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScapeNet_Spawner : ScapeNet_Behaviour
{
    [Header("Item must be in clients networkable list.")]
    public GameObject itemToSpawn;

    public override void OnNetworkConnect(){
        client.SpawnServersideRequest(itemToSpawn.name, gameObject.transform.position);
    }
}
