using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    public Text upgradeTextTitle;
    public string upgradeName;
    public string upgradeStage;
    public GameObject starIcon;
    public RectTransform starPos;
    public List<GameObject> starList = new List<GameObject>();
    public Text upgradeDesc;
    public string myDesc;
    public Text upgradeCost;
    public string myCost;

    // Start is called before the first frame update
    void Start()
    {
        upgradeStage = GameHandler.Instance.currentLevelData.GetUpgradeCardStage(upgradeName);

        for (int i = 0; i < GameHandler.Instance.currentLevelData.UpgradesBuyTotal(upgradeStage,upgradeName); i++)
        {
            Vector3 newPos = new Vector3(100f + i*75f,0f,0f);

            GameObject blankImage = Instantiate(starIcon, starPos);
            blankImage.GetComponent<RectTransform>().position = newPos;
            blankImage.GetComponent<Image>().color = Color.black;

            GameObject iconImage = Instantiate(starIcon, starPos);
            iconImage.GetComponent<RectTransform>().position = newPos;
            starList.Add(iconImage);
            iconImage.SetActive(false);
        }
        for (int i = 0; i < GameHandler.Instance.currentLevelData.UpgradeCount(upgradeStage,upgradeName); i++)
        {
            starList[i].SetActive(true);
        }

        upgradeTextTitle.text = GameHandler.Instance.currentLevelData.GetUpgradeCardTitle(upgradeStage,upgradeName);
        upgradeDesc.text = GameHandler.Instance.currentLevelData.GetUpgradeCardDesc(upgradeStage, upgradeName);
        upgradeCost.text = GameHandler.Instance.currentLevelData.GetUpgradeCardCost(upgradeStage, upgradeName);
    }

    public void PurchaseUpgrade()
    {

        if (GameHandler.Instance.currentLevelData.levelGold < GameHandler.Instance.currentLevelData.UpgradeCost(upgradeStage, upgradeName)) return;

        GameHandler.Instance.currentLevelData.UpgradeHazard(upgradeStage, upgradeName);
        for (int i = 0; i < GameHandler.Instance.currentLevelData.UpgradeCount(upgradeStage, upgradeName); i++)
        {
            starList[i].SetActive(true);
        }
        upgradeCost.text = GameHandler.Instance.currentLevelData.GetUpgradeCardCost(upgradeStage, upgradeName);
        GameHandler.Instance.uiHandler.UpdateGold();
    }
}
