using System;
using Chess;
using Chess.Rules;
using Chess.Chessboard;

namespace Chess.Pieces
{
    public class Rook : Piece
    {
        public Rook(bool color)
        {
            this.color = color;
            this.nameShort = 'R';
            this.materialValue = 5;
        }

        public override bool IsValidMove(Move move)
        {
            if (!move.IsPerpendicular()) return false;
            return true;
        }

        public override List<(int, int)> GetValidTiles(Tile fromTile, Tile[,] tiles)
        {
            var ValidTiles = new List<(int, int)>();
            for (int i = 0; i < 7; i++)
            {
                for (int ii = 0; ii < 7; ii++)
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
    }
}
