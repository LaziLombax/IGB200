using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Save", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    [System.Serializable]
    public class DialogueShelldon
    {
        public string callKey;
        public bool hasRead;
        [TextArea]
        public List<string> dialogueList = new List<string>();
    }
    [Header("Game Data")]
    public List<LevelData> levelDatas = new List<LevelData>();
    public LevelData currentLevel;

    public List<DialogueShelldon> myDialogue = new List<DialogueShelldon>();


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



    // Dialogue bs
    public bool CheckRead(string callKey)
    {
        foreach (DialogueShelldon gameDialogue in myDialogue)
        {
            if (callKey == gameDialogue.callKey)
            {
                if (gameDialogue.hasRead) 
                    return true;
                return false;
            }
        }
        return false;
    }
    public void ReadDialogue(string callKey)
    {
        foreach (DialogueShelldon gameDialogue in myDialogue)
        {
            if (callKey == gameDialogue.callKey)
            {
                gameDialogue.hasRead = true;
                return;
            }
        }
    }
    public string GetDialogue(string callKey, int index)
    {
        foreach (DialogueShelldon gameDialogue in myDialogue)
        {
            if (callKey == gameDialogue.callKey)
            {
                if (gameDialogue.dialogueList.ElementAtOrDefault(index) == null) return null;
                return gameDialogue.dialogueList[index];
            }
        }
        return null;
    }
    public int GetNumberOfLines(string callKey)
    {
        foreach (DialogueShelldon gameDialogue in myDialogue)
        {
            if (callKey == gameDialogue.callKey)
            {
                return gameDialogue.dialogueList.Count;
            }
        }
        return 0;
    }

    public string ReturnCurrentIndex(string callKey, int index)
    {
        foreach (DialogueShelldon gameDialogue in myDialogue)
        {
            if (callKey == gameDialogue.callKey)
            {
                return gameDialogue.dialogueList[index];
            }
        }
        return null;
    }
}
