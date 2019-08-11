using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScapeNet_Settings : MonoBehaviour
{
   public string ip;
   public int port;

   public void Awake() {
       DontDestroyOnLoad(gameObject);
   }
}
