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
}
