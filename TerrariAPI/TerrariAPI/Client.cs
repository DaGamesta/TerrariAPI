﻿using System;
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
using TerrariAPI.Plugins;
using XNAForms;

namespace TerrariAPI
{
    internal static class Client
    {
        private static ConsoleForm consoleForm;
        internal static bool disableKeys;
        internal static bool disableMouse;
        internal static Game game;
        internal static State state;

        internal static void Initialize()
        {
            state = State.INIT;
            Plugin.Initialize();
            Command.Add(new Command("clear", Clear));
            Command.Add(new Command("help", Help));
            Command.Add(new Command("netsend", NetSend));
            Command.Add(new Command("repeat", Repeat));
            Command.Add(new Command("say", Say));
            GUI.Initialize(game);
            GUI.Add(consoleForm = new ConsoleForm());
        }
        internal static void LoadContent()
        {
            state = State.CONTENT;
            ContentManager content = game.Content;
            GUI.BindCursor(CursorType.NORMAL, content.Load<Texture2D>("TerrariAPI/Cursors/Normal"));
            GUI.BindCursor(CursorType.RESIZE_VERTICAL, content.Load<Texture2D>("TerrariAPI/Cursors/ResizeVert"));
            GUI.BindCursor(CursorType.RESIZE_HORIZONTAL, content.Load<Texture2D>("TerrariAPI/Cursors/ResizeHorz"));
            GUI.BindCursor(CursorType.RESIZE_DIAGONAL, content.Load<Texture2D>("TerrariAPI/Cursors/ResizeDiag"));
            GUI.BindCursor(CursorType.TEXT, content.Load<Texture2D>("TerrariAPI/Cursors/Text"));
            GUI.BindFont(content.Load<SpriteFont>("TerrariAPI/Font"));
            Main.cursorTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            Plugin.Content();
        }
        internal static void Update()
        {
            state = State.UPDATE;
            Plugin.Update();
            GUI.Update();
        }
        internal static void DKeys()
        {
            if (disableKeys)
            {
                Main.keyState = new KeyboardState();
            }
        }
        internal static void DMouse()
        {
            if (disableMouse)
            {
                Main.mouseState =  new MouseState(0, 0, Mouse.GetState().ScrollWheelValue, 0, 0, 0, 0, 0);
                Main.oldMouseState = new MouseState(0, 0, Mouse.GetState().ScrollWheelValue, 0, 0, 0, 0, 0);
            }
        }
        internal static string InputText(string oldStr)
        {
            Main.inputTextEnter = false;
            if (!disableKeys)
            {
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
        internal static void Update2()
        {
            disableKeys = disableMouse = false;
        }
        internal static void Draw()
        {
            state = State.DRAW;
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            Plugin.Draw();
            Main.spriteBatch.End();
            GUI.Draw(Main.spriteBatch);
        }

        internal static void Print(string str, Color color)
        {
            consoleForm.AddMessage(str, color);
        }
        internal static void PrintError(string str)
        {
            consoleForm.AddMessage(str, new Color(225, 25, 25));
        }
        internal static void PrintNotification(string str)
        {
            consoleForm.AddMessage(str, new Color(25, 225, 25));
        }
        [Alias("clr")]
        [Description("Clears the chat/console.")]
        static void Clear(object sender, CommandEventArgs e)
        {
            consoleForm.Clear();
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
                        Print(c.name + ": " + c.desc, new Color(25, 225, 25));
                        return;
                    }
                }
                PrintError("Invalid command.");
            }
            else
            {
                List<string> commands = new List<string>();
                foreach (Command c in Command.commands)
                {
                    commands.Add(c.name);
                }
                commands.Sort();
                Print("Commands:", new Color(25, 155, 25));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < commands.Count; i++)
                {
                    sb.Append(sb.ToString() == "" ? commands[i] : ", " + commands[i]);
                }
                Print(sb.ToString(), new Color(225, 225, 25));
            }
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
