using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Start is called before the first frame update
    public Canvas inventory;
    public Canvas pauseMenu;
    public GameObject player;
    public Camera cam;
    public Terrain terrain;
    public GameObject rockprefab;
    public GameObject woodprefab;
    private Collider pcollider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        pcollider = player.GetComponent<CapsuleCollider>();
        RaycastHit hit;
        
        for (int i = 0; i < 5001; i++)
        {
            Vector3 pos = new Vector3(Random.Range(2, 1998), 700, Random.Range(2, 1998));
            if (Physics.Raycast(pos, Vector3.down, out hit))
            {
                if (hit.transform.tag != "terrain")
                {
                    i -= 1;
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        Instantiate(rockprefab, hit.point, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(woodprefab, hit.point, Quaternion.identity);
                    }
                } 
            }
                
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            if (inventory.gameObject.active)
            {
                inventory.gameObject.SetActive(false);
                resumeGame();
            }
            else
            {
                inventory.gameObject.SetActive(true);
                pauseGame();
            }
        }
        if (Input.GetKeyDown("escape"))
        {
            if (pauseMenu.gameObject.active)
            {
                pauseMenu.gameObject.SetActive(false);
                resumeGame();
            }
            else
            {
                pauseMenu.gameObject.SetActive(true);
                pauseGame();
            }
        }

        

    }

    public void pauseGame()
    {
        Time.timeScale = 0f;
        player.GetComponent<Movement>().setCanMove(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void resumeGame()
    {
        Time.timeScale = 1f;
        player.GetComponent<Movement>().setCanMove(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void removePauseMenu()
    {
        pauseMenu.gameObject.SetActive(false);
    }
    public Camera getcamera()
    {
        return cam;
    }

    public void addToInventory(string objectTag, string objectname)
    {

        Debug.Log(objectTag);
        Debug.Log(objectname);
        
        foreach (Transform child in inventory.transform)
        {
            Debug.Log(0);
            Debug.Log(child.name);
            if (child.name == objectTag)
            {
                Debug.Log(1);
                foreach(Transform grandchild in child)
                {
                    if (grandchild.name == objectname)
                    {
                        Debug.Log(2);
                        Debug.Log(grandchild.name);
                        GameObject t = grandchild.gameObject;
                        TextMeshProUGUI r = t.GetComponentInChildren<TextMeshProUGUI>();
                        string e = r.text;
                        int num = int.Parse(grandchild.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
                        num += 1;
                        grandchild.GetComponentInChildren<TextMeshProUGUI>().SetText(num.ToString());
                    }
                }
            }
        }
    }
}
