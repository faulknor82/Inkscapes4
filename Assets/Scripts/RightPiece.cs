using UnityEngine;

public class RightPiece : MonoBehaviour
{
    public RightBoard rightBoard { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int rightPosition { get; private set; }
    public int rightRotationIndex { get; private set; }

    public void RightInitialize(RightBoard rightBoard, Vector3Int rightPosition, TetrominoData data)
    {
        this.rightBoard = rightBoard;
        this.rightPosition = rightPosition;
        this.data = data;
        this.rightRotationIndex = 0;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        this.rightBoard.RightClear(this);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RightRotate(-1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector2Int.down);
        }

        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            RightHardDrop();
        }

        this.rightBoard.RightSet(this);
    }

    private void RightHardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newRightPosition = this.rightPosition;
        newRightPosition.x += translation.x;
        newRightPosition.y += translation.y;

        bool valid = this.rightBoard.IsValidPosition(this, newRightPosition);

        if (valid)
        {
            this.rightPosition = newRightPosition;
        }

        return valid;
    }

    private void RightRotate(int direction)
    {
        int originalRotation = this.rightRotationIndex;
        this.rightRotationIndex = RightWrap(this.rightRotationIndex - direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rightRotationIndex, direction))
        {
            this.rightRotationIndex = originalRotation;
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

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }
        return RightWrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    private int RightWrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}