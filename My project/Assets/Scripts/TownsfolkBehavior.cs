using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TownsfolkBehavior : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private Vector3 oripos;
    private Vector3 newpos;
    private Vector3 oldpos;
    private Vector3 stoppos;
    private float startwalk;
    private float startchill;
    private float chilltime;
    private bool walking = false;
    private bool chilling = true;
    private bool scared = false;
    public bool talking;
    public float walkRadius; 


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        oripos = transform.position;
        oldpos = transform.position;
        stoppos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!scared)
        {
            if (!talking)
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
                walking = false;
                chilling = true;
                animator.SetBool("Walking", false);
                animator.SetBool("Chilling", true);
                agent.Stop();
                agent.ResetPath();
                oldpos = transform.position;
                stoppos = transform.position;
                newpos = transform.position;
            }
        }
        else
        {
            //OTHER STUFF
        }
    }

    private void walkin()
    {
        if (!walking)
        {
            float newx = Random.Range(transform.position.x - walkRadius, transform.position.x + walkRadius);
            float newz = Random.Range(transform.position.z - walkRadius, transform.position.z + walkRadius);
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(newx, transform.position.y + 100f, newz), Vector3.down, out hit))
            {
                if (Vector3.Distance(transform.position, new Vector3(newx, transform.position.y, newz)) > 3 && (Vector3.Distance(oripos, new Vector3(newx, transform.position.y, newz)) < 20))
                {

                    if (hit.collider.gameObject.name.Equals("Terrain"))
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
            if (Time.time - startwalk > 5)
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
                chilltime = Random.Range(3f, 10f);
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


    
}
