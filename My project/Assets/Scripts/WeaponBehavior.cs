using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            Debug.Log("Hit!");
            other.gameObject.GetComponent<EnemyMovement>().changeHealth(-damage);
        }
    }
}