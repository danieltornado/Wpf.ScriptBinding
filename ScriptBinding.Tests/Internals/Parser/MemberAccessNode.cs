using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser
{
    partial class ParserTests
    {
        [TestMethod]
        [DynamicData(nameof(MemberAccessNodeTestData), DynamicDataSourceType.Method)]
        public void MemberAccessNodeTests(string expression, object expectedNode)
        {
            DefaultTest<MemberAccessNode>(expression, expectedNode);
        }

        private static IEnumerable<object[]> MemberAccessNodeTestData()
        {
            yield return new object[]
            {
                "System.Visibility",
                new MemberAccessNode(0, 16, new []
                {
                    new IdentifierNode(0, 5, "System"),
                    new IdentifierNode(7, 16, "Visibility")
                })
            };

            yield return new object[]
            {
                "System.Math.Abs(1)",
                new MemberAccessNode(0, 17, new Node[]
                {
                    new IdentifierNode(0, 5, "System"),
                    new IdentifierNode(7, 10, "Math"),
                    new InvokeNode(12, 17, 
                        new IdentifierNode(12, 14, "Abs"), 
                        new []
                        {
                            new IntegerNode(16, 16, "1")
                        })
                })
            };

            yield return new object[]
            {
                "System.Math.Abs(1).Property",
                new MemberAccessNode(0, 26, new Node[]
                {
                    new IdentifierNode(0, 5, "System"),
                    new IdentifierNode(7, 10, "Math"),
                    new InvokeNode(12, 17,
                        new IdentifierNode(12, 14, "Abs"),
                        new []
                        {
                            new IntegerNode(16, 16, "1")
                        }),
                    new IdentifierNode(19, 26, "Property")
                })
            };

            yield return new object[]
            {
                "System.Math.Abs(1).ToString()",
                new MemberAccessNode(0, 28, new Node[]
                {
                    new IdentifierNode(0, 5, "System"),
                    new IdentifierNode(7, 10, "Math"),
                    new InvokeNode(12, 17,
                        new IdentifierNode(12, 14, "Abs"),
                        new []
                        {
                            new IntegerNode(16, 16, "1")
                        }),
                    new InvokeNode(19, 28, new IdentifierNode(19, 26, "ToString"), Array.Empty<Node>())
                })
            };

            yield return new object[]
            {
                "(23).CompareTo(45)",
                new MemberAccessNode(0, 17,
                    new Node[]
                    {
                        new ParensNode(0, 3, 
                            new IntegerNode(1, 2, "23")),
                        new InvokeNode(5, 17, 
                            new IdentifierNode(5, 13, "CompareTo"),
                            new Node[]
                            {
                                new IntegerNode(15, 16, "45")
                            })
                    })
            };

            yield return new object[]
            {
                "('just a string').Contains('just')",
                new MemberAccessNode(0, 33,
                    new Node[]
                    {
                        new ParensNode(0, 16, 
                            new StringNode(1, 15, "just a string")),
                        new InvokeNode(18, 33, 
                            new IdentifierNode(18, 25, "Contains"), 
                            new Node[]
                            {
                                new StringNode(27, 32, "just")
                            }),
                    })
            };
        }
    }
}
