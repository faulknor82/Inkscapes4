using UnityEngine;
using UnityEngine.Tilemaps;

public class LeftBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominoes;

    private void Awake()
    {
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
    }

    public void LeftSet()
    {

    }
}
