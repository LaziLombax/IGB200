using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIHandler : MonoBehaviour
{
    public RectTransform fadeImage;
    public float fadeSpeed = 1.0f;
    #region UI Variables
    [Header("UI Data")]
    public GameHandler gameHandler;
    public GameData gameData;
    public Text levelTimer;
    public Text levelGold;
    public Text goldGained;
    public float goldCount;
    public Text factText;
    public string factToDisplay;

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

    [Header("EndGame Menu")]
    public GameObject endgamePanel;
    public Button restartButton;
    public Button homeButton;
    public Button exitButtonInEnd;
    public Button upgradeButton;

    [Header("Upgrade Menu")]
    public GameObject upgradePanel;
    public Button homeButtonInUpgrade;
    public Slider cleanProgress;
    public Text upgradeLevelGold;
    public Button upgradeRestartButton;

    //temp
    public GameObject upgradeToHide;
    public string upgradeName;


    [Header("Location Menu")]
    public Text locationName;
    public Button levelButton;
    public Button levelBackButton;

    [Header("Loading Menu")]
    public GameObject loadingPanel;
    public Slider loadSlider;
    public bool isLoading;
    public float alphaTimer;
    public float progressTimer;
    [Space(10)]
    [Header("Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button infoButton;
    public Button exitButton;

    public Button backFromInfoButton;
    public Button backFromSettingsButton;
    public Button backFromMapButton;

    [Header("etc.")]
    public float exampleFloat;

    private bool isPaused = false;

    private void Awake()
    {
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

        if (backFromInfoButton != null)
            backFromInfoButton.onClick.AddListener(OnBackButtonClick);
        if (backFromSettingsButton != null)
            backFromSettingsButton.onClick.AddListener(OnBackButtonClick);
        if (backFromMapButton != null)
            backFromMapButton.onClick.AddListener(OnBackButtonClick);
        if (levelBackButton != null)
            levelBackButton.onClick.AddListener(OnLevelBackButtonClick);

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


        if (upgradeToHide != null)
            homeButtonInUpgrade.onClick.AddListener(OnBackToMenuButtonClick);

        // Hide the pause menu initially
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);


    }
    #endregion

    private void Update()
    {
        if (isLoading)
        {
            LoadingScreen();
        }
        if (gameHandler != null)
        {
            if (gameHandler.gameEnded)
            {
                EndGameScreen();
                levelGold.text = "Owned: " + gameHandler.currentLevelData.levelGold.ToString();
                upgradeLevelGold.text = "Owned: " + gameHandler.currentLevelData.levelGold.ToString();
                cleanProgress.value = Mathf.Round(gameHandler.currentLevelData.CleanProgression() * 100);
                if (gameHandler.currentLevelData.UpgradeCheck(gameHandler.stageName, upgradeName) == 0) upgradeToHide.SetActive(false);
            }
            if (levelTimer != null)
            {
                if (!gameHandler.timerOn) return;
                float minutes = Mathf.FloorToInt(gameHandler.currentTimer / 60);
                float seconds = Mathf.FloorToInt(gameHandler.currentTimer % 60);

                levelTimer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
            }
        }
        
    }
    private void OnPauseButtonClick()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true); // Show the pause panel
        Time.timeScale = 0f; // Freeze the game
    }

    private void ResumeGame()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false); // Hide the pause panel
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
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
        SceneManager.LoadScene("Menu"); // Load the main menu scene (replace with your scene name)
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

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        // Trigger the transition animation before loading the scene
        StartFadeTransition("Beach");  // Replace "Beach" with the desired scene name
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

        DOVirtual.DelayedCall(0.5f, () =>
        {
            fadeImage.anchoredPosition = new Vector2(-Screen.width, 0); 
            fadeImage.GetComponent<CanvasGroup>().alpha = 0;

            float slideDuration = 3.0f; 
            fadeImage.DOAnchorPosX(0, slideDuration).SetEase(Ease.Linear);  
            fadeImage.GetComponent<CanvasGroup>().DOFade(1, slideDuration).SetEase(Ease.Linear);  
            StartCoroutine(LoadSceneAsyncWithTransition(sceneName, slideDuration));
        });
    }

    private IEnumerator LoadSceneAsyncWithTransition(string sceneName, float slideDuration)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; 
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f && slideDuration <= 0)
            {
                asyncLoad.allowSceneActivation = true;
            }

            slideDuration -= Time.deltaTime; 
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


    private void OnUpgradeButtonClick()
    {

        if (endgamePanel != null)
            endgamePanel.SetActive(false);
        if (upgradePanel != null)
            upgradePanel.SetActive(true);
        cleanProgress.value = Mathf.Round(GameHandler.Instance.currentLevelData.CleanProgression() * 100);
        upgradeLevelGold.text = "Owned: " + gameHandler.currentLevelData.levelGold.ToString();
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

        if (startMenuPanel != null)
        {
            startMenuPanel.SetActive(true);
            CanvasGroup startCanvasGroup = startMenuPanel.GetComponent<CanvasGroup>();
            startCanvasGroup.alpha = 0;
            startCanvasGroup.DOFade(1, 0.5f);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void EndGameScreen()
    {
        endgamePanel.SetActive(true);
        if(goldCount <= gameHandler.goldGained)
            goldCount += Time.deltaTime * 0.5f;
        float gold = Mathf.Lerp(0, gameHandler.goldGained, goldCount);
        goldGained.text = "Gained: " + gold.ToString("F0");
        factText.text = factToDisplay;
    }

    public void LoadingScreen()
    {
        loadingPanel.SetActive(true);
        alphaTimer += Time.deltaTime * 0.002f;
        float alpha = Mathf.Lerp(0, 255, alphaTimer);
        Debug.Log(alpha.ToString());
        loadingPanel.GetComponent<Image>().color = new Color(255, 255, 255, alpha);
        if (alpha >= 1)
            progressTimer +=  Time.deltaTime * 0.5f;
        loadSlider.value = Mathf.Lerp(0, 100, progressTimer);

        if (loadSlider.value >= 100)
            SceneManager.LoadScene("Reef");
    }

    public void OnClickUpgrade(string hazardName)
    {
        if (GameHandler.Instance.currentLevelData.levelGold < 50) return;

        GameHandler.Instance.currentLevelData.levelGold -= 50;
        GameHandler.Instance.currentLevelData.UpgradeHazard(GameHandler.Instance.stageName, hazardName);
    }

}
