using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f; // The maximum health of the object
    private float currentHealth; // The current health of the object

    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Subtract damage from current health

        if (currentHealth <= 0) // If current health is 0 or less
        {
            Die(); // The object dies
        }
    }

    private void Die()
    {
        Destroy(gameObject); // Destroy the object
    }
}
