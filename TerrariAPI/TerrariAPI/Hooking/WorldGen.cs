using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.WorldGen wrapper.
    /// </summary>
    public sealed class WorldGen : Wrapper
    {
        internal static WorldGen instance;

        internal WorldGen()
            : base((Type)null)
        {
        }
        /// <summary>
        /// Invokes the SquareTileFrame method.
        /// </summary>
        /// <param name="X">X position of the tile.</param>
        /// <param name="Y">Y position of the tile.</param>
        /// <param name="resetFrame">Reset frames or not.</param>
        public static void SquareTileFrame(int X, int Y, bool resetFrame = true)
        {
            instance.Invoke("SquareTileFrame", X, Y, resetFrame);
        }
        /// <summary>
        /// Invokes the SquareWallFrame method.
        /// </summary>
        /// <param name="X">X position of the wall.</param>
        /// <param name="Y">Y position of the wall.</param>
        /// <param name="resetFrame">Reset frames or not.</param>
        public static void SquareWallFrame(int X, int Y, bool resetFrame = true)
        {
            instance.Invoke("SquareWallFrame", X, Y, resetFrame);
        }
    }
}
