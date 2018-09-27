using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        private const string easy1 = "003020600900305001001806400008102900700000008006708200002609500800203009005010300";
        private const string easy2 = "619030040270061008000047621486302079000014580031009060005720806320106057160400030";
        private const string medium1 = "037060000205000800006908000000600024001503600650009000000302700009000402000050360";
        private const string diabolic1 = "000000000000003085001020000000507000004000100090000000500000073002010000000040009";
        private const string diabolic2 = "900040000000010200370000005000000090001000400000705000000020100580300000000000000";
        private const string zen = "000000000000000000000000000000000000000010000000000000000000000000000000000000000";
        private const string unsolvable1 = "..9.287..8.6..4..5..3.....46.........2.71345.........23.....5..9..4..8.7..125.3..";
        private const string unsolvable2 = ".9.3....1....8..46......8..4.5.6..3...32756...6..1.9.4..1......58..2....2....7.6.";
        private const string unsolvable3 = "....41....6.....2...2......32.6.........5..417.......2......23..48......5.1..2...";
        private const string unsolvable4 = "9..1....4.14.3.8....3....9....7.8..18....3..........3..21....7...9.4.5..5...16..3";
        private const string unsolvable5 = ".4.1..35.............2.5......4.89..26.....12.5.3....7..4...16.6....7....1..8..2.";


        static void Main(string[] args)
        {

            Sudoku game = new Sudoku(medium1);

            game.Solve();
        }
    }
}
