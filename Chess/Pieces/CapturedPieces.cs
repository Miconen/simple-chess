using System;
using Chess;
using Chess.Rules;

namespace Chess.Pieces
{
    public class CapturedPieces
    {
        public List<Piece> List;

        public CapturedPieces()
        {
            this.List = new List<Piece>();
        }

        public int CalculateMaterialSum()
        {
            int materialSum = 0;

            foreach (var piece in this.List)
            {
                materialSum += piece.materialValue;
            }
            return materialSum;
        }

        public void PrintList()
        {
            Console.Write(string.Join(", ", this.List));
        }
    }
}
