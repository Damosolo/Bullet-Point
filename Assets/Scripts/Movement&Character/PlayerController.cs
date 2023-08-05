using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2;
    public float sprintSpeed = 6;
    public float jumpHeight = 2;
    public float slideSpeed = 8;
    public float slideDuration = 1;
    public float gravity = 9.81f;
    public float lookSpeed = 2f;
    public float lookSensitivity = 1f;
    public float slideCameraHeight = 0.5f;
    public int maxJumpCount = 2;
    public int playerIndex = 0;
    public bool canAct = true;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isSliding = false;
    private float slideTimer = 0;
    private int jumps = 0;
    private float baseHeight;
    private Vector3 originalCameraPosition;
    private Camera playerCamera;
    private float xRotation = 0f;
    private Gamepad gamepad;
    private Health playerHealth;
    private PlayerStatisticsDisplay statsDisplay;


    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    private PlayerInput playerInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        baseHeight = controller.height;
        originalCameraPosition = playerCamera.transform.localPosition;
        playerHealth = GetComponent<Health>();
        statsDisplay = FindObjectOfType<PlayerStatisticsDisplay>();
        playerInput = GetComponent<PlayerInput>();

        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    private void Update()
    {
        if (Gamepad.all.Count <= playerIndex)
            return;

        gamepad = Gamepad.all[playerIndex];

        float speed = walkSpeed;

        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);


        bool runPressed = gamepad.leftStickButton.isPressed; // Running

        HandleInput();
        HandleMovement();



        // If gamepad is true and player runs
        if (isRunning && (runPressed || fowardPressed))
        {
            speed = sprintSpeed;
            animator.SetBool(isRunningHash, true);
        }
        // If gamepad is false then player stops running
        if (isRunning && (!runPressed || !fowardPressed))
        {
            animator.SetBool(isRunningHash, false);
        }



        if (controller.isGrounded)
        {
            jumps = 0;
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                jumps++;
                velocity.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
        }
        else
        {
            if (gamepad.buttonSouth.wasPressedThisFrame && jumps < maxJumpCount)
            {
                jumps++;
                velocity.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
        }


        velocity.y -= gravity * Time.deltaTime;

        if (gamepad.buttonEast.wasPressedThisFrame && !isSliding)
        {
            isSliding = true;
            slideTimer = 0;
            controller.height = baseHeight / 2;
            playerCamera.transform.localPosition = new Vector3(originalCameraPosition.x, originalCameraPosition.y - slideCameraHeight, originalCameraPosition.z);
        }

        if (isSliding)
        {
            slideTimer += Time.deltaTime;
            if (slideTimer >= slideDuration)
            {
                isSliding = false;
                controller.height = baseHeight;
                playerCamera.transform.localPosition = originalCameraPosition;
            }
            else
            {
                horizontalMovement = direction * slideSpeed;
            }

            if (!canAct) return;

            playerHealth = GetComponent<Health>();
            if (playerHealth == null)
            {
                Debug.LogError("Health component not found on player object");
            }
        }


        


        Vector2 lookInput = gamepad.rightStick.ReadValue();

        xRotation -= lookInput.y * lookSpeed * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x * lookSpeed * lookSensitivity);


        if (playerHealth.IsDead())
        {
            if (playerIndex == 0) // assuming playerIndex 0 is player 1
            {
                statsDisplay.AddDeathForPlayer1();
            }
            else if (playerIndex == 1) // assuming playerIndex 1 is player 2
            {
                statsDisplay.AddDeathForPlayer2();
            }
            Die();
        }
    }

    void HandleInput()
    {
        Vector2 stickInput = gamepad.leftStick.ReadValue();
        Vector3 input = new Vector3(stickInput.x, 0, stickInput.y);
        Vector3 direction = transform.TransformDirection(input).normalized;
        Vector3 horizontalMovement = direction * speed;

        controller.Move(input * Time.deltaTime * speed);

    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(movement().x, HandleMovement,y);
    }

    public void Die()
    {
        Debug.Log("PlayerDied");
        Vector3 respawnPoint = RespawnManager.Instance.GetRandomRespawnPoint();
        controller.enabled = false;
        transform.position = respawnPoint;
        controller.enabled = true;
        Debug.Log(respawnPoint);
        Debug.Log("Die method called");

        playerHealth.health = 100;
        Invoke("EnableAct", 1.0f);
    }

    public Gamepad GetGamepad()
    {
        return gamepad;
    }

    public void EnableAct()
    {
        canAct = true;
    }
}
