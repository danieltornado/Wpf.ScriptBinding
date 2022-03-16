using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser
{
    partial class ParserTests
    {
        [TestMethod]
        [DynamicData(nameof(IntegerNodeTestData), DynamicDataSourceType.Method)]
        public void IntegerNodeTests(string expression, object expectedNode)
        {
            DefaultTest<IntegerNode>(expression, expectedNode);
        }

        private static IEnumerable<object[]> IntegerNodeTestData()
        {
            yield return new object[]
            {
                "1",
                new IntegerNode(0, 0, "1")
            };

            yield return new object[]
            {
                "123",
                new IntegerNode(0, 2, "123")
            };

            yield return new object[]
            {
                "123456789",
                new IntegerNode(0, 8, "123456789")
            };

            yield return new object[]
            {
                "123l",
                new IntegerNode(0, 3, "123", IntegerModifiers.L)
            };

            yield return new object[]
            {
                "123L",
                new IntegerNode(0, 3, "123", IntegerModifiers.L)
            };
        }
    }
}
