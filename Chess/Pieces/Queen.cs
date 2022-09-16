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
            if (!move.IsDiagonal()) return false;
            return true;
        }
    }
}
