using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(CallElementBindingTestData), DynamicDataSourceType.Method)]
        public void CallElementBindingTests(string expression, string propertyPath, string elementName, object bindingValue, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            bindingProvider.GetValue(propertyPath, elementName).Set(bindingValue);

            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);

            bindingProvider.GetValue(propertyPath, elementName).CountOfReading().Should().Be(1);
        }

        private static IEnumerable<object[]> CallElementBindingTestData()
        {
            yield return new object[]
            {
                "b(abc, control)",
                "abc", 
                "control", 
                123, 
                123
            };

            yield return new object[]
            {
                "b(abc, control).Length", 
                "abc", 
                "control", 
                "abcabc", 
                6
            };

            yield return new object[]
            {
                "b(abc, control).Substring(1, 3)", 
                "abc", 
                "control", 
                "abcabc", 
                "bca"
            };
        }
    }
}
