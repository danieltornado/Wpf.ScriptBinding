using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(UnaryTestData), DynamicDataSourceType.Method)]
        public void UnaryTests(string expression, object expectedExpr)
        {
            DefaultTest<Unary>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> UnaryTestData()
        {
            yield return new object[]
            {
                "not(true)",
                new Unary(0, 8,
                    new ConstantBoolean(4, 7, true),
                    UnaryType.Not)
            };

            yield return new object[]
            {
                "not ( true ) ",
                new Unary(0, 11,
                    new ConstantBoolean(6, 9, true),
                    UnaryType.Not)
            };
        }
    }
}
