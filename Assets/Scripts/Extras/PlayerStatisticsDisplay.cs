using TMPro;
using UnityEngine;

public class PlayerStatisticsDisplay : MonoBehaviour
{
    public TextMeshProUGUI player1KillsText;
    public TextMeshProUGUI player1DeathsText;
    public TextMeshProUGUI player2KillsText;
    public TextMeshProUGUI player2DeathsText;

    public TextMeshProUGUI player1KillsTextSecondDisplay;
    public TextMeshProUGUI player1DeathsTextSecondDisplay;
    public TextMeshProUGUI player2KillsTextSecondDisplay;
    public TextMeshProUGUI player2DeathsTextSecondDisplay;

    private int player1Kills;
    private int player1Deaths;
    private int player2Kills;
    private int player2Deaths;

    private void Start()
    {
        // Initialize statistics
        player1Kills = 0;
        player1Deaths = 0;
        player2Kills = 0;
        player2Deaths = 0;
        UpdateStatsText();
    }

    // Call these methods to update the kills and deaths statistics for each player
    public void AddKillForPlayer1()
    {
        player1Kills++;
        UpdateStatsText();
    }

    public void AddDeathForPlayer1()
    {
        player1Deaths++;
        UpdateStatsText();
    }

    public void AddKillForPlayer2()
    {
        player2Kills++;
        UpdateStatsText();
    }

    public void AddDeathForPlayer2()
    {
        player2Deaths++;
        UpdateStatsText();
    }

    private void UpdateStatsText()
    {
        // Update the TextMeshPro text content with the current statistics for each player
        player1KillsText.text = $" {player1Kills}";
        player1DeathsText.text = $" {player1Deaths}";
        player2KillsText.text = $" {player2Kills}";
        player2DeathsText.text = $" {player2Deaths}";

        player1KillsTextSecondDisplay.text = $" {player1Kills}";
        player1DeathsTextSecondDisplay.text = $" {player1Deaths}";
        player2KillsTextSecondDisplay.text = $" {player2Kills}";
        player2DeathsTextSecondDisplay.text = $" {player2Deaths}";
    }
}
