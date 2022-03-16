using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(ConstantNumberTestData), DynamicDataSourceType.Method)]
        public void ConstantNumberTests(string expression, object expectedExpr)
        {
            DefaultTest<ConstantNumber>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> ConstantNumberTestData()
        {
            yield return new object[]
            {
                "1000",
                new ConstantNumber(0, 3, 1000)
            };

            yield return new object[]
            {
                "1000l",
                new ConstantNumber(0, 4, 1000L)
            };

            yield return new object[]
            {
                "1000L",
                new ConstantNumber(0, 4, 1000L)
            };

            yield return new object[]
            {
                "2.1",
                new ConstantNumber(0, 2, 2.1)
            };

            yield return new object[]
            {
                "2.1f",
                new ConstantNumber(0, 3, 2.1f)
            };

            yield return new object[]
            {
                "2.1F",
                new ConstantNumber(0, 3, 2.1F)
            };

            yield return new object[]
            {
                "2.1m",
                new ConstantNumber(0, 3, 2.1m)
            };

            yield return new object[]
            {
                "2.1M",
                new ConstantNumber(0, 3, 2.1M)
            };

            yield return new object[]
            {
                "1000d",
                new ConstantNumber(0, 4, 1000d)
            };

            yield return new object[]
            {
                "1000D",
                new ConstantNumber(0, 4, 1000D)
            };
        }
    }
}
