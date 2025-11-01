using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class TurnController : Controller<TurnController>
{   
    [Header("Player Variables")]
    public List<PlayerController> players;
    public List<Vector3> playerStartPositions;
    public List<DetectBulletHit> playerHits;

    [Header("Timer Variables")]
    public float dodgingPhaseDuration;
    public TextMeshProUGUI timerText;
    public Image timerFillArea;
    public Timer dodgingPhaseTimer;

    // Misc variables
    private bool _isDodgingPhaseActive;

    protected override void Awake()
    {
        base.Awake();

        players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).ToList();

        // Register the players' start position
        foreach (PlayerController player in players)
        {
            playerStartPositions.Add(player.transform.position);
        }

        // Register the players' hit boxes
        foreach (PlayerController player in players)
        {
            playerHits.Add(player.gameObject.GetComponentInChildren<DetectBulletHit>());
        }

        // Create the dodging phase timer
        dodgingPhaseTimer = new Timer(dodgingPhaseDuration, Timer.TimerReset.Manual);

        // Subscribe to the onTimerDone event
        dodgingPhaseTimer.OnTimerDone += StartSetupPhase;

        // Subscribe to onHitEvent on player hit boxes, which ends the round in the opponents favour
        playerHits[0].OnHitEvent += () => EndTurn(1);
        playerHits[1].OnHitEvent += () => EndTurn(0);

    }

    void Start()
    {
        StartSetupPhase();
    }

    void Update()
    {   
        // If in the dodging phase
        if(_isDodgingPhaseActive)
        {
            // Count down timer
            dodgingPhaseTimer.CountTimer();

            // Update timer UI
            timerText.text = "" + (int)dodgingPhaseTimer.CurrentTime;
            timerFillArea.fillAmount = dodgingPhaseTimer.CurrentTime / dodgingPhaseDuration;
        }
    }

    public void StartSetupPhase()
    {
        // set _isDodgingPhaseActive to false
        _isDodgingPhaseActive = false;

        // reset phase timer
        dodgingPhaseTimer.ResetTimer();

        // disable player movement
        foreach(PlayerController player in players)
        {
            player.AllowMovement = false;
        }

        // summon previous player ghosts

        // enable current bullet preview
    }

    public void StartDodgingPhase()
    {
        // set _isDodgingPhaseActive to true, which starts the phase timer
        _isDodgingPhaseActive = true;

        // enable player movement
        foreach(PlayerController player in players)
        {
            player.AllowMovement = true;
        }

        // shoot all bullets


    }

    public void EndTurn(int winningPlayer)
    {
        // reset player positions
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = playerStartPositions[i];
        }

        // delete all previews

        // register player score
        ScoreController.Instance.ScorePointForPlayer(winningPlayer);

        // start setup phase
        StartSetupPhase();
    }
}