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

namespace TerrariAPI.Plugins
{
    /// <summary>
    /// Represnts the methods that handle a plugin.
    /// </summary>
    public delegate void PluginEventHandler(object sender, PluginEventArgs e);
    /// <summary>
    /// Represents a plugin.
    /// </summary>
    public abstract class Plugin
    {
        internal static List<Plugin> plugins = new List<Plugin>();
        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        public abstract string author { get; }
        internal string fileName;
        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public abstract string name { get; }
        /// <summary>
        /// Fires when content is loaded.
        /// </summary>
        protected event PluginEventHandler onContent;
        /// <summary>
        /// Fires when drawn.
        /// </summary>
        protected event PluginEventHandler onDraw;
        /// <summary>
        /// Fires when hooking.
        /// </summary>
        protected event PluginEventHandler onHook;
        /// <summary>
        /// Fires when initialized.
        /// </summary>
        protected event PluginEventHandler onInitialize;
        /// <summary>
        /// Fires when unloaded.
        /// </summary>
        protected event PluginEventHandler onUnload;
        /// <summary>
        /// Fires when updated.
        /// </summary>
        protected event PluginEventHandler onUpdate;
        /// <summary>
        /// Gets the version of the plugin.
        /// </summary>
        public abstract int version { get; }

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

        internal static void Content()
        {
            foreach (Plugin p in plugins)
            {
                if (p.onContent != null)
                {
                    p.onContent.Invoke(p, new PluginEventArgs());
                }
            }
        }
        internal static void Draw()
        {
            foreach (Plugin p in plugins)
            {
                if (p.onDraw != null)
                {
                    p.onDraw.Invoke(p, new PluginEventArgs());
                }
            }
        }
        /// <summary>
        /// Gets a field definition.
        /// </summary>
        /// <param name="type">Name of the type the field is in.</param>
        /// <param name="field">Name of the field.</param>
        protected FieldDefinition GetField(string type, string field)
        {
            foreach (TypeDefinition td in Hooks.asm.MainModule.Types)
            {
                if (td.Name == type)
                {
                    foreach (FieldDefinition fd in td.Fields)
                    {
                        if (fd.Name == field)
                        {
                            return fd;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Gets a method definition.
        /// </summary>
        /// <param name="type">Name of the type the method is in.</param>
        /// <param name="method">Name of the method.</param>
        protected MethodDefinition GetMethod(string type, string method)
        {
            foreach (TypeDefinition td in Hooks.asm.MainModule.Types)
            {
                if (td.Name == type)
                {
                    foreach (MethodDefinition md in td.Methods)
                    {
                        if (md.Name == method)
                        {
                            return md;
                        }
                    }
                }
            }
            return null;
        }
        internal static void Hook()
        {
            foreach (Plugin p in plugins)
            {
                if (p.onHook != null)
                {
                    p.onHook.Invoke(p, new PluginEventArgs());
                }
            }
        }
        internal static void Initialize()
        {
            foreach (Plugin p in plugins)
            {
                if (p.onInitialize != null)
                {
                    p.onInitialize.Invoke(p, new PluginEventArgs());
                }
            }
        }
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
                        Plugin p = (Plugin)Activator.CreateInstance(t);
                        p.fileName = path.Substring(path.LastIndexOf('\\') + 1);
                        if (!noCopy && p.onHook != null)
                        {
                            File.Delete(p.fileName);
                            File.Copy(path, p.fileName);
                            LoadFrom(p.fileName, true);
                        }
                        else
                        {
                            plugins.Add(p);
                        }
                    }
                }
            }
            catch (BadImageFormatException)
            {
            }
        }
        internal static void Unload()
        {
            for (int i = 0; i < plugins.Count; i++)
            {
                if (plugins[i].onUnload != null)
                {
                    plugins[i].onUnload.Invoke(plugins[i], new PluginEventArgs());
                }
            }
            plugins.Clear();
        }
        internal static void Update()
        {
            foreach (Plugin p in plugins)
            {
                if (p.onUpdate != null)
                {
                    p.onUpdate.Invoke(p, new PluginEventArgs());
                }
            }
        }
    }
}
