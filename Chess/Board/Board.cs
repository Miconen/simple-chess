using System;
using Chess;
using Chess.Pieces;
using Chess.Rules;

namespace Chess.Chessboard
{
    public class Board
    {
        public int BOARD_HEIGHT;
        public int BOARD_WIDTH;

        public Tile[,] Tiles;

        public Board()
        {
            this.BOARD_WIDTH = 8;
            this.BOARD_HEIGHT = 8;
            // Initialize two dimensional board array
            Tiles = new Tile[this.BOARD_WIDTH, this.BOARD_HEIGHT];

            for (int i = 0; i < this.BOARD_HEIGHT; i++)
            {
                for (int ii = 0; ii < this.BOARD_WIDTH; ii++)
                {
                    this.Tiles[i, ii] = new Tile(i, ii);
                }
            }
        }

        public void Move(Move move)
        {
            move.toTile.piece = move.fromTile.piece;
            move.fromTile.piece = null;
        }

        public void Populate()
        {
            for (int i = 0; i < this.BOARD_WIDTH; i++)
            {
                this.Tiles[1, i].piece = new Pawn(true);
                this.Tiles[6, i].piece = new Pawn(false);
            }

            // White pieces
            this.Tiles[0, 0].piece = new Rook(true);
            this.Tiles[0, 1].piece = new Knight(true);
            this.Tiles[0, 2].piece = new Bishop(true);
            this.Tiles[0, 3].piece = new Queen(true);
            this.Tiles[0, 4].piece = new King(true);
            this.Tiles[0, 5].piece = new Bishop(true);
            this.Tiles[0, 6].piece = new Knight(true);
            this.Tiles[0, 7].piece = new Rook(true);

            // Black pieces
            this.Tiles[7, 0].piece = new Rook(false);
            this.Tiles[7, 1].piece = new Knight(false);
            this.Tiles[7, 2].piece = new Bishop(false);
            this.Tiles[7, 3].piece = new Queen(false);
            this.Tiles[7, 4].piece = new King(false);
            this.Tiles[7, 5].piece = new Bishop(false);
            this.Tiles[7, 6].piece = new Knight(false);
            this.Tiles[7, 7].piece = new Rook(false);
        }

        public void PrintBoard()
        {
            Console.Clear();
            // File, width coordinates
            Console.WriteLine("  " + 1 + " " + 2 + " " + 3 + " " + 4 + " " + 5 + " " + 6 + " " + 7 + " " + 8);
            int boardHeightCoordinates = 1;

            for (int i = 0; i < this.BOARD_HEIGHT; i++)
            {
                // Rank, Height coordinates 
                Console.Write(boardHeightCoordinates + " ");
                boardHeightCoordinates++;

                for (int ii = 0; ii < this.BOARD_WIDTH; ii++)
                {

                    Tile currentTile = this.Tiles[i, ii];

                    if ((i + ii) % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }

                    if (currentTile.Occupied())
                    {
                        if (currentTile.piece.color)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        // Console.ForegroundColor = ConsoleColor.;
                        Console.Write(currentTile.piece.nameShort.ToString() + " ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public bool InBounds(int rank, int file)
        {
            if (rank < 0 || rank > this.BOARD_WIDTH) return false;
            if (file < 0 || file > this.BOARD_HEIGHT) return false;
            return true;
        }
    }
}

