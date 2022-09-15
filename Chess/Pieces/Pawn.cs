using System;
using Chess;
using Chess.Rules;

namespace Chess.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(bool color)
        {
            this.color = color;
            this.nameShort = 'P';
        }   

        public override bool IsValidMove(Move move)
        {
            if (move.fromRank != move.toRank) return false;
            return true;
        }
    }
}

