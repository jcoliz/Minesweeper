using Minesweeper.Logic;
using System;

namespace Minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Minesweeper!");

            var playfield = new MarkerGrid(9);

            bool done = false;
            while(!done)
            {
                try
                {
                    var lines = playfield.Render();

                    Console.WriteLine();
                    foreach (var line in lines)
                        Console.WriteLine(line);
                    Console.WriteLine();

                    Console.Write("Enter col,row to play or q to quit> ");

                    var input = Console.ReadLine();

                    if (input == "q")
                        break;

                    var values = input.Split(",");

                    int col = int.Parse(values[0]);
                    int row = int.Parse(values[1]);

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
