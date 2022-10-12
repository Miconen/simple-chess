using System;
using SimpleChess.Rules;
using SimpleChess.Chessboard;

namespace SimpleChess.Pieces;

public class Knight : Piece
{
    public Knight(bool color)
    {
        this.color = color;
        this.nameShort = 'N';
        this.ghosting = true;
        this.materialValue = 3;
    }

    public override bool IsValidMove(Move move)
    {
        // Determine smaller (min) and bigger (max) value from inputs
        int min = Math.Min(move.fromTile.rank - move.toTile.rank, move.fromTile.file - move.toTile.file);
        int max = Math.Max(move.fromTile.rank - move.toTile.rank, move.fromTile.file - move.toTile.file);

        // If inputs form an L shape move, count as valid move
        if (Math.Abs(min) == 1 && Math.Abs(max) == 2) return true;
        if (Math.Abs(min) == 2 && Math.Abs(max) == 1) return true;
        return false;
    }

    public override bool[,] GetValidMoves(Tile tile, Tile[,] board)
    {
        int x = tile.rank;
        int y = tile.file;

        // Initialize two dimensional bool array
        bool[,] response = new bool[8, 8];

        var knightHelper = new(int x, int y) [] {
            (2, 1),
            (2, -1),
            (1, 2),
            (1, -2),
            (-1, 2),
            (-1, -2),
            (-2, 1),
            (-2, -1),
        };

        // Check offset coordinates 
        foreach (var offset in knightHelper)
        {
            int newX = x + offset.x;
            int newY = y + offset.y;

            // Prevent index out of bounds errors
            if (newX < 0 || newY < 0) continue;
            if (newX > 7 || newY > 7) continue;

            response[newX, newY] = _validMoveHelper(board[newX, newY]);
        }

        return response;
    }

    private bool _validMoveHelper(Tile tile)
    {
        // TODO: Add more checks
        return !tile.Occupied();
    }
}
