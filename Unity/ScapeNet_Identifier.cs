using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScapeNet_Identifier : MonoBehaviour
{

    public List<GameObject> networkableObjects = new List<GameObject>();
    public List<GameObject> currentAliveNetworkedObjects = new List<GameObject>();

    [HideInInspector]
    public bool isServer;

    [HideInInspector]
    public bool forceServer; 

    void Awake(){
        if(!forceServer){
            #if UNITY_SERVER
                isServer = true;
            #else
                isServer = false;
            #endif
        }else
        {
            isServer = true;
        }
    }
}
