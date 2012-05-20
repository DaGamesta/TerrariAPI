using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Manages Terraria hooking.
    /// </summary>
    public static class Hooks
    {
        internal static AssemblyDefinition asm;
        private static ModuleDefinition mod;
        /// <summary>
        /// Used to lock until all wrappers are created and the game is started.
        /// </summary>
        public static bool Locked = true;

        internal static void Start()
        {
            asm = AssemblyDefinition.ReadAssembly("Terraria.exe");
            mod = asm.MainModule;
            HookKeys();
            HookMain();
            HookNetMessage();
            HookMessageBuffer();
            GameHooks.OnHook();
            MemoryStream ms = new MemoryStream();
            asm.Write(ms);
#if DEBUG
            File.WriteAllBytes("debug.exe", ms.GetBuffer());
#endif
            Assembly terraria = Assembly.Load(ms.GetBuffer());
            
            Item.instance = new Item() { type = terraria.GetType("Terraria.Item") };
            Lighting.instance = new Lighting() { type = terraria.GetType("Terraria.Lighting") };
            NPC.instance = new NPC() { type = terraria.GetType("Terraria.NPC") };
            NetMessage.instance = new NetMessage() { type = terraria.GetType("Terraria.NetMessage") };
            Netplay.instance = new Netplay() { type = terraria.GetType("Terraria.Netplay") };
            Player.instance = new Player() { type = terraria.GetType("Terraria.Player") };
            Projectile.instance = new Projectile() { type = terraria.GetType("Terraria.Projectile") };
            Tile.instance = new Tile() { type = terraria.GetType("Terraria.Tile") };
            WorldGen.instance = new WorldGen() { type = terraria.GetType("Terraria.WorldGen") };

            Main.instance = new Main(terraria.GetType("Terraria.Main").GetConstructor(new Type[] { }).Invoke(null));
            Locked = false;
            try
            {
                Main.Run();
            }
            finally
            {
                Main.Dispose();
            }
        }
        private static void HookKeys()
        {
            #region .cctor
            asm.GetMethod("keyBoardInput", ".cctor").Body.GetILProcessor().Insert(Target.START,
                Instruction.Create(OpCodes.Ret)
                );
            #endregion
        }
        private static void HookMain()
        {
            #region .cctor
            asm.GetMethod("Main", ".cctor").Body.GetILProcessor().Insert(Instruction.Create(OpCodes.Stsfld, asm.GetField("Main", "numChatLines")), false,
                Instruction.Create(OpCodes.Ldc_I4, 13),
                Instruction.Create(OpCodes.Add)
                );
            #endregion
            #region .ctor
            asm.GetMethod("Main", ".ctor").Body.GetILProcessor().Insert(Target.START,
                Instruction.Create(OpCodes.Ldarg_0),
                Instruction.Create(OpCodes.Stsfld, mod.Import(GetClientField("Game")))
                );
            #endregion
            #region Initialize
            asm.GetMethod("Main", "Initialize").Body.GetILProcessor().Insert(Target.END,
                Instruction.Create(OpCodes.Call, mod.Import(GetClientMethod("Initialize")))
                );
            #endregion
            #region LoadContent
            asm.GetMethod("Main", "LoadContent").Body.GetILProcessor().Insert(Target.END,
                Instruction.Create(OpCodes.Call, mod.Import(GetClientMethod("LoadContent")))
                );
            #endregion
            #region Update
            ILProcessor update = asm.GetMethod("Main", "Update").Body.GetILProcessor();
            update.Insert(Target.START,
                Instruction.Create(OpCodes.Call, mod.Import(GetClientMethod("Update")))
                );
            update.Insert(Instruction.Create(OpCodes.Ret), false,
                Instruction.Create(OpCodes.Call, mod.Import(GetClientMethod("Update2")))
                );
            update.Insert(Instruction.Create(OpCodes.Stsfld, asm.GetField("Main", "keyState")), true,
                Instruction.Create(OpCodes.Call, mod.Import(GetClientMethod("DKeys")))
                );
            update.Insert(Instruction.Create(OpCodes.Stsfld, asm.GetField("Main", "mouseState")), true,
                Instruction.Create(OpCodes.Call, mod.Import(GetClientMethod("DMouse")))
                );
            #endregion
            #region GetInputText
            asm.GetMethod("Main", "GetInputText").Body.GetILProcessor().Insert(Target.START,
                Instruction.Create(OpCodes.Ldarg_0),
                Instruction.Create(OpCodes.Call, mod.Import(GetClientMethod("InputText"))),
                Instruction.Create(OpCodes.Ret)
                );
            #endregion
            #region Draw
            MethodDefinition draw = asm.GetMethod("Main", "Draw");
            ILProcessor drawILP = draw.Body.GetILProcessor();
            var onDraw = mod.Import(GetClientMethod("Draw"));
            for (int i = draw.Body.Instructions.Count - 1; i >= 0; i--)
            {
                Instruction curr = draw.Body.Instructions[i];
                if (curr.OpCode == OpCodes.Ret)
                {
                    Instruction inserted = Instruction.Create(OpCodes.Call, onDraw);
                    drawILP.InsertBefore(curr, inserted);
                    for (int j = 0; j < draw.Body.Instructions.Count - 1; j++)
                    {
                        Instruction t = draw.Body.Instructions[j];
                        if (t.Operand is Instruction && (Instruction)t.Operand == curr)
                        {
                            t.Operand = inserted;
                        }
                    }
                }
            }
            #endregion
        }
        private static void HookNetMessage()
        {
            #region SendData
            MethodDefinition sendData = asm.GetMethod("NetMessage", "SendData");
            sendData.Body.GetILProcessor().Insert(Target.START,
                Instruction.Create(OpCodes.Ldarg_0),
                Instruction.Create(OpCodes.Ldarg_3),
                Instruction.Create(OpCodes.Ldarg_S, sendData.Parameters[4]),
                Instruction.Create(OpCodes.Ldarg_S, sendData.Parameters[5]),
                Instruction.Create(OpCodes.Ldarg_S, sendData.Parameters[6]),
                Instruction.Create(OpCodes.Ldarg_S, sendData.Parameters[7]),
                Instruction.Create(OpCodes.Ldarg_S, sendData.Parameters[8]),
                Instruction.Create(OpCodes.Call, mod.Import(GetClientMethod("SendData")))
                );
            #endregion
        }
        private static void HookMessageBuffer()
        {
            #region GetData
            asm.GetMethod("messageBuffer", "GetData").Body.GetILProcessor().Insert(Target.START,
                Instruction.Create(OpCodes.Ldarg_1),
                Instruction.Create(OpCodes.Ldarg_2),
                Instruction.Create(OpCodes.Call, mod.Import(GetClientMethod("GetData")))
                );
            #endregion
        }
        private static FieldInfo GetClientField(string name)
        {
            return typeof(Client).GetField(name, BindingFlags.NonPublic | BindingFlags.Static);
        }
        private static MethodInfo GetClientMethod(string name)
        {
            return typeof(Client).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
        }
    }
}
