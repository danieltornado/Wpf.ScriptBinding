using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(CallDynamicMethodTestData), DynamicDataSourceType.Method)]
        public void CallDynamicMethodTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> CallDynamicMethodTestData()
        {
            yield return new object[]
            {
                "(1 + 1).ToString()",
                "2"
            };

            yield return new object[]
            {
                "('abc' + 'abc').Substring(1, 3)", // TODO: Script have to parse int values instead of decimal
                "bca"
            };
        }
    }
}
