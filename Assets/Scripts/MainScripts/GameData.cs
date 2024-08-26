using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int Level;
    public int Health;
    public int Diamonds;
    public int MaxLevel;
    public Dictionary<string, bool[]> LevelsDiamondsStatus;

    public GameData (int level, int health, int diamonds, int maxLevel, Dictionary<string, bool[]> levelsDiamondsStatus)
    {
        Level = level;
        Health = health;
        Diamonds = diamonds;
        MaxLevel = maxLevel;
        LevelsDiamondsStatus = levelsDiamondsStatus; 
    }
}
