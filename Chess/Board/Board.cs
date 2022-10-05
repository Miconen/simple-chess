using System;
using System.Collections.Generic;
using Chess.Pieces;
using Chess.Rules;
using Chess.Logging;
using Chess.Logging.Levels;

namespace Chess.Chessboard;

public class Board
{
    public int BOARD_HEIGHT;
    public int BOARD_WIDTH;
    public List<char> BOARD_LETTERS;
    public Logger Logger;
    public BoardRenderer Renderer;
    public Move LastMove;
    public Tile[,] Tiles;

    public Board(Logger Logger)
    {
        this.BOARD_WIDTH = 8;
        this.BOARD_HEIGHT = 8;
        this.BOARD_LETTERS = new List<char>();
        this.BOARD_LETTERS.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
        this.Logger = Logger;

        // Initialize two dimensional board array
        this.Tiles = new Tile[this.BOARD_WIDTH, this.BOARD_HEIGHT];

        for (int i = 7; i >= 0; i--)
        {
            for (int ii = 0; ii < this.BOARD_WIDTH; ii++)
            {
                this.Tiles[i, ii] = new Tile(i, ii);
            }
        }

        this.Renderer = new BoardRenderer(this.BOARD_HEIGHT, this.BOARD_WIDTH, this.Tiles);
    }

    // Used when dealing with user input in which case the other input will be a char
    public Tile GetTile(int rank, char file)
    {
        int fileIndex = this.BOARD_LETTERS.IndexOf(Char.ToUpper(file));
        // Substract one from rank since it's currently the same as user input
        // After substraction we can use it as an array/list index
        return this.Tiles[rank - 1, fileIndex];
    }

    // Used when dealing with integers, such as the CoordinateListToTiles() method
    public Tile GetTile(int rank, int file)
    {
        // Substract one from rank since it's currently the same as user input
        // After substraction we can use it as an array/list index
        return this.Tiles[rank - 1, file - 1];
    }

    // Move piece to new tile, gets called AFTER move has been validated and is legal
    public void Move(Move move, List<Piece> list)
    {
        // Add "eaten" pieces to corresponding list
        if (move.toTile.piece != null)
        {
            list.Add(move.toTile.piece);
        }
        // Move piece from old tile to new tile
        move.toTile.piece = move.fromTile.piece;

        // TODO: Determine if we need this duplication and where will this end up?
        this.LastMove = move;
        this.Renderer.LastMove = move;

        // Remove piece from old tile
        move.fromTile.piece = null;
    }

    // With selected fromTile
    public void Render(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces, Tile fromTile)
    {
        this.Renderer.Render(this.Tiles, BlackCapturedPieces, WhiteCapturedPieces, fromTile);
    }

    // Without a selected tile
    public void Render(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces)
    {
        this.Renderer.Render(this.Tiles, BlackCapturedPieces, WhiteCapturedPieces);
    }

    public void Populate()
    {
        for (int i = 0; i < this.BOARD_WIDTH; i++)
        {
            // White pawns
            this.Tiles[1, i].piece = new Pawn(true);
            // Black pawns
            this.Tiles[6, i].piece = new Pawn(false);
        }

        // White pieces
        this.Tiles[0, 0].piece = new Rook(true);
        this.Tiles[0, 1].piece = new Knight(true);
        this.Tiles[0, 2].piece = new Bishop(true);
        this.Tiles[0, 3].piece = new Queen(true);
        this.Tiles[0, 4].piece = new King(true);
        this.Tiles[0, 5].piece = new Bishop(true);
        this.Tiles[0, 6].piece = new Knight(true);
        this.Tiles[0, 7].piece = new Rook(true);

        // Test piece
        this.Tiles[3, 3].piece = new Knight(true);

        // Black pieces
        this.Tiles[7, 0].piece = new Rook(false);
        this.Tiles[7, 1].piece = new Knight(false);
        this.Tiles[7, 2].piece = new Bishop(false);
        this.Tiles[7, 3].piece = new Queen(false);
        this.Tiles[7, 4].piece = new King(false);
        this.Tiles[7, 5].piece = new Bishop(false);
        this.Tiles[7, 6].piece = new Knight(false);
        this.Tiles[7, 7].piece = new Rook(false);
    }

