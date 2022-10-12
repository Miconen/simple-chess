using System;
using SimpleChess.Rules;

namespace SimpleChess.Pieces;

public class Pawn : Piece
{
    private bool firstMove { get; set; }

    public Pawn(bool color)
    {
        this.color = color;
        this.nameShort = 'P';
        this.firstMove = true;
        this.materialValue = 1;
    }

    public override bool IsValidMove(Move move)
    {
        int moveLength = Math.Abs(move.fromTile.rank - move.toTile.rank);
        int moveOffset = Math.Abs(move.fromTile.file - move.toTile.file);

        // Check early if eating move
        if (move.toTile.Occupied() && moveOffset != 1) return false;
        if (moveOffset == 1 && moveLength == 1 && move.toTile.Occupied()) return true;

        // Only allow to move two files on first move of the piece
        if (moveLength > 2) return false;
        // Prevent moves larger than 1 if not first move
        if (!this.firstMove && moveLength > 1) return false;
        // Prevent moving across files
        if (move.fromTile.file != move.toTile.file) return false;

        // Prevent pawns from moving backwards, direction depends on color of the pawns
        if (this.color && move.fromTile.rank > move.toTile.rank) return false;
        if (!this.color && move.fromTile.rank < move.toTile.rank) return false;

        this.firstMove = false;
        return true;
    }
}
