using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas inventory;
    public Canvas pauseMenu;
    public GameObject player;

    
    void Start()
    {
        
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
}
