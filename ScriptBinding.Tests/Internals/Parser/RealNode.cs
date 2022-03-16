using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser
{
    partial class ParserTests
    {
        [TestMethod]
        [DynamicData(nameof(RealNodeTestData), DynamicDataSourceType.Method)]
        public void RealNodeTests(string expression, object expectedNode)
        {
            DefaultTest<RealNode>(expression, expectedNode);
        }

        private static IEnumerable<object[]> RealNodeTestData()
        {
            yield return new object[]
            {
                "1.2",
                new RealNode(0, 2, "1.2")
            };

            yield return new object[]
            {
                "0.123",
                new RealNode(0, 4, "0.123")
            };

            yield return new object[]
            {
                "1234.56789",
                new RealNode(0, 9, "1234.56789")
            };

            yield return new object[]
            {
                "1.2f",
                new RealNode(0, 3, "1.2", RealModifiers.F)
            };

            yield return new object[]
            {
                "1.2F",
                new RealNode(0, 3, "1.2", RealModifiers.F)
            };

            yield return new object[]
            {
                "1.2d",
                new RealNode(0, 3, "1.2", RealModifiers.D)
            };

            yield return new object[]
            {
                "1.2D",
                new RealNode(0, 3, "1.2", RealModifiers.D)
            };

            yield return new object[]
            {
                "1.2m",
                new RealNode(0, 3, "1.2", RealModifiers.M)
            };

            yield return new object[]
            {
                "1.2M",
                new RealNode(0, 3, "1.2", RealModifiers.M)
            };

            yield return new object[]
            {
                "1f",
                new RealNode(0, 1, "1", RealModifiers.F)
            };

            yield return new object[]
            {
                "1F",
                new RealNode(0, 1, "1", RealModifiers.F)
            };

            yield return new object[]
            {
                "1d",
                new RealNode(0, 1, "1", RealModifiers.D)
            };

            yield return new object[]
            {
                "1D",
                new RealNode(0, 1, "1", RealModifiers.D)
            };

            yield return new object[]
            {
                "1m",
                new RealNode(0, 1, "1", RealModifiers.M)
            };

            yield return new object[]
            {
                "1M",
                new RealNode(0, 1, "1", RealModifiers.M)
            };
        }
    }
}
