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
    public PlayerStatisticsDisplay playerStatisticsDisplay;
    public GameObject muzzleFlashEffect;
    public AudioClip shotSound;
    public AudioClip playerHitSound;
    public AudioSource audioSource;
    public Transform muzzlePosition;

    private Vector3 originalPosition;
    private bool isAiming = false;
    private Coroutine shootCoroutine;
    private float originalLookSpeed;

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalLookSpeed = playerController.lookSpeed;
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
            playerController.lookSpeed = originalLookSpeed / 2;  // Half the look speed when ADS
        }
        else if (leftTriggerValue <= 0.1f && isAiming)
        {
            StartCoroutine(StopAimingDownSights());
            playerController.lookSpeed = originalLookSpeed;  // Restore the look speed when not ADS
        }

        if (rightTriggerValue > 0.1f)
        {
            if (shootCoroutine == null)
            {
                gamepad.SetMotorSpeeds(0.5f, 0.5f);  // Vibrate controller when shooting
                shootCoroutine = StartCoroutine(Shoot());
            }
        }
        else
        {
            if (shootCoroutine != null)
            {
                gamepad.SetMotorSpeeds(0f, 0f);  // Stop vibration when not shooting
                StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
        }
    }

    IEnumerator Shoot()
    {
        while (true)
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
                    Debug.Log("Damage Done");
                    health.TakeDamage(damage);
                    if (health.IsDead())
                    {
                        if (playerController.playerIndex == 1)
                        {
                            playerStatisticsDisplay.AddKillForPlayer1();
                        }
                        else if (playerController.playerIndex == 2)
                        {
                            playerStatisticsDisplay.AddKillForPlayer2();
                        }
                    }

                    // Play the player hit sound
                    if (audioSource != null && playerHitSound != null)
                    {
                        audioSource.PlayOneShot(playerHitSound);
                    }
                }
            }

            // Muzzle flash and shot sound effect
            if (muzzleFlashEffect != null)
            {
                GameObject muzzleFlashInstance = Instantiate(muzzleFlashEffect, muzzlePosition.position, Quaternion.identity);
                Destroy(muzzleFlashInstance, .5f);
            }

            if (audioSource != null && shotSound != null)
            {
                audioSource.PlayOneShot(shotSound);
            }

            yield return new WaitForSeconds(0.1f); // Adjust this for the rate of fire
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

        // After the while loop, force the position to be exactly the adsPosition
        transform.localPosition = adsPositionTransform.localPosition;
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
}
