using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using TerrariAPI.Hooking;

namespace TerrariAPI
{
    /// <summary>
    /// Includes extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Finds a list of matching instructions.
        /// </summary>
        /// <param name="ilp"></param>
        /// <param name="target">Instruction to target for.</param>
        public static List<Instruction> Find(this ILProcessor ilp, Instruction target)
        {
            List<Instruction> instructions = new List<Instruction>();
            foreach (Instruction i in ilp.Body.Instructions)
            {
                if (i.OpCode == target.OpCode && ((i.Operand == null && target.Operand == null) ||
                    i.Operand.ToString() == target.Operand.ToString())) // Hackish fix...?
                {
                    instructions.Add(i);
                }
            }
            return instructions;
        }
        /// <summary>
        /// Gets a field definition.
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="type">Name of the type the field is in.</param>
        /// <param name="field">Name of the field.</param>
        public static FieldDefinition GetField(this AssemblyDefinition asm, string type, string field)
        {
            foreach (TypeDefinition td in asm.MainModule.Types)
            {
                if (td.Name == type)
                {
                    foreach (FieldDefinition fd in td.Fields)
                    {
                        if (fd.Name == field)
                        {
                            return fd;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Gets a method definition.
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="type">Name of the type the method is in.</param>
        /// <param name="method">Name of the method.</param>
        public static MethodDefinition GetMethod(this AssemblyDefinition asm, string type, string method)
        {
            foreach (TypeDefinition td in asm.MainModule.Types)
            {
                if (td.Name == type)
                {
                    foreach (MethodDefinition md in td.Methods)
                    {
                        if (md.Name == method)
                        {
                            return md;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Inserts IL code using the specified target(s).
        /// </summary>
        /// <param name="ilp"></param>
        /// <param name="target">Instruction target.</param>
        /// <param name="after">Whether or not to insert after or before the target.</param>
        /// <param name="instructions">Instructions to insert.</param>
        public static void Insert(this ILProcessor ilp, Instruction target, bool after, params Instruction[] instructions)
        {
            for (int i = 0; i < ilp.Body.Instructions.Count; i++)
            {
                Instruction t = ilp.Body.Instructions[i];
                if (t.OpCode == target.OpCode && ((t.Operand == null && target.Operand == null) ||
                    t.Operand.ToString() == target.Operand.ToString())) // Hackish fix...?
                {
                    foreach (Instruction instr in instructions)
                    {
                        if (after)
                        {
                            ilp.InsertAfter(ilp.Body.Instructions[i], instr);
                        }
                        else
                        {
                            ilp.InsertBefore(t, instr);
                        }
                        i++;
                    }
                }
            }
        }
        /// <summary>
        /// Inserts IL code before the specified target.
        /// </summary>
        /// <param name="ilp"></param>
        /// <param name="target">Index to instruction target.</param>
        /// <param name="instructions">Instructions to insert.</param>
        public static void Insert(this ILProcessor ilp, int target, params Instruction[] instructions)
        {
            Instruction targetInstr = ilp.Body.Instructions[target];
            foreach (Instruction i in instructions)
            {
                ilp.InsertBefore(targetInstr, i);
            }
        }
        /// <summary>
        /// Inserts IL code before the specified target.
        /// </summary>
        /// <param name="ilp"></param>
        /// <param name="target">Instruction target.</param>
        /// <param name="instructions">Instructions to insert.</param>
        public static void Insert(this ILProcessor ilp, Target target, params Instruction[] instructions)
        {
            int targetIndex = 0;
            switch (target)
            {
                case Target.START:
                    targetIndex = 0;
                    break;
                case Target.END:
                    targetIndex = ilp.Body.Instructions.Count - 1;
                    break;
            }
            ilp.Insert(targetIndex, instructions);
        }
    }
}
