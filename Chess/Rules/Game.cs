using System;
using Chess;
using Chess.Chessboard;
using Chess.Rules;
using Chess.Error;

namespace Chess.Rules
{
    public class Game
    {
        public Board board { get; }
        public ErrorHandler ErrorHandler;
        
        public Turns turn;
        public Game()

        {
            this.board = new Board();
            this.ErrorHandler = new ErrorHandler();
            board.Populate();
            this.turn = new Turns(true);
        }

        public void Start()
        {
            this.ErrorHandler.ShowErrors();
            this.GameLoop();
        }

        public void GameLoop()
        {
            this.board.PrintBoard();
            
            // Write errors from previous run here
            // This prevents them from getting cleared earlier
            this.ErrorHandler.Write();
            this.ErrorHandler.Flush();

            // Let the user select a tile to move from
            int inputRank = this.GetValidInput($"Input rank of a {this.turn.ToString()} piece: ");
            if (inputRank == -1) return;
            int inputFile = this.GetValidInput($"Input file of a {this.turn.ToString()} piece: ");
            if (inputFile == -1) return;


            // Check if given input is valid and error free
            this.CheckValidInput(inputRank, inputFile, "origin");

            // Let the user select a tile to move to
            int inputTargetRank = this.GetValidInput($"Input rank of a tile to move to: ");
            if (inputTargetRank == -1) return;
            int inputTargetFile = this.GetValidInput($"Input file of a tile to move to: ");
            if (inputTargetFile == -1) return;
            // Check if given input is valid and error free
            this.CheckValidInput(inputTargetRank, inputTargetFile, "target");

            // Selected tile
            Tile fromTile = board.Tiles[inputRank, inputFile];
            Tile toTile = board.Tiles[inputTargetRank, inputTargetFile];
            Console.WriteLine($"{fromTile.piece.GetColor(true)} {fromTile.piece.nameShort} from ({inputRank + 1},{inputFile + 1}) to ({inputTargetRank + 1},{inputTargetFile + 1})");

            Move move = new Move(fromTile, toTile, inputRank, inputFile, inputTargetRank, inputTargetFile);
            // List of tiles (as coordinates) which the move travels through
            List<Tuple<int,int>> coordinateList = move.GetTileIndexesBetweenInputs();
            // Convert list of tiles to usable tile objects
            List<Tile> tiles = this.board.CoordinateListToTiles(coordinateList);

            // Check if a piece is blocked, bypass if move has ghosting AKA is allowed to move over other pieces
            bool isBlocked = (fromTile.piece.ghosting) ? false : fromTile.piece.IsBlocked(tiles);
            // Check if the move is valid according to it's movement rules
            bool isValid = fromTile.piece.IsValidMove(move); 

            if (isValid && !isBlocked)
            {
                this.board.Move(move);
                this.turn.SwitchTurn();
            }
            if (!isValid) ErrorHandler.New("Move was not valid");
            if (isBlocked) ErrorHandler.New("Move was blocked by another piece");
            
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

        private void CheckValidInput(int inputRank, int inputFile, string tile)
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
                // Using of selectedTile is only safe after performing
                // the potentially fatal checks, IE checking it's in bounds.
                Tile selectedTile = board.Tiles[inputRank, inputFile];
                bool currentTurn = this.turn.CheckTurn();
                if (tile == "origin")
                {
                    // Check if file contains a piece
                    if (!selectedTile.Occupied()) errors.Add("Tile does not contain a piece");
                    // Piece selected doesn't correspond to turn 
                    else if(selectedTile.piece.IsWhite() != currentTurn) errors.Add($"Cannot move this piece on {this.turn.ToString()}s turn");
                }
                else if (tile == "target")
                {
                    // Check if file contains a piece
                    if (selectedTile.Occupied() && selectedTile.piece.color == currentTurn) errors.Add("Unable to move to a tile occupied by your own piece");
                }
            }

            if (ErrorHandler.IsEmpty()) return;
            this.GameLoop();
            return;
        }
    }
}

