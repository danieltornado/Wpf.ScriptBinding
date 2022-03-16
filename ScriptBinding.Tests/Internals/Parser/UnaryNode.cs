using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser
{
    partial class ParserTests
    {
        [TestMethod]
        [DynamicData(nameof(UnaryNodeTestData), DynamicDataSourceType.Method)]
        public void UnaryNodeTests(string expression, object expectedNode)
        {
            DefaultTest<UnaryNode>(expression, expectedNode);
        }

        private static IEnumerable<object[]> UnaryNodeTestData()
        {
            yield return new object[]
            {
                "not(true)",
                new UnaryNode(0, 8, 
                    new IdentifierNode(4, 7, "true"), 
                    UnaryOperation.Not)
            };

            yield return new object[]
            {
                "not ( true ) ",
                new UnaryNode(0, 11,
                    new IdentifierNode(6, 9, "true"),
                    UnaryOperation.Not)
            };
        }
    }
}
