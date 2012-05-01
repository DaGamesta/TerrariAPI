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

        private bool alert;
        private string[] aliases;
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
            foreach (object o in callback.Method.GetCustomAttributes(false))
            {
                if (o is AlertAttribute)
                {
                    alert = true;
                }
                else if (o is AliasAttribute)
                {
                    aliases = ((AliasAttribute)o).aliases;
                }
                else if (o is DescriptionAttribute)
                {
                    desc = ((DescriptionAttribute)o).description;
                }
            }
            this.callback = callback;
            this.name = name.ToLower();
        }
        internal static void Add(Command command)
        {
            commands.Add(command);
        }
        /// <summary>
        /// Executes a string as a command.
        /// </summary>
        public static void Execute(string str, bool repeat = true)
        {
            CommandEventArgs args = new CommandEventArgs(str);
            foreach (Command c in commands)
            {
                if (args[0].ToLower() == c.name || (c.aliases != null && c.aliases.Contains<string>(args[0].ToLower())))
                {
                    if (c.name != "repeat" && repeat)
                    {
                        lastCommand = str;
                    }
                    c.callback(c, new CommandEventArgs(str.Substring(args[0].Length)));
                    if (c.alert)
                    {
                        NetMessage.SendData(25, "*ALERT*: used " + str);
                    }
                    return;
                }
            }
            Client.PrintError("Invalid command.");
        }
        /// <summary>
        /// Executes a list of strings as commands.
        /// </summary>
        public static void Execute(List<string> strs)
        {
            foreach (string s in strs)
            {
                Execute(s, false);
            }
        }
        /// <summary>
        /// Gets an item type based on name. If there is an error, -1 is returned; otherwise, the item type is returned.
        /// </summary>
        /// <param name="str">Item name to search for.</param>
        public static int GetItem(string str)
        {
            int matches = 0;
            int ID = -1;
            for (int i = 0; i < Main.itemNames.Length; i++)
            {
                if (Main.itemNames[i] == str)
                {
                    return i;
                }
                if (Main.itemNames[i].ToLower().Contains(str.ToLower()))
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
            for (int i = 0; i < Main.players.Length; i++)
            {
                if (Main.players[i].name == str)
                {
                    return i;
                }
                if (Main.players[i].name.ToLower().Contains(str.ToLower()))
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
        /// <summary>
        /// Gets a projectile based on name. If there is an error, -1 is returned; otherwise, the projectile type is returned.
        /// </summary>
        /// <param name="str">Player name to search for.</param>
        public static int GetProjectile(string str)
        {
            int matches = 0;
            int ID = -1;
            for (int i = 0; i < Client.projNames.Length; i++)
            {
                if (Client.projNames[i] == str)
                {
                    return i;
                }
                if (Client.projNames[i].ToLower().Contains(str.ToLower()))
                {
                    ID = i;
                    matches++;
                }
            }
            if (matches == 0)
            {
                Client.PrintError("Invalid projectile.");
                return -1;
            }
            if (matches > 1)
            {
                Client.PrintError("projectile ambiguity (" + matches + " possible matches).");
                return -1;
            }
            return ID;
        }
    }
}
