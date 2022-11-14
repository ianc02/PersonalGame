using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GronchMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    private Animator animator;

    private bool peaceful = true;
    public bool walking = false;
    public bool chilling = false;
    private Vector3 newpos;
    private float startwalk;
    private float startchill;
    private float chilltime;
    private Vector3 oldpos;
    private Vector3 stoppos;
    private int health = 20;
    private bool attacking;
    public int distance;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo currentClip = animator.GetCurrentAnimatorStateInfo(0);
        
        if (peaceful) 
        {
            if (!chilling)
            {
                walkin();
            }
            else if (!walking)
            {
                chillin();
            }
        }

        else
        {
            chilling = false;
            animator.SetBool("Chilling", false);
            if (Vector3.Distance(transform.position, player.transform.position) > distance)
            {
                
                agent.SetDestination(player.transform.position);
                animator.SetBool("Walking", true);
                walking = true;

            }
            else
            {
                agent.Stop();
                agent.ResetPath();
                transform.LookAt(player.transform);
                walking = false;
                animator.SetBool("Walking", false);
                if (!currentClip.IsName("Attack02"))
                {
                    attack();
                }

            }
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
                chilltime =  Random.Range(1f, 5f);
                stoppos = transform.position;
            }
        }
    }

    public void chillin()
    {
        transform.position = stoppos;
        if (Time.time - startchill > chilltime)
        {
            
            chilling = false;
            animator.SetBool("Chilling", false);
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
        peaceful = false;
        alertHomies();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void makeAngry()
    {
        peaceful = false;
        Debug.Log("LJDSFLKJSDFLSDKLFJLSDKJFLDKSJFKL");
    }


    public void alertHomies()
    {
        Collider[] things = Physics.OverlapSphere(transform.position, 50);
        foreach(Collider ob in things)
        {
            Debug.Log(ob.name);
            if (ob.CompareTag("Gronch"))
            {
                Debug.Log("WTF");
                GetComponentInParent<GronchMovement>().makeAngry();
            }
        }
    }

}
