using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI threeSecondTimerText;
    public TMPro.TextMeshProUGUI fiveMinuteTimerText;
    public string[] chosenScenes; // List of chosen scenes by name

    private float threeSecondTimer = 3f;
    private float fiveMinuteTimer = 300f; // 5 minutes in seconds
    private bool gameStarted = false;
    private bool sceneSwitched = false;

    void Start()
    {
        // Freeze all players
        FreezePlayers(true);

        // Start 3-second countdown coroutine
        StartCoroutine(ThreeSecondCountdown());
    }

    void Update()
    {
        if (gameStarted)
        {
            // Update 5-minute timer
            fiveMinuteTimer -= Time.deltaTime;
            fiveMinuteTimerText.text = string.Format("{0}:{1}", (int)fiveMinuteTimer / 60, (int)fiveMinuteTimer % 60);

            // Check if 5-minute timer is up
            if (fiveMinuteTimer <= 0 && !sceneSwitched) // Check the flag here
            {
                // Load a random scene from the chosen scenes
                int sceneIndex = Random.Range(0, chosenScenes.Length);
                SceneManager.LoadScene(chosenScenes[sceneIndex]);
                sceneSwitched = true; // Set the flag here
            }
        }
    }

    private void FreezePlayers(bool freeze)
    {
        // Assuming players have a Rigidbody component
        Rigidbody[] players = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody player in players)
        {
            player.constraints = freeze ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
        }
    }

    private IEnumerator ThreeSecondCountdown()
    {
        while (threeSecondTimer > 0)
        {
            threeSecondTimerText.text = threeSecondTimer.ToString("0");
            threeSecondTimer -= 1;
            yield return new WaitForSeconds(1f);
        }

        // Unfreeze players
        FreezePlayers(false);
        threeSecondTimerText.text = "";
        gameStarted = true;
    }
}
