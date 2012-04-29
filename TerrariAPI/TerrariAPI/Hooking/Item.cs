using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.Item wrapper.
    /// </summary>
    public sealed class Item : Wrapper
    {
        internal static Item instance;

        internal Item()
            : base((Type)null)
        {
        }

        /// <summary>
        /// Generates a new item.
        /// </summary>
        public static dynamic New()
        {
            return Activator.CreateInstance(instance.type);
        }
        /// <summary>
        /// Creates an item.
        /// </summary>
        /// <param name="X">X position for the item.</param>
        /// <param name="Y">Y position for the item.</param>
        /// <param name="width">Width of what receives the item.</param>
        /// <param name="height">Height of what receives the item.</param>
        /// <param name="type">Type for the item.</param>
        /// <param name="stack">Stack for the item.</param>
        /// <param name="broadcast">Whether or not to broadcast the item creation.</param>
        /// <param name="prefix">Prefix for the item.</param>
        public static int NewItem(int X, int Y, int width, int height, int type, int stack = 1, bool broadcast = false, int prefix = 0)
        {
            return instance.Invoke("NewItem", X, Y, width, height, type, stack, broadcast, prefix);
        }
    }
}
