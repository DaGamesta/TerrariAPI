using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.Player wrapper.
    /// </summary>
    public sealed class Player : Wrapper
    {
        internal static Player instance;

        /// <summary>
        /// Gets or sets the tile X range.
        /// </summary>
        public static int tileRangeX { get { return instance.Get("tileRangeX"); } set { instance.Set("tileRangeX", value); } }
        /// <summary>
        /// Gets or sets the tile Y range.
        /// </summary>
        public static int tileRangeY { get { return instance.Get("tileRangeY"); } set { instance.Set("tileRangeY", value); } }

        internal Player()
            : base((Type)null)
        {
        }
    }
}
