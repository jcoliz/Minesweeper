using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
                Point position;
                do
                {
                    position = new Point(
                        RandomGenerator.Next(GameBoard.Dimensions.Width),
                        RandomGenerator.Next(GameBoard.Dimensions.Height)
                    );
                }
                while (GameBoard[position].isBomb);

                // Place the bomb
                GameBoard[position].isBomb = true;
            }
        }

        /// <summary>
        /// User plays at this position
        /// </summary>
        /// <param name="position">Position where to play, (col,row) 0-based</param>
        /// <returns>Result of playing at this position</returns>
        public PlayResult PlayAt(Point position)
        {
            PlayResult result = PlayResult.Continue;

            if (!GameBoard.Dimensions.Contains(position))
            {
                result = PlayResult.Invalid;
            }
            else
            {
                var marker = GameBoard[position];

                if (marker.isShowing)
                {
                    // Can't play at a position which is already showing its marker
                    result = PlayResult.Invalid;
                }
                else
                {
                    // Reveal this marker
                    marker.isShowing = true;

                    // So did we die??
                    if (marker.isBomb)
                    {
                        result = PlayResult.GameOver;
                    }
                    else
                    {
                        marker.NumNearbyBombs = CountBombsNear(position);

                        // If this space is empty, recursively play all still-hidden spaces around me
                        if (0 == marker.NumNearbyBombs)
                            PlayAllNear(position);

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
            // Victory is met when every square is either showing or has a bomb
            return 0 == GameBoard.Dimensions.GetEnumerator().Select(p=>GameBoard[p]).Where(m => !m.isShowing && !m.isBomb).Count();
        }

        private int CountBombsNear(Point center)
        {
            // Check in the area immediately surrounding the center (1 away in each direction)
            var nearby = center.Inflate(new Size(1, 1));
            nearby.Intersect(GameBoard.Dimensions);

            // Count up the number of bombs nearby
            return nearby.GetEnumerator().Where(p => GameBoard[p].isBomb).Count();
        }

        private void PlayAllNear(Point center)
        {
            // play the area immediately surrounding the center (1 away in each direction)
            var nearby = center.Inflate(new Size(1, 1));
            nearby.Intersect(GameBoard.Dimensions);

            // Play each nearby item where it's not showing
            nearby.GetEnumerator().Where(p => !GameBoard[p].isShowing).ToList().ForEach(p => PlayAt(p));
        }
        #endregion
    }
    public static class Extensions
    {
        public static IEnumerable<Point> GetEnumerator(this Rectangle r)
        {
            for (int x = r.X; x < r.Right; x++)
            {
                for (int y = r.Y; y < r.Bottom; y++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        public static Rectangle Inflate(this Point p, Size s)
        {
            var result = new Rectangle(p, new Size(1, 1));
            result.Inflate(s);
            return result;
        }
    }
}