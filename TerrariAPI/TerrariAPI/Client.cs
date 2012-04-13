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
        private static ChatForm chatForm;
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
                    chatForm.Clear();
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
                if (e.plainText == "")
                {
                    PrintError("No text to print.");
                    return;
                }
                Print(e.plainText.Substring(1), new Color(255, 255, 255));
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
            GUI.Add(chatForm = new ChatForm());
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
            main.Set("cursorTexture", new Texture2D(game.GraphicsDevice, 1, 1));
            Plugin.Content();
        }
        internal static void Update()
        {
            state = State.UPDATE;
            Plugin.Update();
            GUI.Update();
            switch ((int)main.Get("menuMode"))
            {
                case 3:
                    main.Set(main.Get("loadPlayer")[main.Get("numLoadPlayers")].name, main.Get("loadPlayer")[main.Get("numLoadPlayers")].name + Input.NextStr());
                    break;
                case 7:
                    main.Set("newWorldName", main.Get("newWorldName") + Input.NextStr());
                    break;
                case 13:
                    main.Set("getIP", main.Get("getIP") + Input.NextStr());
                    break;
                case 30:
                case 31:
                    Wrapper.netplay.Set("password", Wrapper.netplay.Get("password") + Input.NextStr());
                    break;
                case 131:
                    main.Set("getPort", main.Get("getPort") + Input.NextStr());
                    break;
            }
            if (main.Get("editSign"))
            {
                main.Set("npcChatText", main.Get("npcChatText") + Input.NextStr());
            }
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
                main.Set("mouseState", new MouseState(0, 0, Mouse.GetState().ScrollWheelValue, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released));
                main.Set("oldMouseState", new MouseState(0, 0, Mouse.GetState().ScrollWheelValue, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released));
            }
        }
        internal static void Update2()
        {
            disableKeys = disableMouse = false;
            main.Set("chatMode", false);
        }
        internal static void AddMessage(string str, int r, int g, int b)
        {
            if (r == 0 && g == 0 && b == 0)
            {
                r = g = b = 255;
            }
            chatForm.AddMessage(str, new Color(r, g, b));
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
            chatForm.AddMessage(str, color);
        }
        internal static void PrintError(string str)
        {
            chatForm.AddMessage(str, new Color(225, 25, 25));
        }
        internal static void PrintNotification(string str)
        {
            chatForm.AddMessage(str, new Color(25, 225, 25));
        }
    }
}
