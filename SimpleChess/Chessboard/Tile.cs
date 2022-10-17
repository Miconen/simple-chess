using SimpleChess.Pieces;

namespace SimpleChess.Chessboard;

public class Tile
{
    public readonly int Rank;
    public readonly int File;
    public Piece? Piece { get; set; }

    public Tile(int rank, int file)
    {
        this.Rank = rank;  // rank == height
        this.File = file;  // file = width
    }

    public bool Occupied()
    {
        return this.Piece != null;
    }
}
