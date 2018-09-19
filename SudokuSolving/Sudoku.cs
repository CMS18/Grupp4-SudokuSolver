using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Sudoku
    {
        public int[,] Board = new int[9, 9];

        public Sudoku(string boardString)
        {
            int col = 0;
            int row = 0;

            foreach (char character in boardString) // Konstruktorn fyller vår array direkt
            {
                int currentNumber = int.Parse(character.ToString());
                Board[col, row] = currentNumber;
                row++;
                if (row == 9)
                {
                    col++;
                    row = 0;
                }
            }
        }

        public void Solve() // Huvudmetoden, körs när programmet startar
        {
            BoardAsText();
            Console.WriteLine();
            Console.WriteLine();

            while (IsboardFull() == false)
            {
                PlaceMatchingNumber();
            }
            Console.WriteLine();
            BoardAsText();

        }

        public void PlaceMatchingNumber()
        {
            bool couldPlaceNumber = false;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (Board[row, col] == 0)
                    {
                        int[] recievedNumber = FindPossibleNumbers(row, col);

                        if (recievedNumber.Length == 1 && CheckBox(row, col, recievedNumber[0]))
                        {
                            Board[row, col] = recievedNumber[0];
                            couldPlaceNumber = true;
                        }
                    }
                }
            }
            if (couldPlaceNumber == false)
            {
                PlaceOddNumber();
            }
        }

        public void PlaceOddNumber()
        {
            int currentNumber = 1;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (Board[row, col] == 0)
                    {
                        if (CheckRow(row, currentNumber) && CheckColumn(col, currentNumber) && CheckBox(row, col, currentNumber))
                        {
                            Board[row, col] = currentNumber;
                            
                        }
                    }
                }
                currentNumber++;
            }
        }

        public bool CheckRow(int row, int currentNumber)
        {
            for (int col = 0; col < 9; col++)
            {
                if (Board[row, col] == currentNumber)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckColumn(int col, int currentNumber)
        {
            for (int row = 0; row < 9; row++)
            {
                if (Board[row, col] == currentNumber)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckBox(int row, int column, int currentNumber)
        {
            int topLeftRow = (row / 3) * 3;
            int topLeftColumn = (column / 3) * 3;

            for (int i = topLeftRow; i < topLeftRow + 3; i++)
            {
                for (int j = topLeftColumn; j < topLeftColumn + 3; j++)
                {
                    if (Board[i, j] == currentNumber)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public int[] GetNumbersInRow(int row)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                result.Add(Board[row, i]);
            }

            return result.ToArray();
        }

        private int[] GetNumbersInColumn(int column)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                result.Add(Board[i, column]);
            }

            return result.ToArray();
        }

        public int[] FindPossibleNumbers(int row, int column)
        {
            List<int> result = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] numbersInRow = GetNumbersInRow(row);
            int[] numbersInColumn = GetNumbersInColumn(column);

            foreach (int number in numbersInRow)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (Board[row, i] == number)
                    {
                        result.Remove(number);
                    }
                }
            }
            foreach (int number in numbersInColumn)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (Board[i, column] == number)
                    {
                        result.Remove(number);
                    }
                }
            }
            return result.ToArray();
        }



        public bool IsboardFull() // Kollar om brädet innehåller en tom plats
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (Board[row, col] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void BoardAsText() // Printar ut brädet
        {
            for (int row = 0; row < 9; row++)
            {
                if (row == 3 || row == 6)
                {
                    Console.Write(" ---------------------------\n");
                }
                for (int col = 0; col < 9; col++)
                {
                    if (Board[row, col] == 0)
                    {
                        Console.Write(" _ ");
                    }
                    else
                    {
                        Console.Write(" " + Board[row, col] + " ");

                    }
                    if (col == 2 || col == 5)
                    {
                        Console.Write("|");
                    }
                    if (col == 8)
                    {
                        Console.WriteLine();
                    }
                }
            }
        }

        public int[,] FillBoard(string boardString) // Ersatte denna genom att använda konstruktorn istället
        {
            int[,] board = new int[9, 9];

            if (boardString.Length == 81)
            {
                int col = 0;
                int row = 0;

                foreach (char character in boardString)
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
    }//Class
} //Namespace

