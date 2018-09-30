using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace SudokuSolver
{
    class Sudoku
    {
        public int[,] Board = new int[9, 9];
        public int[,] ResettedBoard = new int[9, 9];
        public readonly int NumberOfStartingNumbers;
        public int Resets { get; set; }
        public int row = 0;
        public int column = 0;

        public Sudoku(string boardString)
        {
            foreach (char character in boardString) // Konstruktorn fyller vår array direkt
            {
                int currentNumber = 0;

                if (int.TryParse(character.ToString(), out currentNumber)) ;
                if (character == '.')
                {
                    currentNumber = 0;
                }
                Board[row, column] = currentNumber;
                column++;
                if (column == 9)
                {
                    row++;
                    column = 0;
                }
            }
            foreach (int number in Board) // Används bara för CalculatePercentageCompleted() metoden.
            {
                if (number != 0)
                {
                    NumberOfStartingNumbers++;
                }
            }
        }

        public void Solve() // Huvudmetoden, körs när programmet startar
        {
            Stopwatch sw = new Stopwatch();
            BoardAsText();
            sw.Start();

            PlaceMatchingNumber2();

            sw.Stop();
            BoardAsText();
            Console.WriteLine(" I got this far.");
        }

        public bool IsSafe(int row, int column, int currentNumber)
        {
            if (CheckRow(row, currentNumber) && CheckColumn(column, currentNumber) && CheckBox(row, column, currentNumber))
            {
                return true;
            }
            return false;
        }

        public void PlaceMatchingNumber()
        {
            bool couldPlace = false;

            do
            {
                couldPlace = false;
                for (int row = 0; row < 9; row++)
                {
                    for (int column = 0; column < 9; column++)
                    {
                        if (Board[row, column] == 0)
                        {
                            int recievedNumber = FindPossibleNumbers(row, column);

                            if ((recievedNumber != 0) && CheckBox(row, column, recievedNumber))
                            {
                                Board[row, column] = recievedNumber;
                                couldPlace = true;
                            }
                        }
                    }
                }

            } while (couldPlace);
            BoardAsText();
        }

        public void PlaceMatchingNumber2()
        {
            bool couldPlace = false;
            do
            {
                couldPlace = false;
                for (int row = 0; row < 9; row++)
                {
                    for (int column = 0; column < 9; column++)
                    {
                        if (Board[row, column] == 0)
                        {
                            int solutions = 0;
                            int correctNum = 0;

                            for (int i = 9; i > 0; i--)
                            {
                                if (IsSafe(row, column, i))
                                {
                                    solutions++;
                                    correctNum = i;
                                }
                                else if (solutions > 1)
                                {
                                    break;
                                }
                            }
                            if (solutions == 1)
                            {
                                Board[row, column] = correctNum;
                                couldPlace = true;
                                BoardAsText();
                                break;
                            }
                        }
                    }
                }

            } while (couldPlace == true);
        }

        public void PlaceRandomNumber3()
        {
            bool couldPlaceNumber = false;

            do
            {
                couldPlaceNumber = false;
                for (int row = 0; row < 9; row++)
                {
                    for (int column = 0; column < 9; column++)
                    {
                        if (Board[row, column] == 0)
                        {
                            int recievedNumber = FindPossibleNumbers(row, column);

                            if ((recievedNumber != 0) && CheckBox(row, column, recievedNumber))
                            {
                                Board[row, column] = recievedNumber;
                                couldPlaceNumber = true;

                                BoardAsText();
                            }

                        }
                    }
                }
                for (int row = 0; row < 9; row++)
                {
                    for (int column = 0; column < 9; column++)
                    {
                        if (couldPlaceNumber == false)
                        {
                            for (int num = 9; num > 0; num--)
                            {
                                if (IsSafe(row, column, num))
                                {
                                    Board[row, column] = num;
                                }
                            }
                        }
                    }
                }

            } while (couldPlaceNumber == true);
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

        public int FindPossibleNumbers(int row, int column)
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

            if (result.Count == 1)
            {
                return result[0];
            }
            else
            {
                return 0;
            }

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
                        //Thread.Sleep(1);

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
            Console.Write("\n Thinking.. {0:0.}% completed.\n", percentageCompleted);
        }
    }//Class
} //Namespace