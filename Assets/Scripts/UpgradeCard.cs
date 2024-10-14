using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : MonoBehaviour
{
    public TextMeshProUGUI upgradeTextTitle;
    public string upgradeName;
    public string upgradeStage;
    public GameObject starIcon;
    public RectTransform starPos;
    public List<GameObject> starList = new List<GameObject>();
    public TextMeshProUGUI upgradeDesc;
    public string myDesc;
    public TextMeshProUGUI upgradeCost;
    public string myCost;

    // Start is called before the first frame update
    void Start()
    {
        upgradeStage = GameHandler.Instance.currentLevelData.GetUpgradeCardStage(upgradeName);
        GenerateStars();
        for (int i = 0; i < GameHandler.Instance.currentLevelData.UpgradeCount(upgradeStage,upgradeName); i++)
        {
            starList[i].GetComponent<Image>().color = Color.white;
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
            starList[i].GetComponent<Image>().color = Color.white;
        }
        upgradeCost.text = GameHandler.Instance.currentLevelData.GetUpgradeCardCost(upgradeStage, upgradeName);
        GameHandler.Instance.uiHandler.UpdateGold();
    }

    public GameObject starImage;
    private void GenerateStars()
    {

        float gapBetweenStars = 10f;
        int starCount = Mathf.FloorToInt(GameHandler.Instance.currentLevelData.UpgradesBuyTotal(upgradeStage,upgradeName));
        float totalGap = gapBetweenStars + starImage.GetComponent<RectTransform>().rect.width;
        // Calculate the starting point to spawn UI objects
        float totalCardAndGapSize = totalGap * (starCount - 1) + gapBetweenStars;

        float sideOffset = starPos.rect.width - totalCardAndGapSize;

        float startX = 0f - starPos.rect.width / 2 + sideOffset / 2;

        for (int i = 0; i < starCount; i++)
        {
            // Create a new UI object
            GameObject newUIObject = Instantiate(starImage, starPos);
            starList.Add(newUIObject);
            // Set the anchored position for the UI element
            RectTransform uiRect = newUIObject.GetComponent<RectTransform>();
            uiRect.anchoredPosition = new Vector2(startX + i * totalGap, 0);

            newUIObject.GetComponent<Image>().color = Color.black;
            // Optionally: Set the name of the object to distinguish them
            newUIObject.name = "Star_" + i;
        }
    }
}
