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
    private bool stateWorking;

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
        stateWorking = false;
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

                stateWorking = true;

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
        if (stateWorking)
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
                stateWorking = false;
                break;

            case GameState.New:
                SceneManager.LoadScene(SceneName.GAME);
                stateWorking = false;
                break;

            case GameState.Playing:
                // This is me (Buu) cheating, remind me to change this later
                upgradeMenu.transform.Find("Panel").GetComponent<UI_Panel_Slider>().ScrollOut();

                // Not cheat
                CheckUpgradeAvailability();
                CheckPlayerHealth();

                playerHealth = player.Health;
                break;

            case GameState.Pausing:
                // This is me (Buu) cheating, remind me to change this later
                upgradeMenu.transform.Find("Panel").GetComponent<UI_Panel_Slider>().ScrollIn();
                stateWorking = false;
                break;

            case GameState.Lose:
                // Show lose game screen
                gameOverMenu.enabled = true;

                stateWorking = true;
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
        stateWorking = true;
        gameState = GameState.Start;
    }

    public void NewGame()
    {
        stateWorking = true;
        gameState = GameState.New;
    }

    public void PauseGame()
    {
        gameState = GameState.Pausing;
    }

    public void UnPauseGame()
    {
        stateWorking = true;
        gameState = GameState.Playing;
    }

    public void LoseGame()
    {
        stateWorking = true;
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
