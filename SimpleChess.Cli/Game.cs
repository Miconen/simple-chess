using System;

using SimpleChess.Chessboard;
using SimpleChess.Pieces;
using SimpleChess.Logging;
using SimpleChess.Rules;

using SimpleChess.Cli.Renderer;

namespace SimpleChess.Cli.Game;

public class Game
{
    public Board board { get; }
    public BoardRenderer Renderer;
    public Logger Logger;
    public Turns turn;
    public CapturedPieces WhiteCapturedPieces;
    public CapturedPieces BlackCapturedPieces;
    public FEN FEN;

    public Game()
    {
        this.Logger = new Logger();
        this.FEN = new FEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        this.board = new Board(this.Logger);
        this.board.Populate(FEN);
        this.Renderer = new BoardRenderer(this.board);
        this.turn = new Turns(true);
        this.WhiteCapturedPieces = new CapturedPieces();
        this.BlackCapturedPieces = new CapturedPieces();
    }

    public void Start()
    {
        this.Logger.ToggleDebug();
        this.Logger.ToggleInformation();
        this.Logger.ToggleWarnings();
        this.Logger.ToggleErrors();
        this.GameLoop();
    }

    public void GameLoop()
    {
        this.Renderer.Render(BlackCapturedPieces, WhiteCapturedPieces);

        // Write errors from previous run here
        // This prevents them from getting cleared earlier
        this.Logger.Write();
        this.Logger.Flush();

        // Let the user select a tile to move from
        var inputFrom = this._getValidInput($"[{this.turn.ToString(true)}] Select a piece (letter/number): ");
        if (inputFrom.file == '0' && inputFrom.rank == -1) return;
        // Check if given input is valid and error free
        this.CheckValidInput(inputFrom.rank, inputFrom.file, "origin");
        // Selected tile
        Tile fromTile = board.GetTile(inputFrom.rank, inputFrom.file);

        this.Renderer.Render(BlackCapturedPieces, WhiteCapturedPieces, fromTile);
        this.Logger.Info($"{fromTile.piece.GetColor(true)} selected ({fromTile.piece.nameShort}) on ({inputFrom.file}{inputFrom.rank})");

        // Let the user select a tile to move from
        var inputTo = this._getValidInput($"[{this.turn.ToString(true)}] Select a tile to move to (letter/number): ");
        if (inputTo.file == '0' && inputTo.rank == -1) return;
        // Check if given input is valid and error free
        this.CheckValidInput(inputTo.rank, inputTo.file, "target");
        // Selected tile
        Tile toTile = board.GetTile(inputTo.rank, inputTo.file);
        this.Logger.Info($"{fromTile.piece.GetColor(true)} {fromTile.piece.nameShort} from ({inputFrom.file}{inputFrom.rank}) to ({inputTo.file}{inputTo.rank})");

        Move move = new Move(this.Logger, fromTile, toTile);

        // Check if the move is valid and not blocked
        if (fromTile.piece.IsValidMove(move) && board.MoveIsPossible(move))
        {
            this.board.Move(move, this.GetCapturedList(), this.turn);
            if (this.board.canBlackPromote(move, this.turn) || this.board.canWhitePromote(move, this.turn)) {
                promoteInput(move, this.turn);
            }
            this.turn.SwitchTurn();
        }

        this.GameLoop();
    }

    public void promoteInput(Move move, Turns turns)
    {
        move.toTile.piece = null;
        Console.WriteLine();
        Console.WriteLine("Promote to: knight(N), queen(Q), bishop(B), rook(R) ");
        while (true)
        {
            Console.Write("Input: ");
            var userInput = Console.ReadLine();
            if (userInput == "q" || userInput == "Q")
            {
                this.board.PromotePawn(move, turns, PromoteEnum.Queen);
                return;
            }
            else if (userInput == "n" || userInput == "N")
            {
                this.board.PromotePawn(move, turns, PromoteEnum.Knight);
                return;
            }
            else if (userInput == "b" || userInput == "B")
            {
                this.board.PromotePawn(move, turns, PromoteEnum.Bishop);
                return;
            }
            else if (userInput == "r" || userInput == "R")
            {
                this.board.PromotePawn(move, turns, PromoteEnum.Rook);
                return;
            }
            Console.WriteLine("Invalid input");
            continue;
        }
    }

    public ref List<Piece> GetCapturedList()
    {
        if (this.turn.turnBool == true) return ref this.WhiteCapturedPieces.List;
        else return ref this.BlackCapturedPieces.List;
    }

    private (char file, int rank) _getValidInput(string message)
    {
        Console.Write(message);

        string input = Console.ReadLine();
        // Null check
        if (input == null)
        {
            this.Logger.Warning("Invalid input, input can not be empty");
            return ('0', 0);
        }

        // Break out of game loop check
        if (input == "break") 
        {
            this.Logger.Info("Exiting game on break command");
            return ('0', -1);
        }

        // Check if valid input
        if (input.Length != 2)
        {
            this.Logger.Warning("Invalid input, input has to be two characters");
            return ('0', 0);
        }


        // Check if second character is a letter and store in a variable
        if (!Char.IsLetter(input[0]))
        {
            this.Logger.Warning("Invalid input, first input character has to be a letter");
        }
        char file = input[0];

        // Check if first character is a digit and store in a variable
        if (!Char.IsDigit(input[1]))
        {
            this.Logger.Warning("Invalid input, second input character has to be a digit");
        }
        int rank = input[1] - '0';



        // If errors then return out with incorrect input that will be caught later
        if (!this.Logger.IsEmpty())
        {
            return ('0', 0);
        }
        // Return correct input/output on valid input
        return (file, rank);
    }

    private void CheckValidInput(int inputRank, char inputFile, string tile)
    {
        this.Logger.Debug("Checking for valid input with parameters: " + inputRank + ", " + inputFile + ", " + tile);
        // Errors that lead to other errors have to be caught seperately with ifs
        // Check if selected tile is inBounds and has a piece on it
        if (!this.board.InBounds(inputFile, inputRank))
        {
            this.Logger.Error("Invalid input, selected tile is not in bounds of the board");
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
                if (!selectedTile.Occupied()) this.Logger.Warning("Tile does not contain a piece");
                // Piece selected doesn't correspond to turn 
                else if (selectedTile.piece.IsWhite() != currentTurn) this.Logger.Warning($"Cannot move this piece on {this.turn.ToString()}s turn");
            }
            else if (tile == "target")
            {
                // Check if file contains a piece
                if (selectedTile.Occupied() && selectedTile.piece.color == currentTurn) this.Logger.Warning("Unable to move to a tile occupied by your own piece");
            }
        }

        if (this.Logger.IsEmpty())
        {
            this.Logger.Debug("Error buffer was empty, succesfully validated input");
            this.Logger.Write();
            return;
        }
        this.Logger.Debug("Error buffer NOT empty, input couldn't be validated");
        this.GameLoop();
        return;
    }
}

