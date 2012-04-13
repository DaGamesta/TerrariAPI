using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Commands
{
    /// <summary>
    /// Represents a description attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// The description.
        /// </summary>
        public readonly string description;
        /// <summary>
        /// Creates a new description attribute.
        /// </summary>
        public DescriptionAttribute(string description)
        {
            this.description = description;
        }
    }
}
