using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

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
    [System.Serializable]
    public class HatEntry
    {
        public string hatKey;
        public bool canUse;
        public bool inUse;
        public Sprite icon;
    }
    [Header("Game Data")]
    public List<LevelData> levelDatas = new List<LevelData>();
    public LevelData currentLevel;
    public List<HatEntry> myHats = new List<HatEntry>();
    public List<string> myHatKeys = new List<string>();

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

    public DialogueShelldon GetDialogueSet(string key)
    {
        foreach (DialogueShelldon gameDialogue in myDialogue)
        {
            if (key == gameDialogue.callKey)
            {
                gameDialogue.hasRead = true;
                return gameDialogue;
            }
        }
        return null;
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
    /*
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
    */

    //hats
    public void CheckAllHats()
    {
        foreach (var level in levelDatas)
        {
            if (level.hatUnlocked && level.hatKey != null)
            {
                foreach (var hat in myHats)
                {
                    if (level.hatKey == hat.hatKey)
                    {
                        hat.canUse = true;
                    }
                }
            }
        }
    }
    public void SelectHat(string key)
    {
        foreach (var hat in myHats)
        {
            if (hat.hatKey == key)
            {
                hat.inUse = !hat.inUse;
            }
        }
    }
    public bool CheckIfUnlockedHat(string key)
    {
        foreach (var hat in myHats)
        {
            if (hat.hatKey == key && hat.canUse)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckIfInUseHat(string key)
    {
        foreach (var hat in myHats)
        {
            if (hat.hatKey == key && hat.inUse)
            {
                return true;
            }
        }
        return false;
    }
    public List<string> CheckWhichHat()
    {
        List<string> result = new List<string>();
        foreach (var hat in myHats)
        {
            if (hat.inUse)
            {
                result.Add(hat.hatKey);
            }
        }
        return result;
    }
    public Sprite GetSpriteHat(string key)
    {
        foreach (var hat in myHats)
        {
            if (hat.hatKey == key)
            {
                return hat.icon;
            }
        }
        return null;
    }
    public string GetHatKey(int index)
    {
        return myHats[index].hatKey;
    }
    public int TotalOfHats()
    {
        int count = 0;
        foreach (var hat in myHats)
        {
            count++;
        }
        return count;
    }
}
