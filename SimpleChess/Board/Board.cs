using System;
using System.Collections.Generic;

using SimpleChess.Pieces;
using SimpleChess.Rules;
using SimpleChess.Logging;

namespace SimpleChess.Chessboard;

public class Board
{
    public int BOARD_HEIGHT;
    public int BOARD_WIDTH;
    public List<char> BOARD_LETTERS;
    public Logger Logger;
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
        return this.Tiles[rank, file];
    }

    // Move piece to new tile, gets called AFTER move has been validated and is legal
    public void Move(Move move, List<Piece> list, Turns turn)
    {
        // Add "eaten" pieces to corresponding lists
        if (move.toTile.piece != null)
        {
            list.Add(move.toTile.piece);
        }

        // Move piece from old tile to new tile
        move.toTile.piece = move.fromTile.piece;

        this.LastMove = move;

        // Remove piece from old tile
        move.fromTile.piece = null;
    }

    public bool canWhitePromote(Move move, Turns turn) {
        return turn.turnBool == true && move.toTile.rank == 7 && move.toTile.piece.ToString() == "P";
    }

    public bool canBlackPromote(Move move, Turns turn) {
        return turn.turnBool == false && move.toTile.rank == 0 && move.toTile.piece.ToString() == "P";
    }

    public void Populate(FEN FEN)
    {
        int rank = 0;
        int file = 0;
        foreach (char piece in FEN.Placement)
        {
            // Parse through the notations formatting
            // The '/' indicates a new file
            // The rank is an index so it can never exceed 7
            if (piece == '/' || rank > 7)
            {
                file++;
                rank = 0;
                continue;
            }
            // Digits indicate x amount of empty squares
            if (Char.IsDigit(piece))
            {
                rank += piece;
                continue;
            }

            // Populate tile with piece corresponding to the character
            bool IsWhite = Char.IsUpper(piece);
            switch (Char.ToUpper(piece))
            {
                case 'P':
                    this.Tiles[file, rank].piece = new Pawn(IsWhite);
                    break;
                case 'R':
                    this.Tiles[file, rank].piece = new Rook(IsWhite);
                    break;
                case 'N':
                    this.Tiles[file, rank].piece = new Knight(IsWhite);
                    break;
                case 'B':
                    this.Tiles[file, rank].piece = new Bishop(IsWhite);
                    break;
                case 'Q':
                    this.Tiles[file, rank].piece = new Queen(IsWhite);
                    break;
                case 'K':
                    this.Tiles[file, rank].piece = new King(IsWhite);
                    break;
            }

            rank++;
        }
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

    public void PromotePawn(Move move, Turns turn, PromoteEnum promote)
    {
        move.toTile.piece = null;
        if (promote is PromoteEnum.Queen)
        {
            move.toTile.piece = new Queen(turn.turnBool);
            return;
        }
        else if (promote is PromoteEnum.Knight)
        {
            move.toTile.piece = new Knight(turn.turnBool);
            return;
        }
        else if (promote is PromoteEnum.Bishop)
        {
            move.toTile.piece = new Bishop(turn.turnBool);
            return;
        }
        else if (promote is PromoteEnum.Rook)
        {
            move.toTile.piece = new Rook(turn.turnBool);
            return;
        }
    }
}
