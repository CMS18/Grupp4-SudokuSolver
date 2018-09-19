﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SudokuSolver
{
    class Sudoku
    {
        public int[,] Board = new int[9, 9];
        public int[,] ResettedBoard = new int[9, 9];
        public readonly int NumberOfStartingNumbers;
        public int Resets { get; set; }

        public Sudoku(string boardString)
        {

            int col = 0;
            int row = 0;

            foreach (char character in boardString) // Konstruktorn fyller vår array direkt
            {
                int currentNumber = int.Parse(character.ToString());
                Board[col, row] = currentNumber;
                ResettedBoard[col, row] = currentNumber;
                row++;
                if (row == 9)
                {
                    col++;
                    row = 0;
                }
            }
            foreach (int number in Board) // Används bara för min CalculatePercentageCompleted() metod.
            {
                if (number != 0)
                {
                    NumberOfStartingNumbers++;
                }
            }
        }

        public void Solve() // Huvudmetoden, körs när programmet startar
        {
            BoardAsText();

            while (IsboardFull() == false)
            {
                PlaceMatchingNumber();
            }
            Console.Write("\n Det tog \"bara\" {0} resets.. HACKERGOD.exe finished. \n", Resets);
        }

        public void PlaceMatchingNumber()
        {
            bool couldPlaceNumber = false;

            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    if (Board[row, column] == 0)
                    {
                        int[] recievedNumber = FindPossibleNumbers(row, column);

                        if (recievedNumber.Length == 1 && CheckBox(row, column, recievedNumber[0]))
                        {
                            Board[row, column] = recievedNumber[0];
                            couldPlaceNumber = true;
                            BoardAsText();
                        }
                    }
                }
            }

            if (couldPlaceNumber == false)
            {
                couldPlaceNumber = PlaceRandomNumber();

                if(couldPlaceNumber == false)
                {
                    ResetBoard();
                }
            }
        }

        public bool PlaceRandomNumber()
        {
            int min = 0;
            int max = 0;
            int currentNumber = 1;
            bool couldPlaceNumber = false;
            Random rnd = new Random();

            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    if (Board[row, column] == 0)
                    {
                        int[] minMax = FindPossibleNumbers(row, column);
                        if (minMax.Length < 2)
                        {
                            currentNumber = rnd.Next(1, 10);
                        }
                        else
                        {
                            min = minMax.Min();
                            max = minMax.Max();
                        }

                        currentNumber = rnd.Next(min,max);

                        for (int i = 0; i < 9; i++)
                        {
                            if (CheckRow(row, currentNumber) && CheckColumn(column, currentNumber) && CheckBox(row, column, currentNumber))
                            {
                                Board[row, column] = currentNumber;
                                BoardAsText();
                                couldPlaceNumber = true;
                                PlaceMatchingNumber();
                            }
                            //else
                            //{
                            //    currentNumber++;
                            //}
                        }
                    }
                }
            }
            return couldPlaceNumber;
        }

        public void ResetBoard()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    Board[row, column] = ResettedBoard[row, column];
                }
            }
            Resets++;
        }

        public bool CheckRow(int row, int currentNumber)
        {
            for (int column = 0; column < 9; column++)
            {
                if (Board[row, column] == currentNumber)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckColumn(int column, int currentNumber)
        {
            for (int row = 0; row < 9; row++)
            {
                if (Board[row, column] == currentNumber)
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
                for (int column = 0; column < 9; column++)
                {
                    if (Board[row, column] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public double CalculatePercentageCompleted()
        {
            double numbersCompleted = 0;

            foreach (int number in Board)
            {
                if (number != 0)
                {
                    numbersCompleted++;
                }
            }

            return ((numbersCompleted - NumberOfStartingNumbers) * 10 / (81 - NumberOfStartingNumbers)) * 10;
        }

        public void BoardAsText() // Printar ut brädet
        {
            double percentageCompleted = CalculatePercentageCompleted();
            Console.SetCursorPosition(0, 0);
            //Thread.Sleep(10);

            for (int row = 0; row < 9; row++)
            {
                if (row == 3 || row == 6)
                {
                    Console.Write(" ---------------------------\n");
                }
                for (int column = 0; column < 9; column++)
                {
                    if (Board[row, column] == 0)
                    {
                        Console.Write(" _ ");
                    }
                    else
                    {
                        Console.Write(" " + Board[row, column] + " ");

                    }
                    if (column == 2 || column == 5)
                    {
                        Console.Write("|");
                    }
                    if (column == 8)
                    {
                        Console.WriteLine();
                    }
                }
            }
            //Thread.Sleep(10);

            Console.Write("\n Thinking.. {0:0.0}% completed.", percentageCompleted);
        }

        public int[,] FindPossibleEntries(int[,] board, int row, int column)
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
                if (board[y, column] != 0)
                {
                    possibilitiesArray[y, column] = 1;
                }
            }

            int topLeftRow = (row / 3) * 3;
            int topLeftColumn = (column / 3) * 3;

            return possibilitiesArray;
        }
    }//Class
} //Namespace

