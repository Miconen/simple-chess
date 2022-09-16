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
            int min, max;

            // If on same vertical
            if (this.fromFile == this.toFile)
            {
                min = Math.Min(this.fromRank, this.toRank);
                max = Math.Max(this.fromRank, this.toRank);
            }
            // If on same horizontal
            else if (this.fromRank == this.toRank)
            {
                min = Math.Min(this.fromFile, this.toFile);
                max = Math.Max(this.fromFile, this.toFile);
            }
            // If diagonal
            else
            {
                int minFile, maxFile, minRank, maxRank;

                minFile = Math.Min(this.fromFile, this.toFile);
                maxFile = Math.Max(this.fromFile, this.toFile);
                minRank = Math.Min(this.fromRank, this.toRank);
                maxRank = Math.Max(this.fromRank, this.toRank);

                min = Math.Min(minFile, minRank);
                max = Math.Max(maxFile, maxRank);
            }

            for (int i = min; i < max; ++i)
            {
                if (this.fromFile == this.toFile)
                {
                    list.Add(new Tuple<int,int>(i + 1, this.fromFile + 1));
                }
                else if (this.fromRank == this.toRank)
                {
                    list.Add(new Tuple<int,int>(this.fromRank + 1, i + 1));
                }
                // TODO: No worky
                else
                {
                    list.Add(new Tuple<int, int>(min + i + 1, max - i + 1));
                }
            }

            Console.WriteLine(min + " " + max);

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
