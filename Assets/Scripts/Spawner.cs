using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoes;
    private Vector2 spawnPos = new Vector2(5, 22);
    private GameObject nextPiece = null;
    private UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    public void SpawnTetromino()
    {
        int randomIndex = Random.Range(0, tetrominoes.Length);

        if (nextPiece == null)
        {
            Instantiate(tetrominoes[randomIndex], spawnPos, Quaternion.identity);
            randomIndex = Random.Range(0, tetrominoes.Length);
            nextPiece = tetrominoes[randomIndex];
            uiManager.ShowNextPiece(randomIndex);
        } else
        {
            Instantiate(nextPiece, spawnPos, Quaternion.identity);
            nextPiece = tetrominoes[randomIndex];
            uiManager.HideNextPiece();
            uiManager.ShowNextPiece(randomIndex);
        }
    }
}
