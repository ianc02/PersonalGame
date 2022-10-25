using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform self;
    public Transform player;
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(self.position, player.position) < 20)
        {
            enemy.SetDestination(new Vector3 (player.position.x +1, player. position.y, player.position.z -1));
        }
    }

    public void changeHealth(float hp)
    {
        health += hp;
        Debug.Log(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
