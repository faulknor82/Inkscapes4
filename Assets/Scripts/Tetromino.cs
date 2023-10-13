using UnityEngine.Tilemaps;
using UnityEngine;
    
    public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z
}

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells { get; private set; } // remove get and set if want to create custom shapes

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino];
    }
}