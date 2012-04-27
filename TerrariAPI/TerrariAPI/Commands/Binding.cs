using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TerrariAPI.Commands
{
    /// <summary>
    /// Represents a keybinding to a command.
    /// </summary>
    public sealed class Binding
    {
        /// <summary>
        /// Commands to be executed.
        /// </summary>
        public List<string> commands = new List<string>();
        /// <summary>
        /// Key that is bound.
        /// </summary>
        public Keys key;

        internal static void Load(string path)
        {
            using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.OpenOrCreate)))
            {
                bool inBinding = false;
                string line;
                Binding temp = null;
                while ((line = reader.ReadLine()) != null && line.Trim() != "")
                {
                    if (line[0] == '<' && line[line.Length - 1] == '>')
                    {
                        inBinding = !inBinding;
                        if (inBinding)
                        {
                            Keys key;
                            if (Enum.TryParse<Keys>(line.Substring(1, line.Length - 2), true, out key))
                            {
                                temp = new Binding() { key = key };
                            }
                        }
                        else
                        {
                            Client.bindings.Add(new Binding() { commands = temp.commands, key = temp.key });
                            temp = null;
                        }
                    }
                    else if (inBinding && temp != null)
                    {
                        temp.commands.Add(line);
                    }
                }
            }
        }
    }
}
