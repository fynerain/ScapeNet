using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ScapeNetLib;

public class ScapeNet_Client : MonoBehaviour
{
    public List<GameObject> networkableObjects = new List<GameObject>();
    public List<GameObject> currentAliveNetworkedObjects = new List<GameObject>();

    private GameObject localPlayer = null;
    private Networker_Client_Unity client;

    void Awake(){
        DontDestroyOnLoad(gameObject);



        client = new Networker_Client_Unity();
        client.Setup("Forts", 7777);

            client.OnReceive("D_Instantiate", received => {
                InstantiationPacket rp = (InstantiationPacket)received[0];
                int players_id = (int)received[1];

                Debug.Log("Player it was sent from " + players_id);
                Debug.Log("Items network id " + rp.item_net_id);

                if(rp.item_net_id != -1){
                    SpawnLocalCopyOfObject(players_id, rp.obj_name, rp.item_net_id, new Vector3(rp.x, rp.y, rp.z));
                }
                    
                return false; 
            });

            client.OnReceive("D_PositionRotation", received => {

                Debug.Log("Received posrot");

                PositionRotation rp = (PositionRotation)received[0];
                int players_id = (int)received[1];

                Debug.LogWarning("PositionRot received from player " + players_id);

                if(client.GetPlayerID() != players_id){
                    GameObject go = FindSpawnedNetObject(rp.item_net_id);

                    if(go != null){
                        if(rp.isRotation)
                            go.transform.rotation = Quaternion.Euler(rp.x, rp.y, rp.z);
                        else
                            go.transform.position = new Vector3(rp.x, rp.y, rp.z);                                      
                    }
                }
                    
                return false; 
            });

            client.OnReceive("D_Delete", received => {
                DeletePacket rp = (DeletePacket)received[0];
                int players_id = (int)received[1];

                GameObject toDelete = null;

                foreach(GameObject go in currentAliveNetworkedObjects)
                    if(go.GetComponent<ScapeNet_Network_ID>().object_id == rp.item_net_id)
                        toDelete = go;

                currentAliveNetworkedObjects.Remove(toDelete);
                Destroy(toDelete);
            
                return false; 
            });
    }

    void Start(){
        client.StartClient("localhost", 7777, "secret");
    }

    void Update(){     
        client.Update();

        Debug.Log("Con: " + client.IsConnected());
        Debug.Log(client.GetPlayerID());
    }

    public bool IsClientConnected() {
        return client.IsConnected();
    }

    public void SendPacketToServer<T>(T packet) where T : Packet<T>{
        client.SendPacketToServer(packet);
    }


    void SpawnLocalCopyOfObject(int players_id, string object_name, int obj_net_id, Vector3 position)
    {
            GameObject newObj = null;

            newObj = Instantiate(FindNetObject(object_name), position, Quaternion.identity);
            newObj.GetComponent<ScapeNet_Network_ID>().players_id = players_id;
            newObj.GetComponent<ScapeNet_Network_ID>().object_id = obj_net_id;

        //If a player is being spawned
        if (object_name == "Player")
        {     
            //Debug.LogError("Current client id: " + client.GetPlayerID());
            //Debug.LogError("Packed from player with id: " + players_id);
            //Another player - disable their controls
            if (client.GetPlayerID() != players_id)
            {
                newObj.GetComponent<ScapeNet_Network_Disable>().Disable();
                newObj.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                localPlayer = newObj;
            }
        }

        currentAliveNetworkedObjects.Add(newObj); 
    }

    public void SpawnServersideRequest(string obj_name, Vector3 position){

        InstantiationPacket packet = new InstantiationPacket("D_Instantiate");
        packet.obj_name = obj_name;
        packet.item_net_id = -1;
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        client.SendPacketToServer(packet);
    }

    public GameObject FindNetObject(string object_name){
        foreach(GameObject go in networkableObjects)
            if(go.name == object_name)
                return go;

        return null;
    }

    public GameObject FindSpawnedNetObject(int obj_net_id){
          foreach(GameObject go in currentAliveNetworkedObjects)
            if(go.GetComponent<ScapeNet_Network_ID>().object_id == obj_net_id)
                return go;

        return null;
    }

    public GameObject GetLocalPlayer(){
        return localPlayer;
    }
}
