using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoes;
    private Vector2 spawnPos = new Vector2(5, 22);

    public void SpawnTetromino()
    {
        int randomIndex = Random.Range(0, tetrominoes.Length);

        Instantiate(tetrominoes[randomIndex], spawnPos, Quaternion.identity);
    }
}
