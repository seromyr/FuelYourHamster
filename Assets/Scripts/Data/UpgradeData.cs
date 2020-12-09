using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

// This class holds all the upgrade data
public class UpgradeData : MonoBehaviour
{
    public static UpgradeData main;

    [SerializeField]
    private UpgradeStat[] stats;
    public UpgradeStat[] Stats { get { return stats; } }

    private void Awake()
    {
        Singletonizer();
    }

    void Start()
    {
        CreateUpgradeData();
    }

    private void Singletonizer()
    {
        if (main == null)
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("Upgrade Data created");
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }

    private void CreateUpgradeData()
    {
        // There are 5 upgrade stats in the game
        stats = new UpgradeStat[5];

        stats[0] = new UpgradeStat()
        {
            name = "Fuel Efficiency",
            description = "Slower fuel decreasing",
            cost = 10,
            nextCost = 0,
            maxLevel = 5,
            level = 0,
            available = false,
        };

        stats[1] = new UpgradeStat()
        {
            name = "Max Health",
            description = "Extra hearts at start",
            cost = 20,
            nextCost = 0,
            maxLevel = 5,
            level = 0,
            available = false,
        };

        stats[2] = new UpgradeStat()
        {
            name = "Max Fuel",
            description = "More fuel to run",
            cost = 30,
            nextCost = 0,
            maxLevel = 10,
            level = 0,
            available = false,
        };

        stats[3] = new UpgradeStat()
        {
            name = "Hamster Ball",
            description = "Extra collision protection",
            cost = 120,
            nextCost = 0,
            maxLevel = 2,
            level = 0,
            available = false,
        };

        stats[4] = new UpgradeStat()
        {
            name = "Money Magnet",
            description = "Auto coin collecting",
            cost = 500,
            nextCost = 0,
            maxLevel = 1,
            level = 0,
            available = false,
        };
    }

    public bool CheckUpgradeAvailability(int statID)
    {
        return (stats[statID].level < stats[statID].maxLevel); // && stats[statID].cost != -1
    }

    public bool CheckUpgradeIsAlreadyMax(int statID)
    {
        return stats[statID].cost == -1;
    }

    public int PurchaseUpgrade(int statID)
    {
        stats[statID].level++;
        stats[statID].nextCost = stats[statID].cost * 2;
        return stats[statID].cost;
    }
    public void UpdateStat(int statID)
    {
        if (stats[statID].level == stats[statID].maxLevel)
        {
            stats[statID].cost = -1;
        }
        else
        {
            stats[statID].cost = stats[statID].nextCost;
        }
    }

    public void Reset()
    {

        // Re-initialize
        CreateUpgradeData();

        for (int i = 0; i < stats.Length; i++)
        {
            Debug.Log("stat " + i + " level " + stats[i].level  );
        }

        // Clear Money Magnet game object
        Destroy(GameObject.Find("MoneyMagnet"));

        // Clear HamsterBall
        Destroy(GameObject.Find("HamsterBall"));
    }
}
