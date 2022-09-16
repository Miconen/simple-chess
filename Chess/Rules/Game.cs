using System;
using Chess;
using Chess.Chessboard;
using Chess.Rules;

namespace Chess.Rules
{
    public class Game
    {
        public Board board { get; }
        
        public Turns playerTurn;
        public Game()

        {
            this.board = new Board();
            board.Populate();
            this.playerTurn = new Turns(true);
        }

        public void Start()
        {
            //this.board.PrintBoard();
            this.GameLoop();
        }

        public void GameLoop(List<string> errors = null)
        {
            this.board.PrintBoard();

            // Write errors from previous run here
            // This prevents them from getting cleared earlier
            this.WriteErrors(errors);

            // Let the user select a tile to move from
            int inputRank = this.GetValidInput("Input rank: ");
            if (inputRank == -1) return;
            int inputFile = this.GetValidInput("Input file: ");
            if (inputFile == -1) return;


            bool checkTurn = playerTurn.CheckTurn();
            if(board.Tiles[inputRank, inputFile].piece.IsWhite() != checkTurn) 
            {
                // selected wrong piece
                this.GameLoop();
                return;
            }
            playerTurn.SwitchTurn();
            

            // Check if given input is valid and error free
            this.CheckValidInput(inputRank, inputFile);

            // Let the user select a tile to move to
            int inputTargetRank = this.GetValidInput("Input target rank: ");
            if (inputTargetRank == -1) return;
            int inputTargetFile = this.GetValidInput("Input target file: ");
            if (inputTargetFile == -1) return;

            // Selected tile
            Tile fromTile = board.Tiles[inputRank, inputFile];
            Tile toTile = board.Tiles[inputTargetRank, inputTargetFile];
            Console.WriteLine($"{fromTile.piece.GetColor(true)} {fromTile.piece.nameShort}");

            Move move = new Move(fromTile, toTile, inputRank, inputFile, inputTargetRank, inputTargetFile);
            if (fromTile.piece.IsValidMove(move)) board.Move(move);

            this.GameLoop();
        }

        private int GetValidInput(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "break";
            if (input == "break" || input == null) return -1;
            int output;
            if (!int.TryParse(input, out output)) return this.GetValidInput(message);
            return output - 1;
        }

        private void CheckValidInput(int inputRank, int inputFile)
        {
            var errors = new List<string>();
            // Errors that lead to other errors have to be caught seperately with ifs
            // Check if selected tile is inBounds and has a piece on it
            if (!board.InBounds(inputRank, inputFile))
            {
                errors.Add("Selected tile not in bounds of the board");
            }
            // Check for non-fatal errors
            else
            {
                // Check if file contains a piece
                Tile selectedTile = board.Tiles[inputRank, inputFile];
                if (!selectedTile.Occupied()) errors.Add("Tile does not contain a piece");
            }

            if (errors.Count == 0) return;

            this.GameLoop(errors);
            return;
        }

        private void WriteErrors(List<string> errors)
        {
            if (errors == null) return;

            foreach (string error in errors)
            {
                Console.WriteLine(error);
            }
        }
    }
}

