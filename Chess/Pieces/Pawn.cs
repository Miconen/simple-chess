using System;
using Chess;

namespace Chess.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(bool color)
        {
            this.color = color;
            this.nameShort = 'P';
        }
    }
}

