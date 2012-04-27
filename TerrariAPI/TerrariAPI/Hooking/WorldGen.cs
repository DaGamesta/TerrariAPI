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
        /// Invokes the PlaceTile method.
        /// </summary>
        /// <param name="X">X position of the tile.</param>
        /// <param name="Y">Y position of the tile.</param>
        /// <param name="type">Type of the tile.</param>
        public static void PlaceTile(int X, int Y, byte type)
        {
            instance.Invoke("PlaceTile", X, Y, type, false, true, -1, 0);
        }
        /// <summary>
        /// Invokes the PlaceWall method.
        /// </summary>
        /// <param name="X">X position of the wall.</param>
        /// <param name="Y">Y position of the wall.</param>
        /// <param name="type">Type of the wall.</param>
        public static void PlaceWall(int X, int Y, int type)
        {
            instance.Invoke("PlaceWall", X, Y, type, false);
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
