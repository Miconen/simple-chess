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
        }   

        public override bool IsValidMove(Move move)
        {
            int moveLength = Math.Abs(move.fromRank - move.toRank);
            // Only allow to move two files on first move of the piece
            if (moveLength > 2) return false;
            // Prevent moves larger than 1 if not first move
            if (!this.firstMove && moveLength > 1) return false;
            // Prevent moving across files
            if (move.fromFile != move.toFile) return false;

            if (this.color)
            {
                if (move.fromRank > move.toRank) return false;
            }
            else
            {
                if (move.fromRank < move.toRank) return false;
            }

            this.firstMove = false;
            return true;
        }
    }
}

