using System;
using Chess.Rules;
using Chess.Chessboard;

namespace Chess.Pieces;

public class Bishop : Piece
{
    public Bishop(bool color)
    {
        this.color = color;
        this.nameShort = 'B';
        this.materialValue = 3;
    }

    public override bool IsValidMove(Move move)
    {
        if (!move.IsDiagonal()) return false;
        return true;
    }

    public override bool[,] GetValidMoves(Tile tile, Tile[,] board)
    {
        int x = tile.rank;
        int y = tile.file;

        // TODO: Extract these loops to a helper function, currently looks absolutely hideous
        // Initialize two dimensional bool array
        bool[,] response = new bool[8, 8];

        // Check top-left 
        for (int i = y - 1, ii = x + 1; i >= 0; i--, ii++)
        {
            bool IsValid = _validMoveHelper(board[ii, i]);
            if (!IsValid) break;
            response[ii, i] = IsValid;
        }

        // Check top-right
        for (int i = y + 1, ii = x + 1; i >= 0; i++, ii++)
        {
            bool IsValid = _validMoveHelper(board[ii, i]);
            if (!IsValid) break;
            response[ii, i] = IsValid;
        }

        // Check top-right
        for (int i = y - 1, ii = x - 1; i >= 0; i--, ii--)
        {
            bool IsValid = _validMoveHelper(board[ii, i]);
            if (!IsValid) break;
            response[ii, i] = IsValid;
        }

        // Check top-right
        for (int i = y + 1, ii = x - 1; i >= 0; i++, ii--)
        {
            bool IsValid = _validMoveHelper(board[ii, i]);
            if (!IsValid) break;
            response[ii, i] = IsValid;
        }

        return response;
    }

    private bool _validMoveHelper(Tile tile)
    {
        // TODO: Add more checks
        return !tile.Occupied();
    }
}

