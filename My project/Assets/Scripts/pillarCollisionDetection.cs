using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarCollisionDetection : MonoBehaviour
{
    private Canvas waterc;
    private GameObject temple;
    public string keyShape;
    public GameObject key;
    // Start is called before the first frame update

    private void Start()
    {
        waterc = GameManager.Instance.waterlevelCanvas;
        temple = transform.parent.parent.gameObject;
        key = transform.GetChild(0).gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown("e"))
            {

                if (key.active)
                {
                    
                    key.active = false;
                    temple.GetComponent<WaterKeys>().currentPillar = gameObject;
                    temple.GetComponent<WaterKeys>().getKey(key.name);
                    Debug.Log(key.name);
                    foreach(Transform child in waterc.transform.GetChild(1))
                    {
                        Debug.Log(child.name);
                        if (child.gameObject.name.Equals(key.name))
                        {
                            Debug.Log("No clue");
                            child.GetChild(0).gameObject.active = true;
                        }
                    }
                }
                else if (!GameManager.Instance.getPlayer().GetComponent<Movement>().isSwimming) 
                {
                    GameManager.Instance.pauseGame();
                    waterc.gameObject.active = true;
                    temple.GetComponent<WaterKeys>().currentPillar = gameObject;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        waterc.gameObject.active = false;
    }
}
