using System;
using System.Collections.Generic;
using System.Linq;
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
        private static Main main { get { return Wrapper.main; } }
        internal static State state;

        internal static void Initialize()
        {
            state = State.INIT;
            Plugin.Load();
            Plugin.Initialize();
            Command.Add(new Command("clear", (o, e) =>
                {
                    consoleForm.Clear();
                }));
            Command.Add(new Command("help", (o, e) =>
                {
                    List<string> commands = new List<string>();
                    foreach (Command c in Command.commands)
                    {
                        commands.Add(c.name);
                    }
                    commands.Sort();
                    Print("Commands:", new Color(25, 155, 25));
                    string text = "";
                    for (int i = 0; i < commands.Count; i++)
                    {
                        text += text == "" ? commands[i] : ", " + commands[i];
                    }
                    Print(text, new Color(225, 225, 25));
                }));
            Command.Add(new Command("print", (o, e) =>
            {
                if (e.plainText.Length == 5)
                {
                    PrintError("No text to print.");
                    return;
                }
                Print(e.plainText.Substring(5), new Color(255, 255, 255));
            }));
            Command.Add(new Command("repeat", (o, e) =>
            {
                if (Command.lastCommand == null)
                {
                    PrintError("No last command.");
                    return;
                }
                Command.Execute(Command.lastCommand);
            }));
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
                main.Set("keyState", new KeyboardState());
            }
        }
        internal static void DMouse()
        {
            if (disableMouse)
            {
                main.Set("mouseState", new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released));
                main.Set("oldMouseState", new MouseState(0, 0, 0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released));
            }
        }
        internal static void Update2()
        {
            disableKeys = disableMouse = false;
        }
        internal static void Draw()
        {
            state = State.DRAW;
            main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            Plugin.Draw();
            main.spriteBatch.End();
            GUI.Draw(main.spriteBatch);
        }
        internal static void Exit()
        {
            Plugin.Unload();
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
    }
}
