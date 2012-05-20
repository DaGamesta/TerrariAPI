using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TerrariAPI.Commands;
using TerrariAPI.Hooking;
using XNAForms;

namespace TerrariAPI
{
    internal static class Client
    {
        internal static List<Binding> Bindings = new List<Binding>();
        private static ConsoleForm ConsoleForm;
        internal static bool DisableBindings;
        internal static bool DisableKeys;
        internal static bool DisableMouse;
        internal static Game Game;
        private static Keys[] LastPressedKeys = Keyboard.GetState().GetPressedKeys();
        private static Keys[] PressedKeys = Keyboard.GetState().GetPressedKeys();
        internal static string[] ProjNames;
        internal static string[] TileNames = new string[] { "dirt", "stone", "", "", "torch", "tree", "iron", "copper", "gold", "silver", "",
            "", "", "", "", "", "", "", "", "platform", "", "", "demonite", "", "", "ebonstone", "", "", "", "", "wood", "", "", "", "", "",
            "", "meteorite", "gray brick", "red brick", "clay", "blue brick", "", "green brick", "pink brick", "gold brick", "silver brick",
            "copper brick", "spike", "", "", "cobweb", "", "sand", "glass", "","obsidian", "ash", "hellstone", "mud", "", "", "", "sapphire",
            "ruby", "emerald", "topaz", "amethyst", "diamond", "", "", "", "glowing mushroom", "", "", "obsidian brick", "hellstone brick", "",
            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "cobalt",
            "mythril", "", "", "adamantite", "ebonsand", "", "", "", "pearlsand", "pearlstone", "pearlstone brick", "iridescent brick",
            "mudstone block", "cobalt brick", "mythril brick", "silt", "", "", "", "", "", "", "active stone", "inactive stone", "", "", "",
            "", "", "", "", "", "demonite brick", "explosives", "inlet", "outlet", "", "candy cane", "green candy cane", "snow", "snow brick", "" };
        internal static string[] WallNames = new string[] { "", "stone", "", "ebonstone", "wood", "gray brick", "red brick", "", "", "",
            "gold brick", "silver brick", "copper brick", "hellstone brick", "", "", "dirt", "blue brick", "green brick", "pink brick",
            "obsidian brick", "glass", "pearlstone brick", "iridescent brick", "mudstone brick", "cobalt", "mythril", "planked", "pearlstone",
            "candy cane", "green candy cane", "snow brick" };

