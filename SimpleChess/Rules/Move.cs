using SimpleChess.Chessboard;
using SimpleChess.Logging;

namespace SimpleChess.Rules;

public class Move
{
    private readonly Logger _logger;
    public readonly Tile FromTile;
    public readonly Tile ToTile;

    public Move(Logger logger, Tile fromTile, Tile toTile) 
    {
        this._logger = logger;
        this.FromTile = fromTile;
        this.ToTile = toTile;
    }

    public IEnumerable<Tuple<int, int>> GetTileIndexesBetweenInputs()
    {
        var list = new List<Tuple<int, int>>();
        var moveInfo = "";
        // Vertical movement
        if (this.FromTile.File == this.ToTile.File)
        {
            moveInfo = $"{moveInfo}Vertical move, tiles visited: ";
            var i = this.FromTile.Rank;
            while(i != this.ToTile.Rank)
            {
                i = (this.FromTile.Rank > ToTile.Rank) ? i - 1 : i + 1;
                moveInfo = $"{moveInfo}[{FromTile.Rank} {i}]";
                list.Add(new Tuple<int, int>(i, FromTile.File));
            }
        }
        // Horizontal movement
        else if (this.FromTile.Rank == this.ToTile.Rank)
        {
            moveInfo = $"{moveInfo}Horizontal move, tiles visited: ";
            var i = this.FromTile.File;
            while(i != this.ToTile.File)
            {
                i = (this.FromTile.File > this.ToTile.File) ? i - 1 : i + 1;
                moveInfo = $"{moveInfo}[{this.FromTile.File} {i}]";
                list.Add(new Tuple<int, int>(this.FromTile.Rank, i));
            }
        }
        // Diagonal movement
        else
        {
            moveInfo = $"{moveInfo}Diagonal move, tiles visited: ";
            var i = this.FromTile.Rank;
            var ii = this.FromTile.File;
            while(i != this.ToTile.Rank && ii != this.ToTile.File)
            {
                i = (this.FromTile.Rank > this.ToTile.Rank) ? i - 1 : i + 1;
                ii = (this.FromTile.File > this.ToTile.File) ? ii - 1 : ii + 1;
                moveInfo = $"{moveInfo}[{ii} {i}] ";
                list.Add(new Tuple<int, int>(i, ii));
            }
        }

        this._logger.Debug(moveInfo);

        return list;
    }

    public bool IsPerpendicular()
    {
        return (this.FromTile.File == this.ToTile.File || this.FromTile.Rank == this.ToTile.Rank);
    }

    public bool IsDiagonal()
    {
        return (this.FromTile.File - this.FromTile.Rank == this.ToTile.File - this.ToTile.Rank || this.FromTile.File + this.FromTile.Rank == this.ToTile.File + this.ToTile.Rank);
    }

    public bool IsBlocked(List<Tile> list)
    {
        var response = false;
        var targetTile = list[^1];

        // Check if both tiles have pieces on them
        if (targetTile.Occupied())
        {
            // Compared if existing pieces are of different color
            if (targetTile.Piece != null && FromTile.Piece != null && FromTile.Piece.Color != targetTile.Piece.Color) return false;
        }

        // Loop over all tiles in between a move checking if they are occupied
        foreach (var tile in list.Where(tile => !tile.Occupied()))
        {
            response = true;
        }
        
        return response;
    }
}
