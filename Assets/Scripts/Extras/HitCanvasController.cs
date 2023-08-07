using UnityEngine;
using UnityEngine.UI;

public class HitCanvasController : MonoBehaviour
{
    public int playerIndex = 0; // Specify the player index (e.g., 0 for player 1, 1 for player 2, etc.)
    public Canvas hitCanvas;
    public GameObject objectToEnable;
    public float fadeOutTime = 1f;
    private Health playerHealth;

    private void Start()
    {
        // Ensure the hitCanvas is initially disabled
        hitCanvas.enabled = false;
        playerHealth = GetComponent<Health>();
    }

    public void Update()
    {
        if (playerHealth.isHit)
        {
            playerHealth.isHit = false;
            hitCanvas.enabled = true;

            if (objectToEnable != null)
            {
                objectToEnable.SetActive(true);
            }

            Invoke("FadeOutCanvas", 3f);
        }
    }

    private void FadeOutCanvas()
    {
        if (hitCanvas != null)
        {
            Image[] images = hitCanvas.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                image.CrossFadeAlpha(0f, fadeOutTime, false);
            }

            Text[] texts = hitCanvas.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                text.CrossFadeAlpha(0f, fadeOutTime, false);
            }
            objectToEnable.SetActive(false);
           // Invoke("DisableCanvas", fadeOutTime);
        }
    }

    private void DisableCanvas()
    {
        hitCanvas.enabled = false;

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false);
        }

        playerHealth.isHit = false;
    }
}
