using UnityEngine;
using UnityEngine.Tilemaps;

public class LeftBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public LeftPiece leftActivePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector2Int leftBoardSize = new Vector2Int(10, 20);

    public GameObject leftBoardPanel;
    public GameObject controlsPanel;

    public bool IsLeftBoardGameOver = false;
    public int IsLeftBoardGameOver2 = 0;

    public int totalScore;
    public int leftCount;

    public AudioSource oneLineClearAudio;
    public AudioSource twoLinesClearAudio;
    public AudioSource threeLinesClearAudio;
    public AudioSource fourLinesClearAudio;

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
        IsLeftBoardGameOver = false;
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

        if (!IsLeftBoardGameOver)
        {
            this.leftActivePiece.LeftInitialize(this, spawnPosition, data);
        }

        if (IsValidPosition(this.leftActivePiece, this.spawnPosition))
        {
            LeftSet(this.leftActivePiece);
        } else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles();
        leftBoardPanel.SetActive(true);

        IsLeftBoardGameOver = true;
        IsLeftBoardGameOver2 = 1;
        controlsPanel.SetActive(false);
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

    public void ClearLeftLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        
        while(row < bounds.yMax)
        {
            if(IsLineFull(row))
            {
                leftCount++;
                LineClear(row);
                if(leftCount == 1)
                {
                    totalScore += 25;
                    oneLineClearAudio.Play();
                } else if(leftCount == 2)
                {
                    totalScore += 75 - 25;
                    twoLinesClearAudio.Play();
                } else if(leftCount == 3)
                {
                    totalScore += 225 - 75 - 25;
                    threeLinesClearAudio.Play();
                } else if(leftCount > 3)
                {
                    totalScore += 775 - 225 - 75 - 25;
                    fourLinesClearAudio.Play();
                }
            } else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int leftPosition = new Vector3Int(col, row, 0);

            if(!this.tilemap.HasTile(leftPosition))
            {
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int leftPosition = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(leftPosition, null);
        }

        while(row < bounds.yMax)
        {
            for(int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int leftPosition = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(leftPosition);

                leftPosition = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(leftPosition, above);
            }
            row++;
        }
    }
}
