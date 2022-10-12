using System;
using System.Collections.Generic;
using SimpleChess.Rules;
using SimpleChess.Chessboard;
using SimpleChess.Pieces;

namespace SimpleChess.Cli.Renderer;

public class BoardRenderer
{
    public Board board;
    public List<char> BOARD_LETTERS;
    public Move LastMove;

    public BoardRenderer(Board board)
    {
        this.BOARD_LETTERS = new List<char>();
        this.BOARD_LETTERS.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
        this.board = board;
    }

    public void Render(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces)
    {
        this.Render(BlackCapturedPieces, WhiteCapturedPieces, new Tile(0, 0));
    }

    public void Render(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces, Tile selectedTile)
    {
        // Initialize valid move tiles, defaults to false
        bool[,] ValidMoves = new bool[8, 8];

        // Coordinates default to 0, 0 if Render gets called without a selected tile
        if (selectedTile.rank != 0 && selectedTile.file != 0)
        {
            // Determine valid move tiles only if render was provided a selected tile
            ValidMoves = selectedTile.piece.GetValidMoves(selectedTile, this.board.Tiles);
        }

        _printBlackMaterialDifference(BlackCapturedPieces, WhiteCapturedPieces);

        for (int i = this.board.BOARD_HEIGHT - 1; i >= 0; i--)
        {
            _boardPrintMargin(i);
            Console.WriteLine();

            // Rank, Height coordinates 
            _boardStringPadding(i + 1);

            for (int ii = 0; ii < this.board.BOARD_WIDTH; ii++)
            {
                _boardPrintContent(i, ii, ValidMoves[i, ii]);
            }
            _boardPrintMargin(i);
        }

        // File, width coordinates
        Console.WriteLine("\n\n          " + 'a' + "      " + 'b' + "      " + 'c' + "      " + 'd' + "      " + 'e' + "      " + 'f' + "      " + 'g' + "      " + 'h' + "\n");

        _printWhiteMaterialDifference(BlackCapturedPieces, WhiteCapturedPieces);
    }

    private void _boardPrintMargin(int i)
    {
        Console.WriteLine();
        Console.Write("       ");

        for (int ii = 0; ii < this.board.BOARD_WIDTH; ii++)
        {

            Tile currentTile = this.board.Tiles[i, ii];

            if ((i + ii) % 2 == 0)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.White;
            }

            if (currentTile == this.LastMove?.fromTile) Console.BackgroundColor = ConsoleColor.DarkGreen;
            if (currentTile == this.LastMove?.toTile) Console.BackgroundColor = ConsoleColor.Green;

            Console.Write("       ");
            Console.ResetColor();
        }
    }

    private void _printBlackMaterialDifference(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces)
    {
        // Print list of captured pieces and calculate difference in material sum
        int materialDifference = WhiteCapturedPieces.CalculateMaterialSum() - BlackCapturedPieces.CalculateMaterialSum();
        BlackCapturedPieces.PrintList();
        Console.Write("   ");
        if (materialDifference < 0) Console.WriteLine($"+{Math.Abs(materialDifference)}");
    }

    private void _printWhiteMaterialDifference(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces)
    {
        // Print captured list for White pieces
        int materialDifference = WhiteCapturedPieces.CalculateMaterialSum() - BlackCapturedPieces.CalculateMaterialSum();
        WhiteCapturedPieces.PrintList();
        Console.Write("   ");
        if (materialDifference > 0) Console.WriteLine($"+{Math.Abs(materialDifference)}");
        Console.WriteLine();
    }

    private void _boardSetBackground(int i, int ii)
    {
        Tile currentTile = this.board.Tiles[i, ii];

        if ((i + ii) % 2 == 0)
        {
            Console.BackgroundColor = ConsoleColor.Black;
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.White;
        }

        // Set different background colors for previous move played
        if (currentTile == this.LastMove?.fromTile) Console.BackgroundColor = ConsoleColor.DarkGreen;
        if (currentTile == this.LastMove?.toTile) Console.BackgroundColor = ConsoleColor.Green;
    }

    private void _boardSetForeground(Tile currentTile)
    {
        if (currentTile.piece.color)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Blue;
        }
    }

    private void _boardPrintContent(int i, int ii, bool isValidMove = false)
    {
        Tile currentTile = this.board.Tiles[i, ii];

        _boardSetBackground(i, ii);

        if (currentTile.Occupied())
        {
            _boardSetForeground(currentTile);
            _boardStringPadding(currentTile.piece.nameShort.ToString());
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
        const string SPACER = "   ";

        Console.Write(SPACER);
        if (isValidMove) Console.BackgroundColor = ConsoleColor.Cyan;
        Console.Write(content);
        Console.Write(SPACER);
    }

    private void _boardPrintHighlightedMoves(int i, int ii)
    {
        const string SPACER = "  ";

        Console.Write(SPACER);
        Console.BackgroundColor = ConsoleColor.Cyan;
        Console.Write("   ");
        _boardSetBackground(i, ii);
        Console.Write(SPACER);
    }

    private void _boardStringPadding(int number)
    {
        string content = number.ToString();
        _boardStringPadding(content);
    }
}
