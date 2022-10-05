using System;
using Chess;
using Chess.Pieces;
using Chess.Rules;
using Chess.Error;
using Chess.Error.Levels;

namespace Chess.Chessboard
{
    public class Board
    {
        public int BOARD_HEIGHT;
        public int BOARD_WIDTH;
        public List<char> BOARD_LETTERS;
        public ErrorHandler ErrorHandler;
        public Move LastMove;

        public Tile[,] Tiles;

        public Board(ErrorHandler ErrorHandler)
        {
            this.BOARD_WIDTH = 8;
            this.BOARD_HEIGHT = 8;
            this.BOARD_LETTERS = new List<char>();
            this.BOARD_LETTERS.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
            this.ErrorHandler = ErrorHandler;

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

        // Move piece to new tile, gets called AFTER move has been validated and is legal
        public void Move(Move move, List<Piece> list)
        {
            // Add "eaten" pieces to corresponding list
            if (move.toTile.piece != null)
            {
                list.Add(move.toTile.piece);
            }
            // Move piece from old tile to new tile
            move.toTile.piece = move.fromTile.piece;

            this.LastMove = move;

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

            // Test piece
            this.Tiles[4, 4].piece = new Rook(true);

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

        public void PrintBoard(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces)
        {
            int boardHeightCoordinates = 8;
            _printBlackMaterialDifference(BlackCapturedPieces, WhiteCapturedPieces);

            for (int i = 7; i >= 0; i--)
            {
                _boardPrintMargin(i);
                Console.WriteLine();

                // Rank, Height coordinates 
                _boardStringPadding(boardHeightCoordinates);
                boardHeightCoordinates--;

                for (int ii = 0; ii < this.BOARD_WIDTH; ii++)
                {
                    Tile currentTile = this.Tiles[i, ii];
                    _boardSetBackground(i, ii, currentTile);
                    _boardPrintContent(currentTile);

                }
                _boardPrintMargin(i);
            }

            // File, width coordinates
            Console.WriteLine("\n\n          " + 'a' + "      " + 'b' + "      " + 'c' + "      " + 'd' + "      " + 'e' + "      " + 'f' + "      " + 'g' + "      " + 'h' + "\n");

            _printWhiteMaterialDifference(BlackCapturedPieces, WhiteCapturedPieces);
        }

        public void PrintBoard(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces, Tile selectedTile)
        {
            int boardHeightCoordinates = 8;
            bool[,] ValidMoves = selectedTile.piece.GetValidMoves(selectedTile, this.Tiles);

            _printBlackMaterialDifference(BlackCapturedPieces, WhiteCapturedPieces);

            for (int i = 7; i >= 0; i--)
            {
                _boardPrintMargin(i, ValidMoves);
                Console.WriteLine();

                // Rank, Height coordinates 
                _boardStringPadding(boardHeightCoordinates);
                boardHeightCoordinates--;

                for (int ii = 0; ii < this.BOARD_WIDTH; ii++)
                {
                    Tile currentTile = this.Tiles[i, ii];
                    _boardSetBackground(i, ii, currentTile, ValidMoves[i, ii]);
                    _boardPrintContent(currentTile);

                }
                _boardPrintMargin(i, ValidMoves);
            }

            // File, width coordinates
            Console.WriteLine("\n\n          " + 'a' + "      " + 'b' + "      " + 'c' + "      " + 'd' + "      " + 'e' + "      " + 'f' + "      " + 'g' + "      " + 'h' + "\n");
            Console.WriteLine(ValidMoves[4,3]);

            _printWhiteMaterialDifference(BlackCapturedPieces, WhiteCapturedPieces);
        }

        public bool InBounds(char fileLetter, int rank)
        {
            if (fileLetter == '0' && rank == 0) return false;

            int file = this.BOARD_LETTERS.IndexOf(Char.ToUpper(fileLetter)) + 1;
            if (rank < 0 || rank > this.BOARD_WIDTH) return false;
            if (file < 0 || file > this.BOARD_HEIGHT) return false;
            return true;
        }

        public List<Tile> CoordinateListToTiles(List<Tuple<int, int>> coordinates)
        {
            var tiles = new List<Tile>();
            foreach (var tuple in coordinates)
            {
                tiles.Add(this.GetTile(tuple.Item1, (char)tuple.Item2));
            }
            return tiles;
        }

        public bool MoveIsPossible(Move move)
        {
            // Convert list of coordinates to usable tile objects
            List<Tile> tiles = this.CoordinateListToTiles(move.GetTileIndexesBetweenInputs());
            // Check if a piece is blocked, bypass if move has ghosting AKA is allowed to move over other pieces
            bool isBlocked = (move.fromTile.piece.ghosting) ? false : move.IsBlocked(tiles);
            if (isBlocked) this.ErrorHandler.New("Move was blocked by another piece", Level.Warning);
            return !isBlocked;
        }

        private void _boardPrintMargin(int i)
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

                if (currentTile == this.LastMove?.fromTile) Console.BackgroundColor = ConsoleColor.DarkGreen;
                if (currentTile == this.LastMove?.toTile) Console.BackgroundColor = ConsoleColor.Green;

                Console.Write("       ");
                Console.ResetColor();
            }
        }

