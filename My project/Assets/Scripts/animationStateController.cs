using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    private bool isWalking;
    private bool isRunning;
    private bool isAttacking;
    private bool isChilling;
    private bool forwardPressed;
    private bool shiftPressed;
    private bool spacePressed;
    private float mouseRotation;
    private bool mouse0click;
    private bool isSwimming;
    private bool canAttack;
    public bool bowEquipped;
    public GameObject arrow;
    private Animator animator;
    private Vector3 temppos;
    private GameObject player;
    private Camera cam;
    private Vector3 camOriPos;
    private Vector3 camSwimPos;
    private Vector3 camBowPos;
    private Vector3 arrowOriPos;
    private Quaternion arrowOriRot;
    AnimatorStateInfo prevclip;
    AnimatorStateInfo currentClip;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = transform.parent.gameObject;
        cam = Camera.main;
        camOriPos = new Vector3(0f, 4f, -6f);
        camSwimPos = new Vector3(0f, 0f, -6f);
        camBowPos = new Vector3(0.0700000003f, 1.19000006f, -0.469999999f);
        arrowOriRot = new Quaternion(-0.152426913f, -0.612612665f, 0.752381146f, 0.188134149f);
        arrowOriPos = new Vector3(-0.0561383814f, -0.0512951128f, 0.0538773574f);

    }

    public IEnumerator attackwait()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        yield return wait;
        temppos = player.transform.position;
    }
    public IEnumerator shootArrow()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        yield return wait;
        arrow.GetComponent<Rigidbody>().useGravity = true;
        arrow.GetComponent<Rigidbody>().isKinematic = false;
        arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * 20f);
        
    }
    // Update is called once per frame
    void Update()
    {

        isSwimming = player.GetComponent<Movement>().isSwimming;
         isWalking = animator.GetBool("IsWalking");
         isRunning = animator.GetBool("IsRunning");
         isAttacking = animator.GetBool("IsAttacking");
        isChilling = animator.GetBool("IsChilling");
        
         forwardPressed = Input.GetKey("w");
         shiftPressed = Input.GetKey("left shift");
         spacePressed = Input.GetKey("space");
         mouseRotation = Input.GetAxis("Mouse Y");
         mouse0click = Input.GetMouseButton(0);

        canAttack = GameManager.Instance.canAttack();
        try
        {
            prevclip = currentClip;
        }
        catch
        {
            
        }
        currentClip = animator.GetCurrentAnimatorStateInfo(0);

        if (currentClip.IsName("Attacking") || bowEquipped)
        {
            animator.SetBool("IsAttacking", false);
            player.transform.position = temppos;
        }
        if (bowEquipped)
        {

            animator.SetBool("HoldingShot", Input.GetMouseButton(0));
            if (Input.GetMouseButtonDown(0))
            {
                arrow.transform.localPosition = arrowOriPos;
                arrow.transform.localRotation = arrowOriRot;
                arrow.GetComponent<Rigidbody>().useGravity = false;
                arrow.GetComponent<Rigidbody>().isKinematic = true;
            }
            if (Input.GetKey("f"))
            {
                animator.SetBool("BowEquipped", false);
                bowEquipped = false;
                cam.GetComponent<CameraCollision>().enabled = true;
                GameManager.Instance.hideCross();
            }
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camBowPos, Time.deltaTime * 3f);
            if (currentClip.IsName("Shoot") && !prevclip.IsName("Shoot"))
            {
                
                GameObject arrow2 = Instantiate(arrow);
                arrow2.transform.SetPositionAndRotation(arrow.transform.position,arrow.transform.rotation);
                //arrow2.transform.parent = null;
                arrow2.GetComponent<Rigidbody>().useGravity = true;
                arrow2.GetComponent<Rigidbody>().isKinematic = false;
                arrow2.GetComponent<Rigidbody>().AddForce(((cam.transform.forward +(cam.transform.up/10f))) * 800);
                arrow2.GetComponent<TrailRenderer>().enabled = true;
                //StartCoroutine(shootArrow());
            }
        }
        else
        {
            if (!isSwimming)
            {
                if (Vector3.Distance(cam.transform.localPosition, camOriPos) > 0.01f)
                {
                    cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camOriPos, Time.deltaTime * 3f);
                }
            }
            else
            {
                if (Vector3.Distance(cam.transform.localPosition, camSwimPos) > 0.01f)
                {
                    cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camSwimPos, Time.deltaTime * 3f);
                }
            }
        }
        if (!isSwimming)
        {
            animator.SetBool("IsSwimming", false);
            if (canAttack)
            {

                if (mouse0click)
                {
                    isChilling = false;
                    animator.SetBool("IsChilling", false);
                    animator.SetBool("IsAttacking", true);
                    temppos = player.transform.position;
                    StartCoroutine(attackwait());
                    if (GameManager.Instance.currentWeapon.name == "bow")
                    {
                        bowEquipped = true;
                        animator.SetBool("BowEquipped", true);
                        cam.GetComponent<CameraCollision>().enabled = false;
                        GameManager.Instance.showCross();

                    }
                    //temppos = player.transform.position;
                }
            }
            if (forwardPressed)
            {
                isChilling = false;
                animator.SetBool("IsChilling", false);
                if (shiftPressed)
                {
                    if (!isRunning)
                    {
                        animator.SetBool("IsRunning", true);
                    }
                }
                else
                {
                    animator.SetBool("IsRunning", false);

                    if (!isWalking)
                    {
                        animator.SetBool("IsWalking", true);
                    }
                }
            }
            else
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", false);
                isChilling = true;
                animator.SetBool("IsChilling", true);
                //if (mouseRotation >= 0.05)
                //{
                //    animator.SetBool("Rightturn", true);
                //}
                //if (mouseRotation <= -0.05)
                //{
                //    animator.SetBool("Leftturn", true);
                //}
                //if (0.05 > mouseRotation && mouseRotation > -0.05)
                //{
                //    animator.SetBool("Rightturn", false);
                //    animator.SetBool("Leftturn", false);
                //}
                // Debug.Log(mouseRotation);
            }
            if (spacePressed)
            {
                animator.SetBool("IsJumping", true);
            }
            else
            {
                animator.SetBool("IsJumping", false);
            }
        }
        else 
        {
            animator.SetBool("IsSwimming", true);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
        }

        

        



    }
}
