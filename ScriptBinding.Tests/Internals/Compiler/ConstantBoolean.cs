using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(ConstantBooleanTestData), DynamicDataSourceType.Method)]
        public void ConstantBooleanTests(string expression, object expectedExpr)
        {
            DefaultTest<ConstantBoolean>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> ConstantBooleanTestData()
        {
            yield return new object[]
            {
                "true",
                new ConstantBoolean(0, 3, true)
            };

            yield return new object[]
            {
                "false",
                new ConstantBoolean(0, 4, false)
            };
        }
    }
}
