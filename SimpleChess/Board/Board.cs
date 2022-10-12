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
