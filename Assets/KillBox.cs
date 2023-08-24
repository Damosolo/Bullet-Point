using UnityEngine;

public class KillBox : MonoBehaviour
{
    public float damageAmount = 999f; // Amount of damage to instantly kill the player

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health healthComponent = other.GetComponent<Health>();

            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damageAmount);
            }
        }
    }
}
