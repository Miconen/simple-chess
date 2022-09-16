using System;
using Chess;
using Chess.Pieces;
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

        public bool IsFreeToMove(Piece piece)
        {
            if (this.fromFile == this.toFile)
            {
                min = Math.Min(this.fromRank, this.toRank);
                max = Math.Max(this.fromRank, this.toRank);
            }
            else
            {
                min = Math.Min(this.fromFile, this.toFile);
                max = Math.Max(this.fromFile, this.toFile);
            }

            for (int i = min; i < max; ++i)
            {
                if (this.fromFile == this.toFile)
                {
                    Console.WriteLine(this.fromFile + "/" + i);
                }
                else
                {
                    Console.WriteLine(i + "/" + this.fromRank);
                }
            }

            Console.WriteLine(min + " " + max);

            return true;
        }

        public bool IsPerpendicular()
        {
            return (fromFile == toFile || fromRank == toRank);
        }

        public bool IsDiagonal()
        {
            return (fromFile - fromRank == toFile - toRank || fromFile + fromRank == toFile + toRank);
        }
    }
}
