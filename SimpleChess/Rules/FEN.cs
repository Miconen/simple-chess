/*
 * TO READ MORE ABOUT THE PURPOSE AND USAGE OF FEN (Forsyth Edwards Notation)
 * https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation
*/
namespace SimpleChess.Rules;

public class Fen
{
    public string Placement { get; private set; }
    public readonly Turns Turn = new();
    public string Castling { get; set; }
    public string EnPassantSquare { get; set; }

    public Fen(string fenString)
    {
        var fenHelper = fenString.Split(" ") ?? throw new ArgumentNullException(nameof(fenString));
        this.Placement = fenHelper[0];
        this.Castling = fenHelper[2];
        this.EnPassantSquare = fenHelper[3];
        this.Turn.Set(fenHelper[1][0]);
        this.Turn.TurnHalfmove = Convert.ToInt32(fenHelper[4]);
        this.Turn.TurnFullmove = Convert.ToInt32(fenHelper[5]);
    }

    private void Update(string fenString)
    {
        var fenHelper = fenString.Split(" ") ?? throw new ArgumentNullException(nameof(fenString));
        this.Placement = fenHelper[0];
        this.Castling = fenHelper[2];
        this.EnPassantSquare = fenHelper[3];
        this.Turn.Set(fenHelper[1][0]);
        this.Turn.TurnHalfmove = Convert.ToInt32(fenHelper[4]);
        this.Turn.TurnFullmove = Convert.ToInt32(fenHelper[5]);
    }
}
