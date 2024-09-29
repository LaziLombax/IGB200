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
    public float speedUp;
    public bool isSpeed;
    public GameObject decorObject;
    public Transform decSpawnPos;
    public Transform endLocal;
    public GameObject reefEnd;
    public float levelSize = 9f;


    #endregion
    private void Start()
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
        if (gameData == null) return;
        
        currentLevelData = gameData.currentLevel;
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

            for (int i = 0; i < levelSize; i++)
            {
                //SpawnDecor();
            }
            endLocal = Instantiate(reefEnd, decSpawnPos.position, Quaternion.identity).transform;
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
            if (stageName == "Reef")
            {
                if (spawnPos.position.z < player.transform.position.z + 40 && spawnPos.position.z < endLocal.position.z + 40f)
                {
                    SpawnHazard();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameEnded) return;
        if (reefStage != null && timerOn) MoveReefStage();
    }
    private void MoveReefStage()
    {
        Vector3 screenMove;
        Rigidbody reefRb = reefStage.GetComponent<Rigidbody>();
        if (isSpeed)
        {
            screenMove = new Vector3(0, 0, stageSpeed * speedUp * Time.fixedDeltaTime);
        } else
        {
            screenMove = new Vector3(0, 0, stageSpeed * Time.fixedDeltaTime);
        }
        reefRb.MovePosition(reefRb.position + screenMove);
    }

    private void GenerateLevel()
    {
        for (int i = 0; i < beachSize; i++)
        {
            SpawnHazard();
        }
    }

    public void EndGame(bool beatGame)
    {
        if (beatGame)
        {
            uiHandler.gameOverText.SetActive(false);
            uiHandler.completeText.SetActive(true);
            uiHandler.CheckWinDialogue();
            if (currentTimer < 90)
            {
                goldGained += Mathf.FloorToInt((90 - currentTimer) * 5);
                currentLevelData.levelGold += goldGained;
            }
        }
        else
        {
            uiHandler.gameOverText.SetActive(true);
            uiHandler.completeText.SetActive(false);
            uiHandler.CheckLoseDialogue();
        }

        timerOn = false;
        currentLevelGold = currentLevelData.levelGold;
        currentTimer = 0f;
        gameEnded = true;
        uiHandler.EndGameScreen();
        //Time.timeScale = 0;
    }
    public void CompleteLevel(bool beatGame)
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
            if (beatGame)
            {
                
                EndGame(beatGame);
            }
        }
    }
    public void SpawnBeachEnd()
    {
        Instantiate(beachEnd, spawnPos.position, Quaternion.identity);
    }
    private void SpawnHazard()
    {
        GameObject spawnObj = currentLevelData.RandomHazard(stageName);
        Instantiate(spawnObj, spawnPos.position, Quaternion.identity);
        Vector3 newPos = new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z + currentLevelData.HazardSize(stageName, spawnObj));
        spawnPos.position = newPos;
    }
    private void SpawnDecor()
    {
        Instantiate(decorObject, decSpawnPos.position, Quaternion.identity);
        Vector3 newPos = new Vector3(decSpawnPos.position.x, decSpawnPos.position.y, decSpawnPos.position.z + 40f);
        decSpawnPos.position = newPos;
    }
}
