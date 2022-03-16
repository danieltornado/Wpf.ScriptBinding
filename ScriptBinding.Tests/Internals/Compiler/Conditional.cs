using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(ConditionalTestData), DynamicDataSourceType.Method)]
        public void ConditionalTests(string expression, object expectedExpr)
        {
            DefaultTest<Conditional>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> ConditionalTestData()
        {
            yield return new object[]
            {
                "if(1) then(2) else(3)",
                new Conditional(0, 20,
                    new ConstantNumber(3, 3, 1),
                    new ConstantNumber(11, 11, 2),
                    new ConstantNumber(19, 19, 3))
            };

            yield return new object[]
            {
                "if(if(1) then(1) else(1)) then(System.Math) else(b(1))",
                new Conditional(0, 53,
                    new Conditional(3, 23,
                        new ConstantNumber(6, 6, 1),
                        new ConstantNumber(14, 14, 1),
                        new ConstantNumber(22, 22, 1)),
                    new CallType(31, 41, typeof(Math)),
                    new CallBinding(49, 52, 1))
            };
        }
    }
}
