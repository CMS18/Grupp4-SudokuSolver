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
            BoardAsText();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (SolveSudoku())
            {
                sw.Stop();
                BoardAsText(40); //Threadsleep effekt
                Console.Write("\n Beep boop, the Sudoku was solved! It took {0:0.0} seconds.\n", sw.Elapsed.TotalSeconds);
                Console.WriteLine(" Had to backtrack "+ count +" times in order to solve the board.");
                Console.ReadKey();
            }
            else
            {
                sw.Stop();
                BoardAsText(40);
                Console.Write("\n Beep boop, couldn't solve the Sudoku.. We tried for {0:0.0} seconds before giving up.\n", sw.Elapsed.TotalSeconds);
                Console.WriteLine(" Deleted " + count + " numbers before we gave up.");
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
                            if (IsValid(row, column, num)) // Kollar om num går att sätta in på nuvarande index.
                            {
                                Board[row, column] = num;

                                if (SolveSudoku()) // Lyckas vi sätta ut en siffra så anropar vi metoden igen som hittar ett nytt tomt index, och testar det indexet.
                                {
                                    return true; // Returnerar till förra anropet ("varvet") och till slut till Solve(); när den nått första varvet.
                                }
                                else
                                {
                                    Board[row, column] = 0; // Om ovan anrop inte lyckas sätta ut 1-9 så misslyckas den och returnerar false, då hamnar vi här på förra "varvet".
                                    count++; // Beräknar antal backtracks.
                                }
                            }
                        }
                        return false; // Om det rekursiva anropet till Solvesudoku misslyckas (Om nästa tomma ruta inte kan sätta in 1-9) 
                                      // -så returnar vi false. Då nollställs förra siffran via else{}.
                                      // Om vi backtrackar till första tomma indexet och går igenom 1-9 och inte lyckas sätta ut något, då returnerar vi false till Solve();
                    }
                }
            }
            return true; // Vi kommer hit först när det inte finns ett enda tomt index i hela brädet, annars går vi in i rekursiva metoden eller backtracken. 
        }                // Denna return går till if (SolveSudoku(), som då hoppar in i return true;
                         // Den i sin tur returnerar True; till förra varvets anrop. Till slut är den tillbaka på första varvet och då returnerar den true till Solve();

        public bool IsValid(int row, int column, int currentNumber) // Kollar rad, kolumn och box för att se om nuvarande siffra går att sätta in på det tomma indexet.
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
                        Thread.Sleep(threadSleep); //Används för dramatisk effekt.
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