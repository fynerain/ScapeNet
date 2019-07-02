using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using ScapeNetLib;

[RequireComponent(typeof(ScapeNet_Identifier))]
public class ScapeNet_Client : MonoBehaviour
{

    private GameObject localPlayer = null;
    private Networker_Client_Unity client;
    private ScapeNet_Identifier identifier;

    void Awake(){
        DontDestroyOnLoad(gameObject);

        client = new Networker_Client_Unity();
        client.Setup("Forts", 7777);

            client.OnReceive("D_Instantiate", received => {
                PacketData<InstantiationPacket> data = new PacketData<InstantiationPacket>(received);

                if(data.packet.item_net_id != -1){
                    SpawnLocalCopyOfObject(data.playerId, data.packet.obj_name, data.packet.item_net_id, new Vector3(data.packet.x, data.packet.y, data.packet.z));
                }
                    
                return false; 
            });


            client.OnReceive("D_PositionRotation", received => {
                PacketData<PositionRotation> data = new PacketData<PositionRotation>(received);

                if(client.GetPlayerID() != data.playerId){
                    GameObject go = FindSpawnedNetObject(data.packet.item_net_id);

                    if(go != null){
                        if(data.packet.isRotation)
                            go.transform.rotation = Quaternion.Euler(data.packet.x, data.packet.y, data.packet.z);
                        else
                            go.transform.position = new Vector3(data.packet.x, data.packet.y, data.packet.z);                                      
                    }
                }
                    
                return false; 
            });

            client.OnReceive("D_Delete", received => {
                PacketData<DeletePacket> data = new PacketData<DeletePacket>(received);
                GameObject toDelete = null;

                foreach(GameObject go in identifier.currentAliveNetworkedObjects)
                    if(go != null && go.GetComponent<ScapeNet_Network_ID>().object_id == data.packet.item_net_id){
                        toDelete = go;
                        break;
                    }

                identifier.currentAliveNetworkedObjects.Remove(toDelete);
                Destroy(toDelete);
            
                return false; 
            });
    }

    void Start(){
        identifier = GetComponent<ScapeNet_Identifier>();
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

            //If its a server spawned object
            if(players_id == 999 && newObj.GetComponent<ScapeNet_Network_Disable>() != null)
                newObj.GetComponent<ScapeNet_Network_Disable>().Disable();

        //If a player is being spawned
        if (object_name == "Player")
        {     
            //Another player - disable their controls
            if (client.GetPlayerID() != players_id)
            {
                newObj.GetComponent<ScapeNet_Network_Disable>().Disable();
            }
            else
            {
                localPlayer = newObj;
            }
        }

        identifier.currentAliveNetworkedObjects.Add(newObj); 
    }

    public void SpawnRequest(string obj_name, Vector3 position){

        InstantiationPacket packet = new InstantiationPacket("D_Instantiate");
        packet.obj_name = obj_name;
        packet.item_net_id = -1;
        packet.x = position.x;
        packet.y = position.y;
        packet.z = position.z;

        client.SendPacketToServer(packet);
    }

    public GameObject FindNetObject(string object_name){
        foreach(GameObject go in identifier.networkableObjects)
            if(go.name == object_name)
                return go;

        return null;
    }

    public GameObject FindSpawnedNetObject(int obj_net_id){
          foreach(GameObject go in identifier.currentAliveNetworkedObjects)
            if(go != null && go.GetComponent<ScapeNet_Network_ID>().object_id == obj_net_id)
                return go;

        return null;
    }

    public void DeleteNetworkedObject(int obj_net_id){
        DeletePacket packet = new DeletePacket("D_Delete");
        packet.item_net_id = obj_net_id;
        
        client.SendPacketToServer(packet);
    }

    public GameObject GetLocalPlayer(){
        return localPlayer;
    }

    public void OnApplicationQuit()
    {
        client.Close();
    }
}
