using UnityEngine;

public class RightPiece : MonoBehaviour
{
    public RightBoard rightBoard { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int rightPosition { get; private set; }

    public void RightInitialize(RightBoard rightBoard, Vector3Int rightPosition, TetrominoData data)
    {
        this.rightBoard = rightBoard;
        this.rightPosition = rightPosition;
        this.data = data;

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
}
