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
    public bool m3;
    public bool p6;
    private void Start()
    {
        waterc = GameManager.Instance.waterlevelCanvas;
    }
}
