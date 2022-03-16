using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(CallDynamicMethodTestData), DynamicDataSourceType.Method)]
        public void CallDynamicMethodTests(string expression, object expectedExpr)
        {
            DefaultTest<CallDynamicMethod>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> CallDynamicMethodTestData()
        {
            yield return new object[]
            {
                "b(1).ToString()",
                new CallDynamicMethod(0, 14, new CallBinding(0, 3, 1), "ToString", new Expr[] { })
            };

            yield return new object[]
            {
                "b(1).Find(2)",
                new CallDynamicMethod(0, 11, new CallBinding(0, 3, 1), "Find", new Expr[] { new ConstantNumber(10, 10, 2) })
            };
        }
    }
}
