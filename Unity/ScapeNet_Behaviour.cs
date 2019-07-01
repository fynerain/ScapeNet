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

    private bool hasServerStarted = false;


    // Start is called before the first frame update
    public virtual void Start(){

        isServer = GameObject.FindObjectOfType<ScapeNet_Identifier>().isServer;

        client = GameObject.FindObjectOfType<ScapeNet_Client>();
        server = GameObject.FindObjectOfType<ScapeNet_Server>();
    }

    // Update is called once per frame
    public virtual void Update(){

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
