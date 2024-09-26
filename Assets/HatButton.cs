using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatButton : MonoBehaviour
{
    public string hatKey;
    public bool isSelected;
    public bool canUse;
    public Image icon;

    private void Start()
    {
        if (canUse)
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = Color.black;
        }
        CheckSelect();
    }
    public void SelectHat()
    {
        if (!canUse) return;
        if (isSelected) return;
        foreach (var hat in GameHandler.Instance.uiHandler.hatButtonList)
        {
            hat.isSelected = false;
            hat.CheckSelect();
        }
        isSelected = true;
        CheckSelect();
        GameHandler.Instance.gameData.SelectHat(hatKey);
    }
    public void CheckSelect()
    {
        if (isSelected)
        {
            GetComponent<Image>().color = Color.gray;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}
