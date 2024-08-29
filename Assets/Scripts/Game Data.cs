using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Save", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    [Header("Game Data")]
    public List<LevelData> levelDatas = new List<LevelData>();
    public LevelData currentLevel;

    public void SetCurrentLevel(string levelName)
    {
        foreach (LevelData levelData in levelDatas)
        {
            if (levelData.levelName == levelName)
            {
                currentLevel = levelData;
                return;
            }
        }
    }
}
