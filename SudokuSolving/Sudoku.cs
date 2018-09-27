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
        public readonly int NumberOfStartingNumbers;
        public int row = 0;
        public int column = 0;

        public Sudoku(string boardString)
        {
            int col = 0;
            int row = 0;

            foreach (char character in boardString) // Konstruktorn fyller vår array direkt
            {
                int currentNumber = 0;

                if (character == '.')
                {
                    currentNumber = 0;
                }
                else
                {
                    currentNumber = int.Parse(character.ToString());
                }
                
                Board[col, row] = currentNumber;
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
            BoardAsText(5);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (SolveSudoku())
            {
                BoardAsText(20);
                Console.Write("\n Beep boop, the Sudoku was solved!\n");
            }
            else
            {
                sw.Stop();
                Console.Write("\n Beep boop, couldn't solve the Sudoku.. \n");
            }
        }

        bool SolveSudoku()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    if (Board[row, column] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsSafe(row, column, num))
                            {
                                Board[row, column] = num;

                                if (SolveSudoku()) // Om vi lyckas sätta ut en siffra så anropar vi samma metod igen. 
                                {
                                    return true;
                                }
                                else // Lyckas den inte sätta ut nästa siffra så nollställer vi den vi precis la ut.
                                {
                                    Board[row, column] = 0;
                                } 
                            }
                        }
                        return false;
                    }
                }
            }
            return true; //Om vi kollar igenom alla rader och inte lyckas hitta någon 0a så är brädet färdigt.
        }

        public bool IsSafe(int row, int column, int currentNumber)
        {
            if (CheckRow(row, currentNumber) && CheckColumn(column, currentNumber) && CheckBox(row, column, currentNumber))
            {
                return true;
            }
            return false;
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

        public void BoardAsText(int sleepTime) // Printar ut brädet
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
                        Thread.Sleep(sleepTime);
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
            Console.Write("\n Thinking.. {0:0.}% completed.", percentageCompleted);
        }
    }//Class
} //Namespace