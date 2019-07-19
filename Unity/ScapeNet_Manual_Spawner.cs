using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScapeNet_Manual_Spawner : ScapeNet_Behaviour
{
   [Header("Item must be in clients networkable list.")]
    public GameObject itemToSpawn;

    public void SpawnObjectServerside(int playerId){
          server.IssueSpawnCommand(itemToSpawn.name, gameObject.transform.position, gameObject.transform.rotation.eulerAngles, playerId);
    }

    public void OnEnable(){
          foreach(Renderer r in GetComponentsInChildren<Renderer>()){
                r.enabled = false;
          }
    }
}
