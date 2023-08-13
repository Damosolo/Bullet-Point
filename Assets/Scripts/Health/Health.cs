using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f; // Rename the 'health' variable to 'maxHealth'
    public float currentHealth = 100f; // Rename the 'health' variable to 'currentHealth'
    public PlayerStatisticsDisplay playerStatisticsDisplay;
    public PlayerController playerController;

    RagdollScript ragdollScript;
    public bool isHit = false;

    private void Start()
    {
        //ragdollScript = GameObject.Find("Character").GetComponent<RagdollScript>();
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("TakeDamage called, damage amount: " + amount);
        currentHealth -= amount; // Update 'health' to 'currentHealth'
        Debug.Log("Current health: " + currentHealth);
        isHit = true;

        if (currentHealth <= 0f) // Update 'health' to 'currentHealth'
        {
            Debug.Log("Health is zero or less");
            if (playerController.playerIndex == 0)
            {
                playerStatisticsDisplay.AddDeathForPlayer1();
                playerStatisticsDisplay.AddKillForPlayer2(); // Add kill for Player 2 when Player 1 dies
                //ragdollScript.RagdollOn();
            }
            else if (playerController.playerIndex == 1)
            {
                playerStatisticsDisplay.AddDeathForPlayer2();
                playerStatisticsDisplay.AddKillForPlayer1(); // Add kill for Player 1 when Player 2 dies
            }

            playerController.Die(); // Call Die() here before resetting health.

            currentHealth = maxHealth; // Update 'health' to 'currentHealth'
            isHit = false; // Reset the isHit flag after dying.
        }
    }

    public bool IsDead()
    {
        bool isDead = currentHealth <= 0f; // Update 'health' to 'currentHealth'
        Debug.Log("IsDead: " + isDead);
        return isDead;
    }
}
