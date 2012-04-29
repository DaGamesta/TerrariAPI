using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.Tile wrapper.
    /// </summary>
    public sealed class Tile : Wrapper
    {
        internal static Tile instance;

        internal Tile()
            : base((Type)null)
        {
        }

        /// <summary>
        /// Creates a new tile.
        /// </summary>
        public static dynamic New()
        {
            return Activator.CreateInstance(instance.type);
        }
    }
}
