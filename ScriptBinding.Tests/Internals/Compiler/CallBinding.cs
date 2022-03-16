using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(CallBindingTestData), DynamicDataSourceType.Method)]
        public void CallBindingTests(string expression, object expectedExpr)
        {
            DefaultTest<CallBinding>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> CallBindingTestData()
        {
            yield return new object[]
            {
                "b(1)",
                new CallBinding(0, 3, 1)
            };

            yield return new object[]
            {
                "b(1000000)",
                new CallBinding(0, 9, 1000000)
            };
        }
    }
}
