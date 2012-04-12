using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.Main wrapper.
    /// </summary>
    public sealed class Main : Wrapper
    {
        internal SpriteBatch spriteBatch { get { return (SpriteBatch)Get("spriteBatch"); } }

        internal Main(object obj)
            : base(obj)
        {
        }
    }
}
