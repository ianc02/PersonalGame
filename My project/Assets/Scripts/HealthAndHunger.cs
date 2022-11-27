using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndHunger : MonoBehaviour
{

    public float health;
    public float hunger;
    public Image healthImage;
    public Image hungerImage;

    private float healthMax;
    private float hungerMax;
    // Start is called before the first frame update
    void Start()
    {
        healthMax = health;
        hungerMax = hunger;
        StartCoroutine(treeCheck());
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, 3, 8);
        if (rangeChecks.Length > 0)
        {
            GameObject tree = rangeChecks[0].gameObject;
            if (tree.CompareTag("apple") || tree.CompareTag("pear")|| tree.CompareTag("plum"))
            {
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
            gameObject.transform.position = new Vector3(471.412048f, 134.537003f, 515.718567f);

        }
        healthImage.fillAmount = Mathf.Clamp(health / healthMax, 0, 1f);
    }


    public void changeHunger(float value)
    {
        hunger += value;
        if (hunger > hungerMax)
        {
            hunger = hungerMax;
        }
        if (hunger < 0)
        {
            hunger = 0;
        }
        hungerImage.fillAmount = Mathf.Clamp(hunger / hungerMax, 0, 1f);
    }
}
