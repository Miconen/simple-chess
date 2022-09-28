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
            string moveInfo = "";
            // Vertical movement
            if (this.fromFile == this.toFile)
            {
                moveInfo += "Vertical move: ";
                int i = this.fromRank;
                while (i != this.toRank)
                {
                    i = (this.fromRank > this.toRank) ? i - 1 : i + 1;
                    moveInfo += "[" + this.fromRank + " " + i + "] ";
                    list.Add(new Tuple<int, int>(i, this.fromFile));
                }
            }
            // Horizontal movement
            else if (this.fromRank == this.toRank)
            {
                moveInfo += "Horizontal move: ";
                int i = this.fromFile;
                while (i != this.toFile)
                {
                    i = (this.fromFile > this.toFile) ? i - 1 : i + 1;
                    moveInfo += "[" + this.fromFile + " " + i + "] ";
                    list.Add(new Tuple<int, int>(this.fromRank, i));
                }
            }
            // Diagonal movement
            else
            {
                moveInfo += "Diagonal move: ";
                int i = this.fromRank;
                int ii = this.fromFile;
                while (i != this.toRank && ii != this.toFile)
                {
                    i = (this.fromRank > this.toRank) ? i - 1 : i + 1;
                    ii = (this.fromFile > this.toFile) ? ii - 1 : ii + 1;
                    moveInfo += "[" + ii + " " + i + "] ";
                    list.Add(new Tuple<int, int>(i, ii));
                }
            }

            // TODO: Use ErrorHandler class to print this
            Console.WriteLine(moveInfo);

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
