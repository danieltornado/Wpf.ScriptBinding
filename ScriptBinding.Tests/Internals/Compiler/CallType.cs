using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(CallTypeTestData), DynamicDataSourceType.Method)]
        public void CallTypeTests(string expression, object expectedExpr)
        {
            DefaultTest<CallType>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> CallTypeTestData()
        {
            yield return new object[]
            {
                "int",
                new CallType(0, 2, typeof(int))
            };

            yield return new object[]
            {
                "System.Int64",
                new CallType(0, 11, typeof(Int64))
            };

            yield return new object[]
            {
                "System.Math",
                new CallType(0, 10, typeof(Math))
            };

            yield return new object[]
            {
                "System.Windows.PropertyPath",
                new CallType(0, 26, typeof(System.Windows.PropertyPath))
            };
        }
    }
}
