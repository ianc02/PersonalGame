using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class signCollision : MonoBehaviour
{
    // Start is called before the first frame update
   
    public void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.nodeChanger(gameObject);
    }
}
