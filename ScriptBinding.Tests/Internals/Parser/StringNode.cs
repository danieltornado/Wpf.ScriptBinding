using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser
{
    partial class ParserTests
    {
        [TestMethod]
        [DynamicData(nameof(StringNodeTestData), DynamicDataSourceType.Method)]
        public void StringNodeTests(string expression, object expectedNode)
        {
            DefaultTest<StringNode>(expression, expectedNode);
        }

        private static IEnumerable<object[]> StringNodeTestData()
        {
            yield return new object[]
            {
                "\"1\"",
                new StringNode(0, 2, "1")
            };

            yield return new object[]
            {
                "\"1.2\"",
                new StringNode(0, 4, "1.2")
            };

            yield return new object[]
            {
                "\"AAA bbb CC\"",
                new StringNode(0, 11, "AAA bbb CC")
            };

            yield return new object[]
            {
                "\"ABC45678.01234DEF\"",
                new StringNode(0, 18, "ABC45678.01234DEF")
            };

            yield return new object[]
            {
                "'1'",
                new StringNode(0, 2, "1")
            };

            yield return new object[]
            {
                "'1.2'",
                new StringNode(0, 4, "1.2")
            };

            yield return new object[]
            {
                "'AAA bbb CC'",
                new StringNode(0, 11, "AAA bbb CC")
            };

            yield return new object[]
            {
                "'ABC45678.01234DEF'",
                new StringNode(0, 18, "ABC45678.01234DEF")
            };
        }
    }
}
