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
    protected bool editorServer = false;
    protected bool isClient = false;

    private bool hasServerStarted = false;

    public virtual void Start(){

        isServer = GameObject.FindObjectOfType<ScapeNet_Identifier>().isServer;
        //editorServer = GameObject.FindObjectOfType<ScapeNet_Identifier>().forceServer;

        client = GameObject.FindObjectOfType<ScapeNet_Client>();
        server = GameObject.FindObjectOfType<ScapeNet_Server>();
    }

    public virtual void Update(){

        if(client == null)
            client = GameObject.FindObjectOfType<ScapeNet_Client>();

        if(server == null)
           server = GameObject.FindObjectOfType<ScapeNet_Server>();

          if(client.enabled == true){
            if(!isConnected)
                if(client.IsClientConnected()){
                    OnNetworkConnect();
                    isConnected = true;
                }
          }
        
        if(server.enabled == true && hasServerStarted == false){
            OnServerStart();
            hasServerStarted = true;            
        }
    }

    public virtual void OnNetworkConnect(){}
    public virtual void OnServerStart(){}
}
