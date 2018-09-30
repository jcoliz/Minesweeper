using Minesweeper.Logic;
using System;
using System.Collections.Generic;

namespace Minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Minesweeper!");

            var size = 9;
            var playfield = new MarkerGrid(size);

            var headers = new string[3] { "   ", "   ", "   " };
            for(int i = 1; i <= size; ++i)
            {
                headers[0] += (i / 10).ToString();
                headers[1] += (i % 10).ToString();
                headers[2] += '-';
            }
            var footers = new string[3] { headers[2], headers[1], headers[0] };

            bool done = false;
            while(!done)
            {
                try
                {
                    var lines = playfield.Render();

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

                    Console.Write("Enter col,row to play or q to quit> ");

                    var input = Console.ReadLine();

                    if (input == "q")
                        break;

                    var values = input.Split(",");

                    int col = int.Parse(values[0]) - 1;
                    int row = int.Parse(values[1]) - 1;

                    var result = playfield.PlayAt(col, row);

                    Console.WriteLine(result.ToString());

                    if (result == MarkerGrid.PlayResult.GameOver || result == MarkerGrid.PlayResult.Victory)
                        done = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
