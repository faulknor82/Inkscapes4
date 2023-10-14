using UnityEngine;
using UnityEngine.Tilemaps;

public class LeftGhost : MonoBehaviour
{
    public Tile tile;
    public LeftBoard board;
    public LeftPiece trackingPiece;

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int leftTilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(leftTilePosition, null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            this.cells[i] = this.trackingPiece.cells[i];
        }
    }

    private void Drop()
    {
        Vector3Int position = this.trackingPiece.leftPosition;

        int current = position.y;
        int bottom = -this.board.leftBoardSize.y / 2 - 1;

        this.board.LeftClear(this.trackingPiece);

        for(int row = current; row >= bottom; row--)
        {
            position.y = row;

            if(this.board.IsValidPosition(this.trackingPiece, position))
            {
                this.position = position;
            } else
            {
                break;
            }
        }

        this.board.LeftSet(this.trackingPiece);
    }

    private void Set()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int leftTilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(leftTilePosition, this.tile);
        }
    }
}

