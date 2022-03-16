using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(BinaryTestData), DynamicDataSourceType.Method)]
        public void BinaryTests(string expression, object expectedExpr)
        {
            DefaultTest<Binary>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> BinaryTestData()
        {
            yield return new object[]
            {
                "1 + 2.2",
                new Binary(0, 6,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(4, 6, 2.2),
                    BinaryType.Plus)
            };

            yield return new object[]
            {
                "(1.2 + 2.3) + 3",
                new Binary(0, 14,
                    new Parens(0, 10,
                        new Binary(1, 9,
                            new ConstantNumber(1, 3, 1.2),
                            new ConstantNumber(7, 9, 2.3),
                            BinaryType.Plus)),
                    new ConstantNumber(14, 14, 3),
                    BinaryType.Plus)
            };

            yield return new object[]
            {
                "((1.2)) + 1 + 4",
                new Binary(0, 14,
                    new Binary(0, 10,
                        new Parens(0, 6,
                            new Parens(1, 5, new ConstantNumber(2, 4, 1.2))),
                        new ConstantNumber(10, 10, 1),
                        BinaryType.Plus),
                    new ConstantNumber(14, 14, 4),
                    BinaryType.Plus)
            };

            yield return new object[]
            {
                "1 - 2.2",
                new Binary(0, 6,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(4, 6, 2.2),
                    BinaryType.Minus)
            };

            yield return new object[]
            {
                "1 + 2.2 - 3.4",
                new Binary(0, 12,
                    new ConstantNumber(0, 0, 1),
                    new Binary(4, 12, 
                        new ConstantNumber(4, 6, 2.2), 
                        new ConstantNumber(10, 12, 3.4), 
                        BinaryType.Minus),
                    BinaryType.Plus)
            };
            
            yield return new object[]
            {
                "1 * 2.2",
                new Binary(0, 6,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(4, 6, 2.2),
                    BinaryType.Multiply)
            };

            yield return new object[]
            {
                "1 - 2.2 * 3.4",
                new Binary(0, 12,
                    new ConstantNumber(0, 0, 1),
                    new Binary(4, 12,
                        new ConstantNumber(4, 6, 2.2),
                        new ConstantNumber(10, 12, 3.4),
                        BinaryType.Multiply),
                    BinaryType.Minus)
            };

            yield return new object[]
            {
                "1 / 2.2",
                new Binary(0, 6,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(4, 6, 2.2),
                    BinaryType.Divide)
            };

            yield return new object[]
            {
                "1 * 2.2 / 3.4",
                new Binary(0, 12,
                    new ConstantNumber(0, 0, 1),
                    new Binary(4, 12,
                        new ConstantNumber(4, 6, 2.2),
                        new ConstantNumber(10, 12, 3.4),
                        BinaryType.Divide),
                    BinaryType.Multiply)
            };

            yield return new object[]
            {
                "1 % 2.2",
                new Binary(0, 6,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(4, 6, 2.2),
                    BinaryType.Mod)
            };

            yield return new object[]
            {
                "1 / 2.2 % 3.4",
                new Binary(0, 12,
                    new ConstantNumber(0, 0, 1),
                    new Binary(4, 12,
                        new ConstantNumber(4, 6, 2.2),
                        new ConstantNumber(10, 12, 3.4),
                        BinaryType.Mod),
                    BinaryType.Divide)
            };





            yield return new object[]
            {
                "1 or 2",
                new Binary(0, 5,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(5, 5, 2),
                    BinaryType.Or)
            };

            yield return new object[]
            {
                "1 or 2 % 3",
                new Binary(0, 9,
                    new Binary(0, 5,
                        new ConstantNumber(0, 0, 1),
                        new ConstantNumber(5, 5, 2),
                        BinaryType.Or),
                    new ConstantNumber(9, 9, 3),
                    BinaryType.Mod)
            };

            yield return new object[]
            {
                "1 % 2 or 3",
                new Binary(0, 9,
                    new ConstantNumber(0, 0, 1),
                    new Binary(4, 9,
                        new ConstantNumber(4, 4, 2),
                        new ConstantNumber(9, 9, 3),
                        BinaryType.Or),
                    BinaryType.Mod)
            };

            yield return new object[]
            {
                "1 and 2",
                new Binary(0, 6,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(6, 6, 2),
                    BinaryType.And)
            };

            yield return new object[]
            {
                "1 and 2 or 3",
                new Binary(0, 11,
                    new Binary(0, 6,
                        new ConstantNumber(0, 0, 1),
                        new ConstantNumber(6, 6, 2),
                        BinaryType.And),
                    new ConstantNumber(11, 11, 3),
                    BinaryType.Or)
            };

            yield return new object[]
            {
                "1 or 2 and 3",
                new Binary(0, 11,
                    new ConstantNumber(0, 0, 1),
                    new Binary(5, 11,
                        new ConstantNumber(5, 5, 2),
                        new ConstantNumber(11, 11, 3),
                        BinaryType.And),
                    BinaryType.Or)
            };

            yield return new object[]
            {
                "1 > 2",
                new Binary(0, 4,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(4, 4, 2),
                    BinaryType.Greater)
            };

            yield return new object[]
            {
                "1 >= 2",
                new Binary(0, 5,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(5, 5, 2),
                    BinaryType.GreaterOrEquals)
            };

            yield return new object[]
            {
                "1 < 2",
                new Binary(0, 4,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(4, 4, 2),
                    BinaryType.Less)
            };

            yield return new object[]
            {
                "1 <= 2",
                new Binary(0, 5,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(5, 5, 2),
                    BinaryType.LessOrEquals)
            };

            yield return new object[]
            {
                "1 == 2",
                new Binary(0, 5,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(5, 5, 2),
                    BinaryType.Equals)
            };

            yield return new object[]
            {
                "1 <> 2",
                new Binary(0, 5,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(5, 5, 2),
                    BinaryType.NotEquals)
            };

            yield return new object[]
            {
                "1 != 2",
                new Binary(0, 5,
                    new ConstantNumber(0, 0, 1),
                    new ConstantNumber(5, 5, 2),
                    BinaryType.NotEquals)
            };

            yield return new object[]
            {
                "((1 != 2) == (3 <> 4)) <= ((5 > 6) >= (7 < 8))",
                new Binary(0, 45,
                    new Parens(0, 21,
                        new Binary(1, 20,
                            new Parens(1, 8,
                                new Binary(2, 7,
                                    new ConstantNumber(2, 2, 1),
                                    new ConstantNumber(7, 7, 2),
                                    BinaryType.NotEquals)),
                            new Parens(13, 20,
                                new Binary(14, 19,
                                    new ConstantNumber(14, 14, 3),
                                    new ConstantNumber(19, 19, 4),
                                    BinaryType.NotEquals)),
                            BinaryType.Equals)),
                    new Parens(26, 45,
                        new Binary(27, 44,
                            new Parens(27, 33,
                                new Binary(28, 32,
                                    new ConstantNumber(28, 28, 5),
                                    new ConstantNumber(32, 32, 6),
                                    BinaryType.Greater)),
                            new Parens(38, 44,
                                new Binary(39, 43,
                                    new ConstantNumber(39, 39, 7),
                                    new ConstantNumber(43, 43, 8),
                                    BinaryType.Less)),
                            BinaryType.GreaterOrEquals)),
                    BinaryType.LessOrEquals)
            };
        }
    }
}
