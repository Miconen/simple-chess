using System;
using Chess;
using Chess.Chessboard;

namespace Chess.Rules
{
    public class Move
    {
        public Tile tile;
        public int fromRank;
        public int fromFile;
        public int toRank;
        public int toFile;

        public Move(Tile tile, int fromRank, int fromFile, int toRank, int toFile) 
        {
            this.tile = tile;
            this.fromRank = fromRank;
            this.fromFile = fromFile;
            this.toRank = toRank;
            this.toFile = toFile;
        }
    }
}
