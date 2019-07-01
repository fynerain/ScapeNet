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
   
    public void DeleteNetworkedGameObject(GameObject obj){
        client.DeleteNetworkedObject(obj.GetComponent<ScapeNet_Network_ID>().object_id);
    }
}
