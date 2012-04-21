using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using TerrariAPI.Hooking;

namespace TerrariAPI.Plugins
{
    /// <summary>
    /// Represents plugin event data.
    /// </summary>
    public sealed class PluginEventArgs : EventArgs
    {
        /// <summary>
        /// The assembly.
        /// </summary>
        public AssemblyDefinition asm;
        /// <summary>
        /// The content manager.
        /// </summary>
        public readonly ContentManager content;
        /// <summary>
        /// The sprite batch.
        /// </summary>
        public readonly SpriteBatch spriteBatch;

        internal PluginEventArgs()
        {
            switch (Client.state)
            {
                case State.CONTENT:
                    content = Client.game.Content;
                    break;
                case State.DRAW:
                    spriteBatch = Wrapper.main.spriteBatch;
                    break;
                case State.HOOK:
                    asm = Hooks.asm;
                    break;
            }
        }
    }
}
