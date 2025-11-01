using TMPro;
using UnityEngine;

public class ScoreController : Controller<ScoreController>
{
    public int player1Score;
    public int player2Score;
    public string winningPlayer;
    public GameObject endScreen;
    public TextMeshProUGUI victoryMessage;

    public void ScorePointForPlayer(int i)
    {
        switch (i)
        {
            case 0:
                if (++player1Score >= 3)
                {
                    winningPlayer = "Player 1";
                    EndGame();
                }
                break;

            case 1:
                if (++player2Score >= 3)
                {
                    winningPlayer = "Player 2";
                    EndGame();
                }
                break;
        }
    }
    
    public void EndGame()
    {
        victoryMessage.text = winningPlayer + " wins!";
        endScreen.SetActive(true);
    }
}