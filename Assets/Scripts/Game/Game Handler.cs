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
    public AudioData gameAudioData;
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
    public int currentLevelGold;
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
    private void Awake()
    {
        if (!GameObject.FindGameObjectWithTag("Music"))
        {
            GameObject myMusicObject = Instantiate(new GameObject(), transform.position, transform.rotation);
            myMusicObject.name = "GameMusic";
            myMusicObject.tag = "Music";
            AudioSource gameMusic = gameAudioData.AddNewAudioSourceFromStandard("Game", myMusicObject, "Game Music");
            myMusicObject.AddComponent<DontDestroyOverScene>();
            gameMusic.Play();
            gameMusic.loop = true;
        }
    }
    private void Start()
    {
        if (gameData == null) return;
        
        currentLevelData = gameData.currentLevel;
        timerOn = true;
        gameEnded = false;
        if (stageName == "Beach")
        {
            currentTimer = 0f;
            GenerateLevel();
            SpawnBeachEnd();
        }
        else if (stageName == "Reef")
        {
            currentTimer = currentLevelData.currentTimer;
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
        if (gameEnded) return;
        if (gameData == null) return;
        if (timerOn)
        {
           currentTimer += Time.deltaTime;
        }
        if (spawnPos != null)
        {
            if (stageName == "Reef" && spawnPos.position.z < player.transform.position.z + 20 && spawnPos.position.z < 300f)
            {
                SpawnHazard();
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameEnded) return;
        if (reefStage != null) MoveReefStage();
    }
    private void MoveReefStage()
    {
        Rigidbody reefRb = reefStage.GetComponent<Rigidbody>();

        Vector3 screenMove = new Vector3(0, 0, stageSpeed * Time.fixedDeltaTime);
        reefRb.MovePosition(reefRb.position + screenMove);
    }

    private void GenerateLevel()
    {
        for (int i = 0; i < beachSize; i++)
        {
            SpawnHazard();
        }
    }

    public void EndGame()
    {
        goldGained = Mathf.FloorToInt(currentTimer *3);
        currentLevelData.levelGold += goldGained;

        currentLevelGold = currentLevelData.levelGold;
        currentTimer = 0f;
        gameEnded = true;
        //Time.timeScale = 0;
    }
    public void CompleteLevel()
    {
        timerOn = false;
        if (stageName == "Beach")
        {
            currentLevelData.currentTimer = currentTimer;
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
