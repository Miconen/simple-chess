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
            this.rank = rank;  // rank == height
            this.file = file;  // file = width
        }

        public bool Occupied()
        {
            if (this.piece != null) return true;
            return false;
        }

        // Add Equals method to compare if a tile on the board is the same as the one a piece was moved from and to
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Tile)) return false;
            Tile other = (Tile)obj;

            if (this.rank != other.rank) return false;
            if (this.file != other.file) return false;
            return true;
        }

    }
}
