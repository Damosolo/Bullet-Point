using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivityMultiplier;

    private Quaternion refRotation;

    private float xRotation;
    private float yRotation;

    private void Update()
    {
        // get controller input
        Vector2 stickInput = Gamepad.current.rightStick.ReadValue();
        float controllerX = stickInput.x * sensitivityMultiplier;
        float controllerY = stickInput.y * sensitivityMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-controllerY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(controllerX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);
    }
}
