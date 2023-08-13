using UnityEngine;
using UnityEngine.UI;

public class HitCanvasController : MonoBehaviour
{
    public int playerIndex = 0; // Specify the player index (e.g., 0 for player 1, 1 for player 2, etc.)
    public GameObject objectToEnable; // Reference to the object to enable
    public float displayTime = 1f; // Time to display the object before fading out
    public float fadeOutTime = 1f; // Time to fade out the object
    private Health playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<Health>();
        if (objectToEnable != null)
        {
            SetObjectAlpha(0f); // Ensure the object is initially hidden
            objectToEnable.SetActive(false); // Initially disable the object
        }
    }

    public void Update()
    {
        if (playerHealth.isHit)
        {
            playerHealth.isHit = false;
            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true); // Enable the object
                SetObjectAlpha(1f); // Show the object
                Invoke("FadeOutObject", displayTime); // Start fading out after displayTime
            }
        }
    }

    private void SetObjectAlpha(float alpha)
    {
        Image[] images = objectToEnable.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }

    private void FadeOutObject()
    {
        Image[] images = objectToEnable.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.CrossFadeAlpha(0f, fadeOutTime, false);
        }

        Invoke("DisableObject", fadeOutTime); // Disable the object after fading out
    }

    private void DisableObject()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false);
        }
    }
}
