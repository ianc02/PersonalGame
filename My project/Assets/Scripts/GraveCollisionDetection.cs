using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveCollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public bool correct;
    public GameObject skelly;

    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.hasMedallion)
            {
                if (Input.GetKeyDown("e"))
                {
                    if (correct)
                    {
                        Debug.Log("ADD MORE SHIT HERE BUT THIS IS THE END FOR NOW");
                    }
                    else
                    {
                        skelly.active = true;
                    }
                }
            }
        }
    }
}
