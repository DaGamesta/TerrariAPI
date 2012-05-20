using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using TerrariAPI.Commands;
using TerrariAPI.Hooking;

namespace TerrariAPI
{
    /// <summary>
    /// Represents a plugin.
    /// </summary>
    public abstract class Plugin : IDisposable
    {
        internal static List<Plugin> plugins = new List<Plugin>();
        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        public abstract string author { get; }
        /// <summary>
        /// Gets if keys are to be disabled.
        /// </summary>
        protected bool disableKeys { get { return Client.DisableKeys; } }
        /// <summary>
        /// Gets if the mouse is to be disabled.
        /// </summary>
        protected bool disableMouse { get { return Client.DisableMouse; } }
        internal string fileName;
        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public abstract string name { get; }
        internal string version;

        /// <summary>
        /// Adds a command.
        /// </summary>
        protected void AddCommand(Command command)
        {
            Command.Add(command);
        }
        /// <summary>
        /// Prints a message in a specified color.
        /// </summary>
        protected void Print(string str, Color color)
        {
            Client.Print(str, color);
        }
        /// <summary>
        /// Prints an error.
        /// </summary>
        protected void PrintError(string str)
        {
            Client.PrintError(str);
        }
        /// <summary>
        /// Prints a notification.
        /// </summary>
        protected void PrintNotification(string str)
        {
            Client.PrintNotification(str);
        }

        /// <summary>
        /// The disposing mechanism. (DO NOT USE)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// The disposing mechanism.
        /// </summary>
        public abstract void Dispose(bool disposing);
        internal static void Load()
        {
            if (!Directory.Exists("Plugins"))
            {
                Directory.CreateDirectory("Plugins");
            }
            foreach (string fName in Directory.EnumerateFiles("Plugins", "*.dll"))
            {
                LoadFrom(fName);
            }
        }
        internal static void LoadFrom(string path, bool noCopy = false)
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(path);
                foreach (Type t in asm.GetTypes())
                {
                    if (typeof(Plugin).IsAssignableFrom(t) && t.GetConstructor(new Type[] { }) != null)
                    {
                        if (!noCopy)
                        {
                            File.Delete(path.Substring(path.LastIndexOf('\\') + 1));
                            File.Copy(path, path.Substring(path.LastIndexOf('\\') + 1));
                            LoadFrom(path.Substring(path.LastIndexOf('\\') + 1), true);
                        }
                        else
                        {
                            Plugin p = (Plugin)Activator.CreateInstance(t);
                            p.fileName = path.Substring(path.LastIndexOf('\\') + 1);
                            p.version = asm.GetName().Version.Major + "." + asm.GetName().Version.Minor;
                            plugins.Add(p);
                        }
                    }
                }
            }
            catch (BadImageFormatException)
            {
            }
        }
        /// <summary>
        /// Creates the string representation of the plugin.
        /// </summary>
        public override string ToString()
        {
            return name + " v" + version;
        }
    }
}
