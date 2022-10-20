using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    // Start is called before the first frame update
    public Canvas inventory;
    public Canvas pauseMenu;
    public GameObject player;
    private Collider pcollider;


    
    void Start()
    {
        pcollider = player.GetComponent<CapsuleCollider>();
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


    public void addToInventory(string objectTag, string objectname)
    {

        
        foreach (Transform child in transform)
        {
            if (child.name == objectTag)
            {
                foreach(Transform grandchild in child.transform)
                {
                    if (grandchild.name == objectname)
                    {
                        int num = int.Parse(grandchild.GetComponentInChildren<TextMeshPro>().text);
                        num += 1;
                        grandchild.GetComponentInChildren<TextMeshPro>().SetText(num.ToString());
                    }
                }
            }
        }
    }
}
