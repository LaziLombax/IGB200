using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public RectTransform fadeImage;
    public float fadeSpeed = 1.0f;
    #region UI Variables
    [Header("UI Data")]
    public GameHandler gameHandler;
    public GameData gameData;
    public TextMeshProUGUI levelTimer;
    public TextMeshProUGUI levelGold;
    public TextMeshProUGUI goldGained;
    public float goldCount;
    public TextMeshProUGUI factText;
    public string factToDisplay;
    public TextMeshProUGUI goldDisplay;

    [Header("Start Menu")]
    public GameObject startMenuPanel;
    public GameObject infoPanel;
    public GameObject settingPanel;
    public GameObject mapPanel;
    public GameObject levelPanel;

    [Header("Pause Menu")]
    public GameObject pauseMenuPanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button exitButtonInPause;
    public Button backToMenuButton;
    public bool canPause = true;
    public bool canResume = false;

    [Header("EndGame Menu")]
    public GameObject endgamePanel;
    public GameObject funFactText;
    public Button restartButton;
    public Button homeButton;
    public Button exitButtonInEnd;
    public Button upgradeButton;
    public GameObject gameOverText;
    public GameObject completeText;

    [Header("Upgrade Menu")]
    public GameObject upgradePanel;
    public Button homeButtonInUpgrade;
    public Slider cleanProgress;
    public TextMeshProUGUI upgradeLevelGold;
    public Button upgradeRestartButton;

    public RectTransform upgradeCardSpawn;
    public GameObject upgradeCard;
    public float gapBetweenCards;
    public GameObject hatUnlockable;
    public GameObject levelUnlockable;


    [Header("Location Menu")]
    public TextMeshProUGUI locationName;
    public Button levelButton;
    public Button levelBackButton;
    public List<GameObject> progressList = new List<GameObject>();

    [Header("Hover Interaction")]
    public GameObject greenTrashUI;  
    private bool isUIVisible = false;   
    public GameObject targetObject;  

    [Header("Loading Menu")]
    public GameObject loadingPanel;
    public Slider loadSlider;
    public bool isLoading;
    public float alphaTimer;
    public float progressTimer;
    public bool startWave;
    private Vector2 wavePos;
    public bool panelWaves;
    public GameObject panelToActivate;
    [Space(10)]
    [Header("Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button infoButton;
    public Button exitButton;

    public Button backFromInfoButton;
    public Button backFromSettingsButton;
    public Button backFromMapButton;

    [Header("Health")]
    public List<GameObject> healthIcons = new List<GameObject>();
    public List<GameObject> faceIcons = new List<GameObject>();
    [Header("etc.")]
    public int levelUpto;

    public bool isPaused = false;

    private void Awake()
    {
        levelUpto = 0;
        gameHandler = GameHandler.Instance;
        if (gameHandler != null)
            gameHandler.uiHandler = this;
        AssignButtonListeners();

        
    }
    private void AssignButtonListeners()
    {
        // Assign listeners for the pause menu buttons
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseButtonClick);
        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnPauseButtonClick);  // Resume game
        if (exitButtonInPause != null)
            exitButtonInPause.onClick.AddListener(OnExitButtonClick);
        if (backToMenuButton != null)
            backToMenuButton.onClick.AddListener(OnBackToMenuButtonClick);

        // Optional: Assign listeners for Start Menu buttons if they exist
        if (startButton != null)
            startButton.onClick.AddListener(OnStartButtonClick);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
        if (infoButton != null)
            infoButton.onClick.AddListener(OnInfoButtonClick);
        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitButtonClick);
        if (levelButton != null)
            levelButton.onClick.AddListener(OnLevelButtonClick);
        if (goToHatsButton != null)
            goToHatsButton.onClick.AddListener(OnHatsButtonClick);

        if (backFromInfoButton != null)
            backFromInfoButton.onClick.AddListener(OnBackButtonClick);
        if (backFromSettingsButton != null)
            backFromSettingsButton.onClick.AddListener(OnBackButtonClick);
        if (backFromMapButton != null)
            backFromMapButton.onClick.AddListener(OnBackButtonClick);
        if (levelBackButton != null)
            levelBackButton.onClick.AddListener(OnLevelBackButtonClick);
        if (hatBackButton != null)
            hatBackButton.onClick.AddListener(OnBackButtonClick);

        // Ensure Endgame Panel is initially hidden
        if (endgamePanel != null)
            endgamePanel.SetActive(false);
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartButtonClick);
        if (homeButton != null)
            homeButton.onClick.AddListener(OnBackToMenuButtonClick);
        if (exitButtonInEnd != null)
            exitButtonInEnd.onClick.AddListener(OnExitButtonClick);
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnUpgradeButtonClick);
        if (homeButtonInUpgrade != null)
            homeButtonInUpgrade.onClick.AddListener(OnBackToMenuButtonClick);
        if (upgradeRestartButton != null)
            upgradeRestartButton.onClick.AddListener(OnRestartButtonClick);

        // Hide the pause menu initially
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);


    }
    #endregion
    private float timeElapsed;
    private float lerpDuration = 1f;
    public bool finishedWaves;
    private void Update()
    {
        // Timer handling should only be checked if necessary, not every frame.
        if (isPaused != GameHandler.Instance.timerOn)
        {
            GameHandler.Instance.timerOn = !isPaused;
        }

        // Handle dialogue only when the dialogue box is active and LMB is pressed.
        if (dialogueHandler.dialogueBox.gameObject.activeSelf && InputHandler.Instance.LMBDialogue())
        {
            dialogueHandler.DisplayNextParagraph(currentDialogue);
        }
        if (startWave)
        {
            fadeImage.gameObject.SetActive(true);
            timeElapsed += Time.deltaTime* 0.5f;
            // Lerp the x value from 1 to 0 over time
            float lerpedX = Mathf.Lerp(1f, 0f, timeElapsed);

            // Update wavePos with the lerped x value
            wavePos.x = lerpedX;

            // Update the shader with the new wavePos
            fadeImage.GetComponent<Image>().material.SetVector("_Mask_Position", wavePos);


            // Optionally, stop the effect when it's finished
            if (timeElapsed >= lerpDuration)
            {
                timeElapsed = 0f;
                startWave = false; // Stop the wave effect after the duration
            }
        }
        if (!finishedWaves)
        {
            if(fadeImage.GetComponent<CanvasGroup>()) fadeImage.GetComponent<CanvasGroup>().alpha = 1;
            fadeImage.gameObject.SetActive(true);
            if (panelWaves)
            {
                timeElapsed += Time.deltaTime * 0.5f;
                // Lerp the x value from 1 to 0 over time
                float lerpedX = Mathf.Lerp(1f, 0f, timeElapsed);

                // Update wavePos with the lerped x value
                wavePos.x = lerpedX;

                // Update the shader with the new wavePos
                fadeImage.GetComponent<Image>().material.SetVector("_Mask_Position", wavePos);

                // Optionally, stop the effect when it's finished
                if (timeElapsed >= lerpDuration)
                {
                    panelWaves = false;
                    timeElapsed = 0f; // Stop the wave effect after the duration
                }
            }
            else
            {
                timeElapsed += Time.deltaTime * 0.5f;
                // Lerp the x value from 1 to 0 over time
                float lerpedX = Mathf.Lerp(0f, 1f, timeElapsed);

                // Update wavePos with the lerped x value
                wavePos.x = lerpedX;

                // Update the shader with the new wavePos
                fadeImage.GetComponent<Image>().material.SetVector("_Mask_Position", wavePos);

                // Optionally, stop the effect when it's finished
                if (timeElapsed >= lerpDuration)
                {
                    fadeImage.gameObject.SetActive(false);
                    finishedWaves = true;
                    // Check Dialogue when Start
                    if (!gameData.CheckRead("Game Start"))
                    {
                        currentDialogue = gameData.GetDialogueSet("Game Start");
                        dialogueHandler.DisplayNextParagraph(currentDialogue);
                    }
                    if (gameHandler.stageName == "Beach")
                    {
                        if (!gameData.CheckRead("Stage Beach"))
                        {
                            currentDialogue = gameData.GetDialogueSet("Stage Beach");
                            dialogueHandler.DisplayNextParagraph(currentDialogue);
                        }
                    }
                    if (gameHandler.stageName == "Reef")
                    {
                        if (!gameData.CheckRead("Stage Reef"))
                        {
                            currentDialogue = gameData.GetDialogueSet("Stage Reef");
                            dialogueHandler.DisplayNextParagraph(gameData.GetDialogueSet("Stage Reef"));
                        }
                    }
                    timeElapsed = 0f; // Stop the wave effect after the duration
                }
            }

        }

        // Mouse input for object interaction
        if (Input.GetMouseButtonDown(0) && IsMouseOverTargetObject())
        {
            OnMouseClick();
        }

        // Only update goldDisplay if there's a change
        if (goldDisplay != null && goldDisplay.text != GameHandler.Instance.currentLevelData.levelGold.ToString())
        {
            goldDisplay.text = GameHandler.Instance.currentLevelData.levelGold.ToString();
        }

        // Endgame handling
        if (endgamePanel != null && endgamePanel.activeInHierarchy)
        {
            if (goldCount <= gameHandler.goldGained)
            {
                goldCount += Time.deltaTime * 0.5f;
            }
            float gold = Mathf.Lerp(0, gameHandler.goldGained, goldCount);
            goldGained.text = "Speed Bonus: " + gold.ToString("F0");
        }

        // Game ended handling
        if (gameHandler != null && gameHandler.gameEnded)
        {
            UpdateEndGameUI();
        }

        // Timer update only when timerOn is true
        if (levelTimer != null && gameHandler.timerOn)
        {
            UpdateLevelTimer();
        }
        if (hatPanel != null)
        {
            if (hatPanelIsActive)
            {
                targetHatPosition = new Vector3(platformHatsPos.x, platformHatsPos.y, platformHatsPos.z);
            }
            else
            {
                targetHatPosition = new Vector3(platformHatsPos.x, platformHatsPos.y - 2f, platformHatsPos.z);
            }
            platformHats.position = Vector3.Lerp(platformHats.position, targetHatPosition, Time.deltaTime * 2);
            if (rotateLeft)
            {
                RotateLeft();
            }
            else if (rotateRight)
            {
                RotateRight();
            }
        }
    }

    // Separate method for updating end game UI
    private void UpdateEndGameUI()
    {
        levelGold.text = "Owned: " + gameHandler.currentLevelData.levelGold.ToString();
        upgradeLevelGold.text = "Owned: " + gameHandler.currentLevelData.levelGold.ToString();
        cleanProgress.value = Mathf.Round(gameHandler.currentLevelData.CleanProgression() * 100);
    }
    
    // Separate method for updating the level timer
    private void UpdateLevelTimer()
    {
        float minutes = Mathf.FloorToInt(gameHandler.currentTimer / 60);
        float seconds = Mathf.FloorToInt(gameHandler.currentTimer % 60);
        levelTimer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    private void Start()
    {
        wavePos = Vector3.zero;
        if (progressList.Count > 0)
        {
            for (int i = 0; i < progressList.Count; i++)
            {
                if (gameData.levelDatas[i].CleanProgression() * 100 > 50)
                    levelUpto++;
                if (progressList[i].name == gameData.levelDatas[i].levelName && gameData.levelDatas[i].levelNum <= levelUpto)
                {
                    progressList[i].gameObject.SetActive(true);
                    progressList[i].GetComponent<MapPin>().SetValueForBar(gameData.levelDatas[i].CleanProgression() * 100);
                }
                else
                {
                    progressList[i].gameObject.SetActive(false);
                }
            }
        }

        if (upgradePanel != null)
        {
            GenerateUpgrades();
        }

        if (hatPanel != null)
        {
            platformHatsPos = platformHats.position;
            GenerateHats();
        }

    }

    public void OnMouseEnter()
    {
        if (!isUIVisible && greenTrashUI != null)
        {
            greenTrashUI.SetActive(true);
        }
    }

    private void OnPauseButtonClick()
    {
        if (!isPaused && canPause) // If the game is not paused, pause it
        {
            PauseGame();
        }
        else if (canResume)
        {
            // If the game is already paused, resume it
            ResumeGame();
        }
    }


    public void OnMouseExit()
    {
        if (!isUIVisible && greenTrashUI != null)
        {
            greenTrashUI.SetActive(false);
        }
    }

    public void OnMouseClick()
    {
        if (greenTrashUI != null)
        {
            if (isUIVisible)
            {
                greenTrashUI.SetActive(false);
                isUIVisible = false;
            }
            else
            {
                greenTrashUI.SetActive(true);
                isUIVisible = true;
            }
        }
    }

    private bool IsMouseOverTargetObject()
    {
        if (targetObject == null) return false;

        RectTransform rectTransform = targetObject.GetComponent<RectTransform>();
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);

        return rectTransform.rect.Contains(localMousePosition);
    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            canPause = false;
            // Get or add CanvasGroup for the pause panel
            CanvasGroup pauseCanvasGroup = pauseMenuPanel.GetComponent<CanvasGroup>();
            if (pauseCanvasGroup == null)
            {
                pauseCanvasGroup = pauseMenuPanel.AddComponent<CanvasGroup>();
            }

            pauseMenuPanel.SetActive(true);  // Show the pause menu

            // Animate the fade-in of the pause menu
            pauseCanvasGroup.alpha = 0; // Make sure it's fully transparent before animation
            pauseCanvasGroup.DOFade(1, 0.2f).OnComplete(() =>
            {
                Time.timeScale = 0f;  // Freeze the game AFTER the fade-in animation completes
                isPaused = true;      // Mark the game as paused
                canResume = true;
            });
        }
    }


    private void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            // Get or add CanvasGroup for the pause panel
            CanvasGroup pauseCanvasGroup = pauseMenuPanel.GetComponent<CanvasGroup>();
            if (pauseCanvasGroup == null)
            {
                pauseCanvasGroup = pauseMenuPanel.AddComponent<CanvasGroup>();
            }

            // Resume the game BEFORE starting the animation
            Time.timeScale = 1f;  // Resume the game time immediately
            isPaused = false;     // Mark the game as no longer paused

            // Fade out the pause menu
            pauseCanvasGroup.DOFade(0, 0.2f).OnComplete(() =>
            {
                pauseMenuPanel.SetActive(false);  // Hide the pause panel after fade-out completes
                canPause = true;
                canResume = false;
            });
        }
    }



    public void OnExitButtonClick()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnBackToMenuButtonClick()
    {
        ResumeGame(); // Ensure the game is resumed
        StartFadeTransition("Menu");  // Replace "Beach" with the desired scene name
        startWave = true;
    }

    private void OnStartButtonClick()
    {
        if (startMenuPanel != null)
        {
            CanvasGroup startCanvasGroup = startMenuPanel.GetComponent<CanvasGroup>();
            startCanvasGroup.DOFade(0, 0.5f).OnComplete(() => startMenuPanel.SetActive(false));
        }

        if (mapPanel != null)
        {
            if (!gameData.CheckRead("Game Map"))
            {
                dialogueHandler.DisplayNextParagraph(gameData.GetDialogueSet("Game Map"));
            }
            mapPanel.SetActive(true);
            CanvasGroup mapCanvasGroup = mapPanel.GetComponent<CanvasGroup>();
            mapCanvasGroup.alpha = 0;  
            mapCanvasGroup.DOFade(1, 0.5f);
        }
    }


    private void OnRestartButtonClick()
    {
        if (gameHandler != null)
        {
            gameHandler.ResetGameState();
        }

        StartFadeTransition("Beach");  // Replace "Beach" with the desired scene name
        startWave = true;
    }

    private void OnSettingsButtonClick()
    {
        if (startMenuPanel != null)
        {
            CanvasGroup startCanvasGroup = startMenuPanel.GetComponent<CanvasGroup>();
            startCanvasGroup.DOFade(0, 0.5f).OnComplete(() => startMenuPanel.SetActive(false));
        }

        if (settingPanel != null)
        {
            settingPanel.SetActive(true);
            CanvasGroup settingCanvasGroup = settingPanel.GetComponent<CanvasGroup>();
            settingCanvasGroup.alpha = 0;
            settingCanvasGroup.DOFade(1, 0.5f);
        }
    }

    public void OnLevelButtonClick()
    {
        StartFadeTransition("Beach");  // Replace "Beach" with the desired scene name
        startWave = true;
    }
    public void OnBeachEnd()
    {
        StartFadeTransition("Reef");  // Replace "Beach" with the desired scene name
        startWave = true;
    }
    private void OnInfoButtonClick()
    {
        if (startMenuPanel != null)
        {
            CanvasGroup startCanvasGroup = startMenuPanel.GetComponent<CanvasGroup>();
            startCanvasGroup.DOFade(0, 0.5f).OnComplete(() => startMenuPanel.SetActive(false));
        }

        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            CanvasGroup infoCanvasGroup = infoPanel.GetComponent<CanvasGroup>();
            infoCanvasGroup.alpha = 0;
            infoCanvasGroup.DOFade(1, 0.5f);
        }
    }
    private void OnHatsButtonClick()
    {
        if (startMenuPanel != null)
        {
            CanvasGroup startCanvasGroup = startMenuPanel.GetComponent<CanvasGroup>();
            startCanvasGroup.DOFade(0, 0.5f).OnComplete(() => startMenuPanel.SetActive(false));
        }

        if (hatPanel != null)
        {
            platformHats.gameObject.SetActive(true);
            hatPanel.SetActive(true);
            hatPanelIsActive = true;
            CanvasGroup hatCanvasGroup = hatPanel.GetComponent<CanvasGroup>();
            hatCanvasGroup.alpha = 0;
            hatCanvasGroup.DOFade(1, 0.5f);
        }
    }

    public void OnMapPinClick(string levelName)
{
    gameData.SetCurrentLevel(levelName);
    locationName.text = levelName;

    if (levelPanel != null)
    {
        levelPanel.SetActive(true);

        CanvasGroup levelCanvasGroup = levelPanel.GetComponent<CanvasGroup>();
        if (levelCanvasGroup == null)
        {
            levelCanvasGroup = levelPanel.AddComponent<CanvasGroup>();  
        }

        levelCanvasGroup.alpha = 0;

        levelCanvasGroup.DOFade(1, 0.5f);

        RectTransform levelRect = levelPanel.GetComponent<RectTransform>();
        levelRect.anchoredPosition = new Vector2(-1000, levelRect.anchoredPosition.y); 
        levelRect.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutBack); 
    }
}



    private void StartFadeTransition(string sceneName)
    {
        List<GameObject> panelsToFade = new List<GameObject> { startMenuPanel, infoPanel, settingPanel, mapPanel, levelPanel };

        foreach (GameObject panel in panelsToFade)
        {
            if (panel != null && panel.activeSelf)
            {
                CanvasGroup panelCanvasGroup = panel.GetComponent<CanvasGroup>();
                if (panelCanvasGroup == null)
                {
                    panelCanvasGroup = panel.AddComponent<CanvasGroup>();
                }

                panelCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
                {
                    panel.SetActive(false);
                });
            }
        }

        StartCoroutine(LoadSceneAsyncWithTransition(sceneName));
    }

    private IEnumerator LoadSceneAsyncWithTransition(string sceneName)
    {
        Debug.Log("Trying to Load");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        //yield return new WaitForSeconds(lerpDuration);
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f && timeElapsed == 0)
            {
                Debug.Log("Loading now");
                asyncLoad.allowSceneActivation = true;
            }

            yield return null; 
        }
    }





    private void OnLevelBackButtonClick()
    {
        if (levelPanel != null)
        {
            CanvasGroup levelCanvasGroup = levelPanel.GetComponent<CanvasGroup>();

            if (levelCanvasGroup == null)
            {
                levelCanvasGroup = levelPanel.AddComponent<CanvasGroup>();
            }

            levelCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
            {
                levelPanel.SetActive(false);  // Disable the panel after fade-out completes
            });
        }
    }



    private void OnBackButtonClick()
    {
        if (infoPanel != null)
        {
            CanvasGroup infoCanvasGroup = infoPanel.GetComponent<CanvasGroup>();
            infoCanvasGroup.DOFade(0, 0.5f).OnComplete(() => infoPanel.SetActive(false));
        }

        if (settingPanel != null)
        {
            CanvasGroup settingCanvasGroup = settingPanel.GetComponent<CanvasGroup>();
            settingCanvasGroup.DOFade(0, 0.5f).OnComplete(() => settingPanel.SetActive(false));
        }

        if (mapPanel != null)
        {
            CanvasGroup mapCanvasGroup = mapPanel.GetComponent<CanvasGroup>();
            mapCanvasGroup.DOFade(0, 0.5f).OnComplete(() => mapPanel.SetActive(false));
        }

        if (levelPanel != null)
        {
            CanvasGroup levelCanvasGroup = levelPanel.GetComponent<CanvasGroup>();
            levelCanvasGroup.DOFade(0, 0.5f).OnComplete(() => levelPanel.SetActive(false));
        }

        if (hatPanel != null)
        {
            CanvasGroup hatCanvasGroup = hatPanel.GetComponent<CanvasGroup>();
            hatPanelIsActive = false;
            Invoke(nameof(HidePlatform), 0.5f);
            hatCanvasGroup.DOFade(0, 0.5f).OnComplete(() => hatPanel.SetActive(false));
            
        }

        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(true);
            CanvasGroup startCanvasGroup = startMenuPanel.GetComponent<CanvasGroup>();
            startCanvasGroup.alpha = 0;
            startCanvasGroup.DOFade(1, 0.5f);
        }
    }
    private void HidePlatform()
    {
        platformHats.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        StartFadeTransition("Beach");  // Replace "Beach" with the desired scene name
        startWave = true;
    }

    public void EndGameScreen()
    {
        isPaused = true;
        panelWaves = true;
        finishedWaves = false;
        if (gameData.CheckRead("End Explain"))
        {
            if (GameHandler.Instance.hasWon)
            {
                CheckWinDialogue();
            }
            else
            {
                CheckLoseDialogue();
            }
        }
        StartCoroutine(CheckDialogue("End Explain"));
        StartCoroutine(ActivatePanel(endgamePanel, 1.5f));
    }
    IEnumerator ActivatePanel(GameObject panel, float delay)
    {
        yield return new WaitForSeconds(delay);
        panel.SetActive(true);

        if (panel == endgamePanel)
        {
            if (endgamePanel != null)
                endgamePanel.SetActive(true);
            factText.text = factToDisplay;
            if (factText.text == null)
            {
                funFactText.SetActive(false);
            }
            else
            {
                funFactText.SetActive(true);
            }
        } else if (panel == upgradePanel)
        {
            if (endgamePanel != null)
                endgamePanel.SetActive(false);
            if (upgradePanel != null)
                upgradePanel.SetActive(true);
            if (!gameData.CheckRead("Upgrade Screen"))
            {
                dialogueHandler.DisplayNextParagraph(gameData.GetDialogueSet("Upgrade Screen"));
            }
            cleanProgress.value = Mathf.Round(GameHandler.Instance.currentLevelData.CleanProgression() * 100);
            upgradeLevelGold.text = "Owned: " + gameHandler.currentLevelData.levelGold.ToString();
        } //else if (panel == upgradePanel)
        //{

        //}
    }

    public void CheckLoseDialogue()
    {
        if (!gameData.CheckRead("Lose Screen"))
        {
            StartCoroutine(CheckDialogue("Lose Screen"));
        }
    }
    public void CheckWinDialogue()
    {
        if (!gameData.CheckRead("Win Screen"))
        {
            StartCoroutine(CheckDialogue("Win Screen"));
        }
    }
    IEnumerator CheckDialogue(string name)
    {
        yield return new WaitForSeconds(1f);
        if (!gameData.CheckRead(name))
        {
            currentDialogue = gameData.GetDialogueSet(name);
            dialogueHandler.DisplayNextParagraph(currentDialogue);
        }
        yield return null;
    }
    public void LoadingScreen()
    {
    }

    public void LoseHealth()
    {
        for (int i = healthIcons.Count - 1; i >= 0; i--)
        {
            if (healthIcons[i].activeSelf)
            {
                StartCoroutine(FlashAndRemoveIcon(healthIcons[i]));
                break;
            }
        }
    }

    private IEnumerator FlashAndRemoveIcon(GameObject icon)
    {
        Image image = icon.GetComponent<Image>();
        if (image == null) yield break; 

        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0, 0.2f))
                .Append(image.DOFade(1, 0.2f))
                .SetLoops(2);

        yield return sequence.WaitForCompletion();

        icon.SetActive(false);
    }

    #region UpgradeUI
    public void GenerateUpgrades()
    {
        int cardCount = GameHandler.Instance.currentLevelData.UpgradeCardCount();
        float totalGap = gapBetweenCards + upgradeCard.GetComponent<RectTransform>().rect.width;
        // Calculate the starting point to spawn UI objects
        float totalCardAndGapSize = totalGap * (cardCount -1) + gapBetweenCards;

        float sideOffset = upgradeCardSpawn.rect.width - totalCardAndGapSize;

        float startX = 0f - upgradeCardSpawn.rect.width /2 + sideOffset/2;

        for (int i = 0; i < cardCount; i++)
        {
            // Create a new UI object
            GameObject newUIObject = Instantiate(upgradeCard, upgradeCardSpawn);
            UpgradeCard cardComp = newUIObject.GetComponent<UpgradeCard>();
            cardComp.levelNext = levelUnlockable.GetComponent<Unlockable>();
            cardComp.getHat = hatUnlockable.GetComponent<Unlockable>();
            cardComp.upgradeName = GameHandler.Instance.currentLevelData.GetUpgradeCardName(i);
            // Set the anchored position for the UI element
            RectTransform uiRect = newUIObject.GetComponent<RectTransform>();
            uiRect.anchoredPosition = new Vector2(startX + i * totalGap, 0);

            // Optionally: Set the name of the object to distinguish them
            newUIObject.name = "Card_" + i;
        }
    }
    private void OnUpgradeButtonClick()
    {
        isPaused = true;
        StopAllCoroutines();
        panelWaves = true;
        finishedWaves = false;
        StartCoroutine(ActivatePanel(upgradePanel, 1.5f));
    }
    public void UpdateGold()
    {
        upgradeLevelGold.text = "Owned: " + gameHandler.currentLevelData.levelGold.ToString();
    }
    #endregion
    public void UpdateHealth(int healthcount)
    {
        if (healthcount == 2)
        {
            faceIcons[1].SetActive(true);

            healthIcons[2].SetActive(false);
            healthIcons[1].SetActive(true);
            healthIcons[0].SetActive(true);
        }
        else if (healthcount == 1)
        {
            faceIcons[2].SetActive(true);
            healthIcons[2].SetActive(false);
            healthIcons[1].SetActive(false);
            healthIcons[0].SetActive(true);
        }
        else
        {
            healthIcons[2].SetActive(false);
            healthIcons[1].SetActive(false);
            healthIcons[0].SetActive(false);
        }
    }

    #region Dialogue
    [Header("Dialogue")]
    public DialogueController dialogueHandler;
    public GameData.DialogueShelldon currentDialogue;
    /*
    public GameObject dialogueBox;
    private Queue<string> textSets = new Queue<string>();
    private string p;


    public int textIndex;
    public float textSpeed;
    public string currentDialogueKey;
    public GameObject continueEnd;

    public void StartDialogue(string callKey)
    {
        dialogueBox.SetActive(true);
        continueEnd.SetActive(false);
        if (gameHandler != null)
            if (gameHandler.timerOn)
                gameHandler.timerOn = false;
        dialogueText.text = string.Empty;
        isPaused = true;
        currentDialogueKey = callKey;
        for (int i = 0; i < gameData.GetNumberOfLines(currentDialogueKey); i++)
        {
            
        }
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in gameData.GetDialogue(currentDialogueKey, textIndex).ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        continueEnd.SetActive(true);
    }
    
    void NextLine()
    {
        if (textIndex < gameData.GetNumberOfLines(currentDialogueKey) - 1)
        {
            textIndex++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (gameHandler != null)
                if (!gameHandler.timerOn)
                    gameHandler.timerOn = true;
            isPaused = false;
            gameData.ReadDialogue(currentDialogueKey);
            dialogueBox.SetActive(false);
            if(endgamePanel != null)
                if (endgamePanel.activeInHierarchy && !gameData.CheckRead("End Explain"))
                    StartDialogue("End Explain");
        }
    }
    */
    #endregion
    #region HintBubble

    [Header("Hint")]
    public TextMeshProUGUI hintText;
    public GameObject hintBox;

    public void ShowHint()
    {
        if(hintBox.activeInHierarchy) return;
        hintBox.SetActive(true);
        CanvasGroup hintCanvasGroup = hintBox.GetComponent<CanvasGroup>();
        hintCanvasGroup.DOFade(0.9f, 0.5f);
        Invoke(nameof(DisplayHint),5f);
    }
    public void DisplayHint()
    {
        CanvasGroup hintCanvasGroup = hintBox.GetComponent<CanvasGroup>();
        hintCanvasGroup.DOFade(0, 0.5f);
        Invoke(nameof(HintOff),1f);
    }
    void HintOff()
    {
        hintBox.SetActive(false);
    }
    #endregion
    #region Hats

    [Header("Hats")]

    public Button goToHatsButton;
    public GameObject hatPanel;
    public Button hatBackButton;
    public GameObject hatsButton;
    public RectTransform hatsSpawn;
    public List<HatButton> hatButtonList = new List<HatButton>();
    public void GenerateHats()
    {
        GameHandler.Instance.gameData.CheckAllHats();
        hatButtonList.Clear();
        float gapBetweenHats = 20f;
        int cardCount = GameHandler.Instance.gameData.TotalOfHats();
        float totalGap = gapBetweenHats + hatsButton.GetComponent<RectTransform>().rect.width;
        // Calculate the starting point to spawn UI objects
        float totalCardAndGapSize = totalGap * (cardCount - 1) + gapBetweenHats;

        float sideOffset = hatsSpawn.rect.width - totalCardAndGapSize;

        float startX = 0f - hatsSpawn.rect.width / 2 + sideOffset / 2;

        for (int i = 0; i < cardCount; i++)
        {
            // Create a new UI object
            GameObject newUIObject = Instantiate(hatsButton, hatsSpawn);
            HatButton hatComp = newUIObject.GetComponent<HatButton>();
            hatButtonList.Add(hatComp);
            hatComp.hatKey = GameHandler.Instance.gameData.GetHatKey(i);
            hatComp.icon.sprite = GameHandler.Instance.gameData.GetSpriteHat(hatComp.hatKey);
            if (GameHandler.Instance.gameData.CheckIfUnlockedHat(hatComp.hatKey))
            {
                hatComp.canUse = true;
            }
            if (GameHandler.Instance.gameData.CheckIfInUseHat(hatComp.hatKey))
            {
                hatComp.isSelected = true;
            }
            // Set the anchored position for the UI element
            RectTransform uiRect = newUIObject.GetComponent<RectTransform>();
            uiRect.anchoredPosition = new Vector2(startX + i * totalGap, 0);

            // Optionally: Set the name of the object to distinguish them
            newUIObject.name = "Hat_" + i;
        }
    }
    private bool hatPanelIsActive;
    private bool rotateLeft;
    private bool rotateRight;
    public Transform platformHats;
    public Vector3 platformHatsPos;
    private Vector3 targetHatPosition;
    public void RotateLeftPressed()
    {
        rotateLeft = true;
    }

    public void RotateLeftReleased()
    {
        rotateLeft = false;
    }

    public void RotateRightPressed()
    {
        rotateRight = true;
    }

    public void RotateRightReleased()
    {
        rotateRight = false;
    }

    private void RotateLeft()
    {
        platformHats.Rotate(Vector3.up * 50f * Time.deltaTime);
    }

    private void RotateRight()
    {
        platformHats.Rotate(-Vector3.up * 50f * Time.deltaTime);
    }
    #endregion
}
