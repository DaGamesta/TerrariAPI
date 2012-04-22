using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Commands
{
    /// <summary>
    /// Represents an alert attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class AlertAttribute : Attribute
    {
        /// <summary>
        /// Creates a new alert attribute.
        /// </summary>
        public AlertAttribute()
        {
        }
    }
}
