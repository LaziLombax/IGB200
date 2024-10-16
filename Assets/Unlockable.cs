using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unlockable : MonoBehaviour
{
    public Image myIcon;
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
            myIcon.sprite = GameHandler.Instance.gameData.GetSpriteHat(GameHandler.Instance.currentLevelData.hatKey);
        }
        CheckUnlockable();
    }

    public void CheckUnlockable()
    {
        if (reward == Reward.level)
        {
            if (GameHandler.Instance.currentLevelData.nextLevel)
            {
                myIcon.color = Color.white;
            }
            else
            {
                myIcon.color = Color.black;
            }
        }
        else
        {
            if (GameHandler.Instance.currentLevelData.hatUnlocked)
            {
                myIcon.color = Color.white;
            }
            else
            {
                myIcon.color = Color.black;

            }
        }
    }
}
