using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Constants;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    private int currentScene;

    private Canvas mainMenu, upgradeMenu;

    [SerializeField, Header("Money on game start")]
    private int money;
    public int CheckWallet { get { return money; } }

    private UpgradeData upgradeData;
    // Provide access to upgrade data via Game Manager
    public UpgradeData UpgradeData { get { return upgradeData; } }
    
    private void Awake()
    {
        Singleton_dinator();

        currentScene = SceneManager.GetActiveScene().buildIndex;

        mainMenu = GameObject.Find("Main Menu").GetComponent<Canvas>();
        upgradeMenu = GameObject.Find("Upgrade Menu").GetComponent<Canvas>();

        upgradeData = GetComponent<UpgradeData>();
    }

    void Start()
    {
        if (currentScene == 0)
        {
            SceneManager.LoadScene(1);
            mainMenu.enabled = true;
            upgradeMenu.enabled = false;
        }

        // Initialize money at game start
        money = 30000;
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
    private void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 0:
                // This will never happen
                break;
            case 1:
                Debug.Log("Main Menu loaded");

                break;
            case 2:
                Debug.Log("Gameplay loaded");
                mainMenu.enabled = false;
                upgradeMenu.enabled = true;
                break;
        }
    }

    private void Update()
    {
        UpgradeAvailabilityCheck();
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

    private void UpgradeAvailabilityCheck()
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
}

[Serializable]
class PlayerData
{
    // Placeholder
    float currentMoney;
}
