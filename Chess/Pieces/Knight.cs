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
            //Move move = new Move(fromTile, toTile, inputRank, inputFile, inputTargetRank, inputTargetFile);
            /* addMove(this.move.inputRank + 1, y + 2, level);
            addMove(x + 2, y + 1, level);
            addMove(x + 2, y - 1, level);
            addMove(x + 1, y - 2, level);
            addMove(x - 1, y - 2, level);
            addMove(x - 2, y - 1, level);
            addMove(x - 2, y + 1, level);
            addMove(x - 1, y + 2, level);
            return true; */
            int min = Math.Min(move.fromRank - move.toRank, move.fromFile - move.toFile);
            int max = Math.Max(move.fromRank - move.toRank, move.fromFile - move.toFile);

            if (Math.Abs(min) == 1 && Math.Abs(max) == 2) return true;
            return false;
        }
    }
}
