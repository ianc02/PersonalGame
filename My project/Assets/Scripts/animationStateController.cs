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
        bool forwardPressed = Input.GetKey("w");
        bool shiftPressed = Input.GetKey("left shift");
        


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
        }
    }
}