        private void _boardPrintMargin(int i, bool[,] ValidMove)
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

                if (currentTile == this.LastMove?.fromTile) Console.BackgroundColor = ConsoleColor.DarkGreen;
                if (currentTile == this.LastMove?.toTile) Console.BackgroundColor = ConsoleColor.Green;
                if (ValidMove[i, ii]) Console.BackgroundColor = ConsoleColor.Cyan;

                Console.Write("       ");
                Console.ResetColor();
            }
        }

        private void _printBlackMaterialDifference(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces)
        {
            // Print list of captured pieces and calculate difference in material sum
            int materialDifference = WhiteCapturedPieces.CalculateMaterialSum() - BlackCapturedPieces.CalculateMaterialSum();
            BlackCapturedPieces.PrintList();
            Console.Write("   ");
            if (materialDifference < 0) Console.WriteLine($"+{Math.Abs(materialDifference)}");
        }

        private void _printWhiteMaterialDifference(CapturedPieces BlackCapturedPieces, CapturedPieces WhiteCapturedPieces)
        {
            // Print captured list for White pieces
            int materialDifference = WhiteCapturedPieces.CalculateMaterialSum() - BlackCapturedPieces.CalculateMaterialSum();
            WhiteCapturedPieces.PrintList();
            Console.Write("   ");
            if (materialDifference > 0) Console.WriteLine($"+{Math.Abs(materialDifference)}");
        }

        private void _boardSetBackground(int i, int ii, Tile currentTile)
        {
            if ((i + ii) % 2 == 0)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.White;
            }

            // Set different background colors for previous move played
            if (currentTile == this.LastMove?.fromTile) Console.BackgroundColor = ConsoleColor.DarkGreen;
            if (currentTile == this.LastMove?.toTile) Console.BackgroundColor = ConsoleColor.Green;
        }

        private void _boardSetBackground(int i, int ii, Tile currentTile, bool ValidMove)
        {
            if ((i + ii) % 2 == 0)
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.White;
            }

            // Set different background colors for previous move played
            if (currentTile == this.LastMove?.fromTile) Console.BackgroundColor = ConsoleColor.DarkGreen;
            if (currentTile == this.LastMove?.toTile) Console.BackgroundColor = ConsoleColor.Green;
            if (ValidMove) Console.BackgroundColor = ConsoleColor.Cyan;
        }

        private void _boardSetForeground(Tile currentTile)
        {
            if (currentTile.piece.color)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
        }

        private void _boardPrintContent(Tile currentTile)
        {
            if (currentTile.Occupied())
            {
                _boardSetForeground(currentTile);
                _boardStringPadding(currentTile.piece.nameShort.ToString());
            }
            else
            {
                _boardStringPadding();
            }
            Console.ResetColor();
        }

        private void _boardStringPadding(string content = " ", bool newline = false)
        {
            const string SPACER = "   ";
            const string SPACER_EMPTY = "       ";
            string response;

            if (content != " ")
            {
                response = SPACER + content + SPACER; 
            }
            else
            {
                response = SPACER_EMPTY;
            }

            if (newline)
            {
                Console.WriteLine(response);
            }
            else
            {
                Console.Write(response);
            }
        }

        private void _boardStringPadding(int number)
        {
            string content = number.ToString();
            _boardStringPadding(content);
        }
    }
}

