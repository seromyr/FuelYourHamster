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
    public event EventHandler OnGamePlay, OnGameEnd;

    [SerializeField]
    private GameState currentGameState, targetGameState;

    private bool gameStateUpdating, firstRun;
    private float loadDuration;

    private CoffeeMeterMechanic coffee_O_Meter;

    private string summaryText;

    private void Awake()
    {
        // Make the Game Manager become a Singleton
        Singletonize();

        // Setup before opening the Main Menu
        GameInitialization();

        // Create Player
        Player player = new Player();
    }

    private void Start()
    {
        // Initialize money at game start
        Player.main.SetFund(CONST.PLAYER_DEFAULT_MONEY);

        targetGameState = GameState.Start;
        //targetGameState = GameState.New;
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

    private void GameInitialization()
    {
        // Add event system for UI
        gameObject.AddComponent<EventSystem>();
        gameObject.AddComponent<StandaloneInputModule>();

        UI_UpgradeMenu.main.SetCanvasAtive(false);
        UI_RunResultScreen.main.SetCanvasAtive(false);
        UI_VictoryScreen.main.SetCanvasActive(false);

        loadDuration = 1f;

        firstRun = true;
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
            case GameState.Lose:    ShowDebrief(1);     break;
            case GameState.Win:     ShowDebrief(2);     break;
            case GameState.End:     ShowDebrief(3);     break;
        }
    }

    private void LoadRoutine()
    {
        UI_MainMenu.main.SetCanvasActive(true);
        UI_MainMenu.main.FadeOut(loadDuration + 1f);
        UI_MainMenu.main.Reset();

        UI_LoadingScreen.main.SetCanvasActiveWithDelay(true, loadDuration);
        UI_LoadingScreen.main.SetCanvasActiveWithDelay(false, loadDuration + 1.5f);

        if (firstRun)
        {
            UI_LoadingScreen.main.SetHint(0);
            firstRun = false;
        }
        else
        {
            UI_LoadingScreen.main.SetHint(-1);
        }

        Player.main.IsKinematic(true);
        Player.main.SetActive(false);

        QuestController.main.InitializeNextQuest();
        QuestController.main.ActivateNextQuest();
        QuestController.main.TrackingQuest(true);

        switch (targetGameState)
        {
            case GameState.Start:
                StartCoroutine(LoadSceneWithDelay(SceneName.MAINMENU, loadDuration));

                break;
            case GameState.New:
                StartCoroutine(LoadSceneWithDelay(SceneName.GAME, loadDuration));
                Debug.Log("New Run");
                UI_LoadingScreen.main.SetHint(1);
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
        UI_MainMenu.main.SetCanvasActive(false);

        // Broadcast gamplay has started
        OnGamePlay?.Invoke(this, EventArgs.Empty);

        // Player setup
        Player.main.SetActive(true);
        Player.main.SwitchToGameplayAvatar();
        Player.main.IsKinematic(false);
        Player.main.AssignVault();
        Player.main.ResetIncome();
        Player.main.FullLoadHealth();
        Player.main.FullLoadFuel();

        // Difficulty setup
        DiffilcultyController.main.AssignWheel();
        //DiffilcultyController.main.MarkBeginTime();
        DiffilcultyController.main.MarkTime();
        DiffilcultyController.main.SetCheckPointTime(CONST.CHECKPOINT_DURATION);
        DiffilcultyController.main.ResetSpawningRange();

        // Main Quest setup
        QuestController.main.InitializeNextQuest();
        QuestController.main.AssignCollectibleContainer();
        QuestController.main.LoadNextQuestCollectible();
        QuestController.main.ClearCollectedCharacters();

        // Gameplay UI setup
        UI_Gameplay_Mechanic.main.transform.Find("Coffee-O-Meter").TryGetComponent(out coffee_O_Meter);
        UI_Gameplay_Mechanic.main.SetCanvasActive(true);
        UI_Gameplay_Mechanic.main.StartCountDown();
        UI_Gameplay_Mechanic.main.DisplayActiveQuest();
        UI_UpgradeMenu.main.SetCanvasAtive(false);
        UI_RunResultScreen.main.SetCanvasAtive(false);
        UI_VictoryScreen.main.SetCanvasActive(false);

        gameStateUpdating = false;
    }

    private void PerformGameplay()
    {
        gameStateUpdating = true;

        if (QuestController.main.IsCurrentQuestFinished())
        {
            currentGameState = GameState.Win;
            QuestController.main.AdvanceToNextQuest();
            Player.main.PlaySound(SoundController.main.SoundLibrary[10]);
            SoundController.main.StopBGM();
        }
        else if (QuestController.main.IsAllQuestsFinished())
        {
            Debug.LogError("EndGame");
            currentGameState = GameState.End;
            OnGameEnd?.Invoke(this, EventArgs.Empty);
        }

        CheckUpgradeAvailability();
        CheckLoseConditions();
    }

    private void PauseGamePlay()
    {
        UI_UpgradeMenu.main.SetCanvasAtive(true);
    }

    private void ShowDebrief(int sequenceID)
    {
        Player.main.ControlPermission(false);
        Player.main.IsConsumingFuel(false);

        // Mark the end time of the run
        DiffilcultyController.main.MarkEndTime();

        gameStateUpdating = false;

        StartCoroutine(EndRunSequence(sequenceID));
    }

    private void CheckUpgradeAvailability()
    {
        for (int i = 0; i < UpgradeData.main.Stats.Length; i++)
        {
            if (Player.main.Wallet < UpgradeData.main.Stats[i].cost)
            {
                UpgradeData.main.Stats[i].available = false;
            }
            else UpgradeData.main.Stats[i].available = true;
        }
    }

    private void CheckLoseConditions()
    {
        if (Player.main.IsOutOfHealth() || Player.main.IsOutOfCaffeine())
        {
            currentGameState = GameState.Lose;
            Player.main.SwitchToLoseAvatar();
            Player.main.PlaySound(SoundController.main.SoundLibrary[9]);
            SoundController.main.StopBGM();
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

    private IEnumerator EndRunSequence(int sequenceID)
    {
        // Update summary text;
        summaryText =
            (
                  "Run duration: "      + DiffilcultyController.main.RunTime
                + "\nMax speed: "       + DiffilcultyController.main.MaxSpeed + " km/h"
                + "\nCoins collected: " + Player.main.Income
            );

        switch (sequenceID)
        {
            case 1:
                UI_Gameplay_Mechanic.main.ShowEndRunNotice();
                yield return new WaitForSeconds(3);

                // Show lose game screen
                UI_RunResultScreen.main.SetCanvasAtive(true);
                UI_RunResultScreen.main.SetSummaryText(summaryText + "\nCharacter Collected: " + QuestController.main.CharacterCollected);
                break;

            case 2:
                yield return new WaitForSeconds(1);

                // Show victory game screen
                UI_VictoryScreen.main.SetCanvasActive(true);
                UI_VictoryScreen.main.SetTitle("MISSION COMPLETED");
                UI_VictoryScreen.main.SetSummaryText(summaryText + "\nBonus Reward: " + CONST.PLAYER_BONUS_MONEY + " coins");
                Player.main.AddFundToWallet(CONST.PLAYER_BONUS_MONEY);
                break;

            case 3:
                yield return new WaitForSeconds(1);

                // Show victory game screen
                UI_VictoryScreen.main.SetCanvasActive(true);
                UI_VictoryScreen.main.SetTitle("CONGRATULATIONS!");
                UI_VictoryScreen.main.SetSummaryText("You beat Fuel Your Hamster!!\nThank you for playing.");
                break;
        }
    }

    public void PurchaseUpgrade(int statID)
    {
        // Request upgrade
        if (AllowUpgradePurchasing(statID))
        {
            // Purchase & deduct money
            Player.main.AddFundToWallet(-UpgradeData.main.PurchaseUpgrade(statID));

            // Update upgrade stat
            UpgradeData.main.UpdateStat(statID);

            // Update gameplay stat
            UpgadeGameplayStats(statID);
        }
    }

    public bool AllowUpgradePurchasing(int statID)
    {
        return UpgradeData.main.CheckUpgradeAvailability(statID) && Player.main.Wallet >= UpgradeData.main.Stats[statID].cost;
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
        ResetGame();

        targetGameState = GameState.Start;
        gameStateUpdating = true;
        LoadRoutine();
    }

    public void ShowHowToPlay()
    {
        UI_MainMenu.main.SetHowToPlayActive(true);
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

    private void ResetGame()
    {
        // Reset player
        Player.main.Reset();

        // Reset upgrade
        UpgradeData.main.Reset();

        // Reset quest progress
        QuestController.main.Reset();

        // Reset UI
        UI_UpgradeMenu.main.Reset();
        UI_UpgradeMenu.main.SetCanvasAtive(false);

        UI_RunResultScreen.main.SetCanvasAtive(false);
        UI_VictoryScreen.main.SetCanvasActive(false);
        coffee_O_Meter.SetBarLevel(UpgradeData.main.Stats[2].level);
    }
}

[Serializable]
class PlayerData
{
    // Placeholder
    float currentMoney;
}
