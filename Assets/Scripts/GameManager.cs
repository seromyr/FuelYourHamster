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
    [SerializeField]
    private bool stateWorkingContinously;

    private string currentScene;

    private Canvas mainMenu, upgradeMenu, gameOverMenu;

    [SerializeField]
    private Player player;

    [SerializeField]
    private int playerHealth;

    [SerializeField, Header("Money on game start")]
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
        stateWorkingContinously = false;
    }

    void Start()
    {
        // Initialize money at game start
        money = 30000;

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

                // Switch state to playing
                gameState = GameState.Playing;

                stateWorkingContinously = true;

                // Find Player
                player = GameObject.Find(PrimeObj.PLAYER).GetComponent<Player>();

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
        if (stateWorkingContinously)
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
                stateWorkingContinously = false;
                break;

            case GameState.New:
                SceneManager.LoadScene(SceneName.GAME);
                
                break;

            case GameState.Playing:
                //upgradeMenu.GetComponent<UI_Panel_Slider>().ScrollOut();
                upgradeMenu.enabled = false;

                CheckUpgradeAvailability();
                CheckPlayerHealth();

                playerHealth = player.Health;
                break;

            case GameState.Pausing:
                //upgradeMenu.GetComponent<UI_Panel_Slider>().ScrollIn();
                upgradeMenu.enabled = true;
                stateWorkingContinously = false;

                break;

            case GameState.Lose:
                // Show lose game screen
                gameOverMenu.enabled = true;

                stateWorkingContinously = false;
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

    // PUBLIC METHODS
    public void GameStart()
    {
        stateWorkingContinously = true;
        gameState = GameState.Start;
    }

    public void NewGame()
    {
        stateWorkingContinously = true;
        gameState = GameState.New;
    }

    public void PauseGame()
    {
        stateWorkingContinously = true;
        gameState = GameState.Pausing;
    }

    public void UnPauseGame()
    {
        stateWorkingContinously = true;
        gameState = GameState.New;
        player.ResetHealth();
    }

    public void LoseGame()
    {
        stateWorkingContinously = true;
        gameState = GameState.Lose;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void PurchaseUpgrade(int statID)
    {
        if (upgradeData.Stats[statID].level <= upgradeData.Stats[statID].maxLevel)
        {
            upgradeData.Stats[statID].level++;

            AddMoney(-upgradeData.Stats[statID].cost);

            upgradeData.Stats[statID].cost += upgradeData.Stats[statID].nextCosst;
        }
    }
}

[Serializable]
class PlayerData
{
    // Placeholder
    float currentMoney;
}
