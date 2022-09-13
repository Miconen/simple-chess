using System;
using Chess;

namespace Chess.Pieces
{
    public class Piece
    {
        public bool color;
        public char nameShort;

/*         public virtual Move(Move move)
        {
            Console.WriteLine("Hello world");
        }

        public bool isValidMove(Move move)
        {

        } */

        public bool IsWhite()
        {
            return this.color;
        }

        public bool IsBlack()
        {
            return !this.IsWhite();
        }
    }
}

