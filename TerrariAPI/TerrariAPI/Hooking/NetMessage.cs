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
        internal NetMessage()
            : base((Type)null)
        {
        }
        /// <summary>
        /// Invokes NetMessage.SendData.
        /// </summary>
        public void SendData(int msgType, string str = "", int n = 0, float n2 = 0, float n3 = 0, float n4 = 0, int n5 = 0)
        {
            Invoke("SendData", msgType, -1, -1, str, n, n2, n3, n4, n5);
        }
    }
}
