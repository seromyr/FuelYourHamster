using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Constants;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

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

    private UpgradeData upgradeData;
    // Provide access to upgrade data via Game Manager
    public UpgradeData UpgradeData { get { return upgradeData; } }

    private void Awake()
    {
        // Make the Game Manager a Singleton
        Singleton_dinator();

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
    private void Singleton_dinator()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
    }

    private void PreloadSetup()
    {
        // Get current scene index to load the next screen which is Main Menu
        currentScene = SceneManager.GetActiveScene().name;

        // Get Main Menu canvas
        mainMenu = GameObject.Find("Main Menu").GetComponent<Canvas>();
        mainMenu.enabled = false;

        // Get Upgrade Menu canvas
        upgradeMenu = GameObject.Find("Upgrade Menu").GetComponent<Canvas>();
        upgradeMenu.enabled = false;

        // Get Upgrade data 
        upgradeData = GetComponent<UpgradeData>();

        // Get Lose canvas
        gameOverMenu = GameObject.Find("Game Over Menu").GetComponent<Canvas>();
        gameOverMenu.enabled = false;

        // Find Player
        playerOBJ = GameObject.Find(PrimeObj.PLAYER);
        player = playerOBJ.GetComponent<Player>();

        // Listen to events
        player.OnCollectCoin += PlayerCollectedACoin;
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

                break;

            case SceneName.GAME:
                Debug.Log("Gameplay loaded");
                //gameOverMenu.renderMode = RenderMode.ScreenSpaceCamera;
                //gameOverMenu.worldCamera = Camera.main;
                //gameOverMenu.planeDistance = 1;

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
                gameStateUpdate = false;
                break;

            case GameState.New:
                SceneManager.LoadScene(SceneName.GAME);
                
                break;

            case GameState.Playing:
                //upgradeMenu.GetComponent<UI_Panel_Slider>().ScrollOut();
                upgradeMenu.enabled = false;

                CheckUpgradeAvailability();
                CheckPlayerHealth();
                CheckPlayerCaffeineLevel();

                break;

            case GameState.Pausing:
                //upgradeMenu.GetComponent<UI_Panel_Slider>().ScrollIn();
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
        for (int i = 0; i < upgradeData.Stats.Length; i++)
        {
            if (money < upgradeData.Stats[i].cost)
            {
                upgradeData.Stats[i].available = false;
            }
            else upgradeData.Stats[i].available = true;
        }
    }

    private void CheckPlayerHealth()
    {
        if (player.Health <= 0)
        {
            gameState = GameState.Lose;
        }
    }

    private void CheckPlayerCaffeineLevel()
    {
        if (player.CaffeineLevel <= 0)
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
        if (upgradeData.CheckUpgradeAvailability(statID) && money >= upgradeData.Stats[statID].cost)
        {
            // Purchase & deduct money
            AddMoney(-upgradeData.PurchaseUpgrade(statID));

            // Update upgrade stat
            upgradeData.UpdateStat(statID);

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
                break;
            default:
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
