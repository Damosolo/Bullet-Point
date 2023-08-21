using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // Load a random scene from the list of scenes
        string[] scenes = { "Skyline" }; // Add your scene names here
        string randomScene = scenes[Random.Range(0, scenes.Length)];
        SceneManager.LoadScene(randomScene);
    }

    public void OpenSettings()
    {
        // You can load a settings scene or open a settings panel
        Debug.Log("Settings Button Clicked");
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
