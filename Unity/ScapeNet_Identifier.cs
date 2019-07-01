using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScapeNet_Identifier : MonoBehaviour
{

    public bool isServer;
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
