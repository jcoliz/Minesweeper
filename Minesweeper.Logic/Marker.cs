namespace Minesweeper.Logic
{
    /// <summary>
    /// A single position on a minesweeper game board
    /// </summary>
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

        public override string ToString()
        {
            if (isShowing)
            {
                if (isBomb)
                    return "@";
                else if (NumNearbyBombs > 0)
                    return NumNearbyBombs.ToString();
                else
                    return " ";
            }
            else
                return "#";
        }
    }
}
