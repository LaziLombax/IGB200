using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
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
