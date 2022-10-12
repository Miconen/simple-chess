using System;
using SimpleChess.Rules;
using SimpleChess.Chessboard;

namespace SimpleChess.Pieces;

public class Queen : Piece
{
    public Queen(bool color)
    {
        this.color = color;
        this.nameShort = 'Q';
        this.materialValue = 9;
    }

    public override bool IsValidMove(Move move)
    {
        return (move.IsDiagonal() || move.IsPerpendicular());
    }

    public override bool[,] GetValidMoves(Tile tile, Tile[,] board)
    {
        int x = tile.rank;
        int y = tile.file;

        // TODO: Extract these loops to a helper function, currently looks absolutely hideous
        // Initialize two dimensional bool array
        bool[,] response = new bool[8, 8];

        // Check up
        for (int i = x + 1; i < 8; i++)
        {
            bool IsValid = _validMoveHelper(board[i, y]);
            if (!IsValid) break;
            response[i, y] = IsValid;
        }

        // Check down 
        for (int i = x - 1; i >= 1; i--)
        {
            bool IsValid = _validMoveHelper(board[i, y]);
            if (!IsValid) break;
            response[i, y] = IsValid;
        }

        // Check right 
        for (int i = y + 1; i < 8; i++)
        {
            bool IsValid = _validMoveHelper(board[x, i]);
            if (!IsValid) break;
            response[x, i] = IsValid;
        }

        // Check left 
        for (int i = y - 1; i >= 0; i--)
        {
            bool IsValid = _validMoveHelper(board[x, i]);
            if (!IsValid) break;
            response[x, i] = IsValid;
        }

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
