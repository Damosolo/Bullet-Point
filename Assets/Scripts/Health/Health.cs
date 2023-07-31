using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f; // The maximum health

    private float currentHealth; // The current health

    private void Start()
    {
        currentHealth = maxHealth; // Initialize the current health to the maximum health
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Decrease the current health by the damage amount
        Debug.Log(currentHealth);
        // If the current health is less than or equal to zero
        if (currentHealth <= 0)
        {
            Debug.Log("no health");
            // Call the Die method
            GetComponent<PlayerController>().Die();
        }
    }

    public void SetHealth(float amount)
    {
        currentHealth = amount;
    }
}
