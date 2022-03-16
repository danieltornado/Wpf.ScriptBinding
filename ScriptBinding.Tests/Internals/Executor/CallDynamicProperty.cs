using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(CallDynamicPropertyTestData), DynamicDataSourceType.Method)]
        public void CallDynamicPropertyTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> CallDynamicPropertyTestData()
        {
            yield return new object[]
            {
                "('abc' + 'abc').Length",
                6
            };
        }
    }
}
