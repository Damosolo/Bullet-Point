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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        baseHeight = controller.height;
        originalCameraPosition = playerCamera.transform.localPosition;
        playerHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (Gamepad.all.Count <= playerIndex) 
            return;

        gamepad = Gamepad.all[playerIndex];

        float speed = walkSpeed;
        if (gamepad.leftStickButton.isPressed)
        {
            speed = sprintSpeed;
        }

        Vector2 stickInput = gamepad.leftStick.ReadValue();
        Vector3 input = new Vector3(stickInput.x, 0, stickInput.y);
        Vector3 direction = transform.TransformDirection(input).normalized;
        Vector3 horizontalMovement = direction * speed;
        Vector3 verticalMovement = Vector3.up * velocity.y;

   
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
        }

       
        controller.Move((horizontalMovement + verticalMovement) * Time.deltaTime);

       
        Vector2 lookInput = gamepad.rightStick.ReadValue();

        xRotation -= lookInput.y * lookSpeed * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x * lookSpeed * lookSensitivity);
    }

    public void Die()
    {
        
        Vector3 respawnPoint = RespawnManager.Instance.GetRandomRespawnPoint();
        controller.enabled = false;
        transform.position = respawnPoint;
        controller.enabled = true;
        Debug.Log(respawnPoint);

        playerHealth.SetHealth(100);
    }

    public Gamepad GetGamepad()
    {
        return gamepad;
    }
}
