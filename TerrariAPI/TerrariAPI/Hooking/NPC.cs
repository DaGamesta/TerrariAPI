using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.NPC wrapper.
    /// </summary>
    public sealed class NPC : Wrapper
    {
        internal static NPC instance;

        internal NPC()
            : base((Type)null)
        {
        }

        /// <summary>
        /// Generates a new NPC.
        /// </summary>
        public static dynamic New()
        {
            return Activator.CreateInstance(instance.type);
        }
    }
}
