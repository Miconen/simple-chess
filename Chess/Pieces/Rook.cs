using System;
using Chess;

namespace Chess.Pieces
{
    public class Rook : Piece
    {
        public Rook(bool color)
        {
            this.color = color;
            this.nameShort = 'R';
        }
    }
}
