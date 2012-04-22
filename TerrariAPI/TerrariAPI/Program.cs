using System;
using System.IO;
using TerrariAPI.Hooking;
using TerrariAPI.Plugins;

namespace TerrariAPI
{
    internal static class Program
    {
        [STAThread]
        internal static void Main(string[] args)
        {
            try
            {
                if (!File.Exists("Terraria.exe"))
                {
                    throw new FileNotFoundException("Terraria executable could not be found.");
                }
                Plugin.Load();
                Hooks.Start();
            }
            catch (Exception e)
            {
                using (StreamWriter writer = new StreamWriter("crash.log", true))
                {
                    writer.WriteLine("[{0}]:\n{1}\n", DateTime.Now, e);
                }
            }
            Plugin.Unload();
        }
    }
}

