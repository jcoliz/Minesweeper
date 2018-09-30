using Minesweeper.Logic;
using System;
using System.Collections.Generic;

namespace Minesweeper
{
    class Program
    {
        static MarkerGrid Playfield;
        const int Size = 9;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Minesweeper!");

            Playfield = new MarkerGrid(Size);

            bool done = false;
            while(!done)
            {
                try
                {
                    Render();

                    Console.Write("Enter col,row to play or q to quit> ");

                    var input = Console.ReadLine();

                    if (input == "q")
                        break;

                    var values = input.Split(",");

                    int col = int.Parse(values[0]) - 1;
                    int row = int.Parse(values[1]) - 1;

                    var result = Playfield.PlayAt(col, row);

                    Console.WriteLine(result.ToString());

                    if (result == MarkerGrid.PlayResult.GameOver || result == MarkerGrid.PlayResult.Victory)
                        done = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Render();

            Console.Write("Press enter:");
            Console.ReadLine();
        }

        static void Render()
        {
            var headers = new string[3] { "   ", "   ", "   " };
            for (int i = 1; i <= Size; ++i)
            {
                headers[0] += (i / 10).ToString();
                headers[1] += (i % 10).ToString();
                headers[2] += '-';
            }
            var footers = new string[3] { headers[2], headers[1], headers[0] };

            var lines = Playfield.Render();

            Console.WriteLine();
            foreach (var header in headers)
                Console.WriteLine(header);
            int lineno = 1;
            foreach (var line in lines)
            {
                Console.WriteLine($"{lineno,02}|{line}|{lineno,02}");
                ++lineno;
            }
            foreach (var footer in footers)
                Console.WriteLine(footer);
            Console.WriteLine();

        }
    }
}
