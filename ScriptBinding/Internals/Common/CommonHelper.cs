using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Internals.Common
{
    static class CommonHelper
    {
        #region ShortTypes

        private static Dictionary<string, Type> CreateShortTypeDictionary()
        {
            return new Dictionary<string, Type>
            {
                { "byte", typeof(byte) },
                { "sbyte", typeof(sbyte) },
                { "short", typeof(short) },
                { "ushort", typeof(ushort) },
                { "int", typeof(int) },
                { "uint", typeof(uint) },
                { "long", typeof(long) },
                { "ulong", typeof(ulong) },

                { "float", typeof(float) },
                { "double", typeof(double) },
                { "decimal", typeof(decimal) },

                { "char", typeof(char) },
                { "string", typeof(string) },
            };
        }

        public static readonly Dictionary<string, Type> ShortTypes = CreateShortTypeDictionary();
        
        #endregion

        public static bool TryGetProperty(string propertyName, Type fromType, bool isStatic, out PropertyInfo property)
        {
            var bindingAttributes = BindingFlags.GetProperty | BindingFlags.Public;
            if (isStatic)
            {
                bindingAttributes |= BindingFlags.Static;
            }
            else
            {
                bindingAttributes |= BindingFlags.Instance;
            }

            property = fromType.GetProperty(propertyName, bindingAttributes);
            if (property != null)
                return true;

            return false;
        }

        public static bool TryGetEnumValue(string memberName, Type fromType, out Enum value)
        {
            if (fromType.IsEnum)
            {
                var canBeValue = Enum.GetValues(fromType).OfType<Enum>().FirstOrDefault(e => e.ToString() == memberName);
                if (canBeValue != default)
                {
                    value = canBeValue;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public static bool TryGetMethod(string methodName, Type[] parameters, Type fromType, bool isStatic, out MethodInfo method)
        {
            var bindingAttributes = BindingFlags.GetProperty | BindingFlags.Public;
            if (isStatic)
            {
                bindingAttributes |= BindingFlags.Static;
            }
            else
            {
                bindingAttributes |= BindingFlags.Instance;
            }

            method = fromType.GetMethod(methodName, bindingAttributes, Type.DefaultBinder, parameters, Array.Empty<ParameterModifier>());
            if (method != null)
                return true;

            return false;
        }

        public static bool TryGetRealNumber(string value, RealModifiers modifier, out object number)
        {
            bool result;

            switch (modifier)
            {
                case RealModifiers.None:
                case RealModifiers.D:
                    result = double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double d);
                    number = d;
                    return result;
                case RealModifiers.F:
                    result = float.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float f);
                    number = f;
                    return result;
                case RealModifiers.M:
                    result = decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal m);
                    number = m;
                    return result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null);
            }
        }

        public static bool TryGetIntegerNumber(string value, IntegerModifiers modifier, out object number)
        {
            bool result;

            switch (modifier)
            {
                case IntegerModifiers.None:
                    result = int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int i);
                    number = i;
                    return result;
                case IntegerModifiers.L:
                    result = long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long l);
                    number = l;
                    return result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null);
            }
        }

        public static string GetFullMessage(this Exception exception)
        {
            if (exception is AggregateException)
            {
                throw new NotImplementedException();
            }

            var message = new StringBuilder();

            var currentException = exception;
            while (currentException != null)
            {
                message.AppendLine(currentException.Message);
                currentException = exception.InnerException;
            }

            return message.ToString();
        }
    }
}
