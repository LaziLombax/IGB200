using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private static GameHandler _instance;
    public static GameHandler Instance
    {
        get
        {
            _instance = FindObjectOfType<GameHandler>();
            return _instance;
        }
    }
    #region Game Variables
    [Header("Level Data")]
    public LevelData currentLevelData;
    public string stageName;
    public Transform spawnPos;
    public int initialSpawnNum;
    [Space(10)]
    [Header("Beach")]
    public int beachSize;

    [Space(10)]
    [Header("Reef")]
    public float stageTop;
    public float stageSpeed;
    public GameObject reefStage;


    #endregion

    private void Start()
    {
        if(stageName == "Beach")
        {
            GenerateBeach();
        }
        else
        {
            for (int i = 0; i < initialSpawnNum; i++)
            {
                SpawnHazard();
            }
        }
    }

    public void SpawnHazard()
    {
        GameObject spawnObj = currentLevelData.RandomHazard(stageName);
        Vector3 newPos = new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z + currentLevelData.HazardSize(stageName, spawnObj));
        spawnPos.position = newPos;
        Instantiate(spawnObj, newPos, Quaternion.identity);
    }
    private void Update()
    {
        if (reefStage != null) MoveReefStage();
    }
    private void MoveReefStage()
    {
        reefStage.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, stageSpeed);
    }

    private void GenerateBeach()
    {
        for (int i = 0; i < beachSize; i++)
        {
            SpawnHazard();
        }
    }

    public GameObject EndGamePanel;
    public void EndGame(){
        EndGamePanel.SetActive(true);
        Time.timeScale = 0;
    }
}
