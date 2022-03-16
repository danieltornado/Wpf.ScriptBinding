using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(ConstantBooleanTestData), DynamicDataSourceType.Method)]
        public void ConstantBooleanTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> ConstantBooleanTestData()
        {
            yield return new object[]
            {
                "true",
                true
            };

            yield return new object[]
            {
                "false",
                false
            };
        }
    }
}
