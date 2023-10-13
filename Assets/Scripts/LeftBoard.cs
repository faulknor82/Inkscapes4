using UnityEngine;
using UnityEngine.Tilemaps;

public class LeftBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public LeftPiece leftActivePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector2Int leftBoardSize = new Vector2Int(10, 20);

    public RectInt Bounds
    {
        get
        {
            Vector2Int leftPosition = new Vector2Int(-this.leftBoardSize.x / 2, -this.leftBoardSize.y / 2);
            return new RectInt(leftPosition, this.leftBoardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.leftActivePiece = GetComponentInChildren<LeftPiece>();
        for(int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        LeftSpawnPiece();
    }

    public void LeftSpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.leftActivePiece.LeftInitialize(this, spawnPosition, data);
        LeftSet(this.leftActivePiece);
    }

    public void LeftSet(LeftPiece leftPiece)
    {
        for(int i = 0; i < leftPiece.cells.Length; i++)
        {
            Vector3Int leftTilePosition = leftPiece.cells[i] + leftPiece.leftPosition;
            this.tilemap.SetTile(leftTilePosition, leftPiece.data.tile);
        }
    }

    public void LeftClear(LeftPiece leftPiece)
    {
        for (int i = 0; i < leftPiece.cells.Length; i++)
        {
            Vector3Int leftTilePosition = leftPiece.cells[i] + leftPiece.leftPosition;
            this.tilemap.SetTile(leftTilePosition, null);
        }
    }

    public bool IsValidPosition(LeftPiece leftPiece, Vector3Int leftPosition)
    {
        RectInt bounds = this.Bounds;
        for(int i = 0; i < leftPiece.cells.Length; i++)
        {
            Vector3Int leftTilePosition = leftPiece.cells[i] + leftPosition;
        
            if(!bounds.Contains((Vector2Int)leftTilePosition))
            {
                return false;
            }

            if(this.tilemap.HasTile(leftTilePosition))
            {
                return false;
            }
        }
        return true;
    }
}
