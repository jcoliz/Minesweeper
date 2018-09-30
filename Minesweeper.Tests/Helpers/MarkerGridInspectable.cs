using Minesweeper.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.Tests.Helpers
{
    public class MarkerGridInspectable: Game
    {
        /// <summary>
        /// Inspectable storage of markers
        /// </summary>
        /// <remarks>
        /// Exposed so that test code can subclass and inspect
        /// </remarks>
        public List<List<Marker>> Markers => base.GameBoard.Markers;

        /// <summary>
        /// Consntruct a new marker grid
        /// </summary>
        /// <param name="numcols">How many columns</param>
        /// <param name="numrows">How many rows, or leave out for 'same as numcols'</param>
        /// <param name="numbombs">How many bombs, or leave out for 'same as numcols'</param>
        public MarkerGridInspectable(int numcols, int? numrows = null, int? numbombs = null): base(numcols,numrows,numbombs)
        {
        }
    }
}
