using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

///
/// Used to easily set what will be ran, eg a server, client or both.
///

public class ScapeNet_Manager : MonoBehaviour
{

    [MenuItem("ScapeNet/Setup Server")]
    static void StartServer(){
        GameObject.FindObjectOfType<ScapeNet_Identifier>().buildAsServer = true;
        GameObject.FindObjectOfType<ScapeNet_Identifier>().isServer = true;
        GameObject.FindObjectOfType<ScapeNet_Identifier>().isClient = false;
    }

    [MenuItem("ScapeNet/Setup Client")]
    static void StartClient(){
        GameObject.FindObjectOfType<ScapeNet_Identifier>().buildAsServer = false;
        GameObject.FindObjectOfType<ScapeNet_Identifier>().isServer = false;
        GameObject.FindObjectOfType<ScapeNet_Identifier>().isClient = true;
    }

    [MenuItem("ScapeNet/Setup Client+Server")]
    static void StartClientServer(){
        GameObject.FindObjectOfType<ScapeNet_Identifier>().buildAsServer = false;
        GameObject.FindObjectOfType<ScapeNet_Identifier>().isClient = true;
        GameObject.FindObjectOfType<ScapeNet_Identifier>().isServer = true;
    }
}
