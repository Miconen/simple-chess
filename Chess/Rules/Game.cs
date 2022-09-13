using System;
using Chess;
using Chess.Chessboard;


namespace Chess.Rules
{
    public class Game
    {
        public Board board { get; }

        public Game()
        {
            // TODO: Implement game loop
            this.board = new Board();
            board.Populate();
        }

        public void Start() 
        {
            this.board.PrintBoard();
            this.GameLoop();
        }

        public void GameLoop()
        {
            this.board.PrintBoard();
            
            Console.Write("Input rank: ");
            string? inputRank = Console.ReadLine();
            if (inputRank == "break") return;

            Console.Write("Input file: ");
            string? inputFile = Console.ReadLine();
            if (inputFile == "break") return;
            this.GameLoop();
        }
    }
}

