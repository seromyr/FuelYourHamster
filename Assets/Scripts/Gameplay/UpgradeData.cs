using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class holds all the upgrade data
public class UpgradeData : MonoBehaviour
{
    [Serializable]
    public struct UpgradeStat
    {
        // Name of the stat which will be shown in the upgrade UI
        public string name;

        // The cost of the next upgrade purchase
        public int cost;

        // The cost increase after each purchase
        public int nextCosst;

        // The maximum number of upgrade that this stat has
        public int maxLevel;

        // The current upgrade level that player has
        public int level;

        // The state that this upgrade is available to purchase (player has enough money)
        public bool available;
    }

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
