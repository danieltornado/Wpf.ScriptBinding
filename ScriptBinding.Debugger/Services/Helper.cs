using System;
using System.Linq;

namespace ScriptBinding.Debugger.Services
{
    static class Helper
    {
        public static T GetAttribute<T>(this Type type)
            where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), false).Cast<T>().FirstOrDefault();
        }
    }
}
