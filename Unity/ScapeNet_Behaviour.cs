using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScapeNet_Network_ID))]
public class ScapeNet_Behaviour : MonoBehaviour
{
    [HideInInspector]
    public ScapeNet_Client client;

    private bool isConnected = false;

    // Start is called before the first frame update
    public virtual void Start(){
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<ScapeNet_Client>();
    }

    // Update is called once per frame
    public virtual void Update(){
        if(!isConnected)
            if(client.IsClientConnected()){
                OnNetworkConnect();
                isConnected = true;
            }

    }

    public virtual void OnNetworkConnect(){}
}
