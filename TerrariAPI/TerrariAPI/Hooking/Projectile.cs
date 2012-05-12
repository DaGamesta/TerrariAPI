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
        /// <summary>
        /// Creates a new projectile.
        /// </summary>
        /// <param name="X">X position for the new projectile.</param>
        /// <param name="Y">Y position for the new projectile.</param>
        /// <param name="XVelocity">X velocity for the new projectile.</param>
        /// <param name="YVelocity">Y velocity for the new projectile.</param>
        /// <param name="type">Type for the new projectile.</param>
        /// <param name="damage">Damage for the new projectile.</param>
        /// <param name="knockback">Knockback for the new projectile.</param>
        /// <returns></returns>
        public static int NewProjectile(float X, float Y, float XVelocity, float YVelocity, int type, int damage, float knockback = 0)
        {
            return instance.Invoke("NewProjectile", X, Y, XVelocity, YVelocity, type, damage, knockback, Main.playerIndex);
        }
    }
}
