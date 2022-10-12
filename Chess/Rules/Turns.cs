using System;
using Chess.Chessboard;

namespace Chess.Rules;

public class Turns 
{
    public int turnCount = 1;
    public bool turnBool;
    public Turns(bool turnBool)
    {
        this.turnBool = turnBool;
    }

    public void SwitchTurn() 
    {
        this.turnCount++;
        if(this.turnBool) 
        {
            this.turnBool = false;
            return;
        } 
        if(!this.turnBool) this.turnBool = true;
    }

    public bool CheckTurn() 
    {
        if(this.turnCount % 2 != 0) 
        {
            this.turnBool = true;
            //Console.WriteLine("White turn");
            return this.turnBool;
        } 
        this.turnBool = false;
        //Console.WriteLine("Black turn");
        return this.turnBool;
    }

    public string ToString(bool capitalize = false)
    {
        string response = "";
        if (this.turnBool) response = "white";
        else response = "black";

        if (capitalize) response = char.ToUpper(response[0]) + response.Substring(1);
        return response;
    }
}
