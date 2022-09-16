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

        public bool IsBlocked(List<Tile> list)
        {
            bool response = false;
            foreach (Tile tile in list)
            {
                if (tile.piece is Piece) response = true;
            }
            return response;
        }
    }
}

