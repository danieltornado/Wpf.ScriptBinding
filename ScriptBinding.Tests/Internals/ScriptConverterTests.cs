using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals;
using ScriptBinding.Tests.Internals.Executor.Tools;
using ScriptBinding.Tests.Internals.Parser.Tools;

namespace ScriptBinding.Tests.Internals
{
    [TestClass]
    public sealed class ScriptConverterTests
    {
        [TestMethod]
        [DynamicData(nameof(ScriptConverterTestData), DynamicDataSourceType.Method)]
        public void ScriptConverter_Convert_Test(string expression, object[] values, object expectedResult, BindingBase[] bindings)
        {
            var holder = PrepareHolder(expression);

            // The order of the bindings/generated bindings is not guaranteed.
            // Test can have no effects, but production is not.
            var allBindings = bindings.Concat(holder.GeneratedBindings).ToList();

            var converter = PrepareScriptConverter(holder, allBindings);

            var actualResult = converter.Convert(values, expectedResult?.GetType(), null, CultureInfo.CurrentCulture);

            actualResult.Should().Be(expectedResult);
        }

        [TestMethod]
        public void ScriptConverterTest_ConvertBack_Test()
        {
            var holder = PrepareHolder("1");

            var converter = PrepareScriptConverter(holder, Array.Empty<BindingBase>());

            converter
                .Invoking(e => e.ConvertBack(null, Array.Empty<Type>(), null, CultureInfo.CurrentCulture))
                .Should()
                .Throw<NotSupportedException>();
        }

        private static IEnumerable<object[]> ScriptConverterTestData()
        {
            yield return new object[]
            {
                "b(0) + b(1) + b(2)",
                new object[] { 1, 1, 1 },
                3,
                new BindingBase[] { new Binding("Some1"), new Binding("Some2"), new Binding("Some3") }
            };

            // dangerous test: b(0) and b(IsChecked) are the same things
            yield return new object[]
            {
                "b(0) + b(IsChecked)",
                new object[] { 4.5 },
                9.0,
                Array.Empty<BindingBase>()
            };

            yield return new object[]
            {
                "b(IsChecked) + b(IsChecked, Grid)",
                new object[] { 2, 3 },
                5,
                Array.Empty<BindingBase>()
            };

            // dangerous test: bindings "Some1" and "Some2" are before generated bindings, they have values "0".
            // b(IsChecked) and b(IsChecked, Grid) have values 2, 3
            yield return new object[]
            {
                "b(IsChecked) + b(IsChecked, Grid)",
                new object[] { 0, 0, 2, 3 },
                5,
                new BindingBase[] { new Binding("Some1"), new Binding("Some2") }
            };
        }

        [TestMethod]
        public void ScriptConverter_BinaryWithNull_Test()
        {
            var holder = PrepareHolder("b(0) + b(1) + b(2)");
            var bindings = new BindingBase[] { new Binding(), new Binding(), new Binding() };
            var values = new object[] { "text", null, "text" };

            var converter = PrepareScriptConverter(holder, bindings);

            var actualResult = converter.Convert(values, typeof(string), null, CultureInfo.CurrentCulture);
            actualResult.Should().Be("texttext");
        }

        private ScriptConverter PrepareScriptConverter(ExpressionHolder holder, IReadOnlyList<BindingBase> bindings)
        {
            var executorErrorListener = new ExecutorExceptionListener();
            var converter = new ScriptConverter(executorErrorListener);

            converter.SetExpression(holder.Expression, bindings);

            return converter;
        }

        private ExpressionHolder PrepareHolder(string expression)
        {
            var parserErrorListener = new ParserExceptionListener();

            var parser = new ScriptBinding.Internals.Parser.Parser(parserErrorListener);
            var parsedExpression = parser.Parse(expression);

            var expressionHolder = new ExpressionHolder();

            var compiler = new ScriptBinding.Internals.Compiler.Compiler(expressionHolder);
            var compiledExpression = compiler.Compile(parsedExpression);

            expressionHolder.SetExpression(compiledExpression);

            return expressionHolder;
        }
    }
}
