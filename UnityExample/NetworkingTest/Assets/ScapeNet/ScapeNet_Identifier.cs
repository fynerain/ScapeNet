using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScapeNet_Identifier : MonoBehaviour
{

    public List<GameObject> networkableObjects = new List<GameObject>();
    public List<GameObject> currentAliveNetworkedObjects = new List<GameObject>();

   // [HideInInspector]
    public bool isServer;

  //  [HideInInspector]
    public bool isClient;

  //  [HideInInspector]
    public bool buildAsServer; 
}
