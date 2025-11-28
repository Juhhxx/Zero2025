using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class ScoreController : Controller<ScoreController>
{
    public string winningPlayer;
    public GameObject endScreen;
    public TextMeshProUGUI victoryMessage;
    [SerializeField] private AudioResource _turnEndClip;
    [SerializeField] private AudioResource _gameEndClip;


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
                    SoundFXManager.instance.PlaySoundFXResource(_turnEndClip, transform, 0.5f, 2.16f);
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
                    SoundFXManager.instance.PlaySoundFXResource(_turnEndClip, transform, 0.5f, 2.16f);
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
        StartCoroutine(EndOfGameBells());
        victoryMessage.text = winningPlayer;
        endScreen.SetActive(true);

        // Change camera renderer to one that doesnt contain the green vignette
        CameraController.Instance.ChangeToMenuCameraRenderer();
    }
    
    private IEnumerator EndOfGameBells()
    {
        int countdown = 3;

        // Optional: you can use the timerText to display the countdown
        while (countdown > 0)
        {
            SoundFXManager.instance.PlaySoundFXResource(_gameEndClip, transform, 0.5f, 2.16f);
            yield return new WaitForSeconds(0.3f);
            countdown--;
        }
    
    }
}