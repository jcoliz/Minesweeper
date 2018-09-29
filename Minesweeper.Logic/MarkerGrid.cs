using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.Logic
{
    public class MarkerGrid
    {
        public enum PlayResult { Invalid = 0, Continue, GameOver, Victory };

        /// <summary>
        /// Consntruct a new marker grid
        /// </summary>
        /// <param name="numcols">How many columns</param>
        /// <param name="numrows">How many rows, or leave out for 'same as numcols'</param>
        /// <param name="numbombs">How many bombs, or leave out for 'same as numcols'</param>
        /// <param name="backingstore">Optional backing storage, useful for tests to inspect internal state</param>
        public MarkerGrid(int numcols, int? numrows = null, int? numbombs = null, List<List<Marker>> backingstore = null)
        {
        }

        /// <summary>
        /// Render grid in a display-friendly way
        /// </summary>
        /// <returns></returns>
        public string[] Render()
        {
            return new string[] {};
        }

        /// <summary>
        /// User plays at this position
        /// </summary>
        /// <param name="col">which column, from 0</param>
        /// <param name="row">which row, from 0</param>
        /// <returns>Result of playing at this position</returns>
        public PlayResult PlayAt(int col, int row)
        {
            return PlayResult.Continue;
        }
    }
}
