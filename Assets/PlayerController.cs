using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
[Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float jumpSpeed = 2.0f;
    [SerializeField] private float gravity = 10.0f;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float upDownLimit = 65f;


    float currentSpeed;

    private float verticalRotation;

    [SerializeField]private Camera playerCamera;

    private Vector3 currentMovement = Vector3.zero;

    private CharacterController characterController;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = this.GetComponent<CharacterController>();

        playerCamera = GetComponentInChildren<Camera>();
    }


        void Update()
        {
        if (characterController.isGrounded && Input.GetKey("e"))
        {
            currentMovement.y = jumpSpeed;
        }
        currentMovement.y -= gravity * Time.deltaTime;
        characterController.Move(currentMovement * Time.deltaTime);

        if (Input.GetKey("c"))
        {

            characterController.height = 1f;
        }
        else if (Input.GetKeyUp("c"))
        {
            currentSpeed = walkSpeed;
            characterController.height = 1.8f;
        }



        HandleMovement();

            HandleLook();

        }

        void HandleMovement()
        {


        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!Input.GetKey("c"))
            {
                currentSpeed = runSpeed;
            }
        }
        else if (Input.GetKey("c"))
        {
            currentSpeed = crouchSpeed;
        }

        else
        {
            currentSpeed = walkSpeed;
        }


        Vector3 horizontalMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            horizontalMovement = transform.rotation * horizontalMovement;

            currentMovement.x = horizontalMovement.x * currentSpeed;
            currentMovement.z = horizontalMovement.z * currentSpeed;



            characterController.Move(currentMovement * Time.deltaTime);


            if (Input.GetKey(KeyCode.Space))
            {

               // OnTeleport();

                characterController.enabled = false;
                gameObject.transform.position = new Vector3(0, 0.95f, 0);
                characterController.enabled = true;
            }
        }

        void OnTeleport()
        { 
            Vector3 from = gameObject.transform.position;
            Vector3 to = new Vector3(0, 0, 0);

            Vector3 deltaPos = to - from;
            characterController.Move(deltaPos);
        }



        void HandleLook()
        {
            float mouseXrotation = Input.GetAxis("Mouse X") * mouseSensitivity;
            this.transform.Rotate(0, mouseXrotation, 0);

            verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownLimit, upDownLimit);


            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }
}
