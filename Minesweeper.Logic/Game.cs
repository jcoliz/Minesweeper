using System;
using System.Drawing;

namespace Minesweeper.Logic
{
    /// <summary>
    /// Contains the game logic for a game of Minesweeper!
    /// </summary>
    public class Game
    {
        #region Public Properties
        /// <summary>
        /// The universe of things which can happen after we make a move
        /// </summary>
        public enum PlayResult { Invalid = 0, Continue, GameOver, Victory };

        /// <summary>
        /// Internal storage of markers
        /// </summary>
        public Board<Marker> GameBoard { get; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Consntruct a new marker grid
        /// </summary>
        /// <param name="numcols">How many columns</param>
        /// <param name="numrows_optional">How many rows, or leave out for 'same as numcols'</param>
        /// <param name="numbombs_optional">How many bombs, or leave out for 'same as numcols'</param>
        public Game(int numcols, int? numrows_optional = null, int? numbombs_optional = null)
        {
            // Resolve optional parameters

            var numrows = numcols;
            if (numrows_optional.HasValue)
                numrows = numrows_optional.Value;
            int bombs = numcols;
            if (numbombs_optional.HasValue)
                bombs = numbombs_optional.Value;

            // Set up the empty playing surface

            GameBoard = new Board<Marker>(new Size(numcols, numrows));

            // Place bombs

            while (bombs-- > 0)
            {
                // Find a random place where there is not already a bomb
                int atrow;
                int atcol;
                do
                {
                    atrow = RandomGenerator.Next(GameBoard.Dimensions.Height);
                    atcol = RandomGenerator.Next(GameBoard.Dimensions.Width);
                }
                while (GameBoard.Markers[atrow][atcol].isBomb);

                // Place the bomb
                GameBoard.Markers[atrow][atcol].isBomb = true;
            }
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

            if (row < 0 || row >= GameBoard.Dimensions.Height || col < 0 || col >= GameBoard.Dimensions.Width)
                result = PlayResult.Invalid;
            else
            {
                var marker = GameBoard.Markers[row][col];

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
                        marker.NumNearbyBombs = CountBombsNear(row,col);

                        if (isVictoryConditionMet())
                            result = PlayResult.Victory;
                    }
                }
            }
            return result;
        }
        #endregion

        #region Internal Properties
        private Random RandomGenerator = new Random(DateTime.Now.Millisecond);
        #endregion

        #region Internal Methods
        private bool isVictoryConditionMet()
        {
            bool victory = true;

            foreach (var row in GameBoard.Markers)
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

        private int CountBombsNear(int row, int col)
        {
            // Count up the number of nearby bombs
            int foundbombs = 0;
            for (int rowdelta = -1; rowdelta < 2; rowdelta++)
            {
                for (int coldelta = -1; coldelta < 2; coldelta++)
                {
                    int lookrow = row + rowdelta;
                    int lookcol = col + coldelta;

                    if (lookrow >= 0 && lookrow < GameBoard.Dimensions.Height && lookcol >= 0 && lookcol < GameBoard.Dimensions.Width)
                    {
                        if (GameBoard.Markers[lookrow][lookcol].isBomb)
                        {
                            ++foundbombs;
                        }
                    }
                }
            }

            return foundbombs;
        }
        #endregion
    }
}