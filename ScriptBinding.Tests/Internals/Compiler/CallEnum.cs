using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(CallEnumTestData), DynamicDataSourceType.Method)]
        public void CallEnumTests(string expression, object expectedExpr)
        {
            DefaultTest<CallEnum>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> CallEnumTestData()
        {
            yield return new object[]
            {
                "System.AttributeTargets.ReturnValue",
                new CallEnum(0, 34,
                    System.AttributeTargets.ReturnValue)
            };

            yield return new object[]
            {
                "System.AttributeTargets.All",
                new CallEnum(0, 26,
                    System.AttributeTargets.All)
            };
        }
    }
}
