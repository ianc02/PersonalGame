using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    public Material sunset;
    public Material black;
    public GameObject light;
    public GameObject cavernprops;
    public GameObject cavernMobs;
    
    // Start is called before the first frame update
    public Vector3 pos;
    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            if (pos.y < 0)
            {
                RenderSettings.ambientIntensity = 0;
                RenderSettings.skybox = null;
                light.active = false;
                cavernprops.active = true;
                cavernMobs.active = true;
            }
            else
            {
                RenderSettings.skybox = sunset; 
                RenderSettings.ambientIntensity = 1;

                light.active = true;
                cavernprops.active = false;
                cavernMobs.active = false;
            }
            other.transform.position = pos;
        }
    }
}
