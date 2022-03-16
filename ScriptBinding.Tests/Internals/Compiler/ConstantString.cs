using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(ConstantStringTestData), DynamicDataSourceType.Method)]
        public void ConstantStringTests(string expression, object expectedExpr)
        {
            DefaultTest<ConstantString>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> ConstantStringTestData()
        {
            yield return new object[]
            {
                "\"1000\"",
                new ConstantString(0, 5, "1000")
            };

            yield return new object[]
            {
                "\"AAA aaa\"",
                new ConstantString(0, 8, "AAA aaa")
            };

            yield return new object[]
            {
                "'1000'",
                new ConstantString(0, 5, "1000")
            };

            yield return new object[]
            {
                "'AAA aaa'",
                new ConstantString(0, 8, "AAA aaa")
            };
        }
    }
}
