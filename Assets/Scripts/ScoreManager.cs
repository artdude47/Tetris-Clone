using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private int score = 0;
    public int Score { get { return score; } }

    private UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        uiManager.UpdateScore();
    }

    public void AddScore(int linesCleared)
    {
        switch (linesCleared)
        {
            case 1:
                score += 40;
                break;
            case 2:
                score += 100;
                break;
            case 3:
                score += 300;
                break;
            case 4:
                score += 1200;
                break;
        }

        uiManager.UpdateScore();
    }

    public void ResetScore()
    {
        score = 0;
    }
}
