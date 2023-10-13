using UnityEngine;

public class LeftPiece : MonoBehaviour
{
    public LeftBoard leftBoard { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int leftPosition { get; private set; }

    public void LeftInitialize(LeftBoard leftBoard, Vector3Int leftPosition, TetrominoData data)
    {
        this.leftBoard = leftBoard;
        this.leftPosition = leftPosition;
        this.data = data;

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
        this.leftBoard.LeftClear(this);

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            LeftHardDrop();
        }

        this.leftBoard.LeftSet(this);
    }

    private void LeftHardDrop()
    {
        while(Move(Vector2Int.down))
        {
            continue;
        }
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
        }

        return valid;
    }
}
