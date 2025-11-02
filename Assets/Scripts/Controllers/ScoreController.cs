using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ScoreController : Controller<ScoreController>
{
    public string winningPlayer;
    public GameObject endScreen;
    public TextMeshProUGUI victoryMessage;


    [Header("Player 1 Score Variables")]
    public int player1Score;
    public List<GameObject> player1ScoreObjects;


    [Header("Player 2 Score Variables")]
    public int player2Score;
    public List<GameObject> player2ScoreObjects;


    public void Start()
    {
        foreach (GameObject scoreObject in player1ScoreObjects)
        {
            scoreObject.SetActive(false);
        }
        
        foreach(GameObject scoreObject in player2ScoreObjects)
        {
            scoreObject.SetActive(false);
        }
    }

    public void ScorePointForPlayer(int i)
    {
        switch (i)
        {
            case 0:
                player1Score++;

                if(player1Score < 3)
                {   
                    // Activate the next score object
                    player1ScoreObjects[player1Score - 1].SetActive(true);
                }
                else
                {
                    winningPlayer = "1";
                    EndGame();
                }
                break;

            case 1:
                player2Score++;

                if(player2Score < 3)
                {   
                    // Activate the next score object
                    player2ScoreObjects[player2Score - 1].SetActive(true);
                }
                else
                {
                    winningPlayer = "2";
                    EndGame();
                }
                break;
        }
    }
    
    public void EndGame()
    {
        victoryMessage.text = winningPlayer;
        endScreen.SetActive(true);
        PauseController.Instance.pauseGame();
    }
}