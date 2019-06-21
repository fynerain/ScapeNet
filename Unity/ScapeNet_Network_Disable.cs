using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScapeNet_Network_Disable : MonoBehaviour
{
    public List<Behaviour> disable;

    //Called externally
    public void Disable()
    {
        foreach (Behaviour b in disable)
        {
            b.enabled = false;
        }
    }
}
