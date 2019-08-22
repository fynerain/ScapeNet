using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///
/// Starts up the server or client and initialises the extension code.
///

public class ScapeNet_Bootstrap : MonoBehaviour
{
    ScapeNet_Identifier identififer;
    ScapeNet_Settings settings;
    
    ScapeNet_Client client;
    ScapeNet_Server server;

    void Awake()
    {
        identififer = GameObject.FindObjectOfType<ScapeNet_Identifier>();
        settings = GameObject.FindObjectOfType<ScapeNet_Settings>();

        client = GameObject.FindObjectOfType<ScapeNet_Client>();
        server = GameObject.FindObjectOfType<ScapeNet_Server>();
    }
    
    void Start()
    {
        Debug.Log(identififer.isServer);
         Debug.Log(identififer.isClient + " " + settings.ip);

        GameObject.FindObjectOfType<ScapeNet_Extension>().AddExtension();

        if(identififer.isServer)
            server.HostServer();

        if(identififer.isClient)
            client.StartClient(settings.ip, 7777);

    }

}
