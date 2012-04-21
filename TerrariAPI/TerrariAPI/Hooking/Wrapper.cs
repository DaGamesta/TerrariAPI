using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TerrariAPI.Hooking
{
    /// <summary>
    /// Represents a generic wrapper.
    /// </summary>
    public abstract class Wrapper
    {
        /// <summary>
        /// A Terraria.Item wrapper.
        /// </summary>
        public static Item item;
        /// <summary>
        /// A Terraria.Lighting wrapper.
        /// </summary>
        public static Lighting lighting;
        /// <summary>
        /// A Terraria.Main wrapper.
        /// </summary>
        public static Main main;
        /// <summary>
        /// A Terraria.NetMessage wrapper.
        /// </summary>
        public static NetMessage netMessage;
        /// <summary>
        /// A Terraria.Netplay wrapper.
        /// </summary>
        public static Netplay netplay;
        internal object obj = null;
        internal Type type;
        /// <summary>
        /// A Terraria.WorldGen wrapper.
        /// </summary>
        public static WorldGen worldGen;

        internal Wrapper(object obj)
        {
            this.obj = obj;
            type = obj.GetType();
        }
        internal Wrapper(Type type)
        {
            this.type = type;
        }
        /// <summary>
        /// Gets the value of a field on the wrapped type or instance.
        /// </summary>
        /// <param name="field">Field to get.</param>
        public dynamic Get(string field)
        {
            return type.GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(obj);
        }
        /// <summary>
        /// Invokes a method on the wrapped type or instance.
        /// </summary>
        /// <param name="method">Method name to invoke.</param>
        /// <param name="parameters">Parameters to invoke the method with.</param>
        public dynamic Invoke(string method, params object[] parameters)
        {
            Type[] types = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                types[i] = parameters[i].GetType();
            }
            return type.GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, types, null).Invoke(obj, parameters);
        }
        /// <summary>
        /// Sets the value of a field on the wrapped type or instance.
        /// </summary>
        /// <param name="field">Field to set.</param>
        /// <param name="val">Value to set the field to.</param>
        public void Set(string field, object val)
        {
            type.GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).SetValue(obj, val);
        }
    }
}
