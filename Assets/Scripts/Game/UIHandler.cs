using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    #region UI Variables
    [Header("UI Data")]
    public GameHandler gameHandler;

    [Header("Start Menu")]
    public GameObject startMenuPanel;
    public GameObject infoPanel;
    public GameObject settingPanel;

    [Header("Pause Menu")]
    public GameObject pauseMenuPanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button exitButtonInPause;
    public Button backToMenuButton;

    [Space(10)]
    [Header("Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button infoButton;
    public Button exitButton;

    public Button backFromInfoButton;
    public Button backFromSettingsButton;

    [Header("etc.")]
    public float exampleFloat;

    private bool isPaused = false;

    private void Awake()
    {
        gameHandler = GameHandler.Instance;

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

        if (backFromInfoButton != null)
            backFromInfoButton.onClick.AddListener(OnBackButtonClick);
        if (backFromSettingsButton != null)
            backFromSettingsButton.onClick.AddListener(OnBackButtonClick);

        // Hide the pause menu initially
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }
    #endregion

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

    private void PauseGame()
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
        SceneManager.LoadScene("Beach");
    }

    private void OnSettingsButtonClick()
    {
        if (startMenuPanel != null)
            startMenuPanel.SetActive(false);
        if (settingPanel != null)
            settingPanel.SetActive(true);
    }

    private void OnInfoButtonClick()
    {
        if (startMenuPanel != null)
            startMenuPanel.SetActive(false);
        if (infoPanel != null)
            infoPanel.SetActive(true);
    }

    private void OnBackButtonClick()
    {
        if (startMenuPanel != null)
            startMenuPanel.SetActive(true);
        if (infoPanel != null)
            infoPanel.SetActive(false);
        if (settingPanel != null)
            settingPanel.SetActive(false);
    }
    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
