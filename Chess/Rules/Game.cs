using System;
using Chess;
using Chess.Chessboard;
using Chess.Rules;

namespace Chess.Rules
{
    public class Game
    {
        public Board board { get; }

        public Game()
        {
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
            
            // Let the user select a tile to move from
            int inputRank = this.GetValidInput("Input rank: ");
            if (inputRank == -1) return;
            int inputFile = this.GetValidInput("Input file: ");
            if (inputFile == -1) return;

            Console.WriteLine($"{inputRank} {inputFile}");

            // Selected tile
            // TODO: Convert user input to usable array indexes
            // Example: coordinates 1,1 should actually access array [0,0]
            Tile selectedTile = board.Tiles[inputRank, inputFile];

            // Check if selected tile is inBounds and has a piece on it
            if (!board.InBounds(inputRank, inputFile))
            {
                this.GameLoop();
                return;
            }
            if (!selectedTile.Occupied())
            {
                this.GameLoop();
                return;
            }

            // Let the user select a tile to move to
            int inputTargetRank = this.GetValidInput("Input target rank: ");
            if (inputTargetRank == -1) return;
            int inputTargetFile = this.GetValidInput("Input target file: ");
            if (inputTargetFile == -1) return;

            Move move = new Move(selectedTile, inputRank, inputFile, inputTargetRank, inputTargetFile);

            this.GameLoop();
        }

        private int GetValidInput(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "break";
            if (input == "break" || input == null) return -1;
            int output;
            if (!int.TryParse(input, out output)) return this.GetValidInput(message);
            return output -1;
        }
    }
}

