using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static int Width = 10;
    public static int Height = 20;
    private Transform[,] grid = new Transform[Width, Height];
    private ScoreManager scoreManager;

    private List<int> rowsToClear = new List<int>();

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    //Check if a position is within the grid boundary
    public bool IsInsideGrid(Vector2 position)
    {
        return (position.x >= 0 && position.x < Width && position.y >= 0 && position.y < Height);
    }

    //Check if a position is already occupied by a block
    public bool IsOccupied(Vector2 position, Transform tetromino)
    {
        if (IsInsideGrid(position))
        {
            return grid[(int)position.x, (int)position.y] != null &&
                grid[(int)position.x, (int)position.y] != tetromino;
        }
        return true;
    }

    //Check if the top row is occupied
    public bool IsTopRowOccupied()
    {
        for (int x = 0; x < Width; x++)
        {
            if (grid[x, Height - 1] != null)
                return true;
        }
        return false;
    }

    //Store a tetromino's position in the grid array
    public void StoreTetrominoInGrid(Transform tetromino)
    {
        foreach (Transform block in tetromino)
        {
            int roundedX = Mathf.RoundToInt(block.position.x);
            int roundedY = Mathf.RoundToInt(block.position.y);

            if (IsInsideGrid(new Vector2(roundedX, roundedY)))
                grid[roundedX, roundedY] = block;
        }
    }

    //Check and remove filled lines
    public void ClearFilledLines()
    {
        rowsToClear.Clear();

        for (int y = 0; y < Height; y++)
        {
            if (IsLineFilled(y))
                rowsToClear.Add(y);
        }

        if (rowsToClear.Count > 0)
        {
            FlashAndDeleteLine(rowsToClear);
            scoreManager.AddScore(rowsToClear.Count);
        }
    }

    private void FlashAndDeleteLine(List<int> rows)
    {
        //Create a sequence for each line
        foreach (int y in rows)
        {
            Sequence flashingSequence = DOTween.Sequence();

            //Add flashing effect to sequence 3 times
            for (int i = 0; i < 3; i++)
            {
                flashingSequence.Append(DOTween.To(() => GetBlocksAlpha(y), alpha => SetBlocksAlpha(y, alpha), 0, 0.1f)); // fade out
                flashingSequence.Append(DOTween.To(() => GetBlocksAlpha(y), alpha => SetBlocksAlpha(y, alpha), 1, 0.1f)); // fade in
            }
        }

        //Once all sequences are completed process cleared lines
        DOTween.Sequence().AppendInterval(0.7f).OnComplete(() => ProcessClearedLines());
    }

    private void ProcessClearedLines()
    {
        List<int> clearedRows = new List<int>();

        //detect and mark cleared lines
        for (int y = 0; y < Height; y++)
        {
            if (IsLineFilled(y))
            {
                clearedRows.Add(y);
            }
        }

        //Delete the lines and shift above lines down
        for (int i = clearedRows.Count - 1; i >= 0; i--)
        {
            int row = clearedRows[i];
            DeleteLine(row);

            for (int j = row; j < Height - 1; j++)
            {
                MoveLineDown(j + 1);
            }
        }
    }

    private float GetBlocksAlpha(int y)
    {
        if (grid[0, y] && grid[0, y].GetComponent<Renderer>())
        {
            return grid[0, y].GetComponent<Renderer>().material.color.a;
        }
        return 1f;
    }

    private void SetBlocksAlpha(int y, float alpha)
    {
        for (int x = 0; x < Width; x++)
        {
            if (grid[x, y] && grid[x, y].GetComponent<Renderer>())
            {
                Color blockColor = grid[x, y].GetComponent<Renderer>().material.color;
                blockColor.a = alpha;
                grid[x, y].GetComponent<Renderer>().material.color = blockColor;
            }
        }
    }

    //Check if line is filled
    private bool IsLineFilled(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    //Delete Filled line
    private void DeleteLine(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }
 

    private void MoveLineDown(int y)
    {
        if (y <= 0)
            return;

        for (int x = 0; x < Width; x++)
        {
            if (grid[x,y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public int GetWidth()
    {
        return Width;
    }

    public int GetHeight()
    {
        return Height;
    }
}
