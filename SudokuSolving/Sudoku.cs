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
                    if(board[row, col] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
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

