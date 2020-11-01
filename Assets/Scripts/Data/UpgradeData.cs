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

    public UpgradeStat FuelEfficiency { get { return stats[0]; } }
    public UpgradeStat MaxHealth { get { return stats[1]; } }
    public UpgradeStat MaxFuel { get { return stats[2]; } }
    public UpgradeStat HamsterBall { get { return stats[3]; } }
    public UpgradeStat MoneyMagnet { get { return stats[4]; } }

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
            cost = 50,
            nextCost = 0,
            maxLevel = 5,
            level = 0,
            available = false,
        };

        stats[1] = new UpgradeStat()
        {
            name = "Max Health",
            cost = 15,
            nextCost = 0,
            maxLevel = 5,
            level = 0,
            available = false,
        };

        stats[2] = new UpgradeStat()
        {
            name = "Max Fuel",
            cost = 15,
            nextCost = 0,
            maxLevel = 10,
            level = 0,
            available = false,
        };

        stats[3] = new UpgradeStat()
        {
            name = "Hamster Ball",
            cost = 15,
            nextCost = 0,
            maxLevel = 2,
            level = 0,
            available = false,
        };

        stats[4] = new UpgradeStat()
        {
            name = "Money Magnet",
            cost = 1000,
            nextCost = 0,
            maxLevel = 1,
            level = 0,
            available = false,
        };
    }

    public bool CheckUpgradeAvailability(int statID)
    {
        return (stats[statID].level < stats[statID].maxLevel);
    }

    public int PurchaseUpgrade(int statID)
    {
        stats[statID].level++;
        stats[statID].nextCost = stats[statID].cost + 20;
        return stats[statID].cost;
    }
    public void UpdateStat(int statID)
    {
        stats[statID].cost = stats[statID].nextCost;
    }
}
