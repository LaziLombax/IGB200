using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockable : MonoBehaviour
{
    public enum Reward
    {
        level,
        hat
    }
    public Reward reward;

    private void Start()
    {
        if (reward == Reward.level) 
        { 
            GameHandler.Instance.uiHandler.levelUnlockable = gameObject;
        } 
        else 
        { 
            GameHandler.Instance.uiHandler.hatUnlockable = gameObject; 
        }
        
    }

    public void CheckUnlockable()
    {
        if (reward == Reward.level)
        {
            if (GameHandler.Instance.currentLevelData.nextLevel)
            {
                //iconChange
            }
        }
        else
        {
            if (GameHandler.Instance.currentLevelData.hatUnlocked)
            {
                //iconChange
            }
        }
    }
}
