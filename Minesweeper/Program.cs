using Minesweeper.Logic;
using System;
using System.Collections.Generic;

namespace Minesweeper
{
    class Program
    {
        static Game Playfield;
        const int Size = 9;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Minesweeper!");

            Playfield = new Game(Size);

            bool done = false;
            while(!done)
            {
                try
                {
                    PrettyRender(Playfield.GameBoard);

                    Console.Write("Enter col,row to play or q to quit> ");

                    var input = Console.ReadLine();

                    if (input == "q")
                        break;

                    var values = input.Split(",");

                    int col = int.Parse(values[0]) - 1;
                    int row = int.Parse(values[1]) - 1;

                    var result = Playfield.PlayAt(col, row);

                    Console.WriteLine(result.ToString());

                    if (result == Game.PlayResult.GameOver || result == Game.PlayResult.Victory)
                        done = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            PrettyRender(Playfield.GameBoard);

            Console.Write("Press enter:");
            Console.ReadLine();
        }

        static void PrettyRender<T>(Board<T> board) where T: new()
        {
            var lines = new List<string>();

            var headers = new string[3] { "   ", "   ", "   " };
            for (int i = 1; i <= board.Dimensions.Width; ++i)
            {
                headers[0] += (i / 10).ToString();
                headers[1] += (i % 10).ToString();
                headers[2] += '-';
            }
            var footers = new string[3] { headers[2], headers[1], headers[0] };

            lines.AddRange(headers);
            int lineno = 1;
            foreach (var line in board.Render())
            {
                lines.Add($"{lineno,02}|{line}|{lineno,02}");
                ++lineno;
            }
            lines.AddRange(footers);

            Console.WriteLine();
            foreach (var line in lines)
                Console.WriteLine(line);
            Console.WriteLine();
        }
    }
}
