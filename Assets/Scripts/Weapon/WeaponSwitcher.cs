using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class WeaponSwitcher : MonoBehaviour
{
    public int playerIndex = 0; // Player index variable
    public GameObject weapon1; // Regular weapon
    public GameObject weapon2; // Sniper
    public GameObject objectToActivateDuringADS; // Object to activate when ADS with weapon2
    public Camera mainCamera; // Main camera
    public float zoomedFOV = 20f; // Field of View for zoomed-in state
    public float zoomTransitionTime = 0.5f; // Transition time for zooming

    private bool isUsingWeapon1 = true;
    private float originalFOV;
    private MeshRenderer[] weapon2MeshRenderers; // Array of mesh renderers

    private void Start()
    {
        originalFOV = mainCamera.fieldOfView;
        weapon1.SetActive(true);
        weapon2.SetActive(false);
        weapon2MeshRenderers = weapon2.GetComponentsInChildren<MeshRenderer>(); // Get all mesh renderers
        objectToActivateDuringADS.SetActive(false); // Ensure the object is initially deactivated
    }

    private void Update()
    {
        Gamepad gamepad = Gamepad.all.Count > playerIndex ? Gamepad.all[playerIndex] : null;
        if (gamepad == null) return;

        // Switch weapons with "Y" button or north button
        if (gamepad.buttonNorth.wasPressedThisFrame)
        {
            isUsingWeapon1 = !isUsingWeapon1;
            weapon1.SetActive(isUsingWeapon1);
            weapon2.SetActive(!isUsingWeapon1);
        }

        // Aim Down Sights (ADS) with left trigger
        if (gamepad.leftTrigger.wasPressedThisFrame && !isUsingWeapon1)
        {
            SetWeapon2MeshRenderers(false); // Disable mesh renderers on weapon2
            objectToActivateDuringADS.SetActive(true); // Activate the specified object
            StartCoroutine(ZoomIn());
        }

        if (gamepad.leftTrigger.wasReleasedThisFrame && !isUsingWeapon1)
        {
            SetWeapon2MeshRenderers(true); // Enable mesh renderers on weapon2
            objectToActivateDuringADS.SetActive(false); // Deactivate the specified object
            StartCoroutine(ZoomOut());
        }
    }


    private void SetWeapon2MeshRenderers(bool enabled)
    {
        foreach (MeshRenderer renderer in weapon2MeshRenderers)
        {
            renderer.enabled = enabled;
        }
    }

    private IEnumerator ZoomIn()
    {

        
        float startTime = Time.time;
        while (Time.time < startTime + zoomTransitionTime)
        {
            float lerpFactor = (Time.time - startTime) / zoomTransitionTime;
            mainCamera.fieldOfView = Mathf.Lerp(originalFOV, zoomedFOV, lerpFactor);
            yield return null;
        }
        mainCamera.fieldOfView = zoomedFOV;
    }

    private IEnumerator ZoomOut()
    {
        float startTime = Time.time;
        while (Time.time < startTime + zoomTransitionTime)
        {
            float lerpFactor = (Time.time - startTime) / zoomTransitionTime;
            mainCamera.fieldOfView = Mathf.Lerp(zoomedFOV, originalFOV, lerpFactor);
            yield return null;
        }
        mainCamera.fieldOfView = originalFOV;
    }
}
