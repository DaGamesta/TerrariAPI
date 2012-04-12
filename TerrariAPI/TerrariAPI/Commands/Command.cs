using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public readonly CommandEventHandler callback;
        public readonly string name;
        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="name">Name for the new command.</param>
        /// <param name="callback">Callback for the new command.</param>
        public Command(string name, CommandEventHandler callback)
        {
            this.callback = callback;
            this.name = name;
        }
        /// <summary>
        /// Adds a command.
        /// </summary>
        public static void Add(Command command)
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
    }
}
