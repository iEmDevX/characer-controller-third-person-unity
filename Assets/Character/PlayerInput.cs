using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float verticalInput;
    public float horizontalInput;
    public float cameraInputX;
    public float cameraInputY;
    public bool jumpInput;
    public bool sprintingInput;

    private void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
        HandleSprintingInput();
    }

    private void HandleMovementInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        cameraInputX = Input.GetAxis("Mouse X");
        cameraInputY = Input.GetAxis("Mouse Y");
    }

    private void HandleJumpInput()
    {
        if (Input.GetButton("Jump"))
        {
            jumpInput = true;
        }
        else
        {
            jumpInput = false;
        }
    }

    private void HandleSprintingInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprintingInput = true;
        }
        else
        {
            sprintingInput = false;
        }
    }
}
