using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tuna
{
    public static class Constants
    {
    }
    
    public enum GameState
    {
        Menu,
        Playing,
        Over
    }

    public enum CollectableType
    {
        Ore,
        Weapon,
        Metal
    }

    public enum PlayerStatus
    {
        Natural,
        Farming,
        Fighting,
        Cleaning
    }
    
}
