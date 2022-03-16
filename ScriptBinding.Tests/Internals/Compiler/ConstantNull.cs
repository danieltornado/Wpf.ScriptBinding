using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(ConstantNullTestData), DynamicDataSourceType.Method)]
        public void ConstantNullTests(string expression, object expectedExpr)
        {
            DefaultTest<ConstantNull>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> ConstantNullTestData()
        {
            yield return new object[]
            {
                "null",
                new ConstantNull(0, 3)
            };
        }
    }
}
