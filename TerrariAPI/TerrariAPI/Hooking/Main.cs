using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.Main wrapper.
    /// </summary>
    public sealed class Main : Wrapper
    {
        internal static Main instance;
        /// <summary>
        /// Gets the buff name array.
        /// </summary>
        public static string[] buffNames { get { return instance.Get("buffName"); } }
        /// <summary>
        /// Gets the string representation of the down control.
        /// </summary>
        public static string cDown { get { return instance.Get("cDown"); } }
        /// <summary>
        /// Gets the string representation of the left control.
        /// </summary>
        public static string cLeft { get { return instance.Get("cLeft"); } }
        internal static bool chatMode { get { return instance.Get("chatMode"); } }
        /// <summary>
        /// Gets the string representation of the right control.
        /// </summary>
        public static string cRight { get { return instance.Get("cRight"); } }
        /// <summary>
        /// Gets the string representation of the up control.
        /// </summary>
        public static string cUp { get { return instance.Get("cUp"); } }
        /// <summary>
        /// Gets the currently selected item.
        /// </summary>
        public static dynamic currItem { get { return currPlayer.inventory[currPlayer.selectedItem]; } }
        /// <summary>
        /// Gets the current player.
        /// </summary>
        public static dynamic currPlayer { get { return instance.Get("player")[(int)instance.Get("myPlayer")]; } }
        internal static Texture2D cursorTexture { set { instance.Set("cursorTexture", value); } }
        /// <summary>
        /// Gets the debuff array.
        /// </summary>
        public static bool[] debuff { get { return instance.Get("debuff"); } }
        internal static bool inputTextEnter { set { instance.Set("inputTextEnter", value); } }
        /// <summary>
        /// Gets the array of item names.
        /// </summary>
        public static string[] itemNames { get { return instance.Get("itemName"); } }
        /// <summary>
        /// Gets the array of items.
        /// </summary>
        public static dynamic[] items { get { return instance.Get("item"); } }
        internal static KeyboardState keyState { set { instance.Set("keyState", value); } }
        internal static MouseState mouseState { set { instance.Set("mouseState", value); } }
        /// <summary>
        /// Gets if the game is in multiplayer or not.
        /// </summary>
        public static bool multiplayer { get { return instance.Get("netMode") == 1 && !instance.Get("gameMenu"); } }
        internal static MouseState oldMouseState { set { instance.Set("oldMouseState", value); } }
        /// <summary>
        /// Gets the current player index.
        /// </summary>
        public static int playerIndex { get { return instance.Get("myPlayer"); } }
        /// <summary>
        /// Gets the array of players.
        /// </summary>
        public static dynamic[] players { get { return instance.Get("player"); } }
        /// <summary>
        /// Gets the screen size.
        /// </summary>
        public static Vector2 screenSize { get { return new Vector2(Client.Game.Window.ClientBounds.Width, Client.Game.Window.ClientBounds.Height); } }
        /// <summary>
        /// Gets the screen position.
        /// </summary>
        public static Vector2 screenPosition { get { return instance.Get("screenPosition"); } }
        /// <summary>
        /// Gets if the game is in single player or not.
        /// </summary>
        public static bool singleplayer { get { return instance.Get("netMode") == 0 && !instance.Get("gameMenu"); } }
        internal static SpriteBatch spriteBatch { get { return (SpriteBatch)instance.Get("spriteBatch"); } }
        /// <summary>
        /// Gets the array of team colors.
        /// </summary>
        public static Color[] teamColors { get { return instance.Get("teamColor"); } }
        /// <summary>
        /// Gets the array of tiles.
        /// </summary>
        public static dynamic[,] tiles { get { return instance.Get("tile"); } }

        internal Main(object obj)
            : base(obj)
        {
        }

        internal static void Dispose()
        {
            instance.Invoke("Dispose");
        }
        internal static void Run()
        {
            instance.Invoke("Run");
        }
    }
}
