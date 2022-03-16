using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(ConstantNumberTestData), DynamicDataSourceType.Method)]
        public void ConstantNumberTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);
            result.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> ConstantNumberTestData()
        {
            yield return new object[]
            {
                "123",
                123
            };

            yield return new object[]
            {
                "123l",
                123L
            };

            yield return new object[]
            {
                "123L",
                123L
            };

            yield return new object[]
            {
                "123f",
                123f
            };

            yield return new object[]
            {
                "123F",
                123F
            };

            yield return new object[]
            {
                "123d",
                123d
            };

            yield return new object[]
            {
                "123D",
                123D
            };

            yield return new object[]
            {
                "123m",
                123m
            };

            yield return new object[]
            {
                "123M",
                123M
            };

            yield return new object[]
            {
                "12.3",
                12.3
            };

            yield return new object[]
            {
                "12.3f",
                12.3f
            };

            yield return new object[]
            {
                "12.3F",
                12.3F
            };

            yield return new object[]
            {
                "12.3d",
                12.3d
            };

            yield return new object[]
            {
                "12.3D",
                12.3D
            };

            yield return new object[]
            {
                "12.3m",
                12.3m
            };

            yield return new object[]
            {
                "12.3M",
                12.3M
            };
        }
    }
}
