using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(CallPropertyBindingTestData), DynamicDataSourceType.Method)]
        public void CallPropertyBindingTests(string expression, string propertyPath, object bindingValue, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            bindingProvider.GetValue(propertyPath).Set(bindingValue);

            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);

            bindingProvider.GetValue(propertyPath).CountOfReading().Should().Be(1);
        }

        private static IEnumerable<object[]> CallPropertyBindingTestData()
        {
            yield return new object[]
            {
                "b(abc)",
                "abc",
                123,
                123
            };

            yield return new object[]
            {
                "b(abc).Length",
                "abc",
                "abcabc",
                6
            };

            yield return new object[]
            {
                "b(abc).Substring(1, 3)",
                "abc",
                "abcabc",
                "bca"
            };
        }
    }
}
