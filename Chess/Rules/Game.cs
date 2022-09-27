using System;
using Chess;
using Chess.Chessboard;
using Chess.Rules;
using Chess.Error;
using Chess.Error.Levels;
using Chess.Pieces;

namespace Chess.Rules
{
    public class Game
    {
        public Board board { get; }
        public ErrorHandler ErrorHandler;
        public Turns turn;
        public CapturedPieces WhiteCapturedPieces;
        public CapturedPieces BlackCapturedPieces;

        public Game()
        {
            this.board = new Board();
            this.ErrorHandler = new ErrorHandler();
            board.Populate();
            this.turn = new Turns(true);
            this.WhiteCapturedPieces = new CapturedPieces();
            this.BlackCapturedPieces = new CapturedPieces();
        }

        public void Start()
        {
            this.ErrorHandler.ToggleDebug();
            this.ErrorHandler.ToggleInformation();
            this.ErrorHandler.ToggleWarnings();
            this.ErrorHandler.ToggleErrors();
            this.GameLoop();
        }

        public void GameLoop()
        {
            this.board.PrintBoard(BlackCapturedPieces, WhiteCapturedPieces);

            // Write errors from previous run here
            // This prevents them from getting cleared earlier
            this.ErrorHandler.Write();
            this.ErrorHandler.Flush();

            // Let the user select a tile to move from
            var inputFrom = this._getValidInput($"[{this.turn.ToString(true)}] Select a piece (letter/number): ");
            if (inputFrom.file == '0' || inputFrom.rank == 0) return;
            // Check if given input is valid and error free
            this.CheckValidInput(inputFrom.rank, inputFrom.file, "origin");
            // Selected tile
            Tile fromTile = board.GetTile(inputFrom.rank, inputFrom.file);
            this.ErrorHandler.New($"{fromTile.piece.GetColor(true)} selected ({fromTile.piece.nameShort}) on ({inputFrom.file}{inputFrom.rank})", Level.Info);

            // Let the user select a tile to move from
            var inputTo = this._getValidInput($"[{this.turn.ToString(true)}] Select a tile to move to (letter/number): ");
            if (inputTo.file == '0' || inputTo.rank == 0) return;
            // Check if given input is valid and error free
            this.CheckValidInput(inputTo.rank, inputTo.file, "target");
            // Selected tile
            Tile toTile = board.GetTile(inputTo.rank, inputTo.file);
            this.ErrorHandler.New($"{fromTile.piece.GetColor(true)} {fromTile.piece.nameShort} from ({inputFrom.file}{inputFrom.rank}) to ({inputTo.file}{inputTo.rank})", Level.Info);

            Move move = new Move(fromTile, toTile, inputFrom.rank, inputFrom.file, inputTo.rank, inputTo.file);
            // List of tiles (as coordinates) which the move travels through
            List<Tuple<int, int>> coordinateList = move.GetTileIndexesBetweenInputs();
            // Convert list of tiles to usable tile objects
            List<Tile> tiles = this.board.CoordinateListToTiles(coordinateList);

            // Check if a piece is blocked, bypass if move has ghosting AKA is allowed to move over other pieces
            bool isBlocked = (fromTile.piece.ghosting) ? false : fromTile.piece.IsBlocked(tiles);
            // Check if the move is valid according to it's movement rules
            bool isValid = fromTile.piece.IsValidMove(move);

            if (isValid && !isBlocked)
            {
                this.board.Move(move, this._getCapturedList());
                this.turn.SwitchTurn();
            }
            if (!isValid) this.ErrorHandler.New("Move was not valid", Level.Warning);
            if (isBlocked) this.ErrorHandler.New("Move was blocked by another piece", Level.Warning);


            this.GameLoop();
        }

        private (char file, int rank) _getValidInput(string message)
        {
            Console.Write(message);

            string input = Console.ReadLine() ?? "break";
            if (input == "break" || input == null) return ('0', 0);

            if (input.Length != 2) return ('0', 0);

            // Check if second character is a letter and store in a variable
            if (!Char.IsLetter(input[0])) return ('0', 0);
            char file = input[0];


            // Check if first character is a digit and store in a variable
            if (!Char.IsDigit(input[1])) return ('0', 0);
            int rank = input[1] - '0';

            if (!this.board.InBounds(file, rank)) return ('0', 0);

            return (file, rank);
        }

        private void CheckValidInput(int inputRank, char inputFile, string tile)
        {
            this.ErrorHandler.New("Checking for valid input with parameters: " + inputRank + ", " + inputFile + ", " + tile, Level.Debug);
            // Errors that lead to other errors have to be caught seperately with ifs
            // Check if selected tile is inBounds and has a piece on it
            if (!this.board.InBounds(inputFile, inputRank))
            {
                this.ErrorHandler.New("Selected tile not in bounds of the board", Level.Warning);
            }
            // Check for non-fatal errors
            else
            {
                // Using of selectedTile is only safe after performing
                // the potentially fatal checks, IE checking it's in bounds.
                Tile selectedTile = board.GetTile(inputRank, inputFile);
                bool currentTurn = this.turn.CheckTurn();
                if (tile == "origin")
                {
                    // Check if file contains a piece
                    if (!selectedTile.Occupied()) this.ErrorHandler.New("Tile does not contain a piece", Level.Warning);
                    // Piece selected doesn't correspond to turn 
                    else if (selectedTile.piece.IsWhite() != currentTurn) this.ErrorHandler.New($"Cannot move this piece on {this.turn.ToString()}s turn", Level.Warning);
                }
                else if (tile == "target")
                {
                    // Check if file contains a piece
                    if (selectedTile.Occupied() && selectedTile.piece.color == currentTurn) this.ErrorHandler.New("Unable to move to a tile occupied by your own piece", Level.Warning);
                }
            }

            if (this.ErrorHandler.IsEmpty())
            {
                this.ErrorHandler.New("Error buffer was empty, succesfully validated input", Level.Debug);
                this.ErrorHandler.Write();
                return;
            }
            this.ErrorHandler.New("Error buffer NOT empty, input couldn't be validated", Level.Debug);
            this.GameLoop();
            return;
        }
        private ref List<Piece> _getCapturedList()
        {
            if (this.turn.turnBool == true) return ref this.WhiteCapturedPieces.List;
            else return ref this.BlackCapturedPieces.List;
        }
    }
}

