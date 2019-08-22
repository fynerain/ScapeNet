using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///
/// Quick class to disable functions that won't need to be called by the clone of another player. Eg, player1 and player2 wont call 
/// the same functions on player1's machine.
///

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
