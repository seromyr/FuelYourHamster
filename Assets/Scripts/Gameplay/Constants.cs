using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public enum GameState
    {
        None,
        Start,
        Loading,
        New,
        Pausing,
        Playing,
        Win,
        Lose,
    }

    // Dodge mechanic
    public enum Lane
    {
        Left,
        Middle,
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

    public enum CollectibleType
    {
        Nothing,
        Coin,
        Bean,
        Good,
        Bad,
        VeryBad,
        Quest
    }

    // Upgrade statistics
    [Serializable]
    public struct UpgradeStat
    {
        // Name of the stat which will be shown in the upgrade UI
        public string                        name;

        // Description of the upgrade
        public string                        description;

        // The cost of the next upgrade purchase
        public int                           cost;

        // The cost increase after each purchase
        public int                           nextCost;

        // The maximum number of upgrade that this stat has
        public int                           maxLevel;

        // The current upgrade level that player has
        public int                           level;

        // The state that this upgrade is available to purchase (player has enough money)
        public bool                          available;
    }

    [Serializable]
    public struct DifficultyLevel
    {
        public int                           startRange;
        public int                           endRange;
    }

    public static class SceneName
    {
        public const string                  PRELOAD                               = "Preload";
        public const string                  MAINMENU                              = "MainMenu";
        public const string                  GAME                                  = "Game";
    }

    public static class PrimeObj
    {
        public const string                  GAMEMANAGER                           = "GameManager";
        public const string                  PLAYER                                = "Player";
        public const string                  VAULT01                               = "VaultAlpha";
        public const string                  VAULT02                               = "VaultBeta";
        public const string                  VAULT03                               = "VaultGamma";
        public const string                  OBJPOOL                               = "ObjectPool";
        public const string                  MUTATIONPOOL                          = "ObjectMutationVariance";
        public const string                  OBJRESERVE                            = "ObjectReserve";
        public const string                  OBJCONTAINER                          = "ObjectContainer";
    }

    public static class StatID
    {
        // Game object names of the UI that relate to the upgrade mechanic
        public const string                  _01                                   = "Stat_00";
        public const string                  _02                                   = "Stat_01";
        public const string                  _03                                   = "Stat_02";
        public const string                  _04                                   = "Stat_03";
        public const string                  _05                                   = "Stat_04";
    }

    public static class CONST
    {
        // Factory performance
        public const int                     VAULT_01_CAPACITY                     = 15;
        public const int                     VAULT_02_CAPACITY                     = 15;
        public const int                     VAULT_03_CAPACITY                     = 15;
                                                                                   
        // Upgrade values                                                          
        public const int                     DEFAULT_MAX_HEALTH                    = 2;
        public const int                     MAX_HEALTH_UPGRADE_VALUE              = 1;
                                                                                   
        public const int                     DEFAULT_MAX_CAFFEINE_LEVEL            = 200;
        public const int                     CAFFEINE_UPGRADE_VALUE                = 50;
                                                                                   
        public const int                     DEFAULT_CAFFEINE_COSUMING_SPEED       = 10;
        public const float                   CAFFEINE_COSUMING_SPEED_UPGRADE_VALUE = 0.25f;
                                                                                   
        public const float                   CHECKPOINT_DURATION                   = 12f;
                                                                                   
        public const float                   DEFAULT_MAX_SPEED                     = 50f;
                                                                                   
        public const int                     DEFAULT_HAMSTERBALL_LEVEL             = 0;
        public const int                     HAMSTERBALL_UPGRADE_VALUE             = 1;
                                                                                   
        // Key code                                                                
        public const KeyCode                 JUMP_KEY                              = KeyCode.Space;
        public const KeyCode                 LEFT_KEY                              = KeyCode.A;
        public const KeyCode                 RIGHT_KEY                             = KeyCode.D;
        public const string                  LEFT_KEY_STRING                       = "a";
        public const string                  RIGHT_KEY_STRING                      = "d";
                                                                                   
        // Player default position                                                 
        public static readonly Vector3       PLAYER_DEFAULT_POSITION               = new Vector3(-37, 1.5f, 0);
        public static readonly Vector3       PLAYER_DEFAULT_AVATAR_POSITION        = new Vector3(0, 0.6f, 0);
        public static readonly Vector3       PLAYER_DEFAULT_AVATAR_SIZE            = Vector3.one * 0.7f;
        public static readonly Quaternion    PLAYER_DEFAULT_AVATAR_ROTATION        = Quaternion.Euler(0, 180, -8.463f);
                                                                                   
        // Player hit color                                                        
        public const string                  PLAYER_HIT_COLOR                      = "#FF4800";
        public const string                  PLAYER_COLLECT_COLOR                  = "#00FF32";
                                                                                   
        // Player default money                                                    
        public const int                     PLAYER_DEFAUL_MONEY                   = 0;
    }                                                                              
                                                                                   
    public static class TAG                                                        
    {                                                                              
        public const string                  SPAWNCOLLIDER                         = "SpawnCollider";
        public const string                  COIN                                  = "Coin";
        public const string                  COLLECTIBLE                           = "Collectible";
        public const string                  BEAN                                  = "CoffeBean";
        public const string                  OBJGOOD                               = "GoodCollectible";
        public const string                  OBJBAD                                = "BadCollectible";
    }

    public static class AudioComponent
    {
        public const string                  SOUND                                 = "Sound";
        public const string                  MUSIC                                 = "Music";
    }
}

