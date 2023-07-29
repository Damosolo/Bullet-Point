using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RaycastShoot : MonoBehaviour
{
    public Camera fpsCamera; 
    public float raycastRange = 100f; 
    public float damage = 10f; 
    public Vector2 recoilAmount = new Vector2(2f, 2f); 
    public Transform adsPositionTransform; 
    public float adsTransitionTime = 0.5f; 
    public PlayerController playerController; 

    private Vector3 originalPosition; 
    private bool isAiming = false; 

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        Gamepad gamepad = playerController.GetGamepad();

        if (gamepad == null)
        {
            Debug.LogWarning("No gamepad assigned to the player controller.");
            return;
        }

        float rightTriggerValue = gamepad.rightTrigger.ReadValue();
        float leftTriggerValue = gamepad.leftTrigger.ReadValue();

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
            Vector3 rayOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)); 
            RaycastHit hit;

            
            if (Physics.Raycast(rayOrigin, fpsCamera.transform.forward, out hit, raycastRange))
            {
                Debug.Log(hit.transform.name);

               
                Debug.DrawLine(rayOrigin, hit.point, Color.red);

               
                Health health = hit.transform.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
            else
            {
               
                Debug.DrawRay(rayOrigin, fpsCamera.transform.forward * raycastRange, Color.green);
            }

            
            gamepad.SetMotorSpeeds(0.5f, 0.5f);
            Invoke("StopVibration", 0.3f);

           
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
        playerController.GetGamepad().SetMotorSpeeds(0f, 0f); 
    }
}
