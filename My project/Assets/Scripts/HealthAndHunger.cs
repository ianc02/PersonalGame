using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthAndHunger : MonoBehaviour
{

    public float health;
    public float hunger;
    public Image healthImage;
    public Image hungerImage;
    public float timeToLoseHunger = 10;

    private float healthMax;
    private float hungerMax;
    // Start is called before the first frame update
    void Start()
    {
        healthMax = health;
        hungerMax = hunger;
        StartCoroutine(treeCheck());
        StartCoroutine(getHungryTime());
    }

    public void Update()
    {
        if (GetComponentInParent<Movement>().isRunning)
        {
            timeToLoseHunger = 5;
        }
    }

    // Update is called once per frame
    public IEnumerator getHungryTime()
    {
        WaitForSeconds wait = new WaitForSeconds(timeToLoseHunger);
        while (true)
        {
            yield return wait;
            changeHunger(-1);
            wait = new WaitForSeconds(timeToLoseHunger);
        }
    }

    public IEnumerator treeCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            treeChecker();
        }
    }

    public void treeChecker()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, 3);
        
        if (rangeChecks.Length > 0)
        {
            GameObject tree = rangeChecks[0].gameObject;
            if (tree.name.Equals("apple") || tree.name.Equals("pear")|| tree.name.Equals("plum"))
            {
                Debug.Log(2);
                GameManager.Instance.addToInventory("Collectable", tree.name);
            }
        }
    }
    public void changeHealth(float value)
    {
        health += value;
        if (health > healthMax)
        {
            health = healthMax;
        }
        if (health <= 0)
        {
            health = 100;
            gameObject.transform.position = new Vector3(471.412048f, 136.537003f, 515.718567f);

        }
        healthImage.fillAmount = Mathf.Clamp(health / healthMax, 0, 1f);
    }


    public void changeHunger(float value)
    {
        if ((float)hunger / (float)hungerMax > 0.85f)
        {
            changeHealth(5);
        }
        hunger += value;
        if (hunger > hungerMax)
        {
            hunger = hungerMax;
        }
        if (hunger < 0)
        {
            changeHealth(-2);
            hunger = 0;
        }
        hungerImage.fillAmount = Mathf.Clamp(hunger / hungerMax, 0, 1f);
        
    }

    public void hungerHelper(string food)
    {
        Debug.Log(food);
        Debug.Log(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
        TextMeshProUGUI t = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
        string e = t.text;
        int num = int.Parse(e);
        if (num > 0)
        {
            num -= 1;
            t.SetText(num.ToString());
            changeHunger(10);
        }
        
    }

    public void usePotion()
    {
        TextMeshProUGUI t = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
        string e = t.text;
        int num = int.Parse(e);
        if (num > 0)
        {
            num -= 1;
            t.SetText(num.ToString());
            changeHealth(20);
        }
    }
}
