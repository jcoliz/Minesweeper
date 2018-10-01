using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Minesweeper.Logic
{
    /// <summary>
    /// Contains a generic grid of items
    /// </summary>
    public class Board<T> where T: new()
    {
        public Rectangle Dimensions { get; }
        public List<List<T>> Markers { get; }

        public Board(Size desireddimensions)
        {
            Dimensions = new Rectangle(new Point(), desireddimensions);

            Markers = new List<List<T>>();

            for (int row = 0; row < Dimensions.Height; ++row)
            {
                var newrow = new List<T>();
                for (int col = 0; col < Dimensions.Width; ++col)
                {
                    newrow.Add(new T());
                }
                Markers.Add(newrow);
            }
        }

        /// <summary>
        /// Indexer for convenient access to markers
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public T this[Point position] => Markers[position.Y][position.X];
        public T this[int x,int y] => Markers[y][x];

        /// <summary>
        /// Render grid in a display-friendly way
        /// </summary>
        /// <returns>List of strings, one line for each row of the board</returns>
        public List<string> Render()
        {
            return
                Markers.Aggregate(
                    new List<string>(), 
                    (result, row) => 
                    {
                        result.Add(
                            row.Aggregate(
                                new StringBuilder(), 
                                (sb, marker) => sb.Append($"[{marker}] ")
                            ).ToString()
                        );
                        return result;
                    }
                );
        }
    }
}
