using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway1 : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivityMultiplier;
    private Gamepad gamepad;

    private void Start()
    {
        // Get the main gamepad connected to the player's input
        gamepad = Gamepad.current;
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
