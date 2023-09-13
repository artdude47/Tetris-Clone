using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public float fallTime = 1.0f;

    private GridManager gridManager;
    private AnimationManager animationManager;

    private GameObject ghost;

    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        animationManager = FindObjectOfType<AnimationManager>();

        //ghost tetromino creation
        ghost = Instantiate(gameObject, transform.position, Quaternion.identity);

        foreach (Transform block in ghost.transform)
        {
            foreach (Transform child in block)
            {
                var childSpriteRenderer = child.GetComponent<SpriteRenderer>();

                if(childSpriteRenderer != null)
                {
                    var color = childSpriteRenderer.color;
                    color.a = 0.22f;
                    childSpriteRenderer.color = color;
                }
            }
            var spriteRenderer = block.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 0.22f;
                spriteRenderer.color = color;
            }
        }
        Destroy(ghost.GetComponent<Tetromino>());
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
                animationManager.PlayTetrominoLanding(this.gameObject);
                this.enabled = false;
                Destroy(ghost);
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

        //Hard drop
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        //Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            ghost.transform.RotateAround(ghost.transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!IsValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                ghost.transform.RotateAround(ghost.transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
        }

        UpdateGhostPosition();
    }

    private void HardDrop()
    {
        while (IsValidMove())
            transform.position += Vector3.down;

        //Once we've found the collision point move up 1 to sit on top
        transform.position -= Vector3.down;

        gridManager.StoreTetrominoInGrid(transform);
        animationManager.PlayTetrominoLanding(this.gameObject);
        this.enabled = false;
        Destroy(ghost);
        //check for game over
        if (gridManager.IsTopRowOccupied())
        {
            GameOver();
            return;
        }
        gridManager.ClearFilledLines();
        FindObjectOfType<Spawner>().SpawnTetromino();
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

    private void UpdateGhostPosition()
    {
        Vector3 ghostPosition = transform.position;

        while (IsValidGhostMove(ghostPosition))
        {
            ghostPosition += Vector3.down;
        }

        ghostPosition -= Vector3.down;

        ghost.transform.position = ghostPosition;
    }

    private bool IsValidGhostMove(Vector3 position)
    {
        foreach (Transform block in transform)
        {
            int x = Mathf.RoundToInt(block.position.x + position.x - transform.position.x);
            int y = Mathf.RoundToInt(block.position.y + position.y - transform.position.y);

            if (x < 0 || x >= gridManager.GetWidth() || y < 0 || (y < gridManager.GetHeight() && gridManager.IsOccupied(new Vector2(x, y), block)))
                return false;
        }
        return true;
    }

}
