using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TerrariAPI.Hooking
{
    internal static class Hooks
    {
        private static AssemblyDefinition asm;
        private static ModuleDefinition mod;

        internal static void Start()
        {
            asm = AssemblyDefinition.ReadAssembly("Terraria.exe");
            mod = asm.MainModule;
            HookKeys();
            HookMain();
            MemoryStream ms = new MemoryStream();
            asm.Write(ms);
            Assembly terraria = Assembly.Load(ms.GetBuffer());
            Wrapper.item = new Item() { type = terraria.GetType("Terraria.Item") };
            Wrapper.netMessage = new NetMessage() { type = terraria.GetType("Terraria.NetMessage") };
            Wrapper.netplay = new Netplay() { type = terraria.GetType("Terraria.Netplay") };
            Wrapper.worldGen = new WorldGen() { type = terraria.GetType("Terraria.WorldGen") };
            Wrapper.main = new Main(terraria.GetType("Terraria.Main").GetConstructor(new Type[] { }).Invoke(null));
            Wrapper.main.Invoke("Run");
        }
        private static void HookKeys()
        {
            MethodDefinition ctor = asm.GetMethod("keyBoardInput", ".cctor");
            ILProcessor ctorp = ctor.Body.GetILProcessor();
            ctorp.InsertBefore(ctor.Body.Instructions[0], ctorp.Create(OpCodes.Ret));
        }
        private static void HookMain()
        {
            ILProcessor temp = null;
            Instruction tempInstr = null;
            MethodDefinition ctor = asm.GetMethod("Main", ".ctor");
            temp = ctor.Body.GetILProcessor();
            tempInstr = ctor.Body.Instructions[0];
            temp.InsertBefore(tempInstr, temp.Create(OpCodes.Ldarg_0));
            temp.InsertBefore(tempInstr, temp.Create(OpCodes.Stsfld, mod.Import(typeof(Client).GetField("game", BindingFlags.NonPublic | BindingFlags.Static))));
            MethodDefinition initialize = asm.GetMethod("Main", "Initialize");
            temp = initialize.Body.GetILProcessor();
            temp.InsertBefore(initialize.Body.Instructions[initialize.Body.Instructions.Count - 1], temp.Create(OpCodes.Call, mod.Import(GetClientMethod("Initialize"))));
            MethodDefinition loadContent = asm.GetMethod("Main", "LoadContent");
            temp = loadContent.Body.GetILProcessor();
            temp.InsertBefore(loadContent.Body.Instructions[loadContent.Body.Instructions.Count - 1], temp.Create(OpCodes.Call, mod.Import(GetClientMethod("LoadContent"))));
            MethodDefinition update = asm.GetMethod("Main", "Update");
            temp = update.Body.GetILProcessor();
            temp.InsertBefore(update.Body.Instructions[0], temp.Create(OpCodes.Call, mod.Import(GetClientMethod("Update"))));
            var onUpdate2 = mod.Import(GetClientMethod("Update2"));
            for (int i = update.Body.Instructions.Count - 1; i >= 0; i--)
            {
                Instruction curr = update.Body.Instructions[i];
                if (curr.OpCode == OpCodes.Ret)
                {
                    Instruction inserted = temp.Create(OpCodes.Call, onUpdate2);
                    temp.InsertBefore(curr, inserted);
                    for (int j = 0; j < update.Body.Instructions.Count - 1; j++)
                    {
                        Instruction t = update.Body.Instructions[j];
                        if (t.Operand is Instruction && (Instruction)t.Operand == curr)
                        {
                            t.Operand = inserted;
                        }
                    }
                }
                else if (curr.OpCode == OpCodes.Call)
                {
                    MethodReference md = (MethodReference)curr.Operand;
                    if (md.Name == "GetState" && md.ReturnType.Name == "MouseState")
                    {
                        temp.InsertAfter(update.Body.Instructions[i + 1], temp.Create(OpCodes.Call, mod.Import(GetClientMethod("DMouse"))));
                    }
                    else if (md.Name == "GetState" && md.ReturnType.Name == "KeyboardState")
                    {
                        temp.InsertAfter(update.Body.Instructions[i + 1], temp.Create(OpCodes.Call, mod.Import(GetClientMethod("DKeys"))));
                    }
                }
            }
            MethodDefinition newText = asm.GetMethod("Main", "NewText");
            temp = newText.Body.GetILProcessor();
            tempInstr = newText.Body.Instructions[0];
            temp.InsertBefore(tempInstr, temp.Create(OpCodes.Ldarg_0));
            temp.InsertBefore(tempInstr, temp.Create(OpCodes.Ldarg_1));
            temp.InsertBefore(tempInstr, temp.Create(OpCodes.Ldarg_2));
            temp.InsertBefore(tempInstr, temp.Create(OpCodes.Ldarg_3));
            temp.InsertBefore(tempInstr, temp.Create(OpCodes.Call, mod.Import(GetClientMethod("AddMessage"))));
            temp.InsertBefore(tempInstr, temp.Create(OpCodes.Ret));
            MethodDefinition draw = asm.GetMethod("Main", "Draw");
            temp = draw.Body.GetILProcessor();
            var onDraw = mod.Import(GetClientMethod("Draw"));
            for (int i = draw.Body.Instructions.Count - 1; i >= 0; i--)
            {
                Instruction curr = draw.Body.Instructions[i];
                if (curr.OpCode == OpCodes.Ret)
                {
                    Instruction inserted = temp.Create(OpCodes.Call, onDraw);
                    temp.InsertBefore(curr, inserted);
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
            MethodDefinition exit = asm.GetMethod("Main", "QuitGame");
            temp = exit.Body.GetILProcessor();
            temp.InsertBefore(exit.Body.Instructions[0], temp.Create(OpCodes.Call, mod.Import(GetClientMethod("Exit"))));
        }

        private static MethodInfo GetClientMethod(string name)
        {
            return typeof(Client).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static);
        }
        private static MethodDefinition GetMethod(this AssemblyDefinition asm, string typeName, string methodName)
        {
            foreach (TypeDefinition td in asm.MainModule.Types)
            {
                if (td.Name.ToLower() == typeName.ToLower())
                {
                    foreach (MethodDefinition md in td.Methods)
                    {
                        if (md.Name == methodName)
                        {
                            return md;
                        }
                    }
                    return null;
                }
            }
            return null;
        }
    }
}
