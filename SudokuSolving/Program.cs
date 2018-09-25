using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Sudoku game = new Sudoku("..9.287..8.6..4..5..3.....46.........2.71345.........23.....5..9..4..8.7..125.3..");

            game.Solve();
        }
    }
}
