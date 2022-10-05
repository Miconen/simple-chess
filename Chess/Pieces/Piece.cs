using System;
using Chess;
using Chess.Chessboard;
using Chess.Rules;

namespace Chess.Pieces
{
    public abstract class Piece
    {
        public bool color;
        public char nameShort;
        // Can the piece go through/over other pieces?
        public bool ghosting;
        public int materialValue;

        public Piece()
        {
            // By default don't allow pieces to traverse through others
            this.ghosting = false;
        }

        public bool IsWhite()
        {
            return this.color;
        }

        public bool IsBlack()
        {
            return !this.IsWhite();
        }

        public string GetColor(bool capitalize = false)
        {
            string response = "";
            if (this.color) response = "white";
            else response = "black";

            if (capitalize) response = char.ToUpper(response[0]) + response.Substring(1);
            return response;
        }

        public virtual bool IsValidMove(Move move)
        {
            return true;
        }

        public virtual List<(int, int)> GetValidTiles(Tile fromTile, Tile[,] tiles)
        {
            var ValidTiles = new List<(int, int)>();
            for (int i = fromTile.file; i < 7; i++)
            {
                for (int ii = fromTile.rank; ii < 7; ii++)
                {
                    Tile currentTile = tiles[i, ii];
                    // Determine if valid move tile



                    if (currentTile.Occupied()) continue;



                    // jos valid move niin
                    ValidTiles.Add((i, ii));
                }
            }
            return ValidTiles;
        }


        public bool IsBlocked(List<Tile> list)
        {
            bool response = false;
            Tile targetTile = list[list.Count - 1];

            // Check if both tiles have pieces on them
            if (targetTile.Occupied())
            {
                // Compared if existing pieces are of different color
                if (this.color != targetTile.piece.color) return false;
            }

            // Loop over all tiles in between a move checking if they are occupied
            foreach (Tile tile in list)
            {
                // If piece is not null but type of Piece, we return true
                if (tile.Occupied()) response = true;
            }
            return response;
        }

        public override string ToString()
        {
            return Char.ToString(this.nameShort);
        }
    }

}

