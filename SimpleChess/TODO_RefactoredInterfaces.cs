interface IPiece
{
    // Check if move is possible, not necessarily valid
    bool IsPossibleMove(Move);
    // Get a list of all possible moves, not necessarily valid
    List<(int, int)> GetPossibleTiles(Move);
}

interface IValidator
{
    // Get list of all valid moves
    List<(int, int)> GetValidTiles(List<(int, int)>);
}

interface IBoard
{
    // Set list of all valid moves to be highlighted
    void SetHighlightedTiles(List<int, int>);
}
