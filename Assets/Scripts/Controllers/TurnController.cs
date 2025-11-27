using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Events;

public class TurnController : Controller<TurnController>
{   
    [Header("Player Variables")]
    public List<PlayerController> players;
    public List<Vector3> playerStartPositions;
    public List<DetectBulletHit> playerHits;
    public List<GameObject> playerAimPreviews;

    [Header("Timer Variables")]
    public float dodgingPhaseDuration;
    public TextMeshProUGUI timerText;
    public Image timerFillArea;
    public Timer dodgingPhaseTimer;
    public TextMeshProUGUI currentRoundText;
    public TextMeshProUGUI countdownText;

    [Header("Bullet Variables")]
    public ShotsStack shotsStack;

    // Misc variables
    private bool _isDodgingPhaseActive;
    private int playersReadyAmount;
    private int _currentTurn;

    [Header("Audio")]
    [SerializeField] private AudioClip _gunshotClip;
    [SerializeField] private AudioClip _tickingClip;
    private bool _hasPlayedTicking = false;
    private AudioSource _tickingSource = null;

    public UnityEvent OnBeginPlanningPhase;
    public UnityEvent OnBeginDodgingPhase;
    

    protected override void Awake()
    {
        base.Awake();

        // Find all players in the scene
        players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).ToList();

        // Find the shots stack
        shotsStack = FindFirstObjectByType<ShotsStack>();

        // Register the players' start position
        foreach (PlayerController player in players)
        {
            playerStartPositions.Add(player.transform.position);
        }

        // Register the players' aim preview
        foreach (PlayerController player in players)
        {
            playerAimPreviews.Add(player.gameObject.GetComponentInChildren<TAG_AimPivot>().gameObject);
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

        // Subscribe to OnShootingInputEvent for every player
        foreach (PlayerController player in players)
        {
            player.OnShootingInputEvent += (shotDirection, spawnPosition, playerPos) => CheckPlayersReady();
        }

        // Initialize playersReadyAmount
        playersReadyAmount = 0;
    }

    void Start()
    {
        StartSetupPhase();
    }

    void Update()
    {
        // If in the dodging phase
        if (_isDodgingPhaseActive)
        {
            // Count down timer
            dodgingPhaseTimer.CountTimer();

            // Update timer UI
            timerText.text = "" + (int)dodgingPhaseTimer.CurrentTime;
            timerFillArea.fillAmount = dodgingPhaseTimer.CurrentTime / dodgingPhaseDuration;

            if (dodgingPhaseTimer.CurrentTime < 5f && !_hasPlayedTicking)
            {
                _hasPlayedTicking = true;
                _tickingSource = SoundFXManager.instance.PlayAndReturnSoundFXClip(_tickingClip, transform, 3f);
            }
        } else
        {
            _hasPlayedTicking = false;
        }
    }

    public void StartSetupPhase()
    {
        // set _isDodgingPhaseActive to false
        _isDodgingPhaseActive = false;
        if (_tickingSource != null) Destroy(_tickingSource.gameObject);

        // enable player aim previews
        foreach(GameObject aimPreview in playerAimPreviews)
        {
            aimPreview.SetActive(true);
        }

        // increment dodgingPhaseDuration and update timer accordingly
        dodgingPhaseDuration++;
        dodgingPhaseTimer.setNewMax(dodgingPhaseDuration);

        // reset phase timer
        dodgingPhaseTimer.ResetTimer();

        // Update timer UI
        timerText.text = "" + dodgingPhaseDuration;
        timerFillArea.fillAmount = 1f;

        // disable player movement & reset keyboard input & enable bullet previews
        foreach (PlayerController player in players)
        {
            player.AllowMovement = false;
            player.ClearKeyboardInput();
            player.SetShowPreview(true);
        }

        // clear bullets from screen
        shotsStack.ClearBullets();

        // summon previous player ghosts
        shotsStack.ShowGhosts();

        // enable current bullet preview

        // update current turn text
        _currentTurn++;
        currentRoundText.text = "Current Round: " + _currentTurn.ToString();

        OnBeginPlanningPhase.Invoke();

        Debug.Log("Started Setup phase");
    }

    private void CheckPlayersReady()
    {
        if (++playersReadyAmount >= players.Count)
        {   
            StartCoroutine(CountdownToDodgingPhase());
        }
    }

    private IEnumerator CountdownToDodgingPhase()
    {
        countdownText.gameObject.SetActive(true);
        int countdown = 3;

        // Optional: you can use the timerText to display the countdown
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.gameObject.SetActive(false);  

        // Once countdown finishes, start the dodging phase
        StartDodgingPhase();
    }

    public void StartDodgingPhase()
    {
        // reset playersReadyAmount
        playersReadyAmount = 0;

        // disable previous rounds ghosts
        shotsStack.DeleteGhosts();

        // set _isDodgingPhaseActive to true, which starts the phase timer
        _isDodgingPhaseActive = true;

        // enable player movement and disable shot previews
        foreach (PlayerController player in players)
        {
            player.AllowMovement = true;
            player.SetShowPreview(false);
        }

        // disable player aim previews
        foreach(GameObject aimPreview in playerAimPreviews)
        {
            aimPreview.SetActive(false);
            Debug.Log("Disabled: " + aimPreview.name);
        }

        // shoot all bullets
        shotsStack.Shoot();
        SoundFXManager.instance.PlaySoundFXClip(_gunshotClip, transform, 1f);

        OnBeginDodgingPhase.Invoke();

        Debug.Log("Started Dodging phase");
    }

    public void EndTurn(int winningPlayer)
    {
        // reset player positions
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = playerStartPositions[i];
        }

        // delete all previews
        shotsStack.ResetStack();

        // register player score
        ScoreController.Instance.ScorePointForPlayer(winningPlayer);

        // reset current turn text
        _currentTurn = 0;
        currentRoundText.text = "Current Round: " + _currentTurn.ToString();

        Debug.Log("Ended round. " + winningPlayer + " scored");

        // reset dodgingPhaseDuration
        dodgingPhaseDuration = 4;

        // start setup phase
        StartSetupPhase();
    }
}