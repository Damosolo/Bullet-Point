using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100f;
    public PlayerStatisticsDisplay playerStatisticsDisplay;
    public PlayerController playerController;

    RagdollScript ragdollScript;
    public bool isHit = false;

    private void Start()
    {
        ragdollScript = GameObject.Find("Character").GetComponent<RagdollScript>();
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("TakeDamage called, damage amount: " + amount);
        health -= amount;
        Debug.Log("Current health: " + health);
        isHit = true;

        if (health <= 0f)
        {
            Debug.Log("Health is zero or less");
            if (playerController.playerIndex == 0)
            {
                playerStatisticsDisplay.AddDeathForPlayer1();
                playerStatisticsDisplay.AddKillForPlayer2(); // Add kill for Player 2 when Player 1 dies
                ragdollScript.RagdollOn();
            }
            else if (playerController.playerIndex == 1)
            {
                playerStatisticsDisplay.AddDeathForPlayer2();
                playerStatisticsDisplay.AddKillForPlayer1(); // Add kill for Player 1 when Player 2 dies
            }

            playerController.Die(); // Call Die() here before resetting health.

            health = 100f;
            isHit = false; // Reset the isHit flag after dying.
        }
    }

    public bool IsDead()
    {
        bool isDead = health <= 0f;
        Debug.Log("IsDead: " + isDead);
        return isDead;
    }
}
