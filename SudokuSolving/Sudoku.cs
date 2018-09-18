using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Sudoku
    {
        public string BoardString { get; set; }
        public int[,] Board = new int[9, 9];

        public Sudoku(string boardString)
        {
            BoardString = boardString;
        }

        public void Solve()
        {
            Board = FillBoard(BoardString);
            PrintSudokuBoard(Board);
        }

        public bool IsboardFull(int[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void FindPossibleEntries(int[,] board, int row, int col)
        {
            int[,] possibilitiesArray = new int[9, 9];

            for (int y = 0; y < 9; y++) //Fyller vår possibilitesArray med 0or som grund.
            {
                for (int x = 0; x < 9; x++)
                {
                    possibilitiesArray[y, x] = 0;
                }
            }
            for (int x = 0; x < 9; x++) //Kollar vågrätt om innehållet är en 0a, annars sätts indexet till en 1a.
            {
                if (board[row, x] != 0)
                {
                    possibilitiesArray[row, x] = 1;
                }
            }
            for (int y = 0; y < 9; y++) //Kollar horisontellt om innehållet är en 0a, annars sätts indexet till en 1a.
            {
                if (board[y, col] != 0)
                {
                    possibilitiesArray[y, col] = 1;
                }
            }

            int k = 0;
            int l = 0;

            if (row >= 0 && row <= 2) k = 0;
            else if (row >= 3 && row <= 5) k = 3;
            else k = 6;

            if (col >= 0 && col <= 2) l = 0;
            else if (col >= 3 && col <= 5) l = 3;
            else l = 6;


        }

        public int[,] FillBoard(string boardString)
        {
            int[,] board = new int[9, 9];

            if (boardString.Length == 81)
            {
                char[] boardCharArray = boardString.ToCharArray();

                int col = 0;
                int row = 0;

                foreach (char character in boardCharArray)
                {
                    int currentNumber = int.Parse(character.ToString());
                    board[col, row] = currentNumber;

                    row++;
                    if (row == 9)
                    {
                        col++;
                        row = 0;
                    }
                }
            }
            else
            {
                Console.WriteLine("Incorrect amount of numbers in boardstring!");
            }
            return board;
        }

        public void PrintSudokuBoard(int[,] board)
        {
            Console.Write("+-----------------------------------+\n");

            for (int col = 0; col < 9; col++)
            {
                Console.Write("|");

                for (int row = 0; row < 9; row++)
                {
                    Console.Write(" " + board[col, row] + " |");

                    if (row == 8)
                    {
                        Console.Write("\n+-----------------------------------+");
                    }
                }
                Console.WriteLine();
            }
        }


    }
}

