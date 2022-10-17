using SimpleChess.Pieces;
using SimpleChess.Rules;
using SimpleChess.Logging;

namespace SimpleChess.Chessboard;

public class Board
{
    public readonly int BoardHeight;
    public readonly int BoardWidth;
    private readonly List<char> _boardLetters;
    private readonly Logger _logger;
    private Move? _lastMove;
    public readonly Tile[,] Tiles;

    public Board(Logger logger)
    {
        BoardWidth = 8;
        BoardHeight = 8;
        _boardLetters = new List<char>();
        _boardLetters.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
        _logger = logger;

        // Initialize two dimensional board array
        Tiles = new Tile[BoardWidth, BoardHeight];

        for (var i = 7; i >= 0; i--)
        {
            for (var ii = 0; ii < BoardWidth; ii++)
            {
                Tiles[i, ii] = new Tile(i, ii);
            }
        }
    }

    // Used when dealing with user input in which case the other input will be a char
    public Tile GetTile(int rank, char file)
    {
        var fileIndex = _boardLetters.IndexOf(char.ToUpper(file));
        // Subtract one from rank since it's currently the same as user input
        // After subtraction we can use it as an array/list index
        return Tiles[rank - 1, fileIndex];
    }

    // Used when dealing with integers, such as the CoordinateListToTiles() method
    private Tile GetTile(int rank, int file)
    {
        // Subtract one from rank since it's currently the same as user input
        // After subtraction we can use it as an array/list index
        return Tiles[rank, file];
    }

    // Move piece to new tile, gets called AFTER move has been validated and is legal
    public void Move(Move move, List<Piece> list, Turns turn)
    {
        // Add "eaten" pieces to corresponding lists
        if (move.ToTile.Piece != null)
        {
            list.Add(move.ToTile.Piece);
        }

        // Move piece from old tile to new tile
        move.ToTile.Piece = move.FromTile.Piece;

        _lastMove = move;

        // Remove piece from old tile
        move.FromTile.Piece = null;
    }

    public bool CanWhitePromote(Move move, Turns turn) {
        return turn.TurnBool && move.ToTile.Rank == 7 && move.ToTile.Piece?.ToString() == "P";
    }

    public bool CanBlackPromote(Move move, Turns turn) {
        return turn.TurnBool == false && move.ToTile.Rank == 0 && move.ToTile.Piece?.ToString() == "P";
    }

    public void Populate(Fen fen)
    {
        var rank = 0;
        var file = 7;
        foreach (var piece in fen.Placement)
        {
            Console.WriteLine($"{piece} {rank} {file}");
            // Digits indicate x amount of empty squares
            if (char.IsDigit(piece))
            {
                rank += piece - '0';
                continue;
            }
            // Parse through the notations formatting
            // The '/' indicates a new file
            // The rank is an index so it can never exceed 7
            if (piece == '/' || rank > 7)
            {
                file--;
                rank = 0;
                continue;
            }

            // Populate tile with piece corresponding to the character
            var isWhite = char.IsUpper(piece);
            Tiles[file, rank].Piece = char.ToUpper(piece) switch
            {
                'P' => new Pawn(isWhite),
                'R' => new Rook(isWhite),
                'N' => new Knight(isWhite),
                'B' => new Bishop(isWhite),
                'Q' => new Queen(isWhite),
                'K' => new King(isWhite),
                _ => Tiles[file, rank].Piece
            };

            rank++;
        }
    }

    public bool InBounds(char fileLetter, int rank)
    {
        if (fileLetter == '0' && rank == 0) return false;

        var file = _boardLetters.IndexOf(char.ToUpper(fileLetter)) + 1;
        return rank >= 0 && rank <= BoardWidth && file >= 0 && file <= BoardHeight;
    }

    private List<Tile> CoordinateListToTiles(IEnumerable<Tuple<int, int>> coordinates)
    {
        return coordinates.Select(tuple => GetTile(tuple.Item1, tuple.Item2)).ToList();
    }

    public bool MoveIsPossible(Move move)
    {
        // Convert list of coordinates to usable tile objects
        var tiles = CoordinateListToTiles(move.GetTileIndexesBetweenInputs());
        // Check if a piece is blocked, bypass if move has ghosting AKA is allowed to move over other pieces
        var isBlocked = move.FromTile.Piece is { Ghosting: false } && move.IsBlocked(tiles);
        if (isBlocked) _logger.Warning("Move was blocked by another piece");
        return !isBlocked;
    }

    public void PromotePawn(Move move, Turns turn, PromoteEnum promote)
    {
        move.ToTile.Piece = promote switch
        {
            PromoteEnum.Queen => new Queen(turn.TurnBool),
            PromoteEnum.Knight => new Knight(turn.TurnBool),
            PromoteEnum.Bishop => new Bishop(turn.TurnBool),
            PromoteEnum.Rook => new Rook(turn.TurnBool),
            _ => null
        };
    }
}
