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
    private float timer;
    Animator animator;
    // Start is called before the first frame update

    //IEnumerator waiter()
    //{
    //    Debug.Log("In waiter");
    //    yield return new WaitForSeconds(2f);
    //    animator.SetBool("attacked", false);
    //}
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool casting = animator.GetBool("isCasting");
        //bool walking = animator.GetBool("isWalking");
        if (Vector3.Distance(self.position, player.position) < 30)
        {   
            if (Vector3.Distance(self.position, player.position) < 10)
            {
                if (!casting)
                {
                    if (animator.GetBool("attacked") && animator.GetCurrentAnimatorStateInfo(0).IsName("Spell Cast End"))
                    {
                        animator.SetBool("attacked", false);
                    }
                    else
                    {
                        enemy.SetDestination(self.position);
                        animator.SetBool("isWalking", false);
                        animator.SetBool("isCasting", true);
                        Invoke("attack", 3f);
                    }
                }
                
            }
            else
            {
                enemy.SetDestination(new Vector3(player.position.x + 2, player.position.y, player.position.z - 2));
                animator.SetBool("isWalking", true);
                animator.SetBool("isCasting", false);
            }
            
        }
        else
        {
            animator.SetBool("isCasting", false);
            animator.SetBool("isWalking", false);
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


    public void attack()
    {
        animator.SetBool("isCasting", false);
        animator.SetBool("attacked", true);
        
    }
}
