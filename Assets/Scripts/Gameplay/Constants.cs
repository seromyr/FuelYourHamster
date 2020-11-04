﻿using System;
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
    public enum Side
    {
        None,
        Left,
        Right,
    }

    // Coin vault names
    public enum Vault
    {
        Alpha,
        Beta,
        Gamma
    }

    // Sound states
    public enum AudioState
    {
        On,
        Off,
    }

    public enum FadeType
    {
        None,
        In,
        Out
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
        public int nextCost;

        // The maximum number of upgrade that this stat has
        public int maxLevel;

        // The current upgrade level that player has
        public int level;

        // The state that this upgrade is available to purchase (player has enough money)
        public bool available;
    }

    // there will be 5 in-game difficulties, the last one is for victory
    public enum Difficulty
    {
        Kindergarten,
        Decent,
        Engaged,
        Difficult,
        Lightspeed,
        Victory
    }

    public static class SceneName
    {
        public const string PRELOAD  = "Preload";
        public const string MAINMENU = "MainMenu";
        public const string GAME     = "Game";
    }

    public static class PrimeObj
    {
        public const string GAMEMANAGER  = "GameManager";
        public const string PLAYER       = "Player";
        public const string VAULT01      = "VaultAlpha";
        public const string VAULT02      = "VaultBeta";
        public const string VAULT03      = "VaultGamma";
        public const string OBJPOOL      = "ObjectPool";
        public const string MUTATIONPOOL = "ObjectMutationVariance";
        public const string OBJRESERVE   = "ObjectReserve";
        public const string OBJCONTAINER = "ObjectContainer";
    }

    public static class Prefab
    {
        public const string COIN = "Coin";
        public const string MUTATED = "MutatedObject";
        public const string EXTREME = "ExtremeObject";
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

    public static class CONST
    {
        public const int VAULT_01_CAPACITY = 5;
        public const int VAULT_02_CAPACITY = 9;
        public const int VAULT_03_CAPACITY = 7;

        public const int DEFAULT_MAX_HEALTH = 2;
        public const int MAX_HEALTH_UPGRADE_VALUE = 1;

        public const int DEFAULT_MAX_CAFFEINE_LEVEL = 100;
        public const int CAFFEINE_UPGRADE_VALUE = 50;

        public const int DEFAULT_CAFFEINE_COSUMING_SPEED = 10;
        public const float CAFFEINE_COSUMING_SPEED_UPGRADE_VALUE = 0.75f;
    }

    public static class TAG
    {
        public const string SPAWNCOLLIDER = "SpawnCollider";
        public const string COIN = "Coin";
        public const string OBJGOOD = "GoodCollectible";
        public const string OBJBAD = "BadCollectible";
    }

    public static class AudioComponent
    {
        public const string SOUND = "Sound";
        public const string MUSIC = "Music";
    }
}

