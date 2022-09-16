using System;
using Chess;
using Chess.Rules;

namespace Chess.Pieces
{
    public class Queen : Piece
    {
        public Queen(bool color)
        {
            this.color = color;
            this.nameShort = 'Q';
        }

        public override bool IsValidMove(Move move)
        {
            bool validMove = (move.IsDiagonal() || move.IsPerpendicular());
            if (!validMove) return false;
            if (move.IsFreeToMove(this));
            return true;
        }
    }
}
