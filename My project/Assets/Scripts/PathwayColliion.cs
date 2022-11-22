using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathwayColliion : MonoBehaviour
{
    public bool inside = false;
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        inside = true;
        GameManager.Instance.inPathway();
    }

    public void OnTriggerStay(Collider other)
    {
        inside = true;
    }
    public void OnTriggerExit(Collider other)
    {
        inside = false;
        GameManager.Instance.outsidePathway();
    }

    
}
