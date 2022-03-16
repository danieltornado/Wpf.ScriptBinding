using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(CallBindingTestData), DynamicDataSourceType.Method)]
        public void CallBindingTests(string expression, int index, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            bindingProvider.GetValue(index).Set(expectedResult);

            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);

            bindingProvider.GetValue(index).CountOfReading().Should().Be(1);
        }

        private static IEnumerable<object[]> CallBindingTestData()
        {
            yield return new object[]
            {
                "b(0)",
                0,
                2M
            };

            yield return new object[]
            {
                "b(1)",
                1,
                "some binding value"
            };

            yield return new object[]
            {
                "b(1000000)",
                1000000,
                true
            };
        }
    }
}
