using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser
{
    partial class ParserTests
    {
        [TestMethod]
        [DynamicData(nameof(ParensNodeTestData), DynamicDataSourceType.Method)]
        public void ParensNodeTests(string expression, object expectedNode)
        {
            DefaultTest<ParensNode>(expression, expectedNode);
        }

        private static IEnumerable<object[]> ParensNodeTestData()
        {
            yield return new object[]
            {
                "(1)",
                new ParensNode(0, 2, 
                    new IntegerNode(1, 1, "1"))
            };

            yield return new object[]
            {
                "((1))",
                new ParensNode(0, 4, 
                    new ParensNode(1, 3, 
                        new IntegerNode(2, 2, "1")))
            };

            yield return new object[]
            {
                "((1 + 2) + b(1))",
                new ParensNode(0, 15, 
                    new BinaryNode(1, 14,
                        new ParensNode(1, 7,
                            new BinaryNode(2, 6,
                                new IntegerNode(2, 2, "1"),
                                new IntegerNode(6, 6, "2"),
                                BinaryOperation.Plus)),
                        new InvokeNode(11, 14, 
                            new IdentifierNode(11, 11, "b"),
                            new Node[]
                            {
                                new IntegerNode(13, 13, "1")
                            }),
                        BinaryOperation.Plus))
            };
        }
    }
}
