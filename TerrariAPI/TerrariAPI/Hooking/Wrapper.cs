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
        /// A Terraria.Main wrapper.
        /// </summary>
        public static Main main;
        internal object obj = null;
        internal Type type;

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
        public void Invoke(string method, params object[] parameters)
        {
            type.GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Invoke(obj, parameters);
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
