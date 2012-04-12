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
        /// <summary>
        /// Gets the current player.
        /// </summary>
        public dynamic currPlayer { get { return ((dynamic[])Get("player"))[(int)Get("myPlayer")]; } }
        /// <summary>
        /// Gets the array of items.
        /// </summary>
        public dynamic[] items { get { return (dynamic[])Get("item"); } }
        /// <summary>
        /// Gets if the game is in single player or not.
        /// </summary>
        public bool singlePlayer { get { return Get("netMode") == 0 && !Get("gameMenu"); } }
        internal SpriteBatch spriteBatch { get { return (SpriteBatch)Get("spriteBatch"); } }
        /// <summary>
        /// Gets the array of tiles.
        /// </summary>
        public dynamic[,] tiles { get { return (dynamic[,])Get("tile"); } }

        internal Main(object obj)
            : base(obj)
        {
        }
    }
}
