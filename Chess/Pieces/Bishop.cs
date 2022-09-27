using System;
using Chess;
using Chess.Rules;

namespace Chess.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(bool color)
        {
            this.color = color;
            this.nameShort = 'B';
            this.materialValue = 3;
        }

        public override bool IsValidMove(Move move)
        {
            if (!move.IsDiagonal()) return false;
            return true;
        }
    }
}