    public void PrintBoard(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces)
    {
        int boardHeightCoordinates = 8;
        _printBlackMaterialDifference(BlackCapturedPieces, WhiteCapturedPieces);

        for (int i = 7; i >= 0; i--)
        {
            _boardPrintMargin(i);
            Console.WriteLine();

            // Rank, Height coordinates 
            _boardStringPadding(boardHeightCoordinates);
            boardHeightCoordinates--;

            for (int ii = 0; ii < this.BOARD_WIDTH; ii++)
            {
                _boardPrintContent(i, ii);
            }
            _boardPrintMargin(i);
        }

        // File, width coordinates
        Console.WriteLine("\n\n          " + 'a' + "      " + 'b' + "      " + 'c' + "      " + 'd' + "      " + 'e' + "      " + 'f' + "      " + 'g' + "      " + 'h' + "\n");

        _printWhiteMaterialDifference(BlackCapturedPieces, WhiteCapturedPieces);
    }

    public void PrintBoard(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces, Tile selectedTile)
    {
        int boardHeightCoordinates = 8;
        bool[,] ValidMoves = selectedTile.piece.GetValidMoves(selectedTile, this.Tiles);

        _printBlackMaterialDifference(BlackCapturedPieces, WhiteCapturedPieces);

        for (int i = 7; i >= 0; i--)
        {
            _boardPrintMargin(i);
            Console.WriteLine();

            // Rank, Height coordinates 
            _boardStringPadding(boardHeightCoordinates);
            boardHeightCoordinates--;

            for (int ii = 0; ii < this.BOARD_WIDTH; ii++)
            {
                _boardPrintContent(i, ii, ValidMoves[i, ii]);
            }
            _boardPrintMargin(i);
        }

        // File, width coordinates
        Console.WriteLine("\n\n          " + 'a' + "      " + 'b' + "      " + 'c' + "      " + 'd' + "      " + 'e' + "      " + 'f' + "      " + 'g' + "      " + 'h' + "\n");

        _printWhiteMaterialDifference(BlackCapturedPieces, WhiteCapturedPieces);
    }

    public bool InBounds(char fileLetter, int rank)
    {
        if (fileLetter == '0' && rank == 0) return false;

        int file = this.BOARD_LETTERS.IndexOf(Char.ToUpper(fileLetter)) + 1;
        if (rank < 0 || rank > this.BOARD_WIDTH) return false;
        if (file < 0 || file > this.BOARD_HEIGHT) return false;
        return true;
    }

    public List<Tile> CoordinateListToTiles(List<Tuple<int, int>> coordinates)
    {
        var tiles = new List<Tile>();
        foreach (var tuple in coordinates)
        {
            tiles.Add(this.GetTile(tuple.Item1, tuple.Item2));
        }
        return tiles;
    }

    public bool MoveIsPossible(Move move)
    {
        // Convert list of coordinates to usable tile objects
        List<Tile> tiles = this.CoordinateListToTiles(move.GetTileIndexesBetweenInputs());
        // Check if a piece is blocked, bypass if move has ghosting AKA is allowed to move over other pieces
        bool isBlocked = (move.fromTile.piece.ghosting) ? false : move.IsBlocked(tiles);
        if (isBlocked) this.Logger.Warning("Move was blocked by another piece");
        return !isBlocked;
    }

    private void _boardPrintMargin(int i)
    {

        Console.WriteLine("");
        Console.Write("       ");

        for (int ii = 0; ii < this.BOARD_WIDTH; ii++)
        {

            Tile currentTile = this.Tiles[i, ii];

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
        Tile currentTile = this.Tiles[i, ii];

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
        Tile currentTile = this.Tiles[i, ii];

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
