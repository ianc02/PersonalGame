using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

//Obtained from https://sharpcoderblog.com/blog/unity-3d-fps-controller
public class Movement : MonoBehaviour
{
    public GameObject body;
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpSpeed;
    public float gravity;
    public float swimspeed;
    public float upmitigator;
    
    public Camera playerCamera;
    public float lookSpeed;
    public float lookXLimit;
    public float lookXLimitwater;
    public float xMin;
    public float xMax;
    public float zMin;
    public float zMax;
    public bool isRunning;
    public bool isSwimming = false;


    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

    [HideInInspector] 
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.5f);
        moveDirection.y = jumpSpeed;
    }
    void Update()
    {
        if (!isSwimming)
        {
            if (transform.rotation.x != 0)
            {
                transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
            }
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            // Press Left Shift to run
            isRunning = Input.GetKey(KeyCode.LeftShift);

            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                if (!Input.GetKey("w"))
                {
                    StartCoroutine(waiter());
                }
                else
                {
                    moveDirection.y = jumpSpeed;
                    //ADD SO THAT WHEN LAND RESET JUMP ANIMATION BOOL
                }
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);



            // Player and Camera rotation
            if (canMove)
            {

                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
                if (!Input.GetKey("w"))
                {
                    body.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }


            }
        }
        else
        {
            moveDirection = Vector3.zero;
            if (Input.GetKey("w"))
            {
                moveDirection = (Camera.main.transform.forward * swimspeed) + (Camera.main.transform.up / upmitigator) ;
            }
            if (!characterController.isGrounded)
            {
                moveDirection.y -= 1 * Time.deltaTime;

            }
            characterController.Move(moveDirection * Time.deltaTime);
            if (canMove)
            {

                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimitwater, lookXLimitwater);
                rotationY += Input.GetAxis("Mouse X") * lookSpeed;
                //rotationY = Mathf.Clamp(rotationY, -360, 360);
                playerCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
                //float xrot = Mathf.Clamp(Input.GetAxis("Mouse X"), -70, 70);
                
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
                transform.rotation = Quaternion.Euler(rotationX, rotationY, transform.rotation.z);



                if (!Input.GetKey("w"))
                {
                    body.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }


            }

        }
    }
    public void setCanMove(bool b)
    {
        canMove = b;
    }
}