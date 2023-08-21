using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;

    public Color fullHealthColor = Color.green;
    public Color midHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    private Health healthComponent; // Reference to the Health script
    private int playerIndex; // The player index for this health bar

    public void Setup(Health health, int index)
    {
        healthComponent = health;
        playerIndex = index;
        SetMaxHealth(healthComponent.maxHealth);
        SetHealth(healthComponent.currentHealth);
    }

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        Debug.Log("HealthUpdate");
        UpdateHealthBarColor();
    }

    private void UpdateHealthBarColor()
    {
        float healthPercentage = slider.value / slider.maxValue;

        if (healthPercentage > 0.5f)
        {
            fillImage.color = Color.Lerp(midHealthColor, fullHealthColor, (healthPercentage - 0.5f) * 2f);
        }
        else
        {
            fillImage.color = Color.Lerp(lowHealthColor, midHealthColor, healthPercentage * 2f);
        }
    }
}
