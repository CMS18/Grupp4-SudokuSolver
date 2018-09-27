using System;
using System.Diagnostics;
using System.Threading;

namespace SudokuSolver
{
    class Sudoku
    {
        public int[,] Board = new int[9, 9];
        public readonly int NumberOfStartingNumbers;
        public int row = 0;
        public int col = 0;
        private int count;

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
                Board[row, col] = currentNumber;
                col++;
                if (col == 9)
                {
                    row++;
                    col = 0;
                }
            }
            foreach (int number in Board) // Används bara för CalculatePercentageCompleted() metod.
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
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (SolveSudoku())
            {
                sw.Stop();
                BoardAsText(40); //Threadsleep effekt
                Console.Write("\n Beep boop, the Sudoku was solved! It took {0:0.0} seconds.\n", sw.Elapsed.TotalSeconds);
                Console.WriteLine("Det tog " + count + " antal försök att lösa.");
                Console.ReadKey();
            }
            else
            {
                sw.Stop();
                BoardAsText(40);

                Console.Write("\n Beep boop, couldn't solve the Sudoku.. We tried for {0:0.0} seconds before giving up.\n", sw.Elapsed.TotalSeconds);
                Console.WriteLine("Det tog " + count + " antal försök att lösa.");
                Console.ReadKey();
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
                        for (int num = 9; num > 0; num--)
                        {
                            if (IsValid(row, column, num)) //Lägger in de numret den hittade på den platsen.
                            {
                                count++;
                                Board[row, column] = num;

                                if (SolveSudoku()) // Siffran som är valid anropar samma metod igen. 
                                {
                                    return true; //Cell 0 testar sätta ut tex en 1. Kör sig själv igen. 
                                    //prova sig fram tills den har en siffra som är valid. IsValid bestämmer vad för siffra som kan testas.
                                }
                                else
                                {
                                    Board[row, column] = 0; //Sätter till noll, stega till nästa nummer. mellan 1-9
                                }
                            }
                        }
                        return false; //Går inte in i metoden solve sudoku om false. Inser att den inte kan placera 1-9 i cellen
                        //Backtracking, hoppar ur sig själv och börjar om.
                    }
                }
            }
            return true; //När sedukon är helt löst. Då blir det true
        }

        public bool IsValid(int row, int column, int currentNumber) //kollar rad, kolumn & box efter möjlig siffra.
        {
            if (CheckRow(row, currentNumber) && CheckColumn(column, currentNumber) && CheckBox(row, column, currentNumber))
            {
                return true;
            }
            return false;
        }

        public bool CheckRow(int row, int currentNumber) //Kollar rad
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

        public bool CheckColumn(int column, int currentNumber) //Kollar kolumn
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

        public bool CheckBox(int row, int column, int currentNumber) //Kollar box
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

        public double CalculatePercentageCompleted() //Effekt endast för procent räknad
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

        public void BoardAsText(int threadSleep = 0) // Printar ut brädet + skickar in thread effekt.
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
                        Thread.Sleep(threadSleep); //Används vid första uppgiften för dramatisk effekt + thread där siffrorna skrivs ut.
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