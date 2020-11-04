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
    private bool gameStateUpdating;
    private float loadDuration;

    private Canvas upgradeMenu, gameOverMenu;

    [SerializeField, Header("Current amount of money")]
    private int money;
    public int CheckWallet { get { return money; } }

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
        // Listen to events
        Player.main.OnCollectToken += PlayerCollectedACoin;

        // Initialize money at game start
        money = 10000;

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

        // Get Upgrade Menu canvas - REMEMBER TO SINGLETONIZE THIS
        upgradeMenu = GameObject.Find("Upgrade Menu").GetComponent<Canvas>();
        upgradeMenu.enabled = false;

        // Get Lose canvas - REMEMBER TO SINGLETONIZE THIS
        gameOverMenu = GameObject.Find("Game Over Menu").GetComponent<Canvas>();
        gameOverMenu.enabled = false;
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

        gameOverMenu.enabled = false;
        upgradeMenu.enabled = false;

        Player.main.SetActive(true);
        Player.main.IsKinematic(false);
        Player.main.AssignVault();

        gameStateUpdating = false;
    }

    private void PerformGameplay()
    {
        gameStateUpdating = true;

        CheckUpgradeAvailability();
        CheckPlayerHealth();
        CheckPlayerCaffeineLevel();
    }

    private void PauseGamePlay()
    {
        upgradeMenu.enabled = true;
    }

    private void ShowHowToPlay(bool value)
    {

    }

    private void ShowSummary()
    {
        // Show lose game screen
        gameOverMenu.enabled = true;
    }

    private void ShowVictory()
    {

    }

    private void CheckUpgradeAvailability()
    {
        for (int i = 0; i < UpgradeData.main.Stats.Length; i++)
        {
            if (money < UpgradeData.main.Stats[i].cost)
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
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void PurchaseUpgrade(int statID)
    {
        // Request upgrade
        if (UpgradeData.main.CheckUpgradeAvailability(statID) && money >= UpgradeData.main.Stats[statID].cost)
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
