using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(CallDynamicPropertyTestData), DynamicDataSourceType.Method)]
        public void CallDynamicPropertyTests(string expression, object expectedExpr)
        {
            DefaultTest<CallDynamicProperty>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> CallDynamicPropertyTestData()
        {
            yield return new object[]
            {
                "b(1).Property1",
                new CallDynamicProperty(0, 13, new CallBinding(0, 3, 1), "Property1")
            };
        }
    }
}
