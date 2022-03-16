using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(ConstantParensTestData), DynamicDataSourceType.Method)]
        public void ConstantParensTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> ConstantParensTestData()
        {
            yield return new object[]
            {
                "(true)",
                true
            };

            yield return new object[]
            {
                "(false)",
                false
            };

            yield return new object[]
            {
                "(1)",
                1M
            };

            yield return new object[]
            {
                "('abc')",
                "abc"
            };

            yield return new object[]
            {
                "((null))",
                null
            };
        }
    }
}
