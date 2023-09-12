using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 1.0f;

    private GridManager gridManager;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    private void Update()
    {
        HandleUserInput();
    }

    private void HandleUserInput()
    {
        //Move left
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            transform.position += Vector3.left;
            if (!IsValidMove())
                transform.position -= Vector3.left;
        }

        //Move Right
        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right;
            if (!IsValidMove())
                transform.position -= Vector3.right;
        }

        //Move down 'Soft Drop'
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Time.time - previousTime > fallTime)
        {
            transform.position += Vector3.down;
            if (!IsValidMove())
            {
                transform.position -= Vector3.down;
                gridManager.StoreTetrominoInGrid((transform));
                this.enabled = false;
                //check for game over
                if (gridManager.IsTopRowOccupied())
                {
                    GameOver();
                    return;
                }
                gridManager.ClearFilledLines();
                FindObjectOfType<Spawner>().SpawnTetromino();
            }
            previousTime = Time.time;
        }

        //Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!IsValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }
    }

    private bool IsValidMove()
    {
        foreach (Transform block in transform)
        {
            int x = Mathf.RoundToInt(block.transform.position.x);
            int y = Mathf.RoundToInt(block.transform.position.y);

            //if block is outside the width of the grid
            if (x < 0 || x > gridManager.GetWidth())
                return false;

            //If block is below the grid
            if (y < 0)
                return false;

            //If block is within grid but position is occupied
            if (y < gridManager.GetHeight() && gridManager.IsOccupied(new Vector2(x, y), block))
                return false;
        }
        return true;
    }

    //Move to a gamemanger eventually
    private void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }
}
