using System;
using Chess;
using Chess.Rules;

namespace Chess.Pieces
{
    public class Knight : Piece
    {
        public Knight(bool color)
        {
            this.color = color;
            this.nameShort = 'N';
            this.ghosting = true;
        }

        public override bool IsValidMove(Move move)
        {
            int min = Math.Min(move.fromRank - move.toRank, move.fromFile - move.toFile);
            int max = Math.Max(move.fromRank - move.toRank, move.fromFile - move.toFile);
            Console.WriteLine(Math.Abs(min) + " " + Math.Abs(max));

            if (Math.Abs(min) == 1 && Math.Abs(max) == 2) return true;
            if (Math.Abs(min) == 2 && Math.Abs(max) == 1) return true;
            return false;
        }
    }
}
