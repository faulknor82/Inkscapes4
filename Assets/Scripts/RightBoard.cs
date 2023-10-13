using UnityEngine;
using UnityEngine.Tilemaps;

public class RightBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public RightPiece rightActivePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector2Int rightBoardSize = new Vector2Int(10, 20);

    public RectInt Bounds
    {
        get
        {
            Vector2Int rightPosition = new Vector2Int(-this.rightBoardSize.x / 2, -this.rightBoardSize.y / 2);
            return new RectInt(rightPosition, this.rightBoardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.rightActivePiece = GetComponentInChildren<RightPiece>();
        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        RightSpawnPiece();
    }

    public void RightSpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.rightActivePiece.RightInitialize(this, spawnPosition, data);
        RightSet(this.rightActivePiece);
    }

    public void RightSet(RightPiece rightPiece)
    {
        for (int i = 0; i < rightPiece.cells.Length; i++)
        {
            Vector3Int rightTilePosition = rightPiece.cells[i] + rightPiece.rightPosition;
            this.tilemap.SetTile(rightTilePosition, rightPiece.data.tile);
        }
    }

    public void RightClear(RightPiece rightPiece)
    {
        for (int i = 0; i < rightPiece.cells.Length; i++)
        {
            Vector3Int rightTilePosition = rightPiece.cells[i] + rightPiece.rightPosition;
            this.tilemap.SetTile(rightTilePosition, null);
        }
    }

    public bool IsValidPosition(RightPiece rightPiece, Vector3Int rightPosition)
    {
        RectInt bounds = this.Bounds;
        for (int i = 0; i < rightPiece.cells.Length; i++)
        {
            Vector3Int rightTilePosition = rightPiece.cells[i] + rightPosition;

            if (!bounds.Contains((Vector2Int)rightTilePosition))
            {
                return false;
            }

            if (this.tilemap.HasTile(rightTilePosition))
            {
                return false;
            }
        }
        return true;
    }

    public void ClearRightLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int rightPosition = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(rightPosition))
            {
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int rightPosition = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(rightPosition, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int rightPosition = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(rightPosition);

                rightPosition = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(rightPosition, above);
            }
            row++;
        }
    }
}