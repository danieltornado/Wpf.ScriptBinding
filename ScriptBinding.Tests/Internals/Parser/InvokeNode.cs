using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser
{
    partial class ParserTests
    {
        [TestMethod]
        [DynamicData(nameof(InvokeNodeTestData), DynamicDataSourceType.Method)]
        public void InvokeNodeTests(string expression, object expectedNode)
        {
            DefaultTest<InvokeNode>(expression, expectedNode);
        }

        private static IEnumerable<object[]> InvokeNodeTestData()
        {
            yield return new object[]
            {
                "a()",
                new InvokeNode(0, 2, 
                    new IdentifierNode(0, 0, "a"), 
                    Array.Empty<Node>())
            };

            yield return new object[]
            {
                "b(1)",
                new InvokeNode(0, 3, 
                    new IdentifierNode(0, 0, "b"), 
                    new []
                    {
                        new IntegerNode(2, 2, "1")
                    })
            };

            yield return new object[]
            {
                "_c(a, b)",
                new InvokeNode(0, 7, 
                    new IdentifierNode(0, 1, "_c"), 
                    new []
                    {
                        new IdentifierNode(3, 3, "a"),
                        new IdentifierNode(6, 6, "b")
                    })
            };

            yield return new object[]
            {
                "_1(1.2, 'text')",
                new InvokeNode(0, 14,
                    new IdentifierNode(0, 1, "_1"),
                    new Node[]
                    {
                        new RealNode(3, 5, "1.2"),
                        new StringNode(8, 13, "text")
                    })
            };
        }
    }
}
