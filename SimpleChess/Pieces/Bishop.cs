using SimpleChess.Rules;
using SimpleChess.Chessboard;

namespace SimpleChess.Pieces;

public class Bishop : Piece
{
    public Bishop(bool color)
    {
        this.Color = color;
        NameShort = 'B';
        MaterialValue = 3;
    }

    public override bool IsValidMove(Move move)
    {
        return move.IsDiagonal();
    }

    public override bool[,] GetValidMoves(Tile tile, Tile[,] board)
    {
        var x = tile.Rank;
        var y = tile.File;

        // TODO: Extract these loops to a helper function, currently looks absolutely hideous
        // Initialize two dimensional bool array
        var response = new bool[8, 8];

        // Check top-left 
        for (int i = y - 1, ii = x + 1; i >= 0; i--, ii++)
        {
            var isValid = _validMoveHelper(board[ii, i]);
            if (!isValid) break;
            response[ii, i] = isValid;
        }

        // Check top-right
        for (int i = y + 1, ii = x + 1; i >= 0; i++, ii++)
        {
            var isValid = _validMoveHelper(board[ii, i]);
            if (!isValid) break;
            response[ii, i] = isValid;
        }

        // Check top-right
        for (int i = y - 1, ii = x - 1; i >= 0; i--, ii--)
        {
            var isValid = _validMoveHelper(board[ii, i]);
            if (!isValid) break;
            response[ii, i] = isValid;
        }

        // Check top-right
        for (int i = y + 1, ii = x - 1; i >= 0; i++, ii--)
        {
            var isValid = _validMoveHelper(board[ii, i]);
            if (!isValid) break;
            response[ii, i] = isValid;
        }

        return response;
    }

    private bool _validMoveHelper(Tile tile)
    {
        // TODO: Add more checks
        return !tile.Occupied();
    }
}

