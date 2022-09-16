using System;
using Chess;
using Chess.Chessboard;

namespace Chess.Rules
{
    public class Move
    {
        public Tile fromTile;
        public Tile toTile;
        public int fromRank;
        public int fromFile;
        public int toRank;
        public int toFile;

        public Move(Tile fromTile, Tile toTile, int fromRank, int fromFile, int toRank, int toFile) 
        {
            this.fromTile = fromTile;
            this.toTile = toTile;
            this.fromRank = fromRank;
            this.fromFile = fromFile;
            this.toRank = toRank;
            this.toFile = toFile;
        }

        public bool IsPerpendicular()
        {
            return true;
        }

        public bool IsDiagonal()
        {
            bool response = (fromFile - fromRank == toFile - toRank || fromFile + fromRank == toFile + toRank);
            Console.WriteLine($"{response} {fromFile - fromRank} {toFile - toRank}");
            return response;
        }
    }
}
