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
        public List<char> BOARD_LETTERS;

        public Tile[,] Tiles;

        public Board()
        {
            this.BOARD_WIDTH = 8;
            this.BOARD_HEIGHT = 8;
            this.BOARD_LETTERS = new List<char>();
            this.BOARD_LETTERS.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());

            // Initialize two dimensional board array
            Tiles = new Tile[this.BOARD_WIDTH, this.BOARD_HEIGHT];

            for (int i = 7; i >= 0; i--)
            {
                for (int ii = 0; ii < this.BOARD_WIDTH; ii++)
                {
                    this.Tiles[i, ii] = new Tile(i, ii);
                }
            }
        }

        public Tile GetTile(int rank, char file)
        {
            int fileIndex = this.BOARD_LETTERS.IndexOf(Char.ToUpper(file));
            // Substract one from rank since it's currently the same as user input
            // After substraction we can use it as an array/list index
            return this.Tiles[rank - 1, fileIndex];
        }

        public void Move(Move move)
        {
            // Move piece from old tile to new tile
            move.toTile.piece = move.fromTile.piece;
            // Remove piece from old tile
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
            /*
             * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             * BLESS YOUR SOUL, WHOEVER HAS TO READ THIS SPAGHETTI USED TO RENDER THE BOARD
             * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            */
            
            const string SPACER = "   ";
            const string SPACER_EMPTY = "       ";
            int boardHeightCoordinates = 8;

            for (int i = 7; i >= 0; i--)
            {
                _boardPrintMargin(i);
                Console.WriteLine();

                // Rank, Height coordinates 
                Console.Write(SPACER + boardHeightCoordinates + SPACER);
                boardHeightCoordinates--;

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
                        Console.Write(SPACER + currentTile.piece.nameShort.ToString() + SPACER);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(SPACER_EMPTY);
                    }
                    Console.ResetColor();
                }
                _boardPrintMargin(i);
            }
                Console.WriteLine();
            // File, width coordinates

            Console.WriteLine();
            Console.WriteLine(SPACER_EMPTY + SPACER + 1 + SPACER + SPACER + 2 + SPACER + SPACER + 3 + SPACER + SPACER + 4 + SPACER + SPACER + 5 + SPACER + SPACER + 6 + SPACER + SPACER + 7 + SPACER + SPACER + 8);
            Console.WriteLine();
        }

        public bool InBounds(char fileLetter, int rank)
        {
            int file = this.BOARD_LETTERS.IndexOf(Char.ToUpper(fileLetter)) + 1;
            if (rank < 0 || rank > this.BOARD_WIDTH) return false;
            if (file < 0 || file > this.BOARD_HEIGHT) return false;
            return true;
        }

        public List<Tile> CoordinateListToTiles(List<Tuple<int, int>> coordinates)
        {
            var tiles = new List<Tile>();
            foreach (Tuple<int, int> tuple in coordinates)
            {
                tiles.Add(this.GetTile(tuple.Item1, (char)tuple.Item2));
            }
            return tiles;
        }

        private void _boardPrintMargin (int i)
        {

            Console.WriteLine("");
            Console.Write("       ");

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

                Console.Write("       ");
                Console.ResetColor();
            }
        }
    }
}

