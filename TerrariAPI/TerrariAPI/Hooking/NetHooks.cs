using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Holds all of the networking hooks.
    /// </summary>
    public static class NetHooks
    {
        /// <summary>
        /// Fires when the NetMessage.GetData() method is invoked.
        /// </summary>
        public static event Action<byte[]> GetData;
        /// <summary>
        /// Fires when the NetMessage.SendData() method is invoked.
        /// </summary>
        public static event Action<byte, string, int, float, float, float, int> SendData;

        internal static void OnGetData(byte[] data)
        {
            if (SendData != null)
            {
                GetData.Invoke(data);
            }
        }
        internal static void OnSendData(byte msg, string str, int n, float n2, float n3, float n4, int n5)
        {
            if (SendData != null)
            {
                SendData.Invoke(msg, str, n, n2, n3, n4, n5);
            }
        }
    }
}
