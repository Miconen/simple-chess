using System;
using System.Collections.Generic;
using SimpleChess.Chessboard;
using SimpleChess.Logging;
using SimpleChess.Logging.Levels;

namespace SimpleChess.Rules;

public class Move
{
    public Logger Logger;
    public Tile fromTile;
    public Tile toTile;

    public Move(Logger Logger, Tile fromTile, Tile toTile) 
    {
        this.Logger = Logger;
        this.fromTile = fromTile;
        this.toTile = toTile;
    }

    public List<Tuple<int, int>> GetTileIndexesBetweenInputs()
    {
        var list = new List<Tuple<int, int>>();
        string moveInfo = "";
        // Vertical movement
        if (this.fromTile.file == this.toTile.file)
        {
            moveInfo += "Vertical move, tiles visited: ";
            int i = this.fromTile.rank;
            while(i != this.toTile.rank)
            {
                i = (this.fromTile.rank > this.toTile.rank) ? i - 1 : i + 1;
                moveInfo += "[" + this.fromTile.rank + " " + i + "] ";
                list.Add(new Tuple<int, int>(i, this.fromTile.file));
            }
        }
        // Horizontal movement
        else if (this.fromTile.rank == this.toTile.rank)
        {
            moveInfo += "Horizontal move, tiles visited: ";
            int i = this.fromTile.file;
            while(i != this.toTile.file)
            {
                i = (this.fromTile.file > this.toTile.file) ? i - 1 : i + 1;
                moveInfo += "[" + this.fromTile.file + " " + i + "] ";
                list.Add(new Tuple<int, int>(this.fromTile.rank, i));
            }
        }
        // Diagonal movement
        else
        {
            moveInfo += "Diagonal move, tiles visited: ";
            int i = this.fromTile.rank;
            int ii = this.fromTile.file;
            while(i != this.toTile.rank && ii != this.toTile.file)
            {
                i = (this.fromTile.rank > this.toTile.rank) ? i - 1 : i + 1;
                ii = (this.fromTile.file > this.toTile.file) ? ii - 1 : ii + 1;
                moveInfo += "[" + ii + " " + i + "] ";
                list.Add(new Tuple<int, int>(i, ii));
            }
        }

        this.Logger.Debug(moveInfo);

        return list;
    }

    public bool IsPerpendicular()
    {
        return (this.fromTile.file == this.toTile.file || this.fromTile.rank == this.toTile.rank);
    }

    public bool IsDiagonal()
    {
        return (this.fromTile.file - this.fromTile.rank == this.toTile.file - this.toTile.rank || this.fromTile.file + this.fromTile.rank == this.toTile.file + this.toTile.rank);
    }

    public bool IsBlocked(List<Tile> list)
    {
        bool response = false;
        Tile targetTile = list[list.Count - 1];

        // Check if both tiles have pieces on them
        if (targetTile.Occupied())
        {
            // Compared if existing pieces are of different color
            if (this.fromTile.piece.color != targetTile.piece.color) return false;
        }

        // Loop over all tiles in between a move checking if they are occupied
        foreach (Tile tile in list)
        {
            // If piece is not null but type of Piece, we return true
            if (tile.Occupied()) response = true;
        }
        return response;
    }
}
