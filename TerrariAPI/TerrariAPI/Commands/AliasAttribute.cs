using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Commands
{
    /// <summary>
    /// Represents an alias attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class AliasAttribute : Attribute
    {
        /// <summary>
        /// The aliases.
        /// </summary>
        public readonly string[] aliases;
        /// <summary>
        /// Creates a new alias attribute.
        /// </summary>
        public AliasAttribute(params string[] aliases)
        {
            for (int i = 0; i < aliases.Length; i++)
            {
                aliases[i] = aliases[i].ToLower();
            }
            this.aliases = aliases;
        }
    }
}
