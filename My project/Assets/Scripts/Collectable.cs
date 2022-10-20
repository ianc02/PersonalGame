using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collectable : MonoBehaviour
{

    private Collider collider;
    private TextMeshPro tmp;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<SphereCollider>();
        tmp = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered collider");
            tmp.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("within collider");
            tmp.transform.LookAt(cam.transform);
            if (Input.GetKey("e"))
            {
                Debug.Log(gameObject.tag);
                Debug.Log(gameObject.name);
                Debug.Log(GameManager.Instance.name);
                GameManager.Instance.addToInventory(gameObject.tag, gameObject.name);
                Destroy(gameObject);
                
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("leaving collider");
            tmp.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
