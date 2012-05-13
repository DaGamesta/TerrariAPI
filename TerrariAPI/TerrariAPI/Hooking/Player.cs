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

        internal Player()
            : base((Type)null)
        {
        }
    }
}
