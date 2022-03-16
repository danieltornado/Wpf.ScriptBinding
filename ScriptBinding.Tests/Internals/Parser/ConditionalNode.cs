using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser
{
    partial class ParserTests
    {
        [TestMethod]
        [DynamicData(nameof(ConditionalNodeTestData), DynamicDataSourceType.Method)]
        public void ConditionalNodeTests(string expression, object expectedNode)
        {
            DefaultTest<ConditionalNode>(expression, expectedNode);
        }

        private static IEnumerable<object[]> ConditionalNodeTestData()
        {
            yield return new object[]
            {
                "if(1) then(2) else(3)",
                new ConditionalNode(0, 20,
                    new IntegerNode(3, 3, "1"),
                    new IntegerNode(11, 11, "2"),
                    new IntegerNode(19, 19, "3"))
            };

            yield return new object[]
            {
                "if(if(1) then(1) else(1)) then(System.Math) else(b(1))",
                new ConditionalNode(0, 53,
                    new ConditionalNode(3, 23, 
                        new IntegerNode(6, 6, "1"), 
                        new IntegerNode(14, 14, "1"), 
                        new IntegerNode(22, 22, "1")),
                    new MemberAccessNode(31, 41, new [] { new IdentifierNode(31, 36, "System"), new IdentifierNode(38, 41, "Math") }),
                    new InvokeNode(49, 52, new IdentifierNode(49, 49, "b"), new [] { new IntegerNode(51, 51, "1") }))
            };
        }
    }
}
