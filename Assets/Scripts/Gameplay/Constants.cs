using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public enum GameState
    {
        Start,
        Loading,
        New,
        Pausing,
        Playing,
        Win,
        Lose,
    }

    // Dodge mechanic
    [Serializable]
    public enum Side
    {
        none,
        left,
        right,
    }

    // Upgrade statistics
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

    public static class SceneName
    {
        public const string PRELOAD  = "Preload";
        public const string MAINMENU = "MainMenu";
        public const string GAME     = "Game";
    }

    public static class PrimeObj
    {
        public const string GAMEMANAGER = "GameManager";
        public const string PLAYER = "Player";
    }
    public static class StatID
    {
        // Game object names of the UI that relate to the upgrade mechanic
        public const string _01 = "Stat_00";
        public const string _02 = "Stat_01";
        public const string _03 = "Stat_02";
        public const string _04 = "Stat_03";
        public const string _05 = "Stat_04";
    }
}

