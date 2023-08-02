using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class TabManager : MonoBehaviour
{
    public List<GameObject> player1TabObjects; // List of objects to enable/disable on tab press for Player 1
    public List<GameObject> player2TabObjects; // List of objects to enable/disable on tab press for Player 2
    public TextMeshProUGUI sceneNameText; // Text field to display current scene name
    public PlayerController player1Controller; // PlayerController for Player 1
    public PlayerController player2Controller; // PlayerController for Player 2

    private bool isTab1Pressed = false;
    private bool isTab2Pressed = false;

    private void Update()
    {
        var gamepad1 = player1Controller.GetGamepad(); // get the current gamepad for Player 1
        var gamepad2 = player2Controller.GetGamepad(); // get the current gamepad for Player 2

        if (gamepad1 != null)
        {
            bool isSelectPressed = gamepad1.selectButton.isPressed; // Access the "Select" button for Player 1

            if (isSelectPressed && !isTab1Pressed) // if the "Select" button is being pressed down
            {
                isTab1Pressed = true;

                foreach (var obj in player1TabObjects)
                {
                    obj.SetActive(true); // Enable all the objects in the list for Player 1
                }

                // Set the text to the current scene's name
                sceneNameText.text = SceneManager.GetActiveScene().name;
            }
            else if (!isSelectPressed && isTab1Pressed) // if the "Select" button is not being pressed down
            {
                isTab1Pressed = false;

                foreach (var obj in player1TabObjects)
                {
                    obj.SetActive(false); // Disable all the objects in the list for Player 1
                }
            }
        }

        if (gamepad2 != null)
        {
            bool isSelectPressed = gamepad2.selectButton.isPressed; // Access the "Select" button for Player 2

            if (isSelectPressed && !isTab2Pressed) // if the "Select" button is being pressed down
            {
                isTab2Pressed = true;

                foreach (var obj in player2TabObjects)
                {
                    obj.SetActive(true); // Enable all the objects in the list for Player 2
                }

                // Set the text to the current scene's name
                sceneNameText.text = SceneManager.GetActiveScene().name;
            }
            else if (!isSelectPressed && isTab2Pressed) // if the "Select" button is not being pressed down
            {
                isTab2Pressed = false;

                foreach (var obj in player2TabObjects)
                {
                    obj.SetActive(false); // Disable all the objects in the list for Player 2
                }
            }
        }
    }
}
