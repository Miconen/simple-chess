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
            this.materialValue = 9;
        }

        public override bool IsValidMove(Move move)
        {
            return (move.IsDiagonal() || move.IsPerpendicular());
        }
    }
}
