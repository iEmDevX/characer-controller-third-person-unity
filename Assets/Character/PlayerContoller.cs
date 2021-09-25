using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravityValue = -8.9f;

    [Header("Movement Speeds")]
    [SerializeField] float warkingSpeed = 1.5f;
    [SerializeField] float runningSpeed = 5f;
    [SerializeField] float sprintingSpeed = 7f;
    [SerializeField] float rotationSpeed = 15f;

    // Movement
    private CharacterController characterController;
    private bool groundedPlayer;
    public Vector3 playerVelocity;
    private Vector3 moveDirection;

    // Animatin
    private PlayerAnimation playerAnimation;

    private Transform cameraObject;
    private CameraManager cameraManager;
    private PlayerInput playerInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimation = GetComponent<PlayerAnimation>();

        cameraManager = FindObjectOfType<CameraManager>();
        playerInput = FindObjectOfType<PlayerInput>();

        cameraObject = Camera.main.transform;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;// keep confined in the game window
        //Cursor.lockState = CursorLockMode.Locked;   // keep confined to center of screen
        //Cursor.lockState = CursorLockMode.None;     // set to default default
    }

    void Update()
    {
        HandleVelocity();
        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleGravity();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
    }

    private void HandleVelocity()
    {
        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }

    private void HandleMovement()
    {
        float moveAnount = Mathf.Clamp01(Mathf.Abs(playerInput.horizontalInput) + Mathf.Abs(playerInput.verticalInput));
        moveDirection = gameObject.transform.forward;
        if (moveAnount <= 0)
        {
            moveDirection = Vector3.zero;
        }
        if (playerInput.sprintingInput)
        {
            moveDirection = moveDirection * sprintingSpeed;
            moveAnount = 2f;
        }
        else if (moveAnount >= 0.5f)
        {
            moveDirection = moveDirection * runningSpeed;
        }
        else
        {
            moveDirection = moveDirection * warkingSpeed;
        }

        characterController.Move(moveDirection * Time.deltaTime);
        playerAnimation.UpdateAnimatorValues(0, Mathf.Abs(moveAnount));

    }

    public void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * playerInput.verticalInput;
        targetDirection = targetDirection + cameraObject.right * playerInput.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotaion = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotaion = Quaternion.Slerp(transform.rotation, targetRotaion, rotationSpeed * Time.deltaTime);
        transform.rotation = playerRotaion;
    }

    private void HandleJump()
    {
        if (playerInput.jumpInput && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    private void HandleGravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

}