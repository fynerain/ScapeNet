using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScapeNet_Network_ID))]
public class ScapeNet_Behaviour : MonoBehaviour
{
    [HideInInspector]
    public ScapeNet_Client client;

    [HideInInspector]
    public ScapeNet_Server server;

    protected bool isConnected = false;
    protected bool isServer = false;

 
    // Start is called before the first frame update
    public virtual void Start(){

        #if UNITY_SERVER
            isServer = true;
        #endif

        client = GameObject.FindGameObjectWithTag("Client").GetComponent<ScapeNet_Client>();
        server = GameObject.FindGameObjectWithTag("Server").GetComponent<ScapeNet_Server>();
    }

    // Update is called once per frame
    public virtual void Update(){
        if(!isServer){
            if(!isConnected)
                if(client.IsClientConnected()){
                    OnNetworkConnect();
                    isConnected = true;
                }
        }else
        {
            OnServerStart();
        }
    }

    public virtual void OnNetworkConnect(){}
    public virtual void OnServerStart(){}
}
