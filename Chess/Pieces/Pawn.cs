using System;
using Chess;
using Chess.Rules;

namespace Chess.Pieces
{
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
            int moveLength = Math.Abs(move.fromRank - move.toRank);
            int moveOffset = Math.Abs(move.fromFile - move.toFile);

            // Check early if eating move
            if (move.toTile.Occupied() && moveOffset != 1) return false;
            if (moveOffset == 1 && moveLength == 1 && move.toTile.Occupied()) return true;

            // Only allow to move two files on first move of the piece
            if (moveLength > 2) return false;
            // Prevent moves larger than 1 if not first move
            if (!this.firstMove && moveLength > 1) return false;
            // Prevent moving across files
            if (move.fromFile != move.toFile) return false;

            // Prevent pawns from moving backwards, direction depends on color of the pawns
            if (this.color && move.fromRank > move.toRank) return false;
            if (!this.color && move.fromRank < move.toRank) return false;

            this.firstMove = false;
            return true;
        }
    }
}

