using System;
using Chess;

namespace Chess.Pieces
{
    public class Queen : Piece
    {
        public Queen(bool color)
        {
            this.color = color;
            this.nameShort = 'Q';
        }
    }
}
