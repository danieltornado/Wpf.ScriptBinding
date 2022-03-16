using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(CallMethodTestData), DynamicDataSourceType.Method)]
        public void CallMethodTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> CallMethodTestData()
        {
            yield return new object[]
            {
                "(1).ToString()",
                "1"
            };

            yield return new object[]
            {
                "('abc').ToString()",
                "abc"
            };

            yield return new object[]
            {
                "(' abc ').Trim()",
                "abc"
            };
        }
    }
}
