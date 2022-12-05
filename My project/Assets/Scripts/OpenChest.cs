using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenChest : MonoBehaviour
{
    TextMeshPro text;
    private bool lerp;
    private bool lootlerp;
    private bool open;
    public GameObject loot;
    private Vector3 opos;
    private Vector3 newpos;
    private bool cangrab = false;
    private float orotx;
        // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        open = false;
        if (loot != null)
        {
            opos = loot.transform.position;
            newpos = new Vector3(opos.x, opos.y + 1.5f, opos.z);
        }
        orotx = transform.localRotation.x;
    }

    // Update is called once per frame
    void Update()
    {

        if (lerp || open)
        {
            if (loot != null && lootlerp)
            {
                
                Vector3 temprot = new Vector3(0, loot.transform.localEulerAngles.y + 2, 0);
                loot.transform.localEulerAngles = temprot;
            }
        }
        if (lerp)
        {
            
            if (!open)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(orotx-90, 0, 0), Time.deltaTime * 1.5f);
                if (loot != null && lootlerp)
                {
                    loot.transform.position = Vector3.Lerp(loot.transform.position, newpos, Time.deltaTime * 0.5f);
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
                if (loot != null && lootlerp)
                {
                    loot.transform.position = Vector3.Lerp(loot.transform.position, opos, Time.deltaTime * 1f);
                }
                if ( Mathf.Abs(transform.localEulerAngles.x) < 10 || transform.localEulerAngles.x >350)
                {
                    lerp = false;
                    open = false;
                }
            }
        }
        if (open && loot!=null)
        {
            if (Input.GetKey("e") )
            {
                Debug.Log(loot.tag);
                Debug.Log(loot.name);
                if (loot.name.Equals("gold"))
                {
                    Destroy(loot);
                    int n = Random.RandomRange(1, 5);
                    for (int i = 0; i < n; i++)
                    {
                        GameManager.Instance.addToInventory("Collectable", "coin");
                    }
                }
                else if (loot.name.Equals("food"))
                {
                    Destroy(loot);
                    int n = Random.RandomRange(1, 5);
                    for (int i = 0; i < n; i++)
                    {
                        GameManager.Instance.addToInventory("Collectable", "coin");
                    }
                    int t = Random.RandomRange(1, 3);
                    int h = Random.RandomRange(2, 4);
                    string fruit = "apple";
                    if (t == 3) { fruit = "apple"; }
                    if (t == 2) { fruit = "plum"; }
                    if (t == 1) { fruit = "pear"; }
                    for (int i = 0; i < h; i++)
                    {
                        GameManager.Instance.addToInventory("Collectable", fruit);
                    }
                }
                else
                {
                    GameManager.Instance.addToInventory(loot.tag, loot.name);
                    Destroy(loot);
                    if (loot.name.Equals("lantern"))
                    {

                        GameManager.Instance.activateLantern();
                        GameManager.Instance.addProgress();
                    }
                    if (loot.name.Equals("snorkel"))
                    {
                        GameManager.Instance.canusesnorkel();
                        GameManager.Instance.addProgress();
                    }
                    if (loot.name.Equals("lensOfTruth"))
                    {

                        GameManager.Instance.activateLensOfTruth();
                        GameManager.Instance.addProgress();
                    }
                    if (loot.name.Equals("medallion"))
                    {

                        GameManager.Instance.activateMedallion();
                        GameManager.Instance.addProgress();
                    }
                }
            }
        }
    }

    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.enabled = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey("e") && !GameManager.Instance.getPlayer().GetComponent<Movement>().isSwimming)
            {

                lerp = true;
                text.enabled = false;
                lootlerp = true;
                if (loot.name.Equals("gold") || loot.name.Equals("food")) { lootlerp = false; }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (open)
            {
                lerp = true;
            }
            text.enabled = false;
        }
    }
}
