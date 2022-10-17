using SimpleChess.Rules;
using SimpleChess.Chessboard;

namespace SimpleChess.Pieces;

public class Knight : Piece
{
    public Knight(bool color)
    {
        this.Color = color;
        this.NameShort = 'N';
        this.Ghosting = true;
        this.MaterialValue = 3;
    }

    public override bool IsValidMove(Move move)
    {
        // Determine smaller (min) and bigger (max) value from inputs
        var min = Math.Min(move.FromTile.Rank - move.ToTile.Rank, move.FromTile.File - move.ToTile.File);
        var max = Math.Max(move.FromTile.Rank - move.ToTile.Rank, move.FromTile.File - move.ToTile.File);

        // If inputs form an L shape move, count as valid move
        return Math.Abs(min) == 1 && Math.Abs(max) == 2 || Math.Abs(min) == 2 && Math.Abs(max) == 1;
    }

    public override bool[,] GetValidMoves(Tile tile, Tile[,] board)
    {
        var x = tile.Rank;
        var y = tile.File;

        // Initialize two dimensional bool array
        var response = new bool[8, 8];

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
            var newX = x + offset.x;
            var newY = y + offset.y;

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
