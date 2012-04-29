using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.Lighting wrapper.
    /// </summary>
    public sealed class Lighting : Wrapper
    {
        internal static Lighting instance;

        /// <summary>
        /// Gets or sets the negLight field.
        /// </summary>
        public static float negLight { get { return instance.Get("negLight"); } set { instance.Set("negLight", value); } }
        /// <summary>
        /// Gets or sets the wetLightR field.
        /// </summary>
        public static float wetLightR { get { return instance.Get("wetLightR"); } set { instance.Set("wetLightR", value); } }
        /// <summary>
        /// Gets or sets the negLight2 field.
        /// </summary>
        public static float negLight2 { get { return instance.Get("negLight2"); } set { instance.Set("negLight2", value); } }

        internal Lighting()
            : base((Type)null)
        {
        }

        /// <summary>
        /// Lights a tile.
        /// </summary>
        /// <param name="X">X coordinate of the tile.</param>
        /// <param name="Y">Y coordinate of the tile.</param>
        /// <param name="light">Light intensity.</param>
        public static void LightTile(int X, int Y, float light = 1.0f)
        {
            instance.Invoke("addLight", X, Y, light);
        }
        /// <summary>
        /// Lights a tile.
        /// </summary>
        /// <param name="X">X coordinate of the tile.</param>
        /// <param name="Y">Y coordinate of the tile.</param>
        /// <param name="R">R component of the color.</param>
        /// <param name="G">G component of the color.</param>
        /// <param name="B">B component of the color.</param>
        public static void LightTile(int X, int Y, float R, float G, float B)
        {
            instance.Invoke("addLight", X, Y, R, G, B);
        }
    }
}
