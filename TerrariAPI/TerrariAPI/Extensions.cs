using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace TerrariAPI
{
    /// <summary>
    /// Includes extension methods.
    /// </summary>
    public static class Extensions
    {
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
    }
}
