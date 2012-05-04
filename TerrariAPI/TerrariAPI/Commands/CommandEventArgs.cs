using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TerrariAPI.Commands
{
    /// <summary>
    /// Represents command arguments.
    /// </summary>
    public sealed class CommandEventArgs : EventArgs
    {
        private string[] args;
        /// <summary>
        /// Gets the number of arguments.
        /// </summary>
        public int length
        {
            get
            {
                return args.Length;
            }
        }
        /// <summary>
        /// The original, plain text of the supplied string.
        /// </summary>
        public readonly string plainText;
        /// <summary>
        /// Gets an argument based on the index.
        /// </summary>
        /// <param name="index">Index of the argument.</param>
        public string this[int index]
        {
            get
            {
                return args[index];
            }
        }

        internal CommandEventArgs(string str)
        {
            plainText = str;
            str = Regex.Replace(str.Trim(), @"\s+", " ");
            List<string> args = new List<string>();
            string temp = "";
            bool quotes = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '"')
                {
                    quotes = !quotes;
                }
                if (str[i] != ' ' || (str[i] == ' ' && quotes))
                {
                    temp += str[i];
                }
                if ((str[i] == ' ' || i == str.Length - 1) && !quotes)
                {
                    args.Add((temp != "" && temp[0] == '"') ? temp.Substring(1, temp.Length - 2) : temp);
                    temp = "";
                }
            }
            this.args = args.ToArray();
        }

        /// <summary>
        /// Gives the plain text after the index'th parameter.
        /// </summary>
        /// <param name="index"></param>
        public string Eol(int index)
        {
            string str = "";
            for (int i = index; i < length; i++)
            {
                str += this[i];
            }
            return str;
        }
    }
}
