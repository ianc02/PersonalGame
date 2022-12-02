using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElementalMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float hitDistance;




    private FieldOfView fov;
    private NavMeshAgent agent;
    private Animator animator;
    private bool attacking;
    private bool angry = false;
    private bool chilling = true;
    private bool walking = false;
    private Vector3 newpos;
    private float startwalk;
    private float startchill;
    private float chilltime;
    private Vector3 oldpos;
    private Vector3 stoppos;
    private int health = 50;
    private int coinmin = 20;
    private int coinmax = 30;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo currentClip = animator.GetCurrentAnimatorStateInfo(0);
        if (!angry)
        {
            walkin();
        }
        else
        {
            chilling = false;
            animator.SetBool("Chilling", false);
            float dist = Vector3.Distance(transform.position, player.transform.position);

            if (dist > 50)
            {
                angry = false;
            }
             else if (dist > hitDistance)
            {

                agent.SetDestination(player.transform.position);
                animator.SetBool("Walking", true);
                walking = true;

            }
            else
            {
                agent.Stop();
                agent.ResetPath();
                
                walking = false;
                animator.SetBool("Walking", false);
                transform.LookAt(player.transform);
                if (!currentClip.IsName("Attack02"))
                {
                    attack();
                }
                

            }
        }
        if (fov.canSeePlayer)
        {
            angry = true;
        }
    }

    public void walkin()
    {

        if (!walking)
        {
            float newx = Random.Range(transform.position.x - 40f, transform.position.x + 40f);
            float newz = Random.Range(transform.position.z - 40f, transform.position.z + 40f);
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(newx, -100, newz), Vector3.down, out hit))
            {
                if (Vector3.Distance(transform.position, new Vector3(newx, transform.position.y, newz)) > 3)
                {
                    if (hit.collider.gameObject.name.Equals("Cavern"))
                    {
                        newpos = new Vector3(newx, transform.position.y, newz);
                        agent.SetDestination(newpos);
                        walking = true;
                        startwalk = Time.time;
                        animator.SetBool("Walking", true);
                        chilling = false;
                        animator.SetBool("Chilling", false);
                    }
                }
            }
        }
        else
        {
            if (Time.time - startwalk > 3)
            {
                oldpos = transform.position;
            }
            if (Vector3.Distance(transform.position, newpos) < 1f || Vector3.Distance(transform.position, oldpos) < 0.25f)
            {
                walking = false;
                chilling = true;
                animator.SetBool("Walking", false);
                animator.SetBool("Chilling", true);
                startchill = Time.time;
                chilltime = Random.Range(1f, 5f);
                stoppos = transform.position;
            }
        }
    }

    private void attack()
    {
        attacking = true;
        animator.SetBool("Attacking", true);

    }
    public void damage()
    {
        health -= 5;
        angry = true;
        if (health <= 0)
        {
            GameManager.Instance.spawnCoins((int)Mathf.Round(Random.RandomRange(coinmin, coinmax)), transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }


    
}
