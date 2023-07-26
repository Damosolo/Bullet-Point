using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2;
    public float sprintSpeed = 6;
    public float jumpHeight = 2;
    public float slideSpeed = 8;
    public float slideDuration = 1;
    public float gravity = 9.81f; // Gravity force, pulling down
    public float headBobFrequency = 1.5f;
    public float headBobSwayAngle = 5f;
    public float headBobHeight = 3f;
    public float headBobSideMovement = 5f;
    public float lookSpeed = 2f;
    public float lookSensitivity = 1f;
    public float slideCameraHeight = 0.5f;  // adjust this as per your requirement

    private CharacterController controller;
    private Vector3 velocity;
    private bool isSliding = false;
    private float slideTimer = 0;
    private int jumps = 0;
    private float baseHeight;
    private Vector3 originalCameraPosition;
    private Camera playerCamera;
    private float xRotation = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        baseHeight = controller.height;
        originalCameraPosition = playerCamera.transform.localPosition;
    }

    private void Update()
    {
        float speed = walkSpeed;
        if (Input.GetButton("Sprint"))
        {
            speed = sprintSpeed;
        }

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 direction = transform.TransformDirection(input).normalized;
        velocity = direction * speed;
        velocity.y -= gravity * Time.deltaTime; // Changed += to -= for gravity

        if (controller.isGrounded)
        {
            velocity.y = 0;
            jumps = 0;

            if (Input.GetButtonDown("Jump") && !isSliding)
            {
                velocity.y = Mathf.Sqrt(2 * jumpHeight * gravity); // Changed -gravity to gravity
                jumps++;
            }
        }
        else if (Input.GetButtonDown("Jump") && jumps < 2)
        {
            velocity.y = Mathf.Sqrt(2 * jumpHeight * gravity); // Changed -gravity to gravity
            jumps++;
        }

        if (Input.GetButtonDown("Slide") && !isSliding)
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
                velocity = direction * slideSpeed;
            }
        }

        controller.Move(velocity * Time.deltaTime);

        // Head bobbing
        if (velocity.magnitude > 0 && controller.isGrounded && !isSliding) // Disable head bobbing when sliding
        {
            playerCamera.transform.localPosition += new Vector3(Mathf.Cos(Time.time * headBobFrequency) * headBobSideMovement, Mathf.Abs(Mathf.Sin(Time.time * headBobFrequency)) * headBobHeight, 0);
            playerCamera.transform.localRotation = Quaternion.Euler(playerCamera.transform.localPosition.y * headBobSwayAngle, playerCamera.transform.localPosition.x * headBobSwayAngle, 0);
        }
        else
        {
            playerCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        // Look around
        float lookHorizontal = Input.GetAxis("LookHorizontal") * lookSensitivity;
        float lookVertical = Input.GetAxis("LookVertical") * lookSensitivity;

        if (Mathf.Abs(lookHorizontal) < 0.1f) lookHorizontal = 0;
        if (Mathf.Abs(lookVertical) < 0.1f) lookVertical = 0;

        xRotation -= lookVertical * lookSpeed;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookHorizontal * lookSpeed);
    }
}
