using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using TerrariAPI.Hooking;

namespace TerrariAPI.Commands
{
    /// <summary>
    /// Represnts the method that runs when a command is executed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CommandEventHandler(object sender, CommandEventArgs e);
    /// <summary>
    /// Represents an executable command.
    /// </summary>
    public sealed class Command
    {
        internal static List<Command> commands = new List<Command>();
        internal static string lastCommand;

        /// <summary>
        /// Callback to be run when the command is executed.
        /// </summary>
        public readonly CommandEventHandler callback;
        internal readonly string desc;
        /// <summary>
        /// Name of the command.
        /// </summary>
        public readonly string name;
        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="name">Name for the new command.</param>
        /// <param name="callback">Callback for the new command.</param>
        public Command(string name, CommandEventHandler callback)
        {
            DescriptionAttribute attrib = callback.Method.GetCustomAttributes(false).First(a => a is DescriptionAttribute) as DescriptionAttribute;
            this.callback = callback;
            desc = attrib.description;
            this.name = name;
        }
        internal static void Add(Command command)
        {
            commands.Add(command);
        }
        /// <summary>
        /// Executes a string as a command.
        /// </summary>
        public static void Execute(string str)
        {
            CommandEventArgs args = new CommandEventArgs(str);
            foreach (Command c in commands)
            {
                if (args[0].ToLower() == c.name.ToLower())
                {
                    if (c.name != "repeat")
                    {
                        lastCommand = str;
                    }
                    c.callback(c, new CommandEventArgs(str.Substring(args[0].Length)));
                    return;
                }
            }
            Client.PrintError("Invalid command.");
        }
        /// <summary>
        /// Gets an item type based on name. If there is an error, -1 is returned; otherwise, the item type is returned.
        /// </summary>
        /// <param name="str">Item name to search for.</param>
        public static int GetItem(string str)
        {
            int matches = 0;
            int ID = -1;
            for (int i = 0; i < Wrapper.main.itemNames.Length; i++)
            {
                if (Wrapper.main.itemNames[i] == str)
                {
                    return i;
                }
                if (Wrapper.main.itemNames[i].ToLower().Contains(str.ToLower()))
                {
                    ID = i;
                    matches++;
                }
            }
            if (matches == 0)
            {
                Client.PrintError("Invalid item.");
                return -1;
            }
            if (matches > 1)
            {
                Client.PrintError("Item ambiguity (" + matches + " possible matches).");
                return -1;
            }
            return ID;
        }
        /// <summary>
        /// Gets a player index based on name. If there is an error, -1 is returned; otherwise, the player index is returned.
        /// </summary>
        /// <param name="str">Player name to search for.</param>
        public static int GetPlayer(string str)
        {
            int matches = 0;
            int ID = -1;
            for (int i = 0; i < Wrapper.main.players.Length; i++)
            {
                if (Wrapper.main.players[i].name == str)
                {
                    return i;
                }
                if (Wrapper.main.players[i].name.ToLower().Contains(str.ToLower()))
                {
                    ID = i;
                    matches++;
                }
            }
            if (matches == 0)
            {
                Client.PrintError("Invalid player.");
                return -1;
            }
            if (matches > 1)
            {
                Client.PrintError("Player ambiguity (" + matches + " possible matches).");
                return -1;
            }
            return ID;
        }
    }
}
