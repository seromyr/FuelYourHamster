using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

// This class holds all the upgrade data
public class UpgradeData : MonoBehaviour
{
    [SerializeField]
    private UpgradeStat[] stats;
    public UpgradeStat[] Stats { get { return stats; } }

    public UpgradeStat FuelEfficiency { get { return stats[0]; } }
    public UpgradeStat MaxHealth { get { return stats[1]; } }

    void Start()
    {
        // There are 5 upgrade stats in the game
        stats = new UpgradeStat[5];

        for (int i = 2; i < stats.Length; i++)
        {
            stats[i] = new UpgradeStat()
            {
                name = "Upgrade Stat_0" + (i + 1).ToString(),
                cost = 100,
                nextCost = 200,
                maxLevel = 10,
                level = 1,
                available = false,
            };
        }

        stats[0] = new UpgradeStat()
        {
            name = "Fuel Efficiency",
            cost = 50,
            nextCost = 0,
            maxLevel = 10,
            level = 1,
            available = false,
        };

        stats[1] = new UpgradeStat()
        {
            name = "Max Health",
            cost = 15,
            nextCost = 0,
            maxLevel = 10,
            level = 1,
            available = false,
        };
    }


    public bool CheckUpgradeAvailability(int statID)
    {
        return (stats[statID].level <= stats[statID].maxLevel);
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
