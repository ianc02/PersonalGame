using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterKeys : MonoBehaviour
{
    // Start is called before the first frame update
    private Canvas waterc;
    public GameObject topWater;
    public GameObject bottomWater;
    public GameObject currentPillar;

    public bool p3;
    public bool p1;
    public bool m1;
    public bool p6;
    public bool lerp;
    public float dist;
    private void Start()
    {
        waterc = GameManager.Instance.waterlevelCanvas;
    }
    private void Update()
    {
        if (lerp)
        {
            topWater.transform.localPosition = Vector3.Lerp(topWater.transform.localPosition, new Vector3(topWater.transform.localPosition.x, (-0.6f + (dist * 2f)), topWater.transform.localPosition.z), Time.deltaTime );
            bottomWater.transform.localPosition = Vector3.Lerp(bottomWater.transform.localPosition, new Vector3(bottomWater.transform.localPosition.x, (-0.6f + (dist * 2f)), bottomWater.transform.localPosition.z), Time.deltaTime );
            if (Vector3.Distance(topWater.transform.localPosition, new Vector3(topWater.transform.localPosition.x, (-0.6f + (dist * 2f)))) < 0.01)
            {
                lerp = false;
            }
        }
    }


    public void addToPillar(string keyShape)
    {
        GameObject selected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameManager.Instance.resumeGame();
        if (keyShape.Equals(currentPillar.gameObject.GetComponent<pillarCollisionDetection>().keyShape))
        {
            currentPillar.transform.GetChild(0).gameObject.active = true;
            currentPillar.transform.GetChild(0).gameObject.name = selected.name;
            
            loseKey(selected.name);
            selected.transform.GetChild(0).gameObject.active = false;
            calcDist();
            lerp = true;

        }
        GameManager.Instance.waterlevelCanvas.gameObject.active = false;
    }

    public void calcDist()
    {
        dist = 0;
        if (p3)
        {
            dist += 3;
        }
        if (p1)
        {
            dist += 1;
        }
        if (m1)
        {
            dist -= 1;
        }
        if (p6)
        {
            dist += 6;
        }
    }
    public void getKey(string n)
    {
        if (n.Equals("p3"))
        {
            p3 = false;
        }
        if (n.Equals("p1"))
        {
            p1 = false;
        }
        if (n.Equals("m1"))
        {
            m1 = false;
        }
        if (n.Equals("p6"))
        {
            p6 = false;
        }
    }
    public void loseKey(string n)
    {
        if (n.Equals("p3"))
        {
            p3 = true;
        }
        if (n.Equals("p1"))
        {
            p1 = true;
        }
        if (n.Equals("m1"))
        {
            m1 = true;
        }
        if (n.Equals("p6"))
        {
            p6 = true;
        }
    }
}
