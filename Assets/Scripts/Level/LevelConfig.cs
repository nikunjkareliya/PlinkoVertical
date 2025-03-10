using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int levelID;
    public string levelName;
    public GameTheme theme;
}

public enum GameTheme
{
    Theme1,
    Theme2,
    Theme3
}
