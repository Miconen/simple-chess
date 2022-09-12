using System;
using Chess;

namespace Chess.Pieces
{
    public class Pawn : Piece
    {
        public Pawn()
        {
        }

        public override Move(Move move)
        {
            if (isValidMove(move)) Console.WriteLine("Hello world from pawn");
        }
    }
}

