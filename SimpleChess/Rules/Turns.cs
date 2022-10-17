namespace SimpleChess.Rules;

public class Turns
{
    public int TurnHalfmove = 1;
    public int TurnFullmove = 1;
    // Default to whites turn
    private char _turnChar = 'w';
    public bool TurnBool = true;

    public void Set(char turn)
    {
        this._turnChar = turn;
        this.TurnBool = turn switch
        {
            'w' => true,
            'b' => false,
            _ => true
        };
    }

    public void Next() 
    {
        this.TurnHalfmove++;
        // Full move rolls over after each black turn
        if (this.TurnBool == false) this.TurnFullmove++;
        // Flip turn
        this.TurnBool = !this.TurnBool;
    }

    public bool Check() 
    {
        if(this.TurnHalfmove % 2 != 0) 
        {
            this.TurnBool = true;
            return this.TurnBool;
        } 
        this.TurnBool = false;
        return this.TurnBool;
    }

    public string ToString(bool capitalize = false)
    {
        var response = this.TurnBool ? "white" : "black";
        if (capitalize) response = $"{char.ToUpper(response[0])}{response[1..]}";
        return response;
    }
}
