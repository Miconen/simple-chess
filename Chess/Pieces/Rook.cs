using Chess.Rules;
using Chess.Chessboard;

namespace Chess.Pieces;

public class Rook : Piece
{
    public Rook(bool color)
    {
        this.color = color;
        this.nameShort = 'R';
        this.materialValue = 5;
    }

    public override bool IsValidMove(Move move)
    {
        if (!move.IsPerpendicular()) return false;
        return true;
    }

    public override bool[,] GetValidMoves(Tile tile, Tile[,] board)
    {
        int x = tile.rank;
        int y = tile.file;

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

        return response;
    }

    private bool _validMoveHelper(Tile tile)
    {
        bool response = true;
        if (tile.Occupied()) response = false;
        return response;
    }
}
