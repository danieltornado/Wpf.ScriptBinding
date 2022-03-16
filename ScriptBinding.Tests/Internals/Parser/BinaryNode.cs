using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser
{
    partial class ParserTests
    {
        [TestMethod]
        [DynamicData(nameof(BinaryNodeTestData), DynamicDataSourceType.Method)]
        public void BinaryNodeTests(string expression, object expectedNode)
        {
            DefaultTest<BinaryNode>(expression, expectedNode);
        }

        private static IEnumerable<object[]> BinaryNodeTestData()
        {
            yield return new object[]
            {
                "a + b",
                new BinaryNode(0, 4,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(4, 4, "b"),
                    BinaryOperation.Plus)
            };

            yield return new object[]
            {
                "a + b + c",
                new BinaryNode(0, 8,
                    new BinaryNode(0, 4,
                        new IdentifierNode(0, 0, "a"),
                        new IdentifierNode(4, 4, "b"),
                        BinaryOperation.Plus),
                    new IdentifierNode(8, 8, "c"),
                    BinaryOperation.Plus)
            };

            yield return new object[]
            {
                "(1.2 + 2.3) + b",
                new BinaryNode(0, 14,
                    new ParensNode(0, 10,
                        new BinaryNode(1, 9,
                            new RealNode(1, 3, "1.2"),
                            new RealNode(7, 9, "2.3"),
                            BinaryOperation.Plus)),
                    new IdentifierNode(14, 14, "b"),
                    BinaryOperation.Plus)
            };

            yield return new object[]
            {
                "((1.2)) + 1 + b",
                new BinaryNode(0, 14, 
                    new BinaryNode(0, 10,
                        new ParensNode(0, 6, 
                            new ParensNode(1, 5, new RealNode(2, 4, "1.2"))),
                        new IntegerNode(10, 10, "1"),
                        BinaryOperation.Plus),
                    new IdentifierNode(14, 14, "b"),
                    BinaryOperation.Plus)
            };

            yield return new object[]
            {
                "(1 + 1.2) + (a + b)",
                new BinaryNode(0, 18,
                    new ParensNode(0, 8, 
                        new BinaryNode(1, 7,
                            new IntegerNode(1, 1, "1"),
                            new RealNode(5, 7, "1.2"),
                            BinaryOperation.Plus)),
                    new ParensNode(12, 18,
                        new BinaryNode(13, 17,
                            new IdentifierNode(13, 13, "a"),
                            new IdentifierNode(17, 17, "b"),
                            BinaryOperation.Plus)),
                    BinaryOperation.Plus)
            };

            yield return new object[]
            {
                "a - b",
                new BinaryNode(0, 4,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(4, 4, "b"),
                    BinaryOperation.Minus)
            };

            yield return new object[]
            {
                "a - b - c",
                new BinaryNode(0, 8,
                    new BinaryNode(0, 4,
                        new IdentifierNode(0, 0, "a"),
                        new IdentifierNode(4, 4, "b"),
                        BinaryOperation.Minus),
                    new IdentifierNode(8, 8, "c"),
                    BinaryOperation.Minus)
            };

            yield return new object[]
            {
                "a - b + c",
                new BinaryNode(0, 8,
                    new BinaryNode(0, 4,
                        new IdentifierNode(0, 0, "a"),
                        new IdentifierNode(4, 4, "b"),
                        BinaryOperation.Minus),
                    new IdentifierNode(8, 8, "c"),
                    BinaryOperation.Plus)
            };

            yield return new object[]
            {
                "a + b - c",
                new BinaryNode(0, 8,
                    new IdentifierNode(0, 0, "a"),
                    new BinaryNode(4, 8,
                        new IdentifierNode(4, 4, "b"),
                        new IdentifierNode(8, 8, "c"),
                        BinaryOperation.Minus),
                    BinaryOperation.Plus)
            };

            yield return new object[]
            {
                "a * b",
                new BinaryNode(0, 4,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(4, 4, "b"),
                    BinaryOperation.Multiply)
            };

            yield return new object[]
            {
                "a / b",
                new BinaryNode(0, 4,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(4, 4, "b"),
                    BinaryOperation.Divide)
            };

            yield return new object[]
            {
                "a % b",
                new BinaryNode(0, 4,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(4, 4, "b"),
                    BinaryOperation.Mod)
            };

            yield return new object[]
            {
                "a / b * c",
                new BinaryNode(0, 8,
                    new BinaryNode(0, 4,
                        new IdentifierNode(0, 0, "a"),
                        new IdentifierNode(4, 4, "b"),
                        BinaryOperation.Divide),
                    new IdentifierNode(8, 8, "c"),
                    BinaryOperation.Multiply)
            };

            yield return new object[]
            {
                "a * b / c",
                new BinaryNode(0, 8,
                    new IdentifierNode(0, 0, "a"),
                    new BinaryNode(4, 8,
                        new IdentifierNode(4, 4, "b"),
                        new IdentifierNode(8, 8, "c"),
                        BinaryOperation.Divide),
                    BinaryOperation.Multiply)
            };

            yield return new object[]
            {
                "a % b / c",
                new BinaryNode(0, 8,
                    new BinaryNode(0, 4,
                        new IdentifierNode(0, 0, "a"),
                        new IdentifierNode(4, 4, "b"),
                        BinaryOperation.Mod),
                    new IdentifierNode(8, 8, "c"),
                    BinaryOperation.Divide)
            };

            yield return new object[]
            {
                "a / b % c",
                new BinaryNode(0, 8,
                    new IdentifierNode(0, 0, "a"),
                    new BinaryNode(4, 8,
                        new IdentifierNode(4, 4, "b"),
                        new IdentifierNode(8, 8, "c"),
                        BinaryOperation.Mod),
                    BinaryOperation.Divide)
            };

            yield return new object[]
            {
                "a + b - c * i / j % k",
                new BinaryNode(0, 20, 
                    new IdentifierNode(0, 0, "a"), 
                    new BinaryNode(4, 20, 
                        new IdentifierNode(4, 4, "b"), 
                        new BinaryNode(8, 20, 
                            new IdentifierNode(8, 8, "c"), 
                            new BinaryNode(12, 20, 
                                new IdentifierNode(12, 12, "i"), 
                                new BinaryNode(16, 20, 
                                    new IdentifierNode(16, 16, "j"), 
                                    new IdentifierNode(20, 20, "k"), 
                                    BinaryOperation.Mod), 
                                BinaryOperation.Divide), 
                            BinaryOperation.Multiply), 
                        BinaryOperation.Minus), 
                    BinaryOperation.Plus)
            };
            
            yield return new object[]
            {
                "a or b",
                new BinaryNode(0, 5,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(5, 5, "b"),
                    BinaryOperation.Or)
            };

            yield return new object[]
            {
                "a or b % c",
                new BinaryNode(0, 9,
                    new BinaryNode(0, 5, 
                        new IdentifierNode(0, 0, "a"),
                        new IdentifierNode(5, 5, "b"),
                        BinaryOperation.Or),
                    new IdentifierNode(9, 9, "c"),
                    BinaryOperation.Mod)
            };

            yield return new object[]
            {
                "a % b or c",
                new BinaryNode(0, 9,
                    new IdentifierNode(0, 0, "a"),
                    new BinaryNode(4, 9, 
                        new IdentifierNode(4, 4, "b"),
                        new IdentifierNode(9, 9, "c"),
                        BinaryOperation.Or),
                    BinaryOperation.Mod)
            };
            
            yield return new object[]
            {
                "a and b",
                new BinaryNode(0, 6,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(6, 6, "b"),
                    BinaryOperation.And)
            };

            yield return new object[]
            {
                "a and b or c",
                new BinaryNode(0, 11,
                    new BinaryNode(0, 6,
                        new IdentifierNode(0, 0, "a"),
                        new IdentifierNode(6, 6, "b"),
                        BinaryOperation.And),
                    new IdentifierNode(11, 11, "c"),
                    BinaryOperation.Or)
            };

            yield return new object[]
            {
                "a or b and c",
                new BinaryNode(0, 11,
                    new IdentifierNode(0, 0, "a"),
                    new BinaryNode(5, 11,
                        new IdentifierNode(5, 5, "b"),
                        new IdentifierNode(11, 11, "c"),
                        BinaryOperation.And),
                    BinaryOperation.Or)
            };

            yield return new object[]
            {
                "a > b",
                new BinaryNode(0, 4,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(4, 4, "b"),
                    BinaryOperation.Greater)
            };

            yield return new object[]
            {
                "a >= b",
                new BinaryNode(0, 5,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(5, 5, "b"),
                    BinaryOperation.GreaterOrEquals)
            };

            yield return new object[]
            {
                "a < b",
                new BinaryNode(0, 4,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(4, 4, "b"),
                    BinaryOperation.Less)
            };

            yield return new object[]
            {
                "a <= b",
                new BinaryNode(0, 5,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(5, 5, "b"),
                    BinaryOperation.LessOrEquals)
            };

            yield return new object[]
            {
                "a == b",
                new BinaryNode(0, 5,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(5, 5, "b"),
                    BinaryOperation.Equals)
            };

            yield return new object[]
            {
                "a <> b",
                new BinaryNode(0, 5,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(5, 5, "b"),
                    BinaryOperation.NotEquals)
            };

            yield return new object[]
            {
                "a != b",
                new BinaryNode(0, 5,
                    new IdentifierNode(0, 0, "a"),
                    new IdentifierNode(5, 5, "b"),
                    BinaryOperation.NotEquals)
            };

            yield return new object[]
            {
                "((a != b) == (a <> b)) <= ((a > b) >= (a < b))",
                new BinaryNode(0, 45,
                    new ParensNode(0, 21, 
                        new BinaryNode(1, 20, 
                            new ParensNode(1, 8, 
                                new BinaryNode(2, 7, 
                                    new IdentifierNode(2, 2, "a"),
                                    new IdentifierNode(7, 7, "b"),
                                    BinaryOperation.NotEquals)),
                            new ParensNode(13, 20,
                                new BinaryNode(14, 19, 
                                    new IdentifierNode(14, 14, "a"),
                                    new IdentifierNode(19, 19, "b"),
                                    BinaryOperation.NotEquals)),
                            BinaryOperation.Equals)),
                    new ParensNode(26, 45, 
                        new BinaryNode(27, 44,
                            new ParensNode(27, 33, 
                                new BinaryNode(28, 32,
                                    new IdentifierNode(28, 28, "a"),
                                    new IdentifierNode(32, 32, "b"),
                                    BinaryOperation.Greater)),
                            new ParensNode(38, 44,
                                new BinaryNode(39, 43,
                                    new IdentifierNode(39, 39, "a"),
                                    new IdentifierNode(43, 43, "b"),
                                    BinaryOperation.Less)),
                            BinaryOperation.GreaterOrEquals)),
                    BinaryOperation.LessOrEquals)
            };
        }
    }
}
