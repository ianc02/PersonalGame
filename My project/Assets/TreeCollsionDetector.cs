using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCollsionDetector : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("What");
        if (other.CompareTag("Player"))
        {
            Debug.Log("HOLY FUCK THIS WSORKS");
        }
    }
}
