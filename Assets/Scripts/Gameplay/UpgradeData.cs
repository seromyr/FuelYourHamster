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

    void Start()
    {
        // There are 5 upgrade stats in the game
        stats = new UpgradeStat[5];

        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = new UpgradeStat()
            {
                name = "Upgrade Stat_0" + (i + 1).ToString(),
                cost = 100,
                nextCosst = 200,
                maxLevel = 10,
                level = 1,
                available = false,
            };
        }
    }
}
