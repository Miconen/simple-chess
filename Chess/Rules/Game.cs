using System;
using Chess;

namespace Chess.Rules
{
    public class Game
    {
        public Board board { get; };

        public Game()
        {
            // TODO: Implement game loop
            this.board = new Board(this.BOARD_WIDTH, this.BOARD_HEIGHT);
            board.Populate();
        }
    }
}

