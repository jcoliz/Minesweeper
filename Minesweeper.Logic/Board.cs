using System.Collections.Generic;
using System.Drawing;

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

        /// <summary>
        /// Render grid in a display-friendly way
        /// </summary>
        /// <returns></returns>
        public List<string> Render()
        {
            List<string> result = new List<string>();

            foreach (var row in Markers)
            {
                string line = string.Empty;

                foreach (var marker in row)
                    line = line + $"[{marker}] ";

                result.Add(line);
            }

            return result;
        }
    }
}
