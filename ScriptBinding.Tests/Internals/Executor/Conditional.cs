using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(ConditionalTestData), DynamicDataSourceType.Method)]
        public void ConditionalTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> ConditionalTestData()
        {
            yield return new object[]
            {
                "if(true) then(2) else(3)",
                2M
            };

            yield return new object[]
            {
                "if(false) then(2) else(3)",
                3M
            };
        }
    }
}
