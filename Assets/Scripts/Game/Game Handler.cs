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
    public Transform playerPos;
    [Space(10)]
    [Header("Beach")]
    public int beachSize;
    public GameObject checkPoint;
    public GameObject beachEnd;

    [Space(10)]
    [Header("Reef")]
    public float stageTop;
    public float stageSpeed;
    public GameObject reefStage;
    public float speedUp;
    public bool isSpeed;
    private AudioSource playerSwim;
    private bool isPlayingSwim;
    public GameObject decorObject;
    public Transform decSpawnPos;
    public Transform endLocal;
    public GameObject reefEnd;
    public int levelSize = 9;


    #endregion
    private void Start()
    {
        if (stageName == string.Empty)
            uiHandler.finishedWaves = true;
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
            AudioSource gulls = gameAudioData.AddNewAudioSourceFromStandard("Game", gameObject, "Gulls");
            gulls.Play();
            gulls.loop = true;
            currentTimer = 0f;
            GenerateLevel();
            SpawnBeachEnd();
        }
        else if (stageName == "Reef")
        {
            AudioSource seaAmb = gameAudioData.AddNewAudioSourceFromStandard("Game", gameObject, "Sea Amb");
            seaAmb.Play();
            seaAmb.loop = true;

            playerSwim = gameAudioData.AddNewAudioSourceFromStandard("Game", gameObject, "Sea Swim");
            playerSwim.loop = true;
            currentTimer = currentLevelData.currentTimer;
            for (int i = 0; i < initialSpawnNum; i++)
            {
                //SpawnHazard();
            }

            Vector3 endPos = decSpawnPos.position;
            endPos.z += 80f + (40f * levelSize);
            endLocal = Instantiate(reefEnd, endPos, Quaternion.identity).transform;
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
            if (!isPlayingSwim) 
            {
                playerSwim.Play();
                isPlayingSwim = true;
            }
            screenMove = new Vector3(0, 0, stageSpeed * speedUp * Time.fixedDeltaTime);
        } else
        {
            if (isPlayingSwim)
            {
                playerSwim.Stop();
                isPlayingSwim = false;
            }
            screenMove = new Vector3(0, 0, stageSpeed * Time.fixedDeltaTime);
        }
        reefRb.MovePosition(reefRb.position + screenMove);
    }
    private void GenerateLevel()
    {
        for (int j = 0; j < 7; j++)
        {
            GameObject spawnObj = checkPoint;
            Instantiate(spawnObj, spawnPos.position, Quaternion.identity);
            Vector3 newPos = new Vector3(spawnPos.position.x, spawnPos.position.y, spawnPos.position.z + 4);
            spawnPos.position = newPos;
            for (int i = 0; i < 5; i++)
            {
                SpawnHazard();
            }
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
            uiHandler.OnBeachEnd();
            uiHandler.fadeImage.gameObject.SetActive(true);
        }
        else
        {
            if (beatGame && stageName == "Reef")
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
    public void TakeDamage()
    {
        if (uiHandler != null)
        {
            uiHandler.LoseHealth();
        }
    }
}
