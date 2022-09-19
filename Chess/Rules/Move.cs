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

        public List<Tuple<int, int>> GetTileIndexesBetweenInputs()
        {
            var list = new List<Tuple<int, int>>();
            // Vertical movement
            if (this.fromFile == this.toFile)
            {
                Console.WriteLine("Vertical");
                int i = this.fromRank;
                while(i != this.toRank)
                {
                    i = (this.fromRank > this.toRank) ? i - 1 : i + 1;
                    Console.WriteLine(this.fromRank + " " + i);
                    list.Add(new Tuple<int, int>(i, this.fromFile));
                }
            }
            // Horizontal movement
            else if (this.fromRank == this.toRank)
            {
                Console.WriteLine("Horizontal");
                int i = this.fromFile;
                while(i != this.toFile)
                {
                    i = (this.fromFile > this.toFile) ? i - 1 : i + 1;
                    Console.WriteLine(this.fromFile + " " + i);
                    list.Add(new Tuple<int, int>(this.fromRank, i));
                }
            }
            // Diagonal movement
            else
            {
                Console.WriteLine("Diagonal");
                int i = this.fromRank;
                int ii = this.fromFile;
                while(i != this.toRank && ii != this.toFile)
                {
                    i = (this.fromRank > this.toRank) ? i - 1 : i + 1;
                    ii = (this.fromFile > this.toFile) ? ii - 1 : ii + 1;
                    list.Add(new Tuple<int, int>(i, ii));
                }
            }

            return list;
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
