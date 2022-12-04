using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarCollisionDetection : MonoBehaviour
{
    private Canvas waterc;
    private GameObject temple;
    public
    // Start is called before the first frame update

    private void Start()
    {
        waterc = GameManager.Instance.waterlevelCanvas;
        temple = transform.parent.parent.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey("e"))
            {
                waterc.gameObject.active = true;
                temple.GetComponent<WaterKeys>().currentPillar = gameObject;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        waterc.gameObject.active = false;
    }
}
