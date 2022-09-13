using System;
using Chess;
using Chess.Pieces;

namespace Chess.Chessboard
{
    public class Tile
    {
        public int rank;
        public int file;
        public Piece? piece { get; set; }

        public Tile(int rank, int file) 
        {
            this.rank = rank;
            this.file = file;
        }

        public bool Occupied()
        {
            if (this.piece != null) return true;
            return false;
        }
    }
}
