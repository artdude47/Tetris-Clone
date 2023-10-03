using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoes;
    private Vector2 spawnPos = new Vector2(5, 22);
    private GameObject nextPiece = null;
    private UIManager uiManager;
    public float fallTime = 1.0f;
    public float speedUpRate = 0.995f;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        fallTime *= Mathf.Pow(speedUpRate, Time.deltaTime);
        fallTime = Mathf.Max(fallTime, 0.1f);
    }

    public void SpawnTetromino()
    {
        int randomIndex = Random.Range(0, tetrominoes.Length);
        GameObject temp = null;

        if (nextPiece == null)
        {
            temp = Instantiate(tetrominoes[randomIndex], spawnPos, Quaternion.identity);
            randomIndex = Random.Range(0, tetrominoes.Length);
            nextPiece = tetrominoes[randomIndex];
            uiManager.ShowNextPiece(randomIndex);
        } else
        {
            temp = Instantiate(nextPiece, spawnPos, Quaternion.identity);
            nextPiece = tetrominoes[randomIndex];
            uiManager.HideNextPiece();
            uiManager.ShowNextPiece(randomIndex);
        }
        temp.GetComponent<Tetromino>().fallTime = fallTime;
    }
}
