using UnityEngine;
using UnityEngine.EventSystems;

public class LeftPiece : MonoBehaviour
{
    public LeftBoard leftBoard { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int leftPosition { get; private set; }
    public int leftRotationIndex { get; private set; }

    public float stepDelay { get; private set; }
    public float lockDelay { get; private set; }

    private float stepTime;
    private float lockTime;

    public bool isPaused = false;

    public GameObject leftBoard2;

    public AudioSource leftPieceMoving;

    public AudioSource pieceAtBottom;

    private SceneMan sceneMan;
    public GameObject sceneMana;

    private void Start()
    {
        sceneMan = sceneMana.GetComponent<SceneMan>();
        stepDelay = 1.2f;
        lockDelay = 0.5f;
        stepDelay = stepDelay - (.04f * sceneMan.nextScene);
        leftBoard = leftBoard2.GetComponent<LeftBoard>();
    }

    public void LeftInitialize(LeftBoard leftBoard, Vector3Int leftPosition, TetrominoData data)
    {
        this.leftBoard = leftBoard;
        this.leftPosition = leftPosition;
        this.data = data;
        this.leftRotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        if(this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for(int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        Pause();

        if (!isPaused)
        {
            this.leftBoard.LeftClear(this);

            this.lockTime += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.W))
            {
                LeftRotate(1);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(Vector2Int.right);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Move(Vector2Int.down);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                LeftHardDrop();
            }

            if (Time.time >= this.stepTime)
            {
                Step();
            }

            this.leftBoard.LeftSet(this);
        }
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);

        if(this.lockTime >= this.lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        this.leftBoard.LeftSet(this);
        this.leftBoard.ClearLeftLines();
        this.leftBoard.LeftSpawnPiece();
        leftBoard.totalScore += 10;
        leftBoard.thisLevelScore += 10;
        pieceAtBottom.Play();
        pieceAtBottom.Stop();
    }

    private void LeftHardDrop()
    {
        while(Move(Vector2Int.down))
        {
            continue;
        }

        Lock();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newLeftPosition = this.leftPosition;
        newLeftPosition.x += translation.x;
        newLeftPosition.y += translation.y;

        bool valid = this.leftBoard.IsValidPosition(this, newLeftPosition);

        if(valid)
        {
            this.leftPosition = newLeftPosition;
            this.lockTime = 0f;
            if (!leftBoard.IsLeftBoardGameOver)
            {
                leftPieceMoving.Play();
            }
        }

        return valid;
    }

    private void LeftRotate(int direction)
    {
        int originalRotation = this.leftRotationIndex;
        this.leftRotationIndex = LeftWrap(this.leftRotationIndex - direction, 0, 4);

        ApplyRotationMatrix(direction);

        if(!TestWallKicks(this.leftRotationIndex, direction))
        {
            this.leftRotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }
            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks( int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);
        
        for(int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if(Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if(rotationDirection < 0)
        {
            wallKickIndex--;
        }
        return LeftWrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    private int LeftWrap(int input, int min, int max)
    {
        if(input < min)
        {
            return max - (min - input) % (max - min);
        } else
        {
            return min + (input - min) % (max - min);
        }
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = true;
        }
    }
}
