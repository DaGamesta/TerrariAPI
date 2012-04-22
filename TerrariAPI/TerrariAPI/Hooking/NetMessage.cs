using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a Terraria.NetMessage wrapper.
    /// </summary>
    public sealed class NetMessage : Wrapper
    {
        internal static NetMessage instance;

        internal NetMessage()
            : base((Type)null)
        {
        }
        /// <summary>
        /// Invokes the NetMessage.SendData method.
        /// </summary>
        public static void SendData(int msgType, string str = "", int n = 0, float n2 = 0, float n3 = 0, float n4 = 0, int n5 = 0)
        {
            instance.Invoke("SendData", msgType, -1, -1, str, n, n2, n3, n4, n5);
        }
        /// <summary>
        /// Invokes the NetMessage.SendTileSquare method.
        /// </summary>
        /// <param name="X">X position of the center tile.</param>
        /// <param name="Y">Y position of the center tile.</param>
        /// <param name="size">Radius of the region, times two.</param>
        public static void SendTileSquare(int X, int Y, int size)
        {
            SendData(20, "", size, X - (size - 1) / 2, Y - (size - 1) / 2);
        }
        /// <summary>
        /// Invokes the NetMessage.SendWater method.
        /// </summary>
        /// <param name="X">X position of the tile.</param>
        /// <param name="Y">Y position of the tile.</param>
        public static void SendWater(int X, int Y)
        {
            SendData(48, "", X, Y);
        }
    }
}
