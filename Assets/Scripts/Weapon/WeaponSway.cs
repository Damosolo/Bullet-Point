using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivityMultiplier;
    [Header("Controller Settings")]
    [SerializeField] private int controllerIndex; // Specify the controller index without the Range attribute
    private Gamepad gamepad;

    private void Start()
    {
        // Check if the controller index is within the range of connected gamepads
        if (controllerIndex >= 0 && controllerIndex < Gamepad.all.Count)
        {
            gamepad = Gamepad.all[controllerIndex];
        }
        else
        {
            Debug.LogError("Invalid controller index. Make sure the index is within the range of connected gamepads.");
        }
    }

    private void Update()
    {
        if (gamepad == null)
        {
            Debug.LogError("No gamepad found. Make sure a gamepad is connected to the player's input.");
            return;
        }

        // get controller input
        Vector2 stickInput = gamepad.rightStick.ReadValue();
        float controllerX = stickInput.x * sensitivityMultiplier;
        float controllerY = stickInput.y * sensitivityMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-controllerY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(controllerX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);
    }
}
