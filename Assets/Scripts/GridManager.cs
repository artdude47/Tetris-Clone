using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int Width = 10;
    public static int Height = 20;
    private Transform[,] grid = new Transform[Width, Height];

    //Check if a position is within the grid boundary
    public bool IsInsideGrid(Vector2 position)
    {
        return (position.x >= 0 && position.x < Width && position.y >= 0);
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

    //Store a tetromino's position in the grid array
    public void StoreTetrominoInGrid(Transform tetromino)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (tetromino.GetChild(0).position.x == x && tetromino.GetChild(0).position.y == y)
                {
                    grid[x, y] = tetromino;
                }
            }
        }
    }

    //Check and remove filled lines
    public int ClearFilledLines()
    {
        int clearedLines = 0;
        for (int y = 0; y < Height; y++)
        {
            if (IsLineFilled(y))
            {
                clearedLines++;
                DeleteLine(y);
                MoveLinesDown(y);
            }
        }
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
}
