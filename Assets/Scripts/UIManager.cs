using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject howToPlayMenu;
    public GameObject pauseMenuPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverHighScoreText;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        HidePauseMenu();
        HideGameOver();
    }

    public void HideMainMenu()
    {
        mainMenuPanel.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);

        gameOverScoreText.text = $"Your Score: {scoreManager.Score.ToString()}";
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowHowToPlayMenu()
    {
        howToPlayMenu.SetActive(true);
    }

    public void HideHowToPlayMenu()
    {
        howToPlayMenu.SetActive(false);
    }

    public void UpdateScore()
    {
        if (currentScoreText != null)
            currentScoreText.text = $"Score: {scoreManager.Score.ToString()}";
    }

    public void HandleStartButton()
    {
        FindObjectOfType<GameManager>().StartGame();
    }

    public void HandleHowToPlayButton()
    {
        ShowHowToPlayMenu();
    }

    public void HandleHowToPlayCloseButton()
    {
        HideHowToPlayMenu();
    }

    public void HandleSettingsButton()
    {

    }

    public void HandleQuitButton()
    {
        Application.Quit();
    }
}
