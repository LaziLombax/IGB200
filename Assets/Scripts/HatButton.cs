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
    private HatDisplay hatDisplay;

    private void Start()
    {
        hatDisplay = HatDisplay.Instance;
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
        isSelected = !isSelected;
        CheckSelect();
        GameHandler.Instance.gameData.SelectHat(hatKey);
        hatDisplay.SetHat();
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
