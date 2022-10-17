using SimpleChess.Chessboard;
using SimpleChess.Rules;

namespace SimpleChess.Pieces;

public abstract class Piece
{
    public bool Color;
    public char NameShort;
    // Can the piece go through/over other pieces?
    public bool Ghosting;
    public int MaterialValue;

    protected Piece()
    {
        // By default don't allow pieces to traverse through others
        this.Ghosting = false;
    }

    public bool IsWhite()
    {
        return this.Color;
    }

    public bool IsBlack()
    {
        return !this.IsWhite();
    }

    public string GetColor(bool capitalize = false)
    {
        var response = "";
        response = this.Color ? "white" : "black";

        if (capitalize) response = $"{char.ToUpper(response[0])}{response[1..]}";
        return response;
    }

    public virtual bool IsValidMove(Move move)
    {
        return true;
    }

    public override string ToString()
    {
        return char.ToString(this.NameShort);
    }

    public virtual bool[,] GetValidMoves(Tile tile, Tile[,] board)
    {
        // Initialize two dimensional bool array
        var response = new bool[8, 8];

        return response;
    }
}
