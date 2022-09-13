using System;
using Chess;

namespace Chess.Pieces
{
    public class Knight : Piece
    {
        public Knight(bool color)
        {
            this.color = color;
            this.nameShort = 'N';
        }
    }
}
