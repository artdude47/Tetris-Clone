using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int Width = 10;
    public static int Height = 20;
    private Transform[,] grid = new Transform[Width, Height];
    private ScoreManager scoreManager;
    private AnimationManager animationManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        animationManager = FindObjectOfType<AnimationManager>();
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
    public int ClearFilledLines()
    {
        int clearedLines = 0;
        for (int y = 0; y < Height;)
        {
            if (IsLineFilled(y))
            {
                clearedLines++;
                DeleteLine(y);
                MoveLinesDown(y);
            }
            else
            {
                y++;
            }
        }

        if (clearedLines > 0)
            scoreManager.AddScore(clearedLines);

        return clearedLines;
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
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    //Shift lines down;
    private void MoveLinesDown(int fromRow)
    {
        for (int y = fromRow; y < Height - 1; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (grid[x, y + 1] != null)
                {
                    grid[x, y] = grid[x, y + 1];
                    grid[x, y + 1] = null;
                    grid[x, y].position += new Vector3(0, -1, 0);
                }
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
