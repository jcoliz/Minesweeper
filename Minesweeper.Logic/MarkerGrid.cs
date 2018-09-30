using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.Logic
{
    public class MarkerGrid
    {
        public enum PlayResult { Invalid = 0, Continue, GameOver, Victory };

        /// <summary>
        /// Internal storage of markers
        /// </summary>
        /// <remarks>
        /// Protected so that test code can subclass and inspect.
        /// Outer list items are rows, inner list items are cols
        /// </remarks>
        protected Marker[,] MarkerStore;

        /// <summary>
        /// Consntruct a new marker grid
        /// </summary>
        /// <param name="numcols">How many columns</param>
        /// <param name="numrows">How many rows, or leave out for 'same as numcols'</param>
        /// <param name="numbombs">How many bombs, or leave out for 'same as numcols'</param>
        public MarkerGrid(int numcols, int? numrows = null, int? numbombs = null)
        {
            int colsize = numcols;

            int rowsize = numcols;
            if (numrows.HasValue)
                rowsize = numrows.Value;

            MarkerStore = new Marker[rowsize,colsize];

            for (int row = 0; row < numrows; ++row)
            {
                for (int col = 0; col < numcols; ++col)
                {
                    MarkerStore[row, col] = new Marker();
                }
            }

        }

        /// <summary>
        /// Render grid in a display-friendly way
        /// </summary>
        /// <returns></returns>
        public string[] Render()
        {
            int numrows = MarkerStore.GetLength(0);
            int numcols = MarkerStore.GetLength(1);

            string[] result = new string[numrows];

            for(int row = 0; row < numrows; ++row)
            {
                string line = string.Empty;

                for (int col = 0; col < numcols; ++col)
                {
                    var markerrender = MarkerStore[row, col].ToString();

                    line = line + markerrender;
                }

                result[row] = line;
            }

            return result;
        }

        /// <summary>
        /// User plays at this position
        /// </summary>
        /// <param name="col">which column, from 0</param>
        /// <param name="row">which row, from 0</param>
        /// <returns>Result of playing at this position</returns>
        public PlayResult PlayAt(int col, int row)
        {
            PlayResult result = PlayResult.Continue;

            int numrows = MarkerStore.GetLength(0);
            int numcols = MarkerStore.GetLength(1);

            if (row < 0 || row >= numrows || col < 0 || col >= numcols )
                result = PlayResult.Invalid;
            else if (MarkerStore[row, col].isShowing)
                result = PlayResult.Invalid;
            else
            {
                // Reveal this marker
                MarkerStore[row, col].isShowing = true;

                if (MarkerStore[row, col].isBomb)
                    result = PlayResult.GameOver;
                else
                {
                    // Search for the other bombs
                    int foundbombs = 0;
                    for (int rowdelta = -1; rowdelta < 2; rowdelta++)
                    {
                        for (int coldelta = -1; coldelta < 2; coldelta++)
                        {
                            int lookrow = row + rowdelta;
                            int lookcol = col + coldelta;

                            if (lookrow >= 0 && lookrow < numrows && lookcol >= 0 && lookcol < numcols)
                            {
                                if (MarkerStore[lookrow, lookcol].isBomb)
                                {
                                    ++foundbombs;
                                }
                            }
                        }
                    }
                    MarkerStore[row, col].NumNearbyBombs = foundbombs;

                    // Test for victory
                    bool victory = true;
                    for (int rowcheck = 0; rowcheck < numrows; rowcheck++)
                    {
                        for (int colcheck = 0; colcheck < numcols; colcheck++)
                        {
                            if (! MarkerStore[rowcheck,colcheck].isBomb && ! MarkerStore[rowcheck, colcheck].isShowing)
                            {
                                victory = false;
                            }
                        }
                    }
                    if (victory)
                        result = PlayResult.Victory;
                }
            }

            return result;
        }
    }
}
