using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///
/// Used to skip login scene straight to game if only the server is required.
///

public class ScapeNet_ServerBuild : MonoBehaviour
{
   void Awake(){
        if(GameObject.FindObjectOfType<ScapeNet_Identifier>().buildAsServer){
            SceneManager.LoadScene("SampleScene");
        }
   }
}
