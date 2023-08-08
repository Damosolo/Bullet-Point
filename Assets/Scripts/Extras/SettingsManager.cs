using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SettingsManager : MonoBehaviour
{
    public Slider player1SensitivitySlider;
    public Slider player2SensitivitySlider;
    public GameObject settingsPanel; // Reference to the Settings Panel
    public Button settingsButton; // Reference to the Settings Button

    private void Start()
    {
        // Load saved settings if any
        player1SensitivitySlider.value = PlayerPrefs.GetFloat("Player1Sensitivity", 1.0f);
        player2SensitivitySlider.value = PlayerPrefs.GetFloat("Player2Sensitivity", 1.0f);

        // Set up the click listener for the Settings button
        settingsButton.onClick.AddListener(ShowSettingsPanel);
    }

    private void Update()
    {
        // Check for the B button press to close the settings
        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            HideSettingsPanel();
        }
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true); // Show the panel
    }

    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false); // Hide the panel

        // Set the focus back to the settings button
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(settingsButton.gameObject);
    }

    public void UpdateSensitivity()
    {
        // Rest of the sensitivity update code remains the same
    }
}
