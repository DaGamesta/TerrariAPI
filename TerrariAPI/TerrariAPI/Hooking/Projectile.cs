using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.Projectile wrapper.
    /// </summary>
    public sealed class Projectile : Wrapper
    {
        internal static Projectile instance;

        internal Projectile()
            : base((Type)null)
        {
        }

        /// <summary>
        /// Generates a new projectile.
        /// </summary>
        public static dynamic New()
        {
            return Activator.CreateInstance(instance.type);
        }
    }
}
