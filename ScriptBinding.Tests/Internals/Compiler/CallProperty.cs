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
        [DynamicData(nameof(CallPropertyTestData), DynamicDataSourceType.Method)]
        public void CallPropertyTests(string expression, object expectedExpr)
        {
            DefaultTest<CallProperty>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> CallPropertyTestData()
        {
            yield return new object[]
            {
                "System.Type.DefaultBinder",
                new CallProperty(0, 24, 
                    new CallType(0, 10, typeof(Type)), 
                    GetProperty(typeof(Type), "DefaultBinder", true))
            };

            yield return new object[]
            {
                "('just a string').Length",
                new CallProperty(0, 23,
                    new Parens(0, 16, 
                        new ConstantString(1, 15, "just a string")), 
                    GetProperty(typeof(string), "Length", false))
            };
        }

        private static PropertyInfo GetProperty(Type type, string propertyName, bool isStatic)
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

            return type.GetProperty(propertyName, bindingAttributes);
        }
    }
}
