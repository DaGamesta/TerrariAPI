using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.Item wrapper.
    /// </summary>
    public sealed class Item : Wrapper
    {
        internal Item()
            : base((Type)null)
        {
        }

        /// <summary>
        /// Generates a new item.
        /// </summary>
        public dynamic New()
        {
            return Activator.CreateInstance(type);
        }
    }
}
