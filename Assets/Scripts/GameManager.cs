using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { MainMenu, Play, Pause, End }
    public GameState currentState = GameState.MainMenu;

    private GridManager gridManager;
    private ScoreManager scoreManager;
    private Spawner spawner;
    private UIManager uiManager;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        spawner = FindObjectOfType<Spawner>();
        uiManager = FindObjectOfType<UIManager>();

        EnterMainMenu();
    }

    private void Update()
    {
        HandleGameStates();
    }

    private void HandleGameStates()
    {
        switch (currentState)
        {
            case GameState.Play:
                //check for game over
                if (gridManager.IsTopRowOccupied())
                    EndGame();

                //check for pause input
                if (Input.GetKeyDown(KeyCode.Escape))
                    PauseGame();
                break;

            case GameState.Pause:
                //Resume game input
                if (Input.GetKeyDown(KeyCode.Escape))
                    ResumeGame();
                break;
        }
    }

    public void EnterMainMenu()
    {
        currentState = GameState.MainMenu;
        uiManager.ShowMainMenu();
    }

    public void StartGame()
    {

        scoreManager.ResetScore();

        spawner.SpawnTetromino();

        currentState = GameState.Play;

        uiManager.HideMainMenu();
        uiManager.HideGameOver();
    }

    public void PauseGame()
    {
        if (currentState == GameState.Play)
        {
            Time.timeScale = 0;
            Debug.Log("Game Paused");
            currentState = GameState.Pause;

            uiManager.ShowPauseMenu();
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Pause)
        {
            Time.timeScale = 1;
            currentState = GameState.Play;
        }

        uiManager.HidePauseMenu();
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        currentState = GameState.End;
        Debug.Log("Game Over");

        uiManager.ShowGameOver();
    }
}
