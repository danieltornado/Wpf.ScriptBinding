using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Compiler.Tools;

// ReSharper disable once CheckNamespace
namespace ScriptBinding.Tests.Internals.Compiler
{
    [TestClass]
    public partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(GeneratedBindingTestData), DynamicDataSourceType.Method)]
        public void GeneratedBindingTests(string expression, object expectedData)
        {
            var expectedGenerator = (BindingGeneratorMock)expectedData;
            var bindingGenerator = new BindingGeneratorMock();

            _ = expression.Compile(bindingGenerator);

            using (new AssertionScope())
            {
                bindingGenerator.PropertyBindings.Should().HaveSameCount(expectedGenerator.PropertyBindings)
                    .And.Subject.Should().BeSubsetOf(expectedGenerator.PropertyBindings);
                
                bindingGenerator.ElementBindings.Should().HaveSameCount(expectedGenerator.ElementBindings)
                    .And.Subject.Should().BeSubsetOf(expectedGenerator.ElementBindings);
            }
        }

        private static IEnumerable<object[]> GeneratedBindingTestData()
        {
            // no expected bindings
            yield return new object[]
            {
                "b(1)",
                new BindingGeneratorMock()
            };

            yield return new object[]
            {
                "1 + b(IsChecked)",
                new BindingGeneratorMock().AddBinding("IsChecked")
            };

            yield return new object[]
            {
                "1 + b(IsChecked) + b(IsChecked)",
                new BindingGeneratorMock().AddBinding("IsChecked").AddBinding("IsChecked")
            };

            yield return new object[]
            {
                "(1 + b(IsChecked)) + (b(IsChecked) + b(IsChecked, control))",
                new BindingGeneratorMock().AddBinding("IsChecked").AddBinding("IsChecked").AddBinding("IsChecked", "control")
            };
        }

        private static void DefaultTest<TExpr>(string expression, object expectedExpr)
            where TExpr : ScriptBinding.Internals.Compiler.Expressions.Expr
        {
            var expected = (TExpr)expectedExpr;
            var expr = expression.Compile();

            expr.Should().BeOfType<TExpr>()
                .And.Subject.Should().BeEquivalentTo(expected, options => options.Using(new EquivalentExprComparer()));
        }
    }
}
