using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(CallTypeTestData), DynamicDataSourceType.Method)]
        public void CallTypeTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> CallTypeTestData()
        {
            yield return new object[]
            {
                "System.Math.Abs(1)",
                1M
            };

            yield return new object[]
            {
                "System.Math",
                null
            };
        }
    }
}
