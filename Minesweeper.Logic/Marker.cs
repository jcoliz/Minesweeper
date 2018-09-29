using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.Logic
{
    public class Marker
    {
        /// <summary>
        /// Whether there is a bomb at this marker
        /// </summary>
        public bool isBomb { get; set; }

        /// <summary>
        /// Whether the marker is showing its contents to the user (true) or hidden (false)
        /// </summary>
        public bool isShowing { get; set; }

        /// <summary>
        /// How many neighbors have bombs
        /// </summary>
        public int NumNearbyBombs { get; set; }
    }
}
