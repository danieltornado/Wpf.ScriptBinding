using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(CallPropertyTestData), DynamicDataSourceType.Method)]
        public void CallPropertyTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> CallPropertyTestData()
        {
            yield return new object[]
            {
                "('abc').Length",
                3
            };

            yield return new object[]
            {
                "(1).ToString().Length",
                1
            };
        }
    }
}
