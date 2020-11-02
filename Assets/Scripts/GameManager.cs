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
    private GameState gameState;
    private bool gameStateUpdate;

    private string currentScene;

    private Canvas mainMenu, upgradeMenu, gameOverMenu;

    private Player player;
    private GameObject playerOBJ;

    [SerializeField, Header("Current amount of money")]
    private int money;
    public int CheckWallet { get { return money; } }

    //private UpgradeData upgradeData;
    //// Provide access to upgrade data via Game Manager
    //public UpgradeData UpgradeData { get { return upgradeData; } }

    private CoffeeMeterMechanic coffee_O_Meter;

    private void Awake()
    {
        // Make the Game Manager become a Singleton
        Singletonizer();

        // Setup before loading the Main Menu
        PreloadSetup();

        // No state switching occurence
        gameStateUpdate = false;
    }

    void Start()
    {
        // Initialize money at game start
        money = 10000;

        // Start game
        GameStart();
    }
    private void Singletonizer()
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

        // Get current scene index to load the next screen which is Main Menu
        currentScene = SceneManager.GetActiveScene().name;

        // Get Main Menu canvas
        mainMenu = GameObject.Find("Main Menu").GetComponent<Canvas>();
        mainMenu.enabled = false;

        // Get Upgrade Menu canvas
        upgradeMenu = GameObject.Find("Upgrade Menu").GetComponent<Canvas>();
        upgradeMenu.enabled = false;

        //// Get Upgrade data 
        //upgradeData = GetComponent<UpgradeData>();

        // Get Lose canvas
        gameOverMenu = GameObject.Find("Game Over Menu").GetComponent<Canvas>();
        gameOverMenu.enabled = false;

        // Find Player
        playerOBJ = GameObject.Find(PrimeObj.PLAYER);
        player = playerOBJ.GetComponent<Player>();

        // Listen to events
        player.OnCollectToken += PlayerCollectedACoin;
    }

    private void OnLevelWasLoaded()
    {
        currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case SceneName.PRELOAD:
                // This will never happen
                break;

            case SceneName.MAINMENU:
                Debug.Log("Main Menu loaded");

                // Activate Main Menu UI
                mainMenu.enabled = true;

                SoundController.main.SwitchBGM();
                SoundController.main.PlayBGM();
                break;

            case SceneName.GAME:
                Debug.Log("Gameplay loaded");
                //gameOverMenu.renderMode = RenderMode.ScreenSpaceCamera;
                //gameOverMenu.worldCamera = Camera.main;
                //gameOverMenu.planeDistance = 1;

                SoundController.main.SwitchBGM();
                SoundController.main.PlayBGM();

                UI_Gameplay_Mechanic.main.transform.Find("Coffee-O-Meter").TryGetComponent(out coffee_O_Meter);
                UI_Gameplay_Mechanic.main.SetCanvasActive(true);

                playerOBJ.SetActive(true);
                playerOBJ.GetComponent<Rigidbody>().isKinematic = false;
                player.AssignVault();

                // Switch state to playing
                gameState = GameState.Playing;

                gameStateUpdate = true;

                // Deactivate Main Menu
                mainMenu.enabled = false;

                // Activate Upgrade Menu
                upgradeMenu.enabled = true;

                // Deactivate Game Over Menu
                gameOverMenu.enabled = false;
                break;
        }
    }

    private void Update()
    {
        if (gameStateUpdate)
        {
            GameStateMonitoring();
        }
    }

    private void GameStateMonitoring()
    {
        switch (gameState)
        {
            case GameState.Start:
                SceneManager.LoadScene(SceneName.MAINMENU);
                UI_Gameplay_Mechanic.main.SetCanvasActive(false);
                gameStateUpdate = false;
                break;

            case GameState.New:
                SceneManager.LoadScene(SceneName.GAME);
                //gameStateUpdate = false;
                break;

            case GameState.Playing:
                upgradeMenu.enabled = false;

                CheckUpgradeAvailability();
                CheckPlayerHealth();
                CheckPlayerCaffeineLevel();

                break;

            case GameState.Pausing:
                upgradeMenu.enabled = true;
                gameStateUpdate = false;

                break;

            case GameState.Lose:
                // Show lose game screen
                gameOverMenu.enabled = true;
                gameStateUpdate = false;

                break;
        }
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
        if (player.CurrentHealth <= 0)
        {
            gameState = GameState.Lose;
        }
    }

    private void CheckPlayerCaffeineLevel()
    {
        if (player.CaffeineCurrentLevel <= 0)
        {
            gameState = GameState.Lose;
        }
    }

    // PUBLIC METHODS
    public void GameStart()
    {
        gameStateUpdate = true;
        gameState = GameState.Start;
        playerOBJ.GetComponent<Rigidbody>().isKinematic = true;
        playerOBJ.SetActive(false);
    }

    public void NewGame()
    {
        gameStateUpdate = true;
        //StartCoroutine(SwitchGameStateWithDelay(GameState.New, 0.2f));
        gameState = GameState.New;
    }

    public void PauseGame()
    {
        gameStateUpdate = true;
        gameState = GameState.Pausing;
    }

    public void UnPauseGame()
    {
        gameStateUpdate = true;
        gameState = GameState.New;
        player.ResetHealth();
        player.ResetCaffeineLevel();
    }

    public void LoseGame()
    {
        gameStateUpdate = true;
        gameState = GameState.Lose;
    }

    private IEnumerator SwitchGameStateWithDelay(GameState state, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameState = state;
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
                player.UpgradeFuelEfficiency();
                break;
            case 1:
                player.UpgradeMaxHealth();
                break;
            case 2:
                player.UpgradeMaxFuel();
                coffee_O_Meter.SetBarLevel(UpgradeData.main.Stats[statID].level);
                break;
            case 3:
                break;
            case 4:
                player.UpgradeMoneyMagnet();
                break;
            default:
                Debug.LogError("Invalid upgrade");
                break;
        }
    }
}

[Serializable]
class PlayerData
{
    // Placeholder
    float currentMoney;
}
