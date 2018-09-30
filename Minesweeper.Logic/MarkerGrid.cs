using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.Logic
{
    public class MarkerGrid
    {
        public enum PlayResult { Invalid = 0, Continue, GameOver, Victory };

        private Random RandomGenerator = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Internal storage of markers
        /// </summary>
        /// <remarks>
        /// Protected so that test code can subclass and inspect.
        /// Outer list items are rows, inner list items are cols
        /// </remarks>
        protected List<List<Marker>> MarkerStore;

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

            MarkerStore = new List<List<Marker>>();

            for (int row = 0; row < rowsize; ++row)
            {
                var newrow = new List<Marker>();
                for (int col = 0; col < colsize; ++col)
                {
                    newrow.Add(new Marker());
                }
                MarkerStore.Add(newrow);
            }

            int bombs = numcols;
            if (numbombs.HasValue)
                bombs = numbombs.Value;

            while(bombs-- > 0)
            {
                // Make sure there is not already a bomb here.
                int atrow;
                int atcol;
                do
                {
                    atrow = RandomGenerator.Next(rowsize);
                    atcol = RandomGenerator.Next(colsize);
                }
                while (MarkerStore[atrow][atcol].isBomb);

                // Place the bomb
                MarkerStore[atrow][atcol].isBomb = true;
            }
        }

        /// <summary>
        /// Render grid in a display-friendly way
        /// </summary>
        /// <returns></returns>
        public string[] Render()
        {
            int numrows = MarkerStore.Count;
            int numcols = MarkerStore[0].Count;

            List<string> result = new List<string>();

            foreach(var row in MarkerStore)
            {
                string line = string.Empty;

                foreach(var marker in row)
                    line = line + marker;

                result.Add(line);
            }

            return result.ToArray();
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

            int numrows = MarkerStore.Count;
            int numcols = MarkerStore[0].Count;

            if (row < 0 || row >= numrows || col < 0 || col >= numcols)
                result = PlayResult.Invalid;
            else
            {
                var marker = MarkerStore[row][col];

                if (marker.isShowing)
                    result = PlayResult.Invalid;
                else
                {
                    // Reveal this marker
                    marker.isShowing = true;

                    // So did we die??
                    if (marker.isBomb)
                        result = PlayResult.GameOver;
                    else
                    {
                        // Count up the number of nearby bombs
                        int foundbombs = 0;
                        for (int rowdelta = -1; rowdelta < 2; rowdelta++)
                        {
                            for (int coldelta = -1; coldelta < 2; coldelta++)
                            {
                                int lookrow = row + rowdelta;
                                int lookcol = col + coldelta;

                                if (lookrow >= 0 && lookrow < numrows && lookcol >= 0 && lookcol < numcols)
                                {
                                    if (MarkerStore[lookrow][lookcol].isBomb)
                                    {
                                        ++foundbombs;
                                    }
                                }
                            }
                        }
                        marker.NumNearbyBombs = foundbombs;

                        // Test for victory
                        if (isVictoryConditionMet())
                            result = PlayResult.Victory;
                    }
                }
            }
            return result;
        }

        protected bool isVictoryConditionMet()
        {
            bool victory = true;

            foreach (var r in MarkerStore)
            {
                foreach (var m in r)
                {
                    if (! m.isBomb && ! m.isShowing)
                    {
                        victory = false;
                    }
                }
            }

            return victory;
        }
    }
}