        internal static void Initialize()
        {
            Binding.Load("bindings.txt");
            Command.Add(new Command("addmacro", AddMacro));
            Command.Add(new Command("clear", Clear));
            Command.Add(new Command("help", Help));
            Command.Add(new Command("macro", Macro));
            Command.Add(new Command("netsend", NetSend));
            Command.Add(new Command("repeat", Repeat));
            Command.Add(new Command("say", Say));
            GUI.Initialize(Game);
            GUI.Add(ConsoleForm = new ConsoleForm());
            GameHooks.OnInitialize();

            ProjNames = new string[Main.instance.Get("projectileTexture").Length];
            dynamic temp = Projectile.New();
            for (int i = 0; i < ProjNames.Length; i++)
            {
                temp.SetDefaults(i);
                ProjNames[i] = temp.name;
            }
            if (File.Exists("startup.macro"))
            {
                Command.Execute("m startup");
            }
        }
        internal static void LoadContent()
        {
            ContentManager content = Game.Content;
            GUI.BindCursor(CursorType.NORMAL, content.Load<Texture2D>("TerrariAPI/Cursors/Normal"));
            GUI.BindCursor(CursorType.RESIZE_VERTICAL, content.Load<Texture2D>("TerrariAPI/Cursors/ResizeVert"));
            GUI.BindCursor(CursorType.RESIZE_HORIZONTAL, content.Load<Texture2D>("TerrariAPI/Cursors/ResizeHorz"));
            GUI.BindCursor(CursorType.RESIZE_DIAGONAL, content.Load<Texture2D>("TerrariAPI/Cursors/ResizeDiag"));
            GUI.BindCursor(CursorType.TEXT, content.Load<Texture2D>("TerrariAPI/Cursors/Text"));
            GUI.BindFont(content.Load<SpriteFont>("TerrariAPI/Font"));
            Main.cursorTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            GameHooks.OnContent();
        }
        internal static void Update()
        {
            GameHooks.OnUpdate();
            GUI.Update();
        }
        internal static void DKeys()
        {
            if (DisableKeys)
            {
                Main.keyState = new KeyboardState();
            }
        }
        internal static void DMouse()
        {
            if (DisableMouse)
            {
                Main.mouseState = new MouseState(-100, -100, Mouse.GetState().ScrollWheelValue, 0, 0, 0, 0, 0);
                Main.oldMouseState = new MouseState(-100, -100, Mouse.GetState().ScrollWheelValue, 0, 0, 0, 0, 0);
            }
        }
        internal static string InputText(string oldStr)
        {
            Main.inputTextEnter = false;
            if (!DisableKeys)
            {
                DisableBindings = true;
                oldStr += Input.nextStr;
                if (Input.TappedKey(Keys.Enter))
                {
                    Main.inputTextEnter = true;
                }
                if ((Input.active & SpecialKeys.BACK) != 0 && oldStr.Length != 0)
                {
                    oldStr = oldStr.Substring(0, oldStr.Length - 1);
                }
            }
            return oldStr;
        }
        internal static void SendData(byte msg, string str, int n, float n2, float n3, float n4, int n5)
        {
            NetHooks.OnSendData(msg, str, n, n2, n3, n4, n5);
        }
        internal static void GetData(int start, int length)
        {
            byte[] data = new byte[length];
            Buffer.BlockCopy(NetMessage.messageBuffers[256].readBuffer, start, data, 0, length);
            if (data[0] == 25)
            {
                string str = Encoding.UTF8.GetString(data, 5, length - 5);
                string hour = DateTime.Now.Hour.ToString();
                if (hour.Length == 1)
                {
                    hour = "0" + hour;
                }
                string min = DateTime.Now.Minute.ToString();
                if (min.Length == 1)
                {
                    min = "0" + min;
                }
                string sec = DateTime.Now.Second.ToString();
                if (sec.Length == 1)
                {
                    sec = "0" + sec;
                }
                using (StreamWriter writer = new StreamWriter("Logs\\" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + Main.instance.Get("worldName") + ".log", true))
                {
                    writer.WriteLine("[" + hour + ":" + min + ":" + sec + "] " + str);
                }
            }
            else if (data[0] == 49)
            {
                string path = "Logs\\" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + Main.instance.Get("worldName") + ".log";
                if (File.Exists(path))
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine();
                        writer.WriteLine("*** END LOGGING ***");
                        writer.WriteLine();
                    }
                }
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine("*** BEGIN LOGGING ***");
                    writer.WriteLine();
                }
            }
            NetHooks.OnGetData(data);
        }
        internal static void Update2()
        {
            LastPressedKeys = PressedKeys;
            PressedKeys = Keyboard.GetState().GetPressedKeys();
            if (!DisableKeys && !DisableBindings)
            {
                foreach (Binding b in Bindings)
                {
                    if (!LastPressedKeys.Contains<Keys>(b.key) && PressedKeys.Contains<Keys>(b.key))
                    {
                        Command.Execute(b.commands);
                    }
                }
            }
            DisableBindings = DisableKeys = DisableMouse = false;
            GameHooks.OnUpdate2();
        }
        internal static void Draw()
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            GameHooks.OnDraw();
            Main.spriteBatch.End();
            GUI.Draw(Main.spriteBatch);
        }

        internal static void Print(string str, Color color)
        {
            ConsoleForm.AddMessage(str, color);
        }
        internal static void PrintError(string str)
        {
            ConsoleForm.AddMessage(str, new Color(225, 25, 25));
        }
        internal static void PrintNotification(string str)
        {
            ConsoleForm.AddMessage(str, new Color(25, 225, 25));
        }

        [Alias("am")]
        [Description("Creates a new macro, and opens it for editing.")]
        static void AddMacro(object sender, CommandEventArgs e)
        {
            if (e.length != 1)
            {
                PrintError("Syntax: addmacro <name>");
                return;
            }
            FileStream fs = File.Create(e[0] + ".macro");
            fs.Dispose();
            System.Diagnostics.Process.Start("notepad.exe", e[0] + ".macro");
        }
        [Alias("clr")]
        [Description("Clears the chat/console.")]
        static void Clear(object sender, CommandEventArgs e)
        {
            ConsoleForm.Clear();
        }
        [Alias("h")]
        [Description("Lists all commands or gives a description of one.")]
        static void Help(object sender, CommandEventArgs e)
        {
            if (e.length > 1)
            {
                PrintError("Syntax: help [<command>]");
                return;
            }
            if (e.length == 1)
            {
                foreach (Command c in Command.commands)
                {
                    if (c.name.ToLower() == e[0].ToLower())
                    {
                        Print(c.name + ": " + c.desc, new Color(225, 225, 25));
                        return;
                    }
                }
                PrintError("Invalid command.");
                return;
            }
            var commands = from c in Command.commands
                                        orderby c.name
                                        select c;
            Print("Commands:", new Color(25, 155, 25));
            foreach (Command c in commands)
            {
                Print(c.name + ": " + c.desc, new Color(225, 225, 25));
            }
        }
        [Alias("m")]
        [Description("Runs a macro.")]
        static void Macro(object sender, CommandEventArgs e)
        {
            if (e.length == 0)
            {
                PrintError("Syntax: macro <name> [<args>]");
                return;
            }
            if (!File.Exists(e[0] + ".macro"))
            {
                PrintError("Invalid macro.");
                return;
            }
            string[] args = new string[e.length - 1];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = e[i + 1];
            }
            using (StreamReader reader = new StreamReader(e[0] + ".macro"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Command.Execute(string.Format(line, args));
                }
            }
            PrintNotification("Executed macro '" + e[0] + ".'");
        }
        [Alias("ns")]
        [Description("Directly sends network data.")]
        static void NetSend(object sender, CommandEventArgs e)
        {
            if (e.length < 1 || e.length > 7)
            {
                PrintError("Syntax: netsend <packet ID> [<arg1>] [<arg2>] [<arg3>] [<arg4>] [<arg5>] [<arg6>]");
                return;
            }
            int packet;
            if (!int.TryParse(e[0], out packet))
            {
                PrintError("Invalid packet ID.");
                return;
            }
            int num1 = 0, num5 = 0;
            float num2 = 0, num3 = 0, num4 = 0;
            string str = "";
            try
            {
                if (!int.TryParse(e[2], out num1))
                {
                    PrintError("Invalid integer.");
                    return;
                }
                if (!float.TryParse(e[3], out num2) || !float.TryParse(e[4], out num3) || !float.TryParse(e[5], out num4))
                {
                    PrintError("Invalid float.");
                    return;
                }
                if (!int.TryParse(e[6], out num5))
                {
                    PrintError("Invalid integer.");
                    return;
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            finally
            {
                NetMessage.SendData(packet, str, num1, num2, num3, num4, num5);
                PrintNotification("Sent " + packet + ", " + (str == "" ? "\"\"" : str) + ", " + num1 + ", " + num2 + ", " + num3 + ", " + num4 + ", " + num5);
            }
        }
        [Alias("r")]
        [Description("Repeats the previously used command.")]
        static void Repeat(object sender, CommandEventArgs e)
        {
            if (Command.lastCommand == null)
            {
                PrintError("No last command.");
                return;
            }
            Command.Execute(Command.lastCommand);
        }
        [Description("Sends a message to the current server, if applicable.")]
        static void Say(object sender, CommandEventArgs e)
        {
            if (e.plainText == "")
            {
                PrintError("No text to send.");
                return;
            }
            if (Main.multiplayer)
            {
                NetMessage.SendData(25, e.plainText.Substring(1));
            }
            else
            {
                PrintError("Not connected to a server.");
            }
        }
    }
}
