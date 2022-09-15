using System;
using Chess;

namespace Chess.Pieces
{
    public abstract class Piece
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

        public string GetColor(bool capitalize = false)
        {
            string response = "";
            if (this.color) response = "white";
            else response = "black";

            if (capitalize) response = char.ToUpper(response[0]) + response.Substring(1);
            return response;
        }
    }
}

