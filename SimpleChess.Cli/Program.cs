using SimpleChess.Cli;

namespace SimpleChess.Cli.Game;

public class Program
{
    static void Main(string[] args) 
    {
        Game game = new Game();
        game.Start();
    }
}
