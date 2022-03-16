using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(CallMethodTestData), DynamicDataSourceType.Method)]
        public void CallMethodTests(string expression, object expectedExpr)
        {
            DefaultTest<CallMethod>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> CallMethodTestData()
        {
            yield return new object[]
            {
                "System.Math.Abs(1)",
                new CallMethod(0, 17, 
                    new CallType(0, 10, typeof(Math)), 
                    GetMethod(typeof(Math), "Abs", new [] { typeof(int) }, true), 
                    new Expr[] { new ConstantNumber(16, 16, 1) })
            };

            yield return new object[]
            {
                "('just a string').Contains('just')",
                new CallMethod(0, 33,
                    new Parens(0, 16, 
                        new ConstantString(1, 15, "just a string")),
                    GetMethod(typeof(string), "Contains", new [] { typeof(string) }, false),
                    new Expr[] { new ConstantString(27, 32, "just") })
            };
        }

        private static MethodInfo GetMethod(Type type, string methodName, Type[] parameters, bool isStatic)
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

            return type.GetMethod(methodName, bindingAttributes, Type.DefaultBinder, parameters, Array.Empty<ParameterModifier>());
        }
    }
}
