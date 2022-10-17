using SimpleChess.Rules;
using SimpleChess.Chessboard;
using SimpleChess.Pieces;

namespace SimpleChess.Cli.Renderer;

public class BoardRenderer
{
    private readonly Board _board;
    private readonly Move? _lastMove = null;
    private readonly List<char> _boardLetters = new();

    public BoardRenderer(Board board)
    {
        _board = board;
        _boardLetters.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
    }

    public void Render(CapturedPieces blackCapturedPieces, CapturedPieces whiteCapturedPieces)
    {
        Render(blackCapturedPieces, whiteCapturedPieces, new Tile(0, 0));
    }

    public void Render(CapturedPieces blackCapturedPieces, CapturedPieces whiteCapturedPieces, Tile selectedTile)
    {
        // Initialize valid move tiles, defaults to false
        var validMoves = new bool[8, 8];

        // Coordinates default to 0, 0 if Render gets called without a selected tile
        if (selectedTile.Rank != 0 && selectedTile.File != 0)
        {
            // Determine valid move tiles only if render was provided a selected tile
            validMoves = selectedTile.Piece?.GetValidMoves(selectedTile, this._board.Tiles);
        }

        _printBlackMaterialDifference(blackCapturedPieces, whiteCapturedPieces);

        int i;
        for (i = this._board.BoardHeight - 1; i >= 0; i--)
        {
            _boardPrintMargin(i);
            Console.WriteLine();

            // Rank, Height coordinates 
            _boardStringPadding(i + 1);

            int ii;
            for (ii = 0; ii < this._board.BoardWidth; ii++)
            {
                _boardPrintContent(i, ii, validMoves != null && validMoves[i, ii]);
            }
            _boardPrintMargin(i);
        }

        // File, width coordinates
        Console.WriteLine("\n\n          a      b      c      d      e      f      g      h\n");

        _printWhiteMaterialDifference(blackCapturedPieces, whiteCapturedPieces);
    }

    private void _boardPrintMargin(int i)
    {
        Console.WriteLine();
        Console.Write("       ");

        int ii;
        for (ii = 0; ii < this._board.BoardWidth; ii++)
        {

            var currentTile = this._board.Tiles[i, ii];

            Console.BackgroundColor = (i + ii) % 2 == 0 ? ConsoleColor.Black : ConsoleColor.White;

            if (currentTile == this._lastMove?.FromTile) Console.BackgroundColor = ConsoleColor.DarkGreen;
            if (currentTile == this._lastMove?.ToTile) Console.BackgroundColor = ConsoleColor.Green;

            Console.Write("       ");
            Console.ResetColor();
        }
    }

    private void _printBlackMaterialDifference(CapturedPieces blackCapturedPieces, CapturedPieces whiteCapturedPieces)
    {
        // Print list of captured pieces and calculate difference in material sum
        int materialDifference = whiteCapturedPieces.CalculateMaterialSum() - blackCapturedPieces.CalculateMaterialSum();
        blackCapturedPieces.PrintList();
        Console.Write("   ");
        if (materialDifference < 0) Console.WriteLine($"+{Math.Abs(materialDifference)}");
    }

    private void _printWhiteMaterialDifference(CapturedPieces blackCapturedPieces, CapturedPieces whiteCapturedPieces)
    {
        // Print captured list for White pieces
        var materialDifference = whiteCapturedPieces.CalculateMaterialSum() - blackCapturedPieces.CalculateMaterialSum();
        whiteCapturedPieces.PrintList();
        Console.Write("   ");
        if (materialDifference > 0) Console.WriteLine($"+{materialDifference}");
        Console.WriteLine();
    }

    private void _boardSetBackground(int i, int ii)
    {
        var currentTile = this._board.Tiles[i, ii];

        Console.BackgroundColor = (i + ii) % 2 == 0 ? ConsoleColor.Black : ConsoleColor.White;

        // Set different background colors for previous move played
        if (currentTile == this._lastMove?.FromTile) Console.BackgroundColor = ConsoleColor.DarkGreen;
        if (currentTile == this._lastMove?.ToTile) Console.BackgroundColor = ConsoleColor.Green;
    }

    private void _boardSetForeground(Tile currentTile)
    {
        Console.ForegroundColor = currentTile.Piece is { Color: true } ? ConsoleColor.DarkRed : ConsoleColor.Blue;
    }

    private void _boardPrintContent(int i, int ii, bool isValidMove = false)
    {
        var currentTile = this._board.Tiles[i, ii];

        _boardSetBackground(i, ii);

        if (currentTile.Occupied())
        {
            _boardSetForeground(currentTile);
            _boardStringPadding(currentTile.Piece?.NameShort.ToString() ?? string.Empty);
        }
        else if (isValidMove)
        {
            _boardPrintHighlightedMoves(i, ii);
        }
        else
        {
            _boardStringPadding();
        }

        Console.ResetColor();
    }

    private void _boardStringPadding(string content = " ", bool isValidMove = false)
    {
        const string spacer = "   ";

        Console.Write(spacer);
        if (isValidMove) Console.BackgroundColor = ConsoleColor.Cyan;
        Console.Write(content);
        Console.Write(spacer);
    }

    private void _boardPrintHighlightedMoves(int i, int ii)
    {
        const string spacer = "  ";

        Console.Write(spacer);
        Console.BackgroundColor = ConsoleColor.Cyan;
        Console.Write("   ");
        _boardSetBackground(i, ii);
        Console.Write(spacer);
    }

    private void _boardStringPadding(int number)
    {
        string content = number.ToString();
        _boardStringPadding(content);
    }
}
