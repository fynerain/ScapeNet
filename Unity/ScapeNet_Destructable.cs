using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ScapeNetLib.Packets;

public class ScapeNet_Destructable : ScapeNet_Behaviour
{
    public float health = 100;
    public float lerpSpeed = 10f;
    public List<Renderer> renderers = new List<Renderer>();


    public List<Color> baseColour = new List<Color>();

    public override void Start()
    {
        base.Start();

        foreach(Renderer renderer in GetComponentsInChildren<Renderer>()){
            renderers.Add(renderer);
        }

        foreach(Renderer renderer in renderers){
            foreach(Material m in renderer.materials)
            baseColour.Add(m.color);
        }
    }

    public override void Update(){
        base.Update();


        if(isServer){
            if(health <= 0){

                DeletePacket packet = new DeletePacket("D_Delete");
                packet.item_net_id = GetComponent<ScapeNet_Network_ID>().object_id;            

                server.SendPacketToAll(packet);
                Destroy(gameObject);
            }
        }

      // UpdateColour();
    }

    
    void UpdateColour()
    {
        foreach(Renderer renderer in renderers){
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                if (renderer.materials[i].color != baseColour[i])
                {
                    renderer.materials[i].color = Color.Lerp(renderer.materials[i].color, baseColour[i], Time.deltaTime * lerpSpeed);
                }
            }
        }
    }

    public void RegisterHit()
    {
      foreach(Renderer renderer in renderers){
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            //renderer.materials[i].color = Color.Lerp(renderer.materials[i].color, new Color(230, 20, 0), Time.deltaTime * lerpSpeed);
            renderer.materials[i].color = new Color(230, 20, 0);
        }
      }
    }
}
