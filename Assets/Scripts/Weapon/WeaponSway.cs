using UnityEngine;

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
        float controllerX = Input.GetAxis("LookHorizontal") * sensitivityMultiplier;
        float controllerY = Input.GetAxis("LookVertical") * sensitivityMultiplier;


        Quaternion rotationX = Quaternion.AngleAxis(-controllerY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(controllerX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);
    }
}
