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
    public UIHandler uiHandler;
    public GameData gameData;
    public string stageName;
    public Transform spawnPos;
    public int initialSpawnNum;
    public GameObject player;
    public bool timerOn;
    public float currentTimer;
    public float loadTimer = 3f;
    public int goldGained;
    public bool gameEnded;
    [Space(10)]
    [Header("Beach")]
    public int beachSize;
    public GameObject beachEnd;

    [Space(10)]
    [Header("Reef")]
    public float stageTop;
    public float stageSpeed;
    public GameObject reefStage;


    #endregion

    private void Start()
    {
        currentLevelData = gameData.currentLevel;
        timerOn = true;
        gameEnded = false;
        currentTimer = 0f;
        if (stageName == "Beach")
        {
            GenerateBeach();
            SpawnBeachEnd();
        }
        else
        {
            for (int i = 0; i < initialSpawnNum; i++)
            {
                SpawnHazard();
            }
        }
    }

    public void ResetGameState()
    {
        timerOn = true;
        gameEnded = false;
        currentTimer = 0f;
        goldGained = 0;
    }


    private void Update()
    {
        if (timerOn)
        {
            currentTimer += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (reefStage != null) MoveReefStage();
    }
    private void MoveReefStage()
    {
        Rigidbody reefRb = reefStage.GetComponent<Rigidbody>();

        Vector3 screenMove = new Vector3(0, 0, stageSpeed * Time.fixedDeltaTime);
        reefRb.MovePosition(reefRb.position + screenMove);
    }

    private void GenerateBeach()
    {
        for (int i = 0; i < beachSize; i++)
        {
            SpawnHazard();
        }
    }

    public void EndGame()
    {
        currentLevelData.levelGold += Mathf.FloorToInt(currentTimer / 2);
        goldGained = Mathf.FloorToInt(currentTimer / 2);
        currentTimer = 0f;
        gameEnded = true;
        Time.timeScale = 0;
    }
    public void CompleteLevel()
    {
        timerOn = false;
        if (stageName == "Beach")
        {
            uiHandler.isLoading = true;
            PlayerController.Instance.GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            EndGame();
        }
    }
    public void SpawnBeachEnd()
    {
        GameObject spawnObj = beachEnd;
        Vector3 newPos = new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z + 4f);
        spawnPos.position = newPos;
        Instantiate(spawnObj, newPos, Quaternion.identity);
    }
    private void SpawnHazard()
    {
        GameObject spawnObj = currentLevelData.RandomHazard(stageName);
        Vector3 newPos = new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z + currentLevelData.HazardSize(stageName, spawnObj));
        spawnPos.position = newPos;
        Instantiate(spawnObj, newPos, Quaternion.identity);
    }
}
