using SimpleChess.Pieces;

namespace SimpleChess.Chessboard;

public class Tile
{
    public int rank;
    public int file;
    public Piece? piece { get; set; }

    public Tile(int rank, int file)
    {
        this.rank = rank;  // rank == height
        this.file = file;  // file = width
    }

    public bool Occupied()
    {
        if (this.piece != null) return true;
        return false;
    }
}
