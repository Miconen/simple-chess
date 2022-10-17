using SimpleChess.Rules;

namespace SimpleChess.Pieces;

public class Pawn : Piece
{
    private bool FirstMove { get; set; }

    public Pawn(bool color)
    {
        this.Color = color;
        NameShort = 'P';
        FirstMove = true;
        MaterialValue = 1;
    }

    public override bool IsValidMove(Move move)
    {
        var moveLength = Math.Abs(move.FromTile.Rank - move.ToTile.Rank);
        var moveOffset = Math.Abs(move.FromTile.File - move.ToTile.File);

        // Check early if eating move
        if (move.ToTile.Occupied() && moveOffset != 1) return false;
        if (moveOffset == 1 && moveLength == 1 && move.ToTile.Occupied()) return true;

        // Only allow to move two files on first move of the piece
        if (moveLength > 2) return false;
        // Prevent moves larger than 1 if not first move
        if (!FirstMove && moveLength > 1) return false;
        // Prevent moving across files
        if (move.FromTile.File != move.ToTile.File) return false;

        // Prevent pawns from moving backwards, direction depends on color of the pawns
        if (Color && move.FromTile.Rank > move.ToTile.Rank) return false;
        if (!Color && move.FromTile.Rank < move.ToTile.Rank) return false;

        FirstMove = false;
        return true;
    }
}
