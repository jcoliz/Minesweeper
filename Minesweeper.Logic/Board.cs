using System.Collections.Generic;
using System.Drawing;

namespace Minesweeper.Logic
{
    /// <summary>
    /// Contains a grid of markers for a console board game
    /// </summary>
    /// <remarks>
    /// This could easily be made generic Board(T) where T: new
    /// </remarks>
    public class Board
    {
        public Size Dimensions { get; }
        public List<List<Marker>> Markers { get; }

        public Board(Size desireddimensions)
        {
            Dimensions = desireddimensions;

            Markers = new List<List<Marker>>();

            for (int row = 0; row < Dimensions.Height; ++row)
            {
                var newrow = new List<Marker>();
                for (int col = 0; col < Dimensions.Width; ++col)
                {
                    newrow.Add(new Marker());
                }
                Markers.Add(newrow);
            }
        }

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
                    line = line + marker;

                result.Add(line);
            }

            return result;
        }
    }
}
