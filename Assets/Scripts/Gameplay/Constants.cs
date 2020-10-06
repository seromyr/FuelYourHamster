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
        Pause,
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

