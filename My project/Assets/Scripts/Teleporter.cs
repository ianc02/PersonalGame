using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    public Material sunset;
    public Material black;
    public GameObject light;
    public GameObject cavernprops;
    public GameObject cavernMobs;
    
    // Start is called before the first frame update
    public Vector3 pos;
    
    public IEnumerator waiter()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        yield return wait;
        while (true)
        {
            yield return wait;
            if (GameManager.Instance.getPlayer().transform.position.y < -325)
            {
                break;
            }
            
        }
        GameManager.Instance.getPlayer().GetComponent<Movement>().canMove = true;
        GameManager.Instance.waterLevel.GetComponent<WaterKeys>().speed = 0.4f;
        GameManager.Instance.waterLevel.GetComponent<WaterKeys>().playerFollow = false;
        GameManager.Instance.getPlayer().gameObject.GetComponent<Movement>().gravity = 30;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            if (pos.y < 0)
            {
                RenderSettings.ambientIntensity = 0;
                RenderSettings.skybox = null;
                light.active = false;
                cavernprops.active = true;
                cavernMobs.active = true;
            }
            if (pos.z > 1300)
            {
                other.gameObject.GetComponent<Movement>().canMove = false;
                other.gameObject.GetComponent<Movement>().gravity = 0;
                GameManager.Instance.waterLevel.GetComponent<WaterKeys>().playerFollow = true;
                GameManager.Instance.waterLevel.GetComponent<WaterKeys>().calcDist();
                GameManager.Instance.waterLevel.GetComponent<WaterKeys>().speed = 1f;
                StartCoroutine(waiter());
            }
            else
            {
                RenderSettings.skybox = sunset; 
                RenderSettings.ambientIntensity = 1;

                light.active = true;
                cavernprops.active = false;
                cavernMobs.active = false;
            }
            other.transform.position = pos;
        }
    }
    
}
