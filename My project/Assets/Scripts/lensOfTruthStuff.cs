using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lensOfTruthStuff : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lotText;
    public GameObject waterWall;
    public GameObject exitPillar;


    public void lensOn()
    {
        lotText.active = true;
        waterWall.active = false;
        exitPillar.active = true;

    }
    public void lensOff()
    {
        lotText.active = false;
        waterWall.active = true;
        exitPillar.active = false;
    }
}
