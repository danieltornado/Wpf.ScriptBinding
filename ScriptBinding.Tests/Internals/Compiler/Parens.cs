using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(ParensTestData), DynamicDataSourceType.Method)]
        public void ParensTests(string expression, object expectedExpr)
        {
            DefaultTest<Parens>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> ParensTestData()
        {
            yield return new object[]
            {
                "(1)",
                new Parens(0, 2,
                    new ConstantNumber(1, 1, 1))
            };

            yield return new object[]
            {
                "((1))",
                new Parens(0, 4,
                    new Parens(1, 3,
                        new ConstantNumber(2, 2, 1)))
            };

            yield return new object[]
            {
                "((1 + 2) + b(1))",
                new Parens(0, 15,
                    new Binary(1, 14,
                        new Parens(1, 7,
                            new Binary(2, 6,
                                new ConstantNumber(2, 2, 1),
                                new ConstantNumber(6, 6, 2),
                                BinaryType.Plus)),
                        new CallBinding(11, 14, 1),
                        BinaryType.Plus))
            };
        }
    }
}
