using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Holds all of the game hooks.
    /// </summary>
    public static class GameHooks
    {
        /// <summary>
        /// Fires when the API loads its content.
        /// </summary>
        public static event Action<ContentManager> Content;
        /// <summary>
        /// Fires when the API is drawn.
        /// </summary>
        public static event Action<SpriteBatch> Draw;
        /// <summary>
        /// Fires when the API hooks.
        /// </summary>
        public static event Action<AssemblyDefinition> Hook;
        /// <summary>
        /// Fires when the API is initialized.
        /// </summary>
        public static event Action Initialize;
        /// <summary>
        /// Fires when the API is updated.
        /// </summary>
        public static event Action Update;
        /// <summary>
        /// Fires when the API is updated, after the original client updates.
        /// </summary>
        public static event Action Update2;

        internal static void OnContent()
        {
            if (Content != null)
            {
                Content.Invoke(Client.Game.Content);
            }
        }
        internal static void OnDraw()
        {
            if (Draw != null)
            {
                Draw.Invoke(Main.spriteBatch);
            }
        }
        internal static void OnHook()
        {
            if (Hook != null)
            {
                Hook.Invoke(Hooks.asm);
            }
        }
        internal static void OnInitialize()
        {
            if (Initialize != null)
            {
                Initialize.Invoke();
            }
        }
        internal static void OnUpdate()
        {
            if (Update != null)
            {
                Update.Invoke();
            }
        }
        internal static void OnUpdate2()
        {
            if (Update2 != null)
            {
                Update2.Invoke();
            }
        }
    }
}
