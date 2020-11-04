using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Constants;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    [SerializeField]
    private GameState currentGameState, targetGameState;
    private Difficulty difficulty;
    public Difficulty Difficulty { get { return difficulty; } }

    private bool gameStateUpdating;
    private float loadDuration;

    private GameObject speedometer;

    private Canvas victoryMenu;

    [SerializeField, Header("Current amount of money")]
    private int moneyTotal;
    private int moneyCurrent;
    public int CheckWallet { get { return moneyTotal; } }

    private CoffeeMeterMechanic coffee_O_Meter;

    private void Awake()
    {
        // Make the Game Manager become a Singleton
        Singletonize();

        // Setup before loading the Main Menu
        PreloadSetup();

        loadDuration = 1f;
    }

    void Start()
    {
        // Listen to Player events
        Player.main.OnCollectToken += PlayerCollectedACoin;

        // Initialize money at game start
        moneyTotal = 10000;

        // Initialize beginning difficulty
        difficulty = Difficulty.Kindergarten;

        targetGameState = GameState.Start;
        gameStateUpdating = true;
        LoadRoutine();
    }
    private void Singletonize()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Game Manager created");
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    private void PreloadSetup()
    {
        // Add event system for UI
        gameObject.AddComponent<EventSystem>();
        gameObject.AddComponent<StandaloneInputModule>();

        UI_UpgradeMenu.main.SetCanvasAtive(false);
        UI_RunResultScreen.main.SetCanvasAtive(false);

        // Get Victory canvas - uhh singletonize this too?
        victoryMenu = GameObject.Find("Victory Menu").GetComponent<Canvas>();
        victoryMenu.enabled = false;
    }

    private void Update()
    {
        if (gameStateUpdating)
        {
            GameStateMonitoring();
        }
    }

    private void GameStateMonitoring()
    {
        switch (currentGameState)
        {
            case GameState.Loading: LoadRoutine();      break;
            case GameState.Start:   MainMenuRoutine();  break;
            case GameState.New:     NewGameRoutine();   break;
            case GameState.Playing: PerformGameplay();  break;
            case GameState.Pausing: PauseGamePlay();    break;
            case GameState.Lose:    ShowSummary();      break;
            case GameState.Win:     ShowVictory();      break;
        }
    }

    private void LoadRoutine()
    {
        UI_MainMenu.main.FadeOut(loadDuration + 1f);

        UI_LoadingScreen.main.SetCanvasActiveWithDelay(true, loadDuration);
        UI_LoadingScreen.main.SetCanvasActiveWithDelay(false, loadDuration + 1.5f);
        
        Player.main.IsKinematic(true);
        Player.main.SetActive(false);
        Player.main.ResetHealth();
        Player.main.ResetCaffeineLevel();

        switch (targetGameState)
        {
            case GameState.Start:
                StartCoroutine(LoadSceneWithDelay(SceneName.MAINMENU, loadDuration));

                break;
            case GameState.New:
                StartCoroutine(LoadSceneWithDelay(SceneName.GAME, loadDuration));
                break;
        }

        StartCoroutine(SwitchGameStateWithDelay(targetGameState, loadDuration));

        gameStateUpdating = false;
    }

    private void MainMenuRoutine()
    {
        UI_Gameplay_Mechanic.main.SetCanvasActive(false);
        UI_MainMenu.main.FadeIn(1f);

        gameStateUpdating = false;
    }

    private void NewGameRoutine()
    {
        StartCoroutine(SwitchGameStateWithDelay(GameState.Playing, 1f));

        UI_Gameplay_Mechanic.main.transform.Find("Coffee-O-Meter").TryGetComponent(out coffee_O_Meter);
        UI_Gameplay_Mechanic.main.SetCanvasActive(true);

        UI_UpgradeMenu.main.SetCanvasAtive(false);
        UI_RunResultScreen.main.SetCanvasAtive(false);
        victoryMenu.enabled = false;

        Player.main.SetActive(true);
        Player.main.IsKinematic(false);
        Player.main.AssignVault();

        speedometer = GameObject.Find("Speedometer");

        moneyCurrent = 0;

        gameStateUpdating = false;
    }

    private void PerformGameplay()
    {
        gameStateUpdating = true;

        if (difficulty == Difficulty.Victory)
        {
            currentGameState = GameState.Win;
        }

        CheckDifficulty();
        CheckUpgradeAvailability();
        CheckPlayerHealth();
        CheckPlayerCaffeineLevel();
    }

    private void PauseGamePlay()
    {
        UI_UpgradeMenu.main.SetCanvasAtive(true);
    }

    private void ShowHowToPlay(bool value)
    {

    }

    private void ShowSummary()
    {
        // Show lose game screen
        UI_RunResultScreen.main.SetCanvasAtive(true);
        UI_RunResultScreen.main.SetSummaryText("Distance: " + speedometer.GetComponent<Speedometer>().Distance + " km\n\n" + "Coins: " + moneyCurrent);
    }

    private void ShowVictory()
    {
        victoryMenu.enabled = true;
    }

    private void CheckUpgradeAvailability()
    {
        for (int i = 0; i < UpgradeData.main.Stats.Length; i++)
        {
            if (moneyTotal < UpgradeData.main.Stats[i].cost)
            {
                UpgradeData.main.Stats[i].available = false;
            }
            else UpgradeData.main.Stats[i].available = true;
        }
    }

    private void CheckPlayerHealth()
    {
        if (Player.main.CurrentHealth <= 0)
        {
            currentGameState = GameState.Lose;
        }
    }

    private void CheckPlayerCaffeineLevel()
    {
        if (Player.main.CaffeineCurrentLevel <= 0)
        {
            currentGameState = GameState.Lose;
        }
    }

    private void CheckDifficulty()
    {
        //DIFFICULTY CHART:
        // distance     difficulty
        //0-99         kindergarten   - easiest
        //100-174      decent
        //175-224      engaged
        //225-324      difficult
        //325-449      lightspeed  - hardest

        float distance = speedometer.GetComponent<Speedometer>().Distance;

        if (distance < 100f && difficulty != Difficulty.Kindergarten) difficulty = Difficulty.Kindergarten;
        else if (distance >= 100f && distance <= 174f && difficulty != Difficulty.Decent) difficulty = Difficulty.Decent;
        else if (distance >= 175f && distance <= 224f && difficulty != Difficulty.Engaged) difficulty = Difficulty.Engaged;
        else if (distance >= 225f && distance <= 324f && difficulty != Difficulty.Difficult) difficulty = Difficulty.Difficult;
        else if (distance >= 325f && distance <= 449f && difficulty != Difficulty.Lightspeed) difficulty = Difficulty.Lightspeed;
        else if (distance >= 450f && difficulty != Difficulty.Victory) difficulty = Difficulty.Victory;
    }

    private IEnumerator SwitchGameStateWithDelay(GameState state, float delay)
    {
        yield return new WaitForSeconds(delay);
        currentGameState = state;
        gameStateUpdating = true;
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(delay / 2);

        SoundController.main.SwitchBGM();
        SoundController.main.PlayBGM();
    }

    private void PlayerCollectedACoin(object sender, EventArgs e)
    {
        // A coin equals a unit of money
        AddMoney(1);
        moneyCurrent++;
    }

    public void AddMoney(int amount)
    {
        moneyTotal += amount;
    }

    public void PurchaseUpgrade(int statID)
    {
        // Request upgrade
        if (UpgradeData.main.CheckUpgradeAvailability(statID) && moneyTotal >= UpgradeData.main.Stats[statID].cost)
        {
            // Purchase & deduct money
            AddMoney(-UpgradeData.main.PurchaseUpgrade(statID));

            // Update upgrade stat
            UpgradeData.main.UpdateStat(statID);

            // Update gameplay stat
            UpgadeGameplayStats(statID);
        }
    }

    private void UpgadeGameplayStats(int statID)
    {
        switch (statID)
        {
            case 0:
                Player.main.UpgradeFuelEfficiency();
                break;
            case 1:
                Player.main.UpgradeMaxHealth();
                break;
            case 2:
                Player.main.UpgradeMaxFuel();
                coffee_O_Meter.SetBarLevel(UpgradeData.main.Stats[statID].level);
                break;
            case 3:
                Player.main.UpgradeHamsterBall();
                break;
            case 4:
                Player.main.UpgradeMoneyMagnet();
                break;
            default:
                Debug.LogError("Invalid upgrade");
                break;
        }
    }

    // PUBLIC METHODS
    public void GoToTheMainMenu()
    {
    }

    public void NewGame()
    {
        targetGameState = GameState.New;
        StartCoroutine(SwitchGameStateWithDelay(GameState.Loading, 1f));
    }

    public void PauseGame()
    {
        gameStateUpdating = true;
        currentGameState = GameState.Pausing;
    }

    public void UnPauseGame()
    {
        gameStateUpdating = true;
        targetGameState = GameState.New;
        currentGameState = GameState.Loading;
    }

    public void LoseGame()
    {
        gameStateUpdating = true;
        currentGameState = GameState.Lose;
    }
}

[Serializable]
class PlayerData
{
    // Placeholder
    float currentMoney;
}
