using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool("IsWalking");
        bool isRunning = animator.GetBool("IsRunning");
        bool isAttacking = animator.GetBool("IsAttacking");
        
        bool forwardPressed = Input.GetKey("w");
        bool shiftPressed = Input.GetKey("left shift");
        bool spacePressed = Input.GetKey("space");
        float mouseRotation = Input.GetAxis("Mouse Y");
        bool mouse0click = Input.GetMouseButton(0);

        animator.SetBool("IsAttacking", false) ;
        if (mouse0click)
        {
            animator.SetBool("IsAttacking", true);
        }
        if (forwardPressed)
        {
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
            if (mouseRotation >= 0.05)
            {
                animator.SetBool("Rightturn", true);
            }
            if (mouseRotation <= -0.05)
            {
                animator.SetBool("Leftturn", true);
            }
            if (0.05 > mouseRotation && mouseRotation > -0.05)
            {
                animator.SetBool("Rightturn", false);
                animator.SetBool("Leftturn", false);
            }
            // Debug.Log(mouseRotation);
        }
        if (spacePressed)
        {
            animator.SetBool("IsJumping",true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }

        



    }
}
