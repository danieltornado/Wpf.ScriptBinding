using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser
{
    partial class ParserTests
    {
        [TestMethod]
        [DynamicData(nameof(IdentifierNodeTestData), DynamicDataSourceType.Method)]
        public void IdentifierNodeTests(string expression, object expectedNode)
        {
            DefaultTest<IdentifierNode>(expression, expectedNode);
        }

        private static IEnumerable<object[]> IdentifierNodeTestData()
        {
            yield return new object[]
            {
                "abc",
                new IdentifierNode(0, 2, "abc")
            };

            yield return new object[]
            {
                "_a",
                new IdentifierNode(0, 1, "_a")
            };

            yield return new object[]
            {
                "_1",
                new IdentifierNode(0, 1, "_1")
            };

            yield return new object[]
            {
                "a1",
                new IdentifierNode(0, 1, "a1")
            };

            yield return new object[]
            {
                "a_a",
                new IdentifierNode(0, 2, "a_a")
            };

            yield return new object[]
            {
                "a_1",
                new IdentifierNode(0, 2, "a_1")
            };

            yield return new object[]
            {
                "a_",
                new IdentifierNode(0, 1, "a_")
            };
        }
    }
}
