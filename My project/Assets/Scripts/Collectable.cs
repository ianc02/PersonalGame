using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collectable : MonoBehaviour
{

    private TextMeshPro tmp;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        cam = GameManager.Instance.getcamera();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tmp.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tmp.transform.LookAt(cam.transform);
            if (Input.GetKey("e"))
            {
                GameManager.Instance.addToInventory(gameObject.tag, gameObject.name);
                Destroy(gameObject);
                
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tmp.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
