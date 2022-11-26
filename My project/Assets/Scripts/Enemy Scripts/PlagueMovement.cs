using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlagueMovement : MonoBehaviour
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
    private Vector3 oripos;
    public bool particleGone = true;
    private float particleStartTime;
    public int health = 20;
    public GameObject particles;
    private ParticleSystem particle;
    private Vector3 particleOriPos;
    public float spd;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();
        oripos = transform.position;
        oldpos = transform.position;
        stoppos = transform.position;
        particle = particles.GetComponent<ParticleSystem>();
        particleOriPos = particles.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo currentClip = animator.GetCurrentAnimatorStateInfo(0);
        if (!angry)
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
                attacking = false;
                animator.SetBool("Attacking", false);
                particle.emissionRate = 1;
                particles.active = false;
                particleGone = true;
                animator.SetBool("ParticleGone", true);
                

            }
            else
            {
                agent.Stop();
                agent.ResetPath();

                walking = false;
                animator.SetBool("Walking", false);
                transform.LookAt(player.transform);
                if (!currentClip.IsName("Casting"))
                {
                    attack();
                }
                if (currentClip.IsName("Casting") || currentClip.IsName("Cast Spell"))
                {
                    
                    particles.active = true;
                    particle.emissionRate = particle.emissionRate + 1;

                }
                else if (currentClip.IsName("Spell Cast End"))
                {
                    if (particleGone && particles.active)
                    {
                        particleStartTime = Time.time;
                        particleGone = false;
                        animator.SetBool("ParticleGone", false);
                    }
                    
                    particles.transform.position = Vector3.MoveTowards(particles.transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), spd*Time.deltaTime);
                    
                    Debug.Log(Time.time - particleStartTime);
                    if (Time.time - particleStartTime > 3f || (particleGone && !particles.active))
                    {
                        particleGone = true;
                        animator.SetBool("ParticleGone", true);
                        particle.emissionRate = 1;
                        particles.active = false;
                        particles.transform.localPosition = particleOriPos;
                    }
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
            float newx = Random.Range(transform.position.x - 30f, transform.position.x + 30f);
            float newz = Random.Range(transform.position.z - 30f, transform.position.z + 30f);
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(newx, transform.position.y + 100f, newz), Vector3.down, out hit))
            {
                if (Vector3.Distance(transform.position, new Vector3(newx, transform.position.y, newz)) > 3 && (Vector3.Distance(oripos, new Vector3(newx, transform.position.y, newz)) < 40))
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
            Destroy(gameObject);
        }
    }



}
