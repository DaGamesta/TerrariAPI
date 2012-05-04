using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Commands
{
    /// <summary>
    /// Represents a match found by searching for something by string (such as projectile, player, etc).
    /// </summary>
    public sealed class Match
    {
        /// <summary>
        /// ID to match.
        /// </summary>
        public int ID;
        /// <summary>
        /// Name to match.
        /// </summary>
        public string name;

        internal Match(int ID, string name)
        {
            this.ID = ID;
            this.name = name;
        }
    }
}
