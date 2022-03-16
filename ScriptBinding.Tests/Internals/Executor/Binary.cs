using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Executor.Tools;

namespace ScriptBinding.Tests.Internals.Executor
{
    partial class ExecutorTests
    {
        [TestMethod]
        [DynamicData(nameof(BinaryTestData), DynamicDataSourceType.Method)]
        public void BinaryTests(string expression, object expectedResult)
        {
            var bindingProvider = new BindingProviderMock();
            var result = expression.Execute(bindingProvider);
            
            result.Should().BeOfType(expectedResult.GetType())
                .And.Subject.Should().Be(expectedResult);
        }

        private static IEnumerable<object[]> BinaryTestData()
        {
            yield return new object[]
            {
                "1 + 1",
                2
            };

            yield return new object[]
            {
                "1.1 + 2.3",
                3.4
            };

            yield return new object[]
            {
                "((1)) + ((2.1))",
                3.1
            };

            yield return new object[]
            {
                "10.0 + System.Math.Abs(1)",
                11.0
            };

            yield return new object[]
            {
                "1 - 1",
                0
            };

            yield return new object[]
            {
                "1 + 1 - 1",
                1
            };

            yield return new object[]
            {
                "1 + 1 - 1M",
                1M
            };
            
            yield return new object[]
            {
                "1 * 3",
                3
            };

            yield return new object[]
            {
                "1 - 1 * 3",
                -2
            };

            yield return new object[]
            {
                "3 / 2",
                1
            };

            yield return new object[]
            {
                "1 * 3 / 2",
                1
            };

            yield return new object[]
            {
                "1 % 3",
                1
            };

            yield return new object[]
            {
                "1 / 5 % 3",
                0
            };

            yield return new object[]
            {
                "true or false",
                true
            };

            yield return new object[]
            {
                "true and false",
                false
            };

            yield return new object[]
            {
                "1 > 2",
                false
            };

            yield return new object[]
            {
                "1 >= 2",
                false
            };

            yield return new object[]
            {
                "1 == 2",
                false
            };

            yield return new object[]
            {
                "1 != 2",
                true
            };

            yield return new object[]
            {
                "1 <> 2",
                true
            };

            yield return new object[]
            {
                "1 < 2",
                true
            };

            yield return new object[]
            {
                "1 <= 2",
                true
            };

            yield return new object[]
            {
                "1 < 2 and 2 <= 3",
                true
            };
        }
    }
}
