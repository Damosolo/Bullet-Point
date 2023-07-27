using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RaycastShoot : MonoBehaviour
{
    public Camera fpsCamera; // The first person shooter camera
    public float raycastRange = 100f; // The range of the raycast
    public float damage = 10f; // The damage dealt by the raycast
    public Vector2 recoilAmount = new Vector2(2f, 2f); // The maximum upward and sideways kick from recoil
    public Transform adsPositionTransform; // The transform of the ADS position
    public float adsTransitionTime = 0.5f; // The time it takes to transition to ADS

    private Vector3 originalPosition; // The original position of the gun
    private bool isAiming = false; // Whether the player is currently aiming

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        float rightTriggerValue = Gamepad.current.rightTrigger.ReadValue();
        float leftTriggerValue = Gamepad.current.leftTrigger.ReadValue();

        if (leftTriggerValue > 0.1f && !isAiming)
        {
            StartCoroutine(AimDownSights());
        }
        else if (leftTriggerValue <= 0.1f && isAiming)
        {
            StartCoroutine(StopAimingDownSights());
        }

        if (rightTriggerValue > 0.1f)
        {
            Vector3 rayOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)); // Center of the screen
            RaycastHit hit;

            // If we hit something
            if (Physics.Raycast(rayOrigin, fpsCamera.transform.forward, out hit, raycastRange))
            {
                Debug.Log(hit.transform.name);

                // Draw a debug raycast line in the Scene view
                Debug.DrawLine(rayOrigin, hit.point, Color.red);

                // Apply damage to the hit object
                Health health = hit.transform.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
            else
            {
                // Draw a debug raycast line in the Scene view
                Debug.DrawRay(rayOrigin, fpsCamera.transform.forward * raycastRange, Color.green);
            }

            // Vibrate the controller
            Gamepad.current.SetMotorSpeeds(0.5f, 0.5f); // Set both motors to half speed
            Invoke("StopVibration", 0.3f); // Stop the vibration after 0.3 seconds

            // Apply recoil
            fpsCamera.transform.Rotate(-recoilAmount.x * Random.Range(0.5f, 1f), recoilAmount.y * Random.Range(-1f, 1f), 0);
        }
    }

    IEnumerator AimDownSights()
    {
        isAiming = true;
        float startTime = Time.time;
        while (Time.time < startTime + adsTransitionTime)
        {
            float lerpFactor = (Time.time - startTime) / adsTransitionTime;
            transform.localPosition = Vector3.Lerp(originalPosition, adsPositionTransform.localPosition, lerpFactor);
            yield return null;
        }
    }

    IEnumerator StopAimingDownSights()
    {
        isAiming = false;
        float startTime = Time.time;
        while (Time.time < startTime + adsTransitionTime)
        {
            float lerpFactor = (Time.time - startTime) / adsTransitionTime;
            transform.localPosition = Vector3.Lerp(adsPositionTransform.localPosition, originalPosition, lerpFactor);
            yield return null;
        }
    }

    void StopVibration()
    {
        Gamepad.current.SetMotorSpeeds(0f, 0f); // Stop the vibration
    }
}
