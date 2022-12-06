using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHelper : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject buttons;
    public GameObject backButton;
    public GameObject volSlider;
    public GameObject creditText;


    public void quitApplication()
    {
        Application.Quit();
    }

    public void back()
    {
        buttons.active = true;
        backButton.active = false;
        volSlider.active = false;
        creditText.active = false;
    }
    public void hideButtons()
    {
        buttons.active = false;
        backButton.active = true;
    }

    public void settings()
    {
        volSlider.active = true;
        buttons.active = false;
        backButton.active = true;
    }

    public void credits()
    {
        creditText.active = true;
        buttons.active = false;
        backButton.active = true;
    }

    public void play()
    {
       
        GameManager.Instance.mainMenuOff();
    }
}
