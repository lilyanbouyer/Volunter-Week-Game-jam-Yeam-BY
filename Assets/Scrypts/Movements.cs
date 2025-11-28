using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class NewMonoBehaviourScript : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float climbSpeed = 3.0f;
    public bool isClimbing = false;
    public bool CanGetUp = true;
    public bool isdialogueActive = false;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    [HideInInspector] public bool canMove = true;

    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;
    private float coyoteTimeCounter = 0f;
    private float jumpBufferCounter = 0f;

    public InputActionAsset actionsAsset;
    private InputAction moveAction;
    private InputAction jumpAction;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (actionsAsset == null)
        {
            Debug.LogError("Assigne InputActionAsset dans l'inspecteur !");
            return;
        }
        moveAction = actionsAsset.FindAction("Move");
        jumpAction = actionsAsset.FindAction("Jump");
        moveAction.Enable();
        jumpAction.Enable();
    }

    void Update()
    {
        // If dialogue is active, stop movement and unlock cursor
        if (isdialogueActive)
        {
            canMove = false;
            // disable input actions so they don't trigger while in dialogue
            if (moveAction != null && moveAction.enabled) moveAction.Disable();
            if (jumpAction != null && jumpAction.enabled) jumpAction.Disable();
            // unlock mouse and make visible for UI interaction
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // stop any residual movement
            moveDirection = Vector3.zero;
            return;
        }

        // If we reach here, dialogue is not active: ensure inputs and cursor are restored
        if (!canMove)
        {
            canMove = true;
            if (moveAction != null && !moveAction.enabled) moveAction.Enable();
            if (jumpAction != null && !jumpAction.enabled) jumpAction.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector2 moveInput = moveAction.ReadValue<Vector2>();
            bool isRunning = Keyboard.current.leftShiftKey.isPressed;
            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * moveInput.y : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * moveInput.x : 0;

            Vector3 horizontalMove = (forward * curSpeedX) + (right * curSpeedY);
            Vector3 finalMove = horizontalMove;

        if(isClimbing) {
            float verticalInput = moveInput.y;
            Vector3 climbDirection = new Vector3(0, verticalInput * climbSpeed, 0);
            finalMove.y = climbDirection.y;
        } else {

            if (characterController.isGrounded) {
                coyoteTimeCounter = coyoteTime;
                moveDirection.y = 0f;
            } else {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (jumpAction.triggered && canMove) {
                jumpBufferCounter = jumpBufferTime;
            } else {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
            {
                moveDirection.y = jumpSpeed;
                jumpBufferCounter = 0f;
                coyoteTimeCounter = 0f;
            }
            moveDirection.y -= gravity * Time.deltaTime;
            finalMove.y = moveDirection.y;
        }
        characterController.Move(finalMove * Time.deltaTime);
        if (canMove) {
            float mouseY = 0f;
            float mouseX = 0f;
            if (Mouse.current != null)
            {
                var mouseDelta = Mouse.current.delta.ReadValue();
                mouseY = mouseDelta.y * 10;
                mouseX = mouseDelta.x * 10;
            }
            rotationX += -mouseY * lookSpeed * Time.deltaTime;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, mouseX * lookSpeed * Time.deltaTime, 0);
        }
        // crawl mechanics
        if (Keyboard.current.cKey.isPressed)
        {
            characterController.height = 1.0f;
        }
        else if (CanGetUp)
        {
            characterController.height = 2.0f;
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Ladder") {
            isClimbing = true;
        }
        if (other.gameObject.tag == "ExitCrawl")
        {
            CanGetUp = true;
        }
        if (other.gameObject.tag == "EnterCrawl")
        {
            CanGetUp = false;
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Ladder") {
            isClimbing = false;
        }
    }
}
