using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(UnaryTestData), DynamicDataSourceType.Method)]
        public void UnaryTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);

            result.Should().BeOfType(expectedResult.GetType())
                .And.Subject.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> UnaryTestData()
        {
            yield return new object[]
            {
                "not(true)",
                false
            };

            yield return new object[]
            {
                "not(false)",
                true
            };

            yield return new object[]
            {
                "not(1 < 2)",
                false
            };
        }
    }
}
