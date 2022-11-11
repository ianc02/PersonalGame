using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    public Material sunset;
    public Material black;
    public GameObject light;
    
    // Start is called before the first frame update
    public Vector3 pos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = pos;
            if (pos.y < 0)
            {
                RenderSettings.ambientIntensity = 0;
                light.active = false;
            }
            else
            {
                RenderSettings.ambientIntensity = 1;
                light.active = true;
            }
        }
    }
}
