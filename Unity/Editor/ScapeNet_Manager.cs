using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ScapeNet_Identifier))]
public class ScapeNet_Manager : MonoBehaviour
{

    [MenuItem("ScapeNet/Start Server")]
    static void StartServer(){
        GameObject.FindObjectOfType<ScapeNet_Identifier>().forceServer = true;
        GameObject.FindObjectOfType<ScapeNet_Client>().enabled = false;
        GameObject.FindObjectOfType<ScapeNet_Server>().enabled = true;
    }

    [MenuItem("ScapeNet/Start Client")]
    static void StartClient(){
        GameObject.FindObjectOfType<ScapeNet_Identifier>().forceServer = false;
        GameObject.FindObjectOfType<ScapeNet_Client>().enabled = true;
        GameObject.FindObjectOfType<ScapeNet_Server>().enabled = false;
    }

    [MenuItem("ScapeNet/Start Client+Server")]
    static void StartClientServer(){
        GameObject.FindObjectOfType<ScapeNet_Identifier>().forceServer = true;
        GameObject.FindObjectOfType<ScapeNet_Client>().enabled = true;
        GameObject.FindObjectOfType<ScapeNet_Server>().enabled = true;
    }
}
