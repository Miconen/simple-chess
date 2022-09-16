using System;
using Chess;
using Chess.Rules;

namespace Chess.Pieces
{
    public class Rook : Piece
    {
        public Rook(bool color)
        {
            this.color = color;
            this.nameShort = 'R';
        }

        public override bool IsValidMove(Move move)
        {
            if (!move.IsPerpendicular()) return false;
            return true;
        }
    }
}
