using UnityEngine;
using UnityEngine.Tilemaps;

public class RightBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominoes;

    private void Awake()
    {
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
    }

    public void RightSet()
    {

    }
}
