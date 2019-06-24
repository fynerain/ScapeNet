﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib;

public class ScapeNet_SyncPositionRotation : ScapeNet_Behaviour
{

    public bool updateRotation = false;
    
    private Vector3 previous_position = Vector3.zero;
    private Vector3 previous_rotation = Vector3.zero;
    
    public override void Start(){
        base.Start();

        if(!isServer)
            StartCoroutine(SendPositionAndRotation());
    }

    public override void Update(){
        base.Update();
    }


    IEnumerator SendPositionAndRotation()
    {        
        yield return new WaitForEndOfFrame();

        
        SyncObjectsPosition();

        if(updateRotation)
            SyncObjectsRotation();

        StartCoroutine(SendPositionAndRotation());

    }

    void SyncObjectsPosition(){
        Vector3 positionToSend;
        //float cap = 10000000;

        //If object has a parent, then convert position before sending.
        if(transform.parent != null)
            positionToSend = transform.TransformVector(transform.localPosition);
        else
            positionToSend = transform.position;
            
        if(previous_position == positionToSend)
            return;

        if(positionToSend.x >= float.MaxValue || positionToSend.y >= float.MaxValue || positionToSend.z >= float.MaxValue)
            return;
        
        PositionRotation pr = new PositionRotation("D_PositionRotation");
        //pr.item_net_id = client.FindSpawnedNetObject(gameObject.GetComponent<ScapeNet_Network_ID>().object_id);
        pr.item_net_id = GetComponent<ScapeNet_Network_ID>().object_id;
        pr.isRotation = false;
        pr.x = positionToSend.x;
        pr.y = positionToSend.y;
        pr.z = positionToSend.z;
    
        client.SendPacketToServer(pr);

        previous_position = positionToSend;
    }

    void SyncObjectsRotation(){
        Vector3 rotationToSend = transform.rotation.eulerAngles;

        
        PositionRotation pr = new PositionRotation("D_PositionRotation");
        pr.item_net_id = GetComponent<ScapeNet_Network_ID>().object_id;
        pr.isRotation = true;
        pr.x = rotationToSend.x;
        pr.y = rotationToSend.y;
        pr.z = rotationToSend.z;
    
        client.SendPacketToServer(pr);

        previous_rotation = rotationToSend;
    }
}
