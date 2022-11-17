using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkDetector : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.name.Equals("E"))
                {
                    child.gameObject.active = true;
                }
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown("e"))
            {
                GameManager.Instance.oldWomanDialogue();
            }
            transform.LookAt(GameManager.Instance.getPlayer().transform);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.name.Equals("E"))
                {
                    child.gameObject.active = false;
                }
            }
        }
    }



}
