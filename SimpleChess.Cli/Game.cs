using SimpleChess.Chessboard;
using SimpleChess.Cli.Renderer;
using SimpleChess.Logging;
using SimpleChess.Pieces;
using SimpleChess.Rules;

namespace SimpleChess.Cli;

public class Game
{
    private Board Board { get; }
    private readonly BoardRenderer _renderer;
    private readonly Logger _logger = new();
    private readonly CapturedPieces _whiteCapturedPieces = new();
    private readonly CapturedPieces _blackCapturedPieces = new();
    private readonly Fen _fen = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

    public Game()
    {
        this.Board = new Board(this._logger);
        this.Board.Populate(_fen);
        this._renderer = new BoardRenderer(this.Board);
    }

    public void Start()
    {
        this._logger.ToggleDebug();
        this._logger.ToggleInformation();
        this._logger.ToggleWarnings();
        this._logger.ToggleErrors();
        this.GameLoop();
    }

    private void GameLoop()
    {
        this._renderer.Render(_blackCapturedPieces, _whiteCapturedPieces);

        // Write errors from previous run here
        // This prevents them from getting cleared earlier
        this._logger.Write();
        this._logger.Flush();

        // Let the user select a tile to move from
        var inputFrom = this._getValidInput($"[{this._fen.Turn.ToString(true)}] Select a piece (letter/number): ");
        if (inputFrom.file == '0' && inputFrom.rank == -1) return;
        // Check if given input is valid and error free
        this.CheckValidInput(inputFrom.rank, inputFrom.file, "origin");
        // Selected tile
        Tile fromTile = Board.GetTile(inputFrom.rank, inputFrom.file);

        this._renderer.Render(_blackCapturedPieces, _whiteCapturedPieces, fromTile);
        this._logger.Info($"{fromTile.Piece?.GetColor(true)} selected ({fromTile.Piece?.NameShort}) on ({inputFrom.file}{inputFrom.rank})");

        // Let the user select a tile to move from
        var inputTo = this._getValidInput($"[{this._fen.Turn.ToString(true)}] Select a tile to move to (letter/number): ");
        if (inputTo.file == '0' && inputTo.rank == -1) return;
        // Check if given input is valid and error free
        this.CheckValidInput(inputTo.rank, inputTo.file, "target");
        // Selected tile
        Tile toTile = Board.GetTile(inputTo.rank, inputTo.file);
        this._logger.Info($"{fromTile.Piece?.GetColor(true)} {fromTile.Piece?.NameShort} from ({inputFrom.file}{inputFrom.rank}) to ({inputTo.file}{inputTo.rank})");

        Move move = new Move(this._logger, fromTile, toTile);

        // Check if the move is valid and not blocked
        if (fromTile.Piece != null && fromTile.Piece.IsValidMove(move) && Board.MoveIsPossible(move))
        {
            this.Board.Move(move, this.GetCapturedList(), this._fen.Turn);
            if (this.Board.CanBlackPromote(move, this._fen.Turn) || this.Board.CanWhitePromote(move, this._fen.Turn)) {
                PromoteInput(move, this._fen.Turn);
            }
            this._fen.Turn.Next();
        }

        this.GameLoop();
    }

    private void PromoteInput(Move move, Turns turns)
    {
        move.ToTile.Piece = null;
        Console.WriteLine();
        Console.WriteLine("Promote to: K(N)ight, (Q)ueen, (B)ishop or (R)ook?");
        while (true)
        {
            Console.Write("Promote to: ");
            var userInput = Console.ReadLine();
            switch (userInput?.ToLower())
            {
                case "q" or "queen":
                    this.Board.PromotePawn(move, turns, PromoteEnum.Queen);
                    return;
                case "n" or "knight":
                    this.Board.PromotePawn(move, turns, PromoteEnum.Knight);
                    return;
                case "b" or "bishop":
                    this.Board.PromotePawn(move, turns, PromoteEnum.Bishop);
                    return;
                case "r" or "rook":
                    this.Board.PromotePawn(move, turns, PromoteEnum.Rook);
                    return;
                default:
                    Console.WriteLine("Invalid input");
                    continue;
            }
        }
    }

    private ref List<Piece> GetCapturedList()
    {
        return ref _fen.Turn.TurnBool ? ref _whiteCapturedPieces.List : ref _blackCapturedPieces.List;
    }

    private (char file, int rank) _getValidInput(string message)
    {
        Console.Write(message);

        var input = Console.ReadLine();
        switch (input)
        {
            // Break out of game loop check
            case null:
                this._logger.Warning("Invalid input, input can not be empty");
                return ('0', 0);
            case "break":
                this._logger.Info("Exiting game on break command");
                return ('0', -1);
        }

        // Check if valid input
        if (input.Length != 2)
        {
            this._logger.Warning("Invalid input, input has to be two characters");
            return ('0', 0);
        }

        // Check if second character is a letter and store in a variable
        if (!char.IsLetter(input[0]))
        {
            this._logger.Warning("Invalid input, first input character has to be a letter");
        }
        var file = input[0];

        // Check if first character is a digit and store in a variable
        if (!char.IsDigit(input[1]))
        {
            this._logger.Warning("Invalid input, second input character has to be a digit");
        }
        var rank = input[1] - '0';


        // If errors then return out with incorrect input that will be caught later
        // Return correct input/output on valid input
        return !this._logger.IsEmpty() ? ('0', 0) : (file, rank);
    }

    private void CheckValidInput(int inputRank, char inputFile, string tile)
    {
        this._logger.Debug($"Checking for valid input with parameters: {inputRank}, {inputFile}, {tile}");
        // Errors that lead to other errors have to be caught separately with ifs
        // Check if selected tile is inBounds and has a piece on it
        if (!this.Board.InBounds(inputFile, inputRank))
        {
            this._logger.Error("Invalid input, selected tile is not in bounds of the board");
        }
        // Check for non-fatal errors
        else
        {
            // Using of selectedTile is only safe after performing
            // the potentially fatal checks, IE checking it's in bounds.
            var selectedTile = Board.GetTile(inputRank, inputFile);
            var currentTurn = this._fen.Turn.Check();
            switch (tile)
            {
                // Check if file contains a piece
                case "origin" when !selectedTile.Occupied():
                    this._logger.Warning("Tile does not contain a piece");
                    break;
                // Piece selected doesn't correspond to turn 
                case "origin":
                {
                    if (selectedTile.Piece != null && selectedTile.Piece.IsWhite() != currentTurn) this._logger.Warning($"Cannot move this piece on {this._fen.Turn.ToString()}s turn");
                    break;
                }
                case "target":
                {
                    // Check if file contains a piece
                    if (selectedTile.Piece != null && selectedTile.Occupied() && selectedTile.Piece.Color == currentTurn) this._logger.Warning("Unable to move to a tile occupied by your own piece");
                    break;
                }
            }
        }

        if (_logger.IsEmpty())
        {
            _logger.Debug("Error buffer was empty, successfully validated input");
            _logger.Write();
            return;
        }
        _logger.Debug("Error buffer NOT empty, input couldn't be validated");
        GameLoop();
    }
}

