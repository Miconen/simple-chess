/*
 * TO READ MORE ABOUT THE PURPOSE AND USAGE OF FEN (Forsyth Edwards Notation)
 * https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation
*/
namespace SimpleChess.Rules;

public class FEN
{
    public string Placement { get; set; }
    public string Turn { get; set; }
    public string Castling { get; set; }
    public string EnPassantSquare { get; set; }
    public string ClockHalfmove { get; set; }
    public string ClockFullmove { get; set; }

    public FEN(string FENString)
    {
        this.Update(FENString);
    }

    public void Update(string FENString)
    {
        string[] _fenHelper = FENString.Split(" ");
        this.Placement = _fenHelper[0];
        this.Turn = _fenHelper[1];
        this.Castling = _fenHelper[2];
        this.EnPassantSquare = _fenHelper[3];
        this.ClockHalfmove = _fenHelper[4];
        this.ClockFullmove = _fenHelper[5];
    }
}
