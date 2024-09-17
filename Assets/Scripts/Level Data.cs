
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Beach Name", menuName = "New Level")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class StageData
    {
        public string name = "Beach";
        public List<HazardData> naturalHazards = new List<HazardData>();
        public List<HazardData> humanHazards = new List<HazardData>();
    }
    [System.Serializable]
    public class HazardData
    {
        public string name;
        public GameObject hazardObject;
        public float maxChance;
        public float chance;
        public float hazardSize;
        [TextArea(15, 20)]
        public string description = "Hazard turtle Fact";
    }
    [Header("Level Data")]
    public string levelName;
    public int levelNum;
    [TextArea(5, 10)]
    public string description = "Beach Description";
    public int levelGold;
    public float currentTimer;
    public List<StageData> stageList = new List<StageData>();

    public float CleanProgression()
    {
        float totalMaxChances = 0f;
        float totalCurrentChances = 0f;
        foreach (var stage in stageList)
        {
            foreach (var hazard in stage.humanHazards)
            {
                totalMaxChances += hazard.maxChance;
                totalCurrentChances += hazard.chance;
            }
        }
        return (totalMaxChances - totalCurrentChances)/totalMaxChances;
    }

    public GameObject RandomHazard(string stageName)
    {
        float currentChance = CalcutaleStageHazards(stageName);
        foreach (var stage in stageList)
        {
            if (stageName == stage.name)
            {
                foreach (var hazard in stage.naturalHazards)
                {
                    if (currentChance - hazard.chance <= 0)
                    {
                        return hazard.hazardObject;
                    }
                    currentChance -= hazard.chance;
                }
                foreach (var hazard in stage.humanHazards)
                {
                    if (currentChance - hazard.chance <= 0)
                    {
                        return hazard.hazardObject;
                    }
                    currentChance -= hazard.chance;
                }
            }
        }
        return null;
    }


    private float CalcutaleStageHazards(string stageName)
    {
        float currentTotal = 0f;
        foreach (var stage in stageList)
        {
            if (stageName == stage.name)
            {
                foreach (var hazard in stage.naturalHazards)
                {
                    currentTotal += hazard.chance;
                }
                foreach (var hazard in stage.humanHazards)
                {
                    currentTotal += hazard.chance;
                }
            }
        }
        return Mathf.Round(Random.Range(1,currentTotal));
    }
    public float HazardSize(string stageName, GameObject checkObj)
    {
        foreach (var stage in stageList)
        {
            if (stageName == stage.name)
            {
                foreach (var hazard in stage.naturalHazards)
                {
                    if (checkObj == hazard.hazardObject)
                    {
                        return hazard.hazardSize;
                    }
                }
                foreach (var hazard in stage.humanHazards)
                {
                    if (checkObj == hazard.hazardObject)
                    {
                        return hazard.hazardSize;
                    }
                }
            }
        }
        return 0f;
    }

    public void UpgradeHazard(string stageName, string hazardName)
    {
        foreach (var stage in stageList)
        {
            if (stageName == stage.name)
            {
                foreach (var hazard in stage.humanHazards)
                {
                    if (hazardName == hazard.name && hazard.chance != 0)
                    {
                        hazard.chance--;
                    }
                }
            }
        }
    }
    public float UpgradeCount(string stageName, string hazardName)
    {
        foreach (var stage in stageList)
        {
            if (stageName == stage.name)
            {
                foreach (var hazard in stage.humanHazards)
                {
                    if (hazardName == hazard.name)
                    {
                        return hazard.maxChance - hazard.chance;
                    }
                }
            }
        }
        return 0;
    }
    public string HazardFact(string stageName, string hazardName)
    {
        string text = "";
        foreach (var stage in stageList)
        {
            if (stageName == stage.name)
            {
                foreach (var hazard in stage.humanHazards)
                {
                    if (hazardName == hazard.name)
                    {
                        text = hazard.description;
                    }
                }
                foreach (var hazard in stage.naturalHazards)
                {
                    if (hazardName == hazard.name)
                    {
                        text = hazard.description;
                    }
                }
            }
        }
        return text;
    }
    public float UpgradeCheck(string stageName, string hazardName)
    {
        foreach (var stage in stageList)
        {
            if (stageName == stage.name)
            {
                foreach (var hazard in stage.humanHazards)
                {
                    if (hazardName == hazard.name)
                    {
                        return hazard.chance;
                    }
                }
            }
        }
        return 0;
    }
}
