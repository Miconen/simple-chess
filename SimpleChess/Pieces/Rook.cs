using SimpleChess.Rules;
using SimpleChess.Chessboard;

namespace SimpleChess.Pieces;

public class Rook : Piece
{
    public Rook(bool color)
    {
        Color = color;
        NameShort = 'R';
        MaterialValue = 5;
    }

    public override bool IsValidMove(Move move)
    {
        return move.IsPerpendicular();
    }

    public override bool[,] GetValidMoves(Tile tile, Tile[,] board)
    {
        var x = tile.Rank;
        var y = tile.File;

        // Initialize two dimensional bool array
        var response = new bool[8, 8];

        // Check up
        for (var i = x + 1; i < 8; i++)
        {
            var isValid = _validMoveHelper(board[i, y]);
            if (!isValid) break;
            response[i, y] = isValid;
        }

        // Check down 
        for (var i = x - 1; i >= 1; i--)
        {
            var isValid = _validMoveHelper(board[i, y]);
            if (!isValid) break;
            response[i, y] = isValid;
        }

        // Check right 
        for (var i = y + 1; i < 8; i++)
        {
            var isValid = _validMoveHelper(board[x, i]);
            if (!isValid) break;
            response[x, i] = isValid;
        }

        // Check left 
        for (var i = y - 1; i >= 0; i--)
        {
            var isValid = _validMoveHelper(board[x, i]);
            if (!isValid) break;
            response[x, i] = isValid;
        }

        return response;
    }

    private bool _validMoveHelper(Tile tile)
    {
        // TODO: Add more checks
        return !tile.Occupied();
    }
}
