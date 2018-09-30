using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Minesweeper.Logic
{
    public class GameBoard
    {
        public Size Dimensions { get; }
        public List<List<Marker>> Markers { get; }

        public GameBoard(Size desireddimensions)
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
    }

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
        protected GameBoard Board;

        /// <summary>
        /// Consntruct a new marker grid
        /// </summary>
        /// <param name="numcols">How many columns</param>
        /// <param name="numrows">How many rows, or leave out for 'same as numcols'</param>
        /// <param name="numbombs">How many bombs, or leave out for 'same as numcols'</param>
        public MarkerGrid(int numcols, int? numrows = null, int? numbombs = null)
        {
            var ColSize = numcols;

            var RowSize = numcols;
            if (numrows.HasValue)
                RowSize = numrows.Value;

            Board = new GameBoard(new Size(ColSize, RowSize));

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
                    atrow = RandomGenerator.Next(RowSize);
                    atcol = RandomGenerator.Next(ColSize);
                }
                while (Board.Markers[atrow][atcol].isBomb);

                // Place the bomb
                Board.Markers[atrow][atcol].isBomb = true;
            }
        }

        /// <summary>
        /// Render grid in a display-friendly way
        /// </summary>
        /// <returns></returns>
        public string[] Render()
        {
            List<string> result = new List<string>();

            foreach(var row in Board.Markers)
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

            if (row < 0 || row >= Board.Dimensions.Height || col < 0 || col >= Board.Dimensions.Width)
                result = PlayResult.Invalid;
            else
            {
                var marker = Board.Markers[row][col];

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
                        marker.NumNearbyBombs = BombsNearPosition(row,col);

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

            foreach (var row in Board.Markers)
            {
                foreach (var marker in row)
                {
                    if (! marker.isBomb && ! marker.isShowing)
                    {
                        victory = false;
                    }
                }
            }

            return victory;
        }

        protected int BombsNearPosition(int row, int col)
        {
            // Count up the number of nearby bombs
            int foundbombs = 0;
            for (int rowdelta = -1; rowdelta < 2; rowdelta++)
            {
                for (int coldelta = -1; coldelta < 2; coldelta++)
                {
                    int lookrow = row + rowdelta;
                    int lookcol = col + coldelta;

                    if (lookrow >= 0 && lookrow < Board.Dimensions.Height && lookcol >= 0 && lookcol < Board.Dimensions.Width)
                    {
                        if (Board.Markers[lookrow][lookcol].isBomb)
                        {
                            ++foundbombs;
                        }
                    }
                }
            }

            return foundbombs;
        }
    }
}