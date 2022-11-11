using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenChest : MonoBehaviour
{
    TextMeshPro text;
    private bool lerp;
    private bool open;
    public GameObject tool;
    private Vector3 opos;
    private Vector3 newpos;
    private bool cangrab = false;
    private float orotx;
        // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        open = false;
        if (tool != null)
        {
            opos = tool.transform.position;
            newpos = new Vector3(opos.x, opos.y + 1.5f, opos.z);
        }
        orotx = transform.localRotation.x;
    }

    // Update is called once per frame
    void Update()
    {

        if (lerp || open)
        {
            if (tool != null)
            {
                Vector3 temprot = new Vector3(0, tool.transform.localEulerAngles.y + 2, 0);
                tool.transform.localEulerAngles = temprot;
            }
        }
        if (lerp)
        {
            
            if (!open)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(orotx-90, 0, 0), Time.deltaTime * 1.5f);
                if (tool != null)
                {
                    tool.transform.position = Vector3.Lerp(tool.transform.position, newpos, Time.deltaTime * 0.5f);
                }
                
                if (Mathf.Abs(transform.localEulerAngles.x -270) < 10)
                {
                    lerp = false;
                    open = true;
                }
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(orotx, 0, 0), Time.deltaTime * 1.5f);
                if (tool != null)
                {
                    tool.transform.position = Vector3.Lerp(tool.transform.position, opos, Time.deltaTime * 1f);
                }
                if ( Mathf.Abs(transform.localEulerAngles.x) < 10 || transform.localEulerAngles.x >350)
                {
                    lerp = false;
                    open = false;
                }
            }
        }
        if (open && tool!=null)
        {
            if (Input.GetKey("e"))
            {
                if (tool.name.Equals("lantern"))
                {
                    Destroy(tool);
                    GameManager.Instance.activateLantern();
                    GameManager.Instance.addProgress();
                }
                if (tool.name.Equals("snorkel"))
                {
                    Destroy(tool);
                    GameManager.Instance.canusesnorkel();
                    GameManager.Instance.addProgress();
                }
            }
        }
    }

    

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(1);
        text.enabled = true;
       
    }

    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKey("e"))
        {
            
            lerp = true;
            text.enabled = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (open)
        {
            lerp = true;
        }
    }
}
