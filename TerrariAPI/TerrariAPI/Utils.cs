using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI
{
    /// <summary>
    /// Contains generic utility methods.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Swaps two items.
        /// </summary>
        /// <typeparam name="T">Type of item to swap.</typeparam>
        /// <param name="a">First item to swap.</param>
        /// <param name="b">Second item to swap.</param>
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
