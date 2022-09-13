using System;
using Chess;

namespace Chess.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(bool color)
        {
            this.color = color;
            this.nameShort = 'B';
        }
    }
}